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

            // Parcours de la liste des trous ...
            foreach(Trou trou in listTrous)
            {
                // Si c'est un trou à nous
                if (trou.Joueur.Equals(joueur))
                {
                    if(trou.Valeur > 0)
                    {
                        return trou;
                    }
                }
            }

            return null;
        }
    }
}
