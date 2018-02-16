using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logique d'interaction pour LobbyEnLigne.xaml
    /// </summary>
    public partial class HostGame : Window , INotifyPropertyChanged
    {
        public String NomJ1 { get; set; }
        private String nomJ2;
        private TcpClient client;
        public Thread Attente { get; set; }

        public String NomJ2
        {
            get
            {
                return nomJ2;
            }
            set
            {
                nomJ2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("nomJ2"));
            }
        }

        public int NbColumns { get; set; }
        public const int DefaultNbColumns = 6;


        public HostGame()
        {
            // Default value for number of columns
            NbColumns = DefaultNbColumns;

            InitializeComponent();

            DataContext = this;

            // Lancement du serveur...
            Thread t = new Thread(LancementServeur);
            t.Start();
        }

        public void LancementServeur()
        {
            TcpListener ServerSocket = new TcpListener(IPAddress.Any, 5000);
            ServerSocket.Start();
            Console.WriteLine("Serveur en marche... " + ServerSocket.ToString());

            client = ServerSocket.AcceptTcpClient();
            Console.WriteLine("Someone connected!!");

            Attente = new Thread(AttenteMessage);
            Attente.Start();
        }

        private void AttenteMessage()
        {
            while (true)
            {
                String message = "";
                
                BinaryReader reader = new BinaryReader(client.GetStream());
                message = reader.ReadString();
                Console.WriteLine(message);

                if(message.Split(';')[0] == "NOM")
                {
                    NomJ2 = message.Split(';')[1];
                }
                if (message.Split(';')[0] == "LANCER")
                {
                    Dispatcher.Invoke((LancerLaPartieDelegate)LancerLaPartie);
                }
            }
        }

        private void LancerPartie(object sender, RoutedEventArgs e)
        {
            LancerLaPartie();
        }

        private delegate void LancerLaPartieDelegate();

        private void LancerLaPartie()
        {
            // Si un des noms n'a pas été donné
            if (NomJ1 == null)
            {
                MessageBox.Show("Veuillez entrer votre nom");
                return;
            }
            if (NomJ2 == null)
            {
                MessageBox.Show("Veuillez attendre qu'un joueur se connecte");
                return;
            }

            new PlateauDeJeu(NomJ1, NomJ2, 6).Show();
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
