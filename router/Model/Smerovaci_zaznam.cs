using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace router.Model
{
    class Smerovaci_zaznam
    {
        public string typ { get; set; }
        public string cielova_siet { get; set; }
        public string maska { get; set; }
        public int ad { get; set; }
        public int metrika { get; set; }
        public string next_hop { get; set; }
        public int casovac { get; set; }
        public int exit_interface { get; set; }


        public Smerovaci_zaznam(string typ, string cielova_siet, string maska, int ad, int metrika, string next_hop, int casovac, int exit_interface)
        {
            this.typ = typ;
            this.cielova_siet = cielova_siet;
            this.maska = maska;
            this.ad = ad;
            this.metrika = metrika;
            this.next_hop = next_hop;
            this.casovac = casovac;
            this.exit_interface = exit_interface;
        }

        public override bool Equals(object obj)
        {
            var porovnavaci = obj as Smerovaci_zaznam;

            if (porovnavaci == null)
                return false;

            if (typ != porovnavaci.typ || cielova_siet != porovnavaci.cielova_siet || maska!= porovnavaci.maska || ad!= porovnavaci.ad || metrika!= porovnavaci.metrika || next_hop!= porovnavaci.next_hop
                || casovac!=porovnavaci.casovac || exit_interface!= porovnavaci.exit_interface)
                return false;

            return true;
        }


    }
}
