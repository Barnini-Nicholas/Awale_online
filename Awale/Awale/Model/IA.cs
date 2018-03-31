using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awale.Model
{
    class IA
    {
        public static Trou ChoisirAction(Joueur joueur, List<Trou> listTrous)
        {
            // temp d'un trou
            Trou tempTrou = null;

            // Parcours de la liste des trous ...
            foreach(Trou trou in listTrous)
            {
                // Si c'est un trou à nous
                if (trou.Joueur.Equals(joueur))
                {
                    if(tempTrou == null)
                    {
                        tempTrou = trou;
                    }

                    if(trou.Valeur > tempTrou.Valeur)
                    {
                        tempTrou = trou;
                    }
                }
            }

            return tempTrou;
        }
    }
}
