using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Awale.Model;

namespace Awale.View
{
    /// <summary>
    /// Logique d'interaction pour LobbyLocal.xaml
    /// </summary>
    public partial class LobbyLocal : Window
    {
        public String NomJ1 { get; set; }
        public String NomJ2 { get; set; }

        public int NbColumns { get; set; }
        public const int DefaultNbColumns = 6;

        public LobbyLocal()
        {
            // Default value for number of columns
            NbColumns = DefaultNbColumns;

            InitializeComponent();

            DataContext = this;
        }

        private void LancerPartie(object sender, RoutedEventArgs e)
        {
            // Si un des noms n'a pas été donné
            if(NomJ1 == null || NomJ2 == null)
            {
                MessageBox.Show("Veuillez entrer un nom pour les deux joueurs");
                return;
            }

            // Lancement de la Partie
            new PlateauDeJeu(NomJ1, NomJ2, NbColumns).Show();

            Close();
        }
    }
}
