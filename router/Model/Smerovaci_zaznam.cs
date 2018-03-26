using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace router.Model
{
    class Smerovaci_zaznam
    {
        public string typ { get; set; }
        public IPAddress cielova_siet { get; set; }
        public IPAddress maska { get; set; }
        public int ad { get; set; }
        public int metrika { get; set; }
        public string next_hop { get; set; }
        public int exit_interface { get; set; }

        public int invalid { get; set; }
        public int holddown { get; set; }
        public int flush { get; set; }


        public Smerovaci_zaznam(string typ, IPAddress cielova_siet, IPAddress maska, int ad, int metrika, string next_hop, int exit_interface)
        {
            this.typ = typ;
            this.cielova_siet = cielova_siet;
            this.maska = maska;
            this.ad = ad;
            this.metrika = metrika;
            this.next_hop = next_hop;
            this.exit_interface = exit_interface;
        }

        public Smerovaci_zaznam(string typ, IPAddress cielova_siet, IPAddress maska, int ad, int metrika, string next_hop, int exit_interface, int invalid, int holddown, int flush)
        {
            this.typ = typ;
            this.cielova_siet = cielova_siet;
            this.maska = maska;
            this.ad = ad;
            this.metrika = metrika;
            this.next_hop = next_hop;
            this.exit_interface = exit_interface;
            this.invalid = invalid;
            this.holddown = holddown;
            this.flush = flush;
        }

        public void nastav_casovace(int invalid, int holddown, int flush)
        {
            this.invalid = invalid;
            this.holddown = holddown;
            this.flush = flush;
        }

        public override bool Equals(object obj)
        {
            var porovnavaci = obj as Smerovaci_zaznam;

            if (porovnavaci == null)
                return false;

            if (typ != porovnavaci.typ || cielova_siet != porovnavaci.cielova_siet || maska!= porovnavaci.maska || ad!= porovnavaci.ad || metrika!= porovnavaci.metrika || next_hop!= porovnavaci.next_hop
                 || exit_interface!= porovnavaci.exit_interface)
                return false;

            return true;
        }


    }
}
