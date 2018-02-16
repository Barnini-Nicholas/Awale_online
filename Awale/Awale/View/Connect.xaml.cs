using System;
using System.Collections.Generic;
using System.IO;
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
        private TcpClient server;

        public String Nom { get; set; }

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
            Button bouton = sender as Button;

            if (bouton.Content.ToString() == "Lancer la partie !")
            {
                BinaryWriter writer = new BinaryWriter(server.GetStream());
                writer.Write("LANCER;" + Nom);
            }
            else
            {
                Thread t = new Thread(ConnectToHost);
                t.Start();

                bouton.Content = "Lancer la partie !";
            }
        }

        private void ConnectToHost()
        {
            IPAddress ip = IPAddress.Parse(Ip);
            int port = 5000;
            try
            {
                server = new TcpClient();
                server.Connect(ip, port);
                Console.WriteLine("client connected!!");

                BinaryWriter writer = new BinaryWriter(server.GetStream());
                writer.Write("NOM;" + Nom);

            }
            catch (Exception)
            {
                Console.WriteLine("CA MARCHE PAS");
            }
        }

        private void AttenteMessage()
        {

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                // Simuler un click hé
                buttonGO.Focus();
                buttonGO.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
