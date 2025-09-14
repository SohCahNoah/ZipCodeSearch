#ZipCodeSearch/program_files/closest_depot.py
#Purpose: Using csv_praser.py to get parsed zip and depot data, find the nearest depot to the user-provided zip-code
#Returns: DepotEntry object containing information on the depot

import program_files.csv_parser as csv_parser
import os
import requests
from typing import List, Optional, Tuple
from pathlib import Path
from dotenv import load_dotenv
from scipy.spatial import KDTree
from time import perf_counter

load_dotenv(Path(__file__).resolve().parent.parent / ".env")

def check_zip(user_zip: str, zips: List[csv_parser.ZipRecord]) -> bool:
    """Checks if zip Code exists within the ZipRecord already
    
    Args:
        zip_code (str): User entry to check
        zips (List[ZipRecord]): List of ZipRecords defined by csv_parser
    
    Returns:
        bool: Returns True if zip code exists, else returns False
    """
    
    target = str(user_zip).strip()
    return any(row["Zip"] == target for row in zips)

def get_zip_coords(user_zip: str, zips: List[csv_parser.ZipRecord]) -> Optional[Tuple[float, float]]:
    """Get zip coordinates (lat, long) tuple based on user entry

    Args:
        user_zip (str): zip code to search
        zips (List[csv_parser.ZipRecord]): Lsit of ZipRecords defined by csv_parser

    Returns:
        Optional[Tuple[float, float]]: (Lat, Long) tuple for the user_zip
    """
    target = str(user_zip).strip()
    for row in zips:
        if row['Zip'] == target:
            return(row['Lat'], row['Long'])
    return None

def get_distance(
    lat1: float,
    lon1: float,
    lat2: float,
    lon2: float,
    api_key: Optional[str] = None,
    timeout: float = -1,
) -> Optional[float]:
    """
    Road distance via OpenRouteService Directions API.
    Returns miles (float) or None on failure.

    NOTE: ORS expects [lon, lat] order in 'coordinates'.
    """
    if api_key is None:
        api_key = os.getenv("ORS_API_KEY")
    if not api_key:
        raise ConnectionError("Missing OpenRouteService API key (pass api_key or set ORS_API_KEY in .env).")

    url = "https://api.openrouteservice.org/v2/directions/driving-car"
    headers = {
        "Accept": "application/json",
        "Authorization": api_key,
        "Content-Type": "application/json; charset=utf-8",
    }
    body = {
        "coordinates": [[lon1, lat1], [lon2, lat2]],
        "units": "mi",
        "instructions": False,
        "maneuvers": False,
        "preference": "recommended",
    }

    # requests: None means "no timeout"
    timeout_arg = None if timeout == -1 else timeout

    try:
        resp = requests.post(url, json=body, headers=headers, timeout=timeout_arg)
        resp.raise_for_status()
        data = resp.json()
        routes = data.get("routes") or []
        if not routes:
            return None
        summary = routes[0].get("summary", {})
        miles = summary.get("distance")
        if miles is None:
            return None
        return float(miles)
    except requests.RequestException as e:
        print(f"ERROR: ORS request failed: {e}")
        return None
    except ValueError as e:
        print(f"ERROR: Failed to parse ORS response: {e}")
        return None

def closest_depot_using_KDtree(
    user_zip: str,
    zips: List[csv_parser.ZipRecord],
    depots: List[csv_parser.DepotRecord],
    api_key: Optional[str] = None,
    k: int = 1
    ) -> Optional[Tuple[List[Optional[float]], List[csv_parser.DepotRecord]]]:
    user_coords = get_zip_coords(user_zip, zips)
    if user_coords is None:
        return None
    
    ulat, ulon = user_coords
    if not depots:
        return [], []

    depot_coords: List[Tuple[float, float]] = [(d["DepotLat"], d["DepotLong"]) for d in depots]
    k = max(1, min(k, len(depots)))

    tree = KDTree(depot_coords)
    _, idxs = tree.query((ulat, ulon), k=k)
    if k == 1:
        idxs = [int(idxs)]
    else:
        idxs = list(map(int, idxs))
        
    #For all 'k' depots, fetch road distance
    road_miles: List[Optional[float]] = []
    chosen_depots: List[csv_parser.DepotRecord] = []
    for i in idxs:
        d = depots[i]
        miles = get_distance(
            lat1=ulat, lon1=ulon,
            lat2=d["DepotLat"], lon2=d["DepotLong"],
            api_key=api_key
        )
        road_miles.append(miles)
        chosen_depots.append(d)
    
    return road_miles, chosen_depots
    
if __name__ == "__main__":
    ZIP_FILE = Path(__file__).resolve().parent.parent / "zipcodes" / "US_Zip_data.txt"
    DEPOT_FILE = Path(__file__).resolve().parent.parent / "depots" / "depot_location_data.txt"

    z = csv_parser.parse_zip_file(ZIP_FILE)
    d = csv_parser.parse_depot_file(DEPOT_FILE)

    hermiston = get_zip_coords("97838", z)
    corvallis = get_zip_coords("97330", z)

    print(f"Zip (97330) in list?: {check_zip('97330', z)}")
    print(f"Zip (ABCDE) in list?: {check_zip('ABCDE', z)}")
    print(f"Zip (97330) Lat/Long: {get_zip_coords('97330', z)}")

    if hermiston and corvallis:
        miles = get_distance(hermiston[0], hermiston[1], corvallis[0], corvallis[1])
        print(f"Distance from Zip (97330) to Zip (97838): {miles}")
    else:
        print("One of the test ZIPs was not found in the ZIP file.")

    user_zip = "97330"   # adjust as needed
    k = 2
    api = os.getenv("ORS_API_KEY")

    t0 = perf_counter()
    result = closest_depot_using_KDtree(
        user_zip=user_zip,
        zips=z,
        depots=d,
        k=k,
        api_key=api,
    )
    t1 = perf_counter()

    if result is None:
        print(f"ZIP {user_zip} not found.")
    else:
        distances, depots = result
        print(f"\nNearest {len(depots)} depots to {user_zip} (computed in {(t1 - t0):.3f}s):")
        for i, (dist, depot) in enumerate(zip(distances, depots), start=1):
            dist_str = f"{dist:.2f} miles" if dist is not None else "no route"
            print(f"{i}. {depot['DepotAddress']}, {depot['DepotCity']} {depot['DepotZip']} — {dist_str}")