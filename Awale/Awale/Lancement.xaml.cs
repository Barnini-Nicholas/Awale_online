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

        private void LancerHebergerPartie(object sender, RoutedEventArgs e)
        {
            new HostGame().Show();
            Close();
        }

        private void LancerConnecterPartie(object sender, RoutedEventArgs e)
        {
            new Connect().Show();
            Close();
        }

        private void LancerHistorique(object sender, RoutedEventArgs e)
        {
            new Historique().Show();
            Close();
        }

        private void Informations(object sender, RoutedEventArgs e)
        {
            new Informations().Show();
            Close();
        }
    }
}
