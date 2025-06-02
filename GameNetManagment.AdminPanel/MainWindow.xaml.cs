using GameNetManagment.Core.Entities;
using GameNetManagment.Core.Enums;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameNetManagment.AdminPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<Station> Stations { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7123/") }; // آدرس سرور خودتان
            Stations = new ObservableCollection<Station>();
            StationsDataGrid.ItemsSource = Stations; // نام DataGrid خودتان
        }

        private async Task LoadStationsAsync()
        {
            try
            {
                var loadedStations = await _httpClient.GetFromJsonAsync<ObservableCollection<Station>>("api/stations");
                if (loadedStations != null)
                {
                    Stations.Clear();
                    foreach (var station in loadedStations)
                    {
                        Stations.Add(station);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stations: {ex.Message}");
            }
        }


        private async void LoadStationsButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadStationsAsync();
        }

        private async void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            // از TextBox ها بخوانید
            var newStation = new Station
            {
                Name = NameTextBox.Text, // نام TextBox خودتان
                IpAddress = IpAddressTextBox.Text, // نام TextBox خودتان
                Status = StationStatus.Free // یا از یک ComboBox
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/stations", newStation);
                response.EnsureSuccessStatusCode(); // اگر خطا بود Exception می دهد
                await LoadStationsAsync(); // بارگذاری مجدد لیست
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding station: {ex.Message}");
            }
        }
    }
}