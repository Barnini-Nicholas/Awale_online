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
    public partial class HostGame : Window, INotifyPropertyChanged
    {
        public String NomJ1 { get; set; }
        private String nomJ2;
        private TcpClient client;
        public Thread Attente { get; set; }
        public TcpListener ServerSocket { get; set; }
        private PlateauDeJeu plateau;

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
            ServerSocket = new TcpListener(IPAddress.Any, 5000);
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

                if (message.Split(';')[0] == "NOM")
                {
                    NomJ2 = message.Split(';')[1];
                }
                if (message.Split(';')[0] == "LANCER")
                {
                    Dispatcher.Invoke((LancerPartieDelegate)LancerLaPartie);
                }
                if (message.Split(';')[0] == "ACTION")
                {
                    int indexTrou = Int32.Parse(message.Split(';')[1]);
                    plateau.ActionJoueurReseau(indexTrou);
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

            BinaryWriter writer = new BinaryWriter(client.GetStream());
            writer.Write("LANCER;"+NomJ1);

            plateau = new PlateauDeJeu(NomJ1, NomJ2, 6, false, true);

            plateau.SetCombatReseau(this, null, true);

            plateau.Show();

            Hide();

            // Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void SendAction(int indexTrou)
        {
            BinaryWriter writer = new BinaryWriter(client.GetStream());
            writer.Write("ACTION;" + indexTrou);

            Console.WriteLine("aaaaa" + indexTrou);
        }

        internal void CloseAll()
        {
            plateau.Close();
            Close();
        }
    }
}
