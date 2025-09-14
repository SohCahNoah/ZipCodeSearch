namespace GUI_test_v3.program_files
{
    public class CSVmethods
    {
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string zipcode_filepath = Path.Combine(baseDirectory, "zipcodes", "US_Zip_data.txt");
        private static readonly string depot_filepath = Path.Combine(baseDirectory, "depots", "depot_location_data.txt");

        private static List<Zip_Data> zipCodeDataCache;
        private static List<Depot_Data> depotDataCache;

        static CSVmethods()
        {
            Console.WriteLine("Zipcode file path: " + Path.GetFullPath(zipcode_filepath));
            Console.WriteLine("Depot file path: " + Path.GetFullPath(depot_filepath));
            
            zipCodeDataCache = ParseZipData(ProcessZipCSV());
            depotDataCache = ParseDepotData(ProcessDepotCSV());
        }

        public static List<string> ProcessZipCSV()
        {
            List<string> data = new List<string>();
            try
            {
                if (File.Exists(zipcode_filepath))
                {
                    string[] contents = File.ReadAllLines(zipcode_filepath);
                    data.AddRange(contents);
                }
                else
                {
                    data.Add($"File not found: {zipcode_filepath}");
                }
            }

            catch (Exception ex)
            {
                data.Add($"Error occured: {ex.Message}");
            }

            return data;
        }
        public static List<Zip_Data> ParseZipData(List<string> csvLines)
        {
            List<Zip_Data> zipCodeDataList = new List<Zip_Data>();

            // Skip the first line (header) and start parsing from the second line
            for (int i = 1; i < csvLines.Count; i++)
            {
                string line = csvLines[i];
                string[] fields = line.Split('\t');
                if (fields.Length >= 1)
                {
                    Zip_Data zipData = new Zip_Data()
                    {
                        ZipCode = fields[1].Trim(),
                        Lat = fields[9].Trim(),
                        Long = fields[10].Trim(),
                        City = fields[2].Trim(),
                        State = fields[3].Trim()
                    };
                    zipCodeDataList.Add(zipData);
                }
            }
            return zipCodeDataList;
        }

        public Zip_Data FindZipCodeData(string zipCodeToFind)
        {
            foreach (var zipData in zipCodeDataCache)
            {
                if (zipData.ZipCode == zipCodeToFind)
                {
                    return zipData;
                }
            }
            return null;
        }

        public static List<string> ProcessDepotCSV()
        {
            List<string> data = new List<string>();
            try
            {
                if (File.Exists(depot_filepath))
                {
                    string[] contents = File.ReadAllLines(depot_filepath);
                    data.AddRange(contents);
                }
                else
                {
                    data.Add($"File not found: {depot_filepath}");

                }
            }

            catch (Exception ex)
            {
                data.Add($"Error occured: {ex.Message}");
            }

            return data;
        }

        public static List<Depot_Data> ParseDepotData(List<string> csvLines)
        {
            List<Depot_Data> depotDataList = new List<Depot_Data>();

            // Skip the first line (header) and start parsing from the second line
            for (int i = 1; i < csvLines.Count; i++)
            {
                string line = csvLines[i];
                string[] fields = line.Split('\t');
                if (fields.Length >= 1)
                {
                    Depot_Data depotData = new Depot_Data()
                    {
                        DepotName = fields[0].Trim(),
                        DepotManager = fields[1].Trim(),
                        DepotAddress = fields[4].Trim(),
                        DepotPhone = fields[2].Trim(),
                        DepotCity = fields[5].Trim(),
                        DepotState = fields[6].Trim(),
                        DepotZip = fields[7].Trim(),
                        DepotLat = fields[8].Trim(),
                        DepotLong = fields[9].Trim(),
                    };
                    depotDataList.Add(depotData);
                }
            }
            return depotDataList;
        }

        public static Depot_Data FindClosestDepot(string zipCodeToFind)
        {
            var zipData = zipCodeDataCache.FirstOrDefault(z => z.ZipCode == zipCodeToFind);
            if (zipData == null)
            {
                Console.WriteLine($"Zip Code not found: {zipCodeToFind}");
                return null;
            }

            double zipLat = double.Parse(zipData.Lat);
            double zipLong = double.Parse(zipData.Long);

            Depot_Data closestDepot = null;
            double closestDistance = double.MaxValue;

            foreach (var depotData in depotDataCache)
            {
                string rawLat = depotData.DepotLat;
                string rawLong = depotData.DepotLong;

                if (double.TryParse(rawLat, out double depotLat) && double.TryParse(rawLong, out double depotLong))
                {
                    double distance = GetDistance(zipLat, zipLong, depotLat, depotLong);
                    Console.WriteLine($"Depot Name: {depotData.DepotAddress} \t Distance to Depot: {distance}");
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestDepot = depotData;
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid latitude or longitude for depot: {depotData.DepotName} (Lat: {rawLat}, Long: {rawLong})");
                }
            }
            Console.WriteLine($"\nClosest Depot: {closestDepot.DepotAddress} \t Distance to Depot: {closestDistance}");
            return closestDepot;
        }

        private static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            //Uses the Haversine formula for quick-and-dirty distance calculation. Precise distance measurements that are served to the user happen in GetResultsAsync method from DataLogic.cs
            const double R = 3958.8; // Radius of the earth in miles
            double latDistance = ToRadians(lat2 - lat1);
            double lonDistance = ToRadians(lon2 - lon1);
            double a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2)
                       + Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2))
                       * Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c; // Convert to miles
            return distance;
        }

        private static double ToRadians(double angle)
        {
            //Returns the radian of a given angle (used in GetDistance function)
            return angle * (Math.PI / 180);
        }
    }
}