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
        public string cielova_ip { get; set; }
        public string maska { get; set; }
        public int ad { get; set; }
        public int metrika { get; set; }
        public string next_hop { get; set; }
        public int casovac { get; set; }
        public string exit_interface { get; set; }


        public Smerovaci_zaznam(string typ, string cielova_ip, string maska, int ad, int metrika, string next_hop, int casovac, string exit_interface)
        {
            this.typ = typ;
            this.cielova_ip = cielova_ip;
            this.maska = maska;
            this.ad = ad;
            this.metrika = metrika;
            this.next_hop = next_hop;
            this.casovac = casovac;
            this.exit_interface = exit_interface;
        }



    }
}
