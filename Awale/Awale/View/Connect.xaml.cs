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
        public String NomJ2 { get; set; }

        public Thread Attente { get; set; }
        private PlateauDeJeu plateau;

        public String Ip { get; set; }

        public Connect()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void SeConnecter(object sender, RoutedEventArgs e)
        {
            Button bouton = sender as Button;

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

            bouton.Content = "En attente...";
            bouton.IsEnabled = false;
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

                Attente = new Thread(AttenteMessage);
                Attente.Start();

            }
            catch (Exception)
            {
                Console.WriteLine("CA MARCHE PAS");
            }
        }

        private void AttenteMessage()
        {
            while (true)
            {
                String message = "";
                BinaryReader reader = new BinaryReader(server.GetStream());
                message = reader.ReadString();
               
                Console.WriteLine(message);

                
                if (message.Split(';')[0] == "LANCER")

                {
                    NomJ2 = message.Split(';')[1];

                    Dispatcher.Invoke((LancerPartieDelegate)LancerLaPartie);
                }

                if (message.Split(';')[0] == "ACTION")
                {
                    int indexTrou = Int32.Parse(message.Split(';')[1]);
                    plateau.ActionJoueurReseau(indexTrou);

                    Console.WriteLine(indexTrou);
                }
            }
        }
        private delegate void LancerPartieDelegate();

        private void LancerPartie(object sender, RoutedEventArgs e)
        {
            LancerLaPartie();
        }


        private void LancerLaPartie()
        {
            
            plateau = new PlateauDeJeu(NomJ2, Nom, 6, false, true);

            plateau.SetCombatReseau(null, this, false);

            plateau.Show();

            Hide();

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

        internal void SendAction(int indexTrou)
        {
            BinaryWriter writer = new BinaryWriter(server.GetStream());
            writer.Write("ACTION;" + indexTrou);
        }

        internal void CloseAll()
        {
            /*
            Attente.Abort();
            plateau.Close();
            Console.WriteLine("CONNECT EXIT");
            new Lancement().Show();

            Close();*/
        }

    }
}
