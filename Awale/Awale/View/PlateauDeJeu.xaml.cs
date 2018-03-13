using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Awale.Model;

namespace Awale.View
{
    // <summary>
    /// Logique d'interaction pour PlateauDeJeu.xaml
    /// </summary>
    public partial class PlateauDeJeu : Window, INotifyPropertyChanged
    {
        public int NbColumns { get; set; }
        public int NbTrous { get; set; }
        public int NbGraines { get; set; }

        public ObservableCollection<Trou> ListTrous { get; set; }
        public List<Trou> ListTrousOrdonnes { get; set; }

        public Joueur J1 { get; set; }
        public Joueur J2 { get; set; }

        public Boolean IsJ2IA { get; set; }
        public Boolean IsCombatReseau { get; set; }
        
        public Joueur JoueurActuelReseau;
        private HostGame hostGame;
        private Connect connect;

        private Joueur joueurCourant;
        public Joueur JoueurCourant
        {
            get
            {
                return joueurCourant;
            }
            set
            {
                joueurCourant = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("joueurCourant"));
            }
        }
        public PlateauDeJeu(string nomJ1, string nomJ2, int nbColumns, bool isIA, bool isCombatReseau)
        {
            InitializeComponent();

            NbColumns = nbColumns;
            NbTrous = nbColumns * 2;
            NbGraines = NbTrous * 4;

            // Initialisation des Joueurs :
            J1 = new Joueur(nomJ1, 1);
            J2 = new Joueur(nomJ2, 2);

            // Indique si le J2 est une IA
            J2.SetIsIA(isIA);

            // Indique si c'est un combat en Réseau
            IsCombatReseau = isCombatReseau;

            // J1 commence
            JoueurCourant = J1;

            // Liste des trous
            ListTrous = new ObservableCollection<Trou>();

            // Initialisation du plateau :
            for (int i = 1; i <= NbTrous; i++)
            {
                // Trou Adversaire (1ère ligne)
                if (i <= NbColumns)
                {
                    ListTrous.Add(new Trou(J1, i - 1));

                }
                // Trou Ou False (2ème ligne)
                else
                {
                    ListTrous.Add(new Trou(J2, i - 1));
                }
            }

            // Remplissage des Trous :
            foreach (Trou trou in ListTrous)
            {
                trou.Valeur = 4;
            }

            // Remplissage d'une autre liste bien ordonnée
            ListTrousOrdonnes = new List<Trou>();
            for (int i = nbColumns - 1; i >= 0; i--)
            {
                Trou trouCourant = ListTrous[i];
                ListTrousOrdonnes.Add(trouCourant);
            }
            for (int i = nbColumns; i < NbTrous; i++)
            {
                Trou trouCourant = ListTrous[i];
                ListTrousOrdonnes.Add(trouCourant);
            }

            JoueurCourant = J1;

            DataContext = this;
        }

        public void ActionJoueurReseau(int indexTrou)
        {
            TraitementActionJoueur(ListTrousOrdonnes[indexTrou]);
        }

        public void SetCombatReseau(HostGame hostGame, Connect connect, Boolean isJ1)
        {
            if(isJ1 == false)
            {
                this.connect = connect;
                JoueurActuelReseau = J2;
            } else
            {
                this.hostGame = hostGame;
                JoueurActuelReseau = J1;
            }

        }

        

        void Button_Click(object sender, RoutedEventArgs e)
        {
            // Bouton clické
            var button = (Button)sender;

            // Trou associé au bouton
            Trou trou = (Trou)button.DataContext;

            // Check si le Joueur clique sur la bonne rangée
            if (trou.Joueur != JoueurCourant)
            {
                MessageBox.Show("Tu triches " + JoueurCourant.Nom +", ce n'est pas ton trou !");
                return;
            }

            // Check si le trou n'est pas vide
            if (trou.Valeur == 0)
            {
                MessageBox.Show("Tu ne peux pas tricheur " + trou.Joueur.Nom);
                return;
            }

            // Check pour le reseau
            if(JoueurCourant != JoueurActuelReseau)
            {
                MessageBox.Show("Ce n'est pas à toi de jouer ! ");
                return;
            }

            

            // Gestion de l'action
            TraitementActionJoueur(trou);
            
            // Si le joueur 2 est une IA ...
            if(J2.IsIA == true)
            {
                // On fait jouer l'IA
                Trou trouChoisiParIA = IA.ChoisirAction(J2, ListTrousOrdonnes);

                // On traite son choix
                TraitementActionJoueur(trouChoisiParIA);
            }

            // Si partie en réseau 
            if (IsCombatReseau == true)
            {
                int indexTrou = ListTrousOrdonnes.IndexOf(trou);

                if (hostGame != null)
                {
                    hostGame.SendAction(indexTrou);
                } else
                {
                    connect.SendAction(indexTrou);
                }
                
            }
        }

        public void TraitementActionJoueur(Trou trou)
        {
            // Valeur du Trou
            int valeur = trou.Valeur;

           

            // On vide le Trou
            trou.Valeur = 0;

            // Distribution des graines sur les cases suivantes :
            int indexTrou = ListTrousOrdonnes.IndexOf(trou);

            int indexTrouCourant = indexTrou;

            // Tant que nous n'avons pas distribué toutes les graines :
            while (valeur != 0)
            {
                // On est à la fin de la liste 
                if (indexTrouCourant == NbTrous - 1)
                {
                    // -> On retourne au début de la list
                    indexTrouCourant = 0;
                }
                else
                {
                    // On passe au trou suivant
                    indexTrouCourant += 1;
                }

                // Si on repasse par le trou cliqué auparavant 
                if (indexTrouCourant == indexTrou)
                {
                    // -> On saute le trou
                    continue;
                }

                // On récupère le Trou courant
                Trou trouCourant = ListTrousOrdonnes[indexTrouCourant];

                // On incrémente de 1
                trouCourant.Valeur += 1;

                // On décrémente valeur de 1
                valeur -= 1;
            }

            // Récupération du trou où on a placé la dernière graine
            int indexDernierTrouPlace = indexTrouCourant;

            Boolean onPeutRecolter = true;
            // On collecte les scores
            // - On parcourt la liste dans l'autre sens à partir du dernier trou
            while (onPeutRecolter)
            {
                // On récupère le Trou courant
                Trou trouCourant = ListTrousOrdonnes[indexTrouCourant];

                // Si condition de récolte bonne 
                // - 2 ou 3 graines dans le trou courant
                if (trouCourant.Valeur == 2 || trouCourant.Valeur == 3)
                {
                    // On donne les graines au joueur
                    JoueurCourant.NbGraines += trouCourant.Valeur;

                    // On retire les graines du trou
                    trouCourant.Valeur = 0;

                    // On est au début de la liste 
                    if (indexTrouCourant == 0)
                    {
                        // -> On retourne au début de la liste
                        indexTrouCourant = NbTrous - 1;
                    }
                    else
                    {
                        // On passe au trou suivant
                        indexTrouCourant -= 1;
                    }
                }
                // Si ce n'est pas le cas
                else
                {
                    // On s'arrête la
                    onPeutRecolter = false;
                }
            }

            // Changement de Joueur Courant
            if (JoueurCourant == J1)
            {
                JoueurCourant = J2;
            }
            else
            {
                JoueurCourant = J1;
            }

            // Check si il y'a un vainqueur
            CheckSiVainqueur();
        }

        public void CheckSiVainqueur()
        {
            // Compteur de score
            int nbGrainesRequis = (NbGraines / 2) + 1;
            if (J1.NbGraines >= nbGrainesRequis)
            {
                MessageBox.Show(J1.Nom + " a gagné !!");
                new Lancement().Show();
                SaveScore();
                if (hostGame != null)
                {
                    hostGame.CloseAll();
                }
                else
                {
                    connect.CloseAll();
                };
            }
            if (J2.NbGraines >= nbGrainesRequis)
            {
                MessageBox.Show(J2.Nom + " a gagné !!");
                new Lancement().Show();
                SaveScore();
                if (hostGame != null)
                {
                    hostGame.CloseAll();
                }
                else
                {
                    connect.CloseAll();
                };
            }
        }

        public void SaveScore()
        {
            String chemin = "./save/score.csv";
            // Determine whether the directory exists.
            if (!Directory.Exists("./save"))
            {
                DirectoryInfo di = Directory.CreateDirectory("./save");
            }

            if (!File.Exists(chemin))
            {
                FileStream f = File.Create(chemin);
                f.Close();
            }

            String scores = J1.Nom + ";" + J1.NbGraines + ";" + J2.Nom + ";" + J2.NbGraines;

            // Ajout du score final dans le fichier
            File.AppendAllText(chemin, scores + Environment.NewLine);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
