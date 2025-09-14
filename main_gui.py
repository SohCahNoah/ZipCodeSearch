from __future__ import annotations
import os
import threading
from pathlib import Path
import tkinter as tk
from tkinter import ttk
import tkinter.scrolledtext as scrolledtext
from tkinter import messagebox
from dotenv import load_dotenv
from program_files import csv_parser
from program_files import closest_depot

BASE_DIR = Path(__file__).resolve().parent
ZIP_FILE = BASE_DIR / "zipcodes" / "US_Zip_data.txt"
DEPOT_FILE = BASE_DIR / "depots" / "depot_location_data.txt"

load_dotenv(BASE_DIR / ".env")

try:
    ZIPS = csv_parser.parse_zip_file(ZIP_FILE)
    DEPOTS = csv_parser.parse_depot_file(DEPOT_FILE)
except Exception as e:
    import traceback
    tk.Tk().withdraw()
    messagebox.showerror("Startup Error", f"Failed to load data files:\n{e}\n\n{traceback.format_exc()}")
    raise

def format_depot_line(d: csv_parser.DepotRecord) -> str:
    return (
        f"{d["DepotName"]}\n"
        f"{d['DepotAddress']}\n"
        f"{d['DepotCity']} {d['DepotZip']}\n"
        f"({d['DepotLat']:.6f}, {d['DepotLong']:.6f})"
    )

def set_output(text: str):
    out_text.config(state="normal")
    out_text.delete("1.0", "end")
    out_text.insert("1.0", text)
    out_text.config(state="disabled")

def copy_output():
    data = out_text.get("1.0", "end-1c")
    if not data.strip():
        return
    root.clipboard_clear()
    root.clipboard_append(data)
    root.update()

def _select_all(event=None):
    out_text.tag_add("sel", "1.0", "end-1c")
    return "break"

def _copy_sel(event=None):
    copy_output()
    return "break"

def modal_message(title: str, message: str):
    win = tk.Toplevel(root)
    win.title(title)
    win.transient(root)
    win.grab_set()
    frame = ttk.Frame(win, padding=16)
    frame.pack(fill="both", expand=True)
    lbl = ttk.Label(frame, text=message, justify="left", anchor="w", wraplength=420)
    lbl.pack(fill="both", expand=True, pady=(0, 12))
    btn_ok = ttk.Button(frame, text="OK", command=win.destroy)
    btn_ok.pack()
    def _close(event=None):
        win.destroy()
        return "break"
    win.bind("<Return>", _close)
    win.bind("<KP_Enter>", _close)
    win.bind("<Escape>", _close)
    btn_ok.focus_set()
    win.update_idletasks()
    x = root.winfo_rootx() + (root.winfo_width() - win.winfo_width()) // 2
    y = root.winfo_rooty() + (root.winfo_height() - win.winfo_height()) // 3
    win.geometry(f"+{max(x, 0)}+{max(y, 0)}")
    win.wait_window()

def start_loading():
    btn.config(state="disabled")
    entry_zip.config(state="disabled")
    root.config(cursor="watch")
    progress.pack(fill="x", pady=(0, 8))
    progress.start(12)
    set_output("Searching...")

def stop_loading():
    progress.stop()
    progress.pack_forget()
    btn.config(state="normal")
    entry_zip.config(state="normal")
    root.config(cursor="")

def on_enter(event=None):
    on_search()
    return "break"

def _entry_select_all(event=None):
    entry_zip.select_range(0, "end")
    entry_zip.icursor("end")
    return "break"

def on_search():
    zip_code = entry_zip.get().strip()
    if not zip_code:
        modal_message("Input needed", "Please enter a ZIP code.")
        return
    thread = threading.Thread(target=_search_worker, args=(zip_code,), daemon=True)
    start_loading()
    thread.start()

def _search_worker(zip_code: str):
    if not closest_depot.check_zip(zip_code, ZIPS):
        def _warn():
            stop_loading()
            modal_message("ZIP not found", f"ZIP {zip_code} was not found in the ZIP list.")
        root.after(0, _warn)
        return
    api = os.getenv("ORS_API_KEY")
    try:
        result = closest_depot.closest_depot_using_KDtree(
            user_zip=zip_code,
            zips=ZIPS,
            depots=DEPOTS,
            k=2,
            api_key=api,
        )
    except ConnectionError:
        def _no_key():
            stop_loading()
            modal_message("Missing API key", "OpenRouteService key not set.\nPut ORS_API_KEY=... in your .env at project root.")
        root.after(0, _no_key)
        return
    except Exception as e:
        def _err():
            stop_loading()
            modal_message("Error", f"An error occurred while searching:\n{e}")
        root.after(0, _err)
        return
    if result is None:
        def _not_found():
            stop_loading()
            modal_message("Not found", f"Could not resolve ZIP {zip_code}.")
        root.after(0, _not_found)
        return
    road_miles_list, depot_list = result
    best_idx = None
    best_miles = None
    for i, miles in enumerate(road_miles_list):
        if miles is None:
            continue
        if best_miles is None or miles < best_miles:
            best_miles = miles
            best_idx = i
    if best_idx is None:
        def _no_route():
            stop_loading()
            set_output(f"No drivable route found from {zip_code} to the nearest candidates.")
        root.after(0, _no_route)
        return
    depot = depot_list[best_idx]
    def _finish():
        stop_loading()
        set_output(
            f"Closest depot to {zip_code}:\n\n"
            f"{format_depot_line(depot)}\n\n"
            f"Road distance: {best_miles:.2f} miles"
        )
    root.after(0, _finish)

root = tk.Tk()
root.title("Closest Depot Finder")
root.geometry("540x380")

main = ttk.Frame(root, padding=16)
main.pack(fill="both", expand=True)

row1 = ttk.Frame(main)
row1.pack(fill="x", pady=(0, 8))

ttk.Label(row1, text="Enter ZIP:").pack(side="left")
entry_zip = ttk.Entry(row1, width=20)
entry_zip.pack(side="left", padx=(8, 8))
entry_zip.focus()
entry_zip.bind("<Return>", on_enter)
entry_zip.bind("<KP_Enter>", on_enter)
entry_zip.bind("<Control-a>", _entry_select_all)
entry_zip.bind("<Control-A>", _entry_select_all)
entry_zip.bind("<Command-a>", _entry_select_all)
entry_zip.bind("<Command-A>", _entry_select_all)

btn = ttk.Button(row1, text="Find Closest Depot", command=on_search)
btn.pack(side="left")

out_text = scrolledtext.ScrolledText(main, height=10, wrap="word")
out_text.pack(fill="both", expand=True, pady=(8, 8))
out_text.config(state="disabled")
out_text.bind("<Control-a>", _select_all)
out_text.bind("<Control-A>", _select_all)
out_text.bind("<Control-c>", _copy_sel)
out_text.bind("<Control-C>", _copy_sel)

progress = ttk.Progressbar(main, mode="indeterminate")
progress.pack_forget()

copy_row = ttk.Frame(main)
copy_row.pack(fill="x")
ttk.Button(copy_row, text="Copy Result", command=copy_output).pack(side="right")

root.mainloop()
