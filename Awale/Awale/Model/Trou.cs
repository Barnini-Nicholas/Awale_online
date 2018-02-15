using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awale.Model
{
    public class Trou : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public Joueur Joueur { get; set; }
        private int valeur;
        public int Valeur
        {
            get
            {
                return valeur;
            }
            set
            {
                valeur = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("valeur"));
            }
        }

        public Trou (Joueur joueur, int id)
        {
            Id = id;
            Joueur = joueur;
            Valeur = 0;
        }


        public override string ToString()
        {
            return "Valeur = " + Valeur;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
