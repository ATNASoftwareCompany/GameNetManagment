using Microsoft.AspNetCore.SignalR.Client;
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

namespace GameNetManagment.ClientAgent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _hubConnection;
        private string _clientName = "DefaultClient";
        public MainWindow()
        {
            InitializeComponent();
            _hubConnection = new HubConnectionBuilder()
           .WithUrl("https://localhost:7123/cafeHub") // آدرس سرور و Hub
           .WithAutomaticReconnect() // تلاش برای اتصال مجدد خودکار
           .Build();

            // تعریف متدهایی که سرور آنها را صدا می زند
            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Dispatcher.Invoke(() => // برای آپدیت UI از ترد دیگر
                {
                    MessagesListBox.Items.Add($"{user}: {message}");
                });
            });

            _hubConnection.On<string, string>("ReceiveAnnouncement", (user, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessagesListBox.Items.Add($"ANNOUNCEMENT from {user}: {message}");
                });
            });

        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _hubConnection.StartAsync();
                    MessagesListBox.Items.Add("Connected to hub.");
                    ConnectButton.Content = "Disconnect";
                    _clientName = ClientNameTextBox.Text; // نام TextBox نام کلاینت
                }
                catch (Exception ex)
                {
                    MessagesListBox.Items.Add($"Connection error: {ex.Message}");
                }
            }
            else
            {
                await _hubConnection.StopAsync();
                MessagesListBox.Items.Add("Disconnected from hub.");
                ConnectButton.Content = "Connect";
            }

        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                try
                {
                    // فراخوانی متد Announce در سرور
                    await _hubConnection.InvokeAsync("Announce", _clientName, MessageTextBox.Text); // نام TextBox پیام
                }
                catch (Exception ex)
                {
                    MessagesListBox.Items.Add($"Error sending message: {ex.Message}");
                }
            }
        }

        // برای قطع اتصال هنگام بسته شدن پنجره
        protected override async void OnClosed(EventArgs e)
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
            base.OnClosed(e);
        }
    }
    
}