using System.Net.Http.Headers;
using System.Text;
using System.Configuration;

namespace GUI_test_v3.program_files

{
    public class Zip_Data
    {
        public string? ZipCode { get; set; }
        public string? Lat { get; set; }
        public string? Long { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

    }

    public class Depot_Data
    {
        public string? DepotName { get; set; }
        public string? DepotManager { get; set; }
        public string? DepotAddress { get; set; }
        public string? DepotPhone { get; set; }
        public string? DepotCity { get; set; }
        public string? DepotState { get; set; }
        public string? DepotZip { get; set; }
        public string? DepotLat { get; set; }
        public string? DepotLong { get; set; }
    }

    public class RouteSummary
    {
        public double distance { get; set; }
        public double duration { get; set; }
    }

    public class Route
    {
        public RouteSummary summary { get; set; }
    }

    public class RouteResponse
    {
        public List<Route> routes { get; set; }
    }
    public class DataLogic
    {
        private readonly HttpClient httpClient;
        private readonly CSVmethods csvMethods;
        private readonly DepotKDTree depotTree;

        //Find solution that does not have hardocded API key at some point
        private readonly string apiKey = ConfigurationManager.AppSettings["API_KEY"];
        public DataLogic()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.openrouteservice.org/v2/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

    public async Task<string> GetResultsAsync(string lat1, string lon1, string lat2, string lon2)
        {
            // Create request body
            var requestBody = new
            {
                coordinates = new[] { new[] { lon1, lat1 }, new[] { lon2, lat2 } },
                format = "geojson",
                units = "mi",
                geometry_simplify = "true",
                instructions = "false",
                maneuvers = "false",
                preference = "recommended"
            };

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);

            // Create StringContent with correct Content-Type header
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("directions/driving-car/json", content);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }

        public class DepotKDTree
        {
            //Implement KD-Tree here to search for the closest depot (using lat/long data) to a user given zipcode (translated into lat/long data using CSVmethods.FindZipCodeData() method
        }
    }
}