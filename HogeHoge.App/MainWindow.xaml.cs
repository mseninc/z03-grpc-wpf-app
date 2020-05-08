using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HogeHoge.Rpc;
using System;
using System.Windows;
using System.Windows.Media;

namespace HogeHoge.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        const int DefaultPort = 50051;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = new CommanderService();
                var server = new Server
                {
                    Services = { Commander.BindService(service) },
                    Ports = { new ServerPort("localhost", DefaultPort, ServerCredentials.Insecure) },
                };
                server.Start();
                Title += " - Server mode";
                ClientPanel.IsEnabled = false;
                MessageTextBox.Text = "Server started.";
            }
            catch (Exception)
            {
                Title += " - Client mode";
                ClientPanel.IsEnabled = true;
            }
        }

        private void ActivateButton_Click(object sender, RoutedEventArgs e)
        {
            var channel = new Channel("localhost", DefaultPort, ChannelCredentials.Insecure);
            var client = new Commander.CommanderClient(channel);
            var result = client.Activate(new Empty());
            MessageTextBox.Foreground = result.Success ? Brushes.Black : Brushes.Red;
            MessageTextBox.Text = result.Message;
        }

        private async void StartConnectButton_Click(object sender, RoutedEventArgs e)
        {
            var channel = new Channel("localhost", DefaultPort, ChannelCredentials.Insecure);
            var client = new Commander.CommanderClient(channel);
            var result = await client.StartConnectAsync(new StartConnectRequest {
                    ConnectionType = ConnectionTypeTextBox.Text,
                    ConnectionId = int.Parse(ConnectionIdTextBox.Text),
            });
            MessageTextBox.Foreground = result.Success ? Brushes.Black : Brushes.Red;
            MessageTextBox.Text = result.Message;
        }
    }
}
