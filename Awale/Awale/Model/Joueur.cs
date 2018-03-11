using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awale.Model
{
    public class Joueur : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public String Nom { get; set; }
        public Boolean IsIA { get; set; }

        private int nbGraines;
        public int NbGraines
        {
            get
            {
                return nbGraines;
            }
            set
            {
                nbGraines = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("nbGraines"));
            }
        }
        public Joueur(String nom, int id)
        {
            Id = id;
            Nom = nom;
        }

        public void SetIsIA(Boolean isIA)
        {
            IsIA = isIA;

            if(isIA == true)
            {
                // Ajout de IA au nom 
                Nom = Nom + " (IA)";
            }   
        }

        public event PropertyChangedEventHandler PropertyChanged;
        }
}
