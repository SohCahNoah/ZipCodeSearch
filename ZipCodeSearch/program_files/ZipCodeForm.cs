using GUI_test_v3.program_files;
using System.Text.Json;
//
namespace GUI_test_v3
{
    public partial class ZipCodeForm : Form
    {
        private bool isButtonClicked = false;
        private readonly DataLogic dataLogic;
        private readonly CSVmethods csvMethods;
        public ZipCodeForm()
        {
            InitializeComponent();
            dataLogic = new DataLogic();
            csvMethods = new CSVmethods();

            //Pulls focus to the zip code entry box when the form is first loaded (QOL addition)
            this.Shown += new EventHandler(Form1_Load);
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            //Pulls focus to the zip code text entry box when function is called
            zip_code_entry_box.Focus();

        }

        private async void Search_button_click(object sender, EventArgs e)
        {
            string userInput = zip_code_entry_box.Text;

            if (userInput != "")
            {
                List<string> z_csvData = CSVmethods.ProcessZipCSV();
                List<Zip_Data> zipCodeDataList = CSVmethods.ParseZipData(z_csvData);
                Zip_Data? z_data = zipCodeDataList.FirstOrDefault(z => z.ZipCode == userInput);

                Depot_Data depot_Data = CSVmethods.FindClosestDepot(userInput);


                if (z_data != null)
                {

                    string RoadDistance_response = await dataLogic.GetResultsAsync(z_data.Lat, z_data.Long, depot_Data.DepotLat, depot_Data.DepotLong);
                    RouteResponse? RoadDistance_deserialized = JsonSerializer.Deserialize<RouteResponse>(RoadDistance_response);
                    double depot_distace = RoadDistance_deserialized.routes.FirstOrDefault()?.summary?.distance ?? 0;

                    results_display_box.Text =
                        $"Customer Location:\n" +
                        $"{z_data.City} {z_data.State}, {z_data.ZipCode}\n" +

                        $"\nDepot Data:\n" +
                        $"{depot_Data.DepotAddress}\n" +
                        $"{depot_Data.DepotCity} {depot_Data.DepotState}, {depot_Data.DepotZip}\n" +

                        $"\nDistance to Depot:\n" +
                        $"{depot_distace} miles";
                }
                else
                {
                    results_display_box.Text =
                        $"Zip Code '{userInput}' was not found in database...";
                }
            }

            zip_code_entry_box.Focus();
            isButtonClicked = true;
        }


        private void MaskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void MaskedTextBox1_MaskInputRejected_1(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void TextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Zip_code_entry_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Set the flag when Enter is pressed
                isButtonClicked = true;
            }
            else if (isButtonClicked)
            {
                // Clear the text when the user starts typing after hitting Enter
                zip_code_entry_box.Text = ""; // Clear the text
                isButtonClicked = false; // Reset the flag
            }
        }
    }
}