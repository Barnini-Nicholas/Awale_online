using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Awale.View
{
    /// <summary>
    /// Logique d'interaction pour Connect.xaml
    /// </summary>
    public partial class Connect : Window
    {
        public String Ip { get; set; }

        public Connect()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void SeConnecter(object sender, RoutedEventArgs e)
        {
            // Si un des noms n'a pas été donné
            if (Ip == null)
            {
                MessageBox.Show("Veuillez entrerune adresse IP");
                return;
            }

            Thread t = new Thread(ConnectToHost);
            t.Start();
        }

        private void ConnectToHost()
        {
            IPAddress ip = IPAddress.Parse(Ip);
            int port = 5000;
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("client connected!!");
        }
    }
}
