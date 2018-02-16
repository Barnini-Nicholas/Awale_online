using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logique d'interaction pour Historique.xaml
    /// </summary>
    public partial class Historique : Window
    {
        public List<Score> ListScores { get; set; }

        public Historique()
        {

            InitializeComponent();

            // Liste des Scores
            ListScores = new List<Score>();

            // Si le fichier historique existe
            if (File.Exists("./save/score.csv"))
            {
                // Read Historique 
                String[] lines = File.ReadAllLines("./save/score.csv");

                foreach (String line in lines)
                {
                    Console.WriteLine(line);
                    Score score = new Score(line);

                    ListScores.Add(score);
                }
            }
            
            DataContext = this;
        }
    }
}
