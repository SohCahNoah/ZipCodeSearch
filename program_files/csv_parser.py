#ZipCodeSearch/program_files/csv_parser.py
#Purpose: Read US_Zip_data.txt (a TSV file) and depot_locations_data.txt (a TSV file)
#Returns: A list of records from the path file

from pathlib import Path
import csv
from typing import List, Union, TypedDict

# --- Typed Records
class ZipRecord(TypedDict):
    Zip: str
    Lat: float
    Long: float

class DepotRecord(TypedDict):
    DepotName: str
    DepotAddress: str
    DepotCity: str
    DepotZip: str
    DepotLat: float
    DepotLong: float
    
def parse_zip_file(path: Union[str, Path]) -> List[ZipRecord]:
    path = Path(path)
    records: List[ZipRecord] = []
    with path.open("r", encoding="utf-8-sig", newline="") as f:
        reader = csv.DictReader(f, delimiter="\t")
        for row in reader:
            rec: ZipRecord = {
                "Zip":  row["Zip"].strip(),
                "Lat":  float(row["Lat"]),
                "Long": float(row["Long"]),
            }
            records.append(rec)
    return records

def parse_depot_file(path: Union[str, Path]) -> List[DepotRecord]:
    path = Path(path)
    records: List[DepotRecord] = []
    with path.open("r", encoding="utf-8-sig", newline="") as f:
        reader = csv.DictReader(f, delimiter="\t")
        for row in reader:
            rec: DepotRecord = {
                "DepotName":    row["Depot Name"].strip(),
                "DepotAddress": row["Depot Address"].strip(),
                "DepotCity":    row["Depot City"].strip(),
                "DepotZip":     row["Depot Zip"].strip(),
                "DepotLat":     float(row["Lat"]),
                "DepotLong":    float(row["Long"]),
            }
            records.append(rec)
    return records

#TEST FUNCTIONS
if __name__ == "__main__":
    ZIP_FILE = Path(__file__).resolve().parent.parent/"zipcodes"/"US_Zip_data.txt"
    DEPOT_FILE = Path(__file__).resolve().parent.parent/"depots"/"depot_location_data.txt"
    
    zip_rows = parse_zip_file(ZIP_FILE)
    depot_rows = parse_depot_file(DEPOT_FILE)
    print(f"Loaded {len(zip_rows)} rows from {ZIP_FILE}")
    print(f"Loaded {len(depot_rows)} rows from {DEPOT_FILE}")
    input("\nPress any key to continue...")
    for rec in zip_rows[:10]:
        print(f"ZIP {rec['Zip']}: Lat={rec['Lat']}, Long={rec['Long']}")
    input("\nPress any key to continue...")
    for rec in depot_rows[:10]:
        print(f"Depot Address {rec['DepotAddress']}: Lat={rec['DepotLat']}, Long={rec['DepotLong']}, Zip={rec['DepotZip']}")