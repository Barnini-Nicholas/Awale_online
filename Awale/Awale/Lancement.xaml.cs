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
using Awale.View;

namespace Awale
{
    /// <summary>
    /// Logique d'interaction pour Lancementxaml.xaml
    /// </summary>
    public partial class Lancement : Window
    {
        public int NbColumns { get; set; }
        public const int DefaultNbColumns = 6;

        public Lancement()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void LancerPartieLocale(object sender, RoutedEventArgs e)
        {
            new LobbyLocal().Show();
            Close();
        }

        private void LancerPartieEnLigne(object sender, RoutedEventArgs e)
        {

        }

        private void Informations(object sender, RoutedEventArgs e)
        {
            new Informations().Show();
            Close();
        }
    }
}
