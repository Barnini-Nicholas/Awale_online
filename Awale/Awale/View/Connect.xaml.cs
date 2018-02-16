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
        public String Name { get; set; }

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
                MessageBox.Show("Veuillez entrer une adresse IP");
                return;
            }

            if (Name == null)
            {
                MessageBox.Show("Veuillez entrer un nom");
                return;
            }

            Thread t = new Thread(ConnectToHost);
            t.Start();
        }

        private void ConnectToHost()
        {
            IPAddress ip = IPAddress.Parse(Ip);
            int port = 5000;
            try {
                TcpClient client = new TcpClient();
                client.Connect(ip, port);
                Console.WriteLine("client connected!!");

                // Envoi d'un message test
                byte[] buffer = Encoding.ASCII.GetBytes(Name + Environment.NewLine);

                NetworkStream stream = client.GetStream();

                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("CA MARCHE PAS");
            }
        }
    }
}
