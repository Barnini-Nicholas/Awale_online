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
    /// Logique d'interaction pour LobbyEnLigne.xaml
    /// </summary>
    public partial class HostGame : Window
    {
        public String NomJ1 { get; set; }
        public String NomJ2 { get; set; }

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

            TcpClient client = ServerSocket.AcceptTcpClient();
            Console.WriteLine("Someone connected!!");

        }

        private void LancerPartie(object sender, RoutedEventArgs e)
        {
            // Si un des noms n'a pas été donné
            if (NomJ1 == null)
            {
                MessageBox.Show("Veuillez entrer votre nom");
                return;
            }

            MessageBox.Show(NomJ1);

            Close();
        }
    }
}
