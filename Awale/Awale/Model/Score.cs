using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awale.Model
{
    public class Score
    {
        public String J1 { get; set; }
        public String ScoreJ1 { get; set; }
        public String J2 { get; set; }
        public String ScoreJ2 { get; set; }

        public Score(String line)
        {
            String[] elements = line.Split(';');
            J1 = elements[0];
            ScoreJ1 = elements[1];
            J2 = elements[2];
            ScoreJ2 = elements[3];
        }
    }
}
