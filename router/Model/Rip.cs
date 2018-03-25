using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace router.Model
{
    class Rip
    {
        public string typ { get; set; }
        public IPAddress cielova_siet { get; set; }
        public IPAddress maska { get; set; }
        public int ad { get; set; }
        public int metrika { get; set; }
        public IPAddress next_hop { get; set; }
        public int update { get; set; }
        public int invalid { get; set; }
        public int holddown { get; set; }
        public int flush { get; set; }
        public int cislo_rozhrania { get; set; }


        public Rip(string typ, IPAddress cielova_siet, IPAddress maska, int ad, int metrika, IPAddress next_hop, int update, int invalid, int holddown, int flush,int cislo_rozhrania)
        {
            this.typ = typ;
            this.cielova_siet = cielova_siet;
            this.maska = maska;
            this.ad = ad;
            this.metrika = metrika;
            this.next_hop = next_hop;

            this.update = update;
            this.invalid = invalid;
            this.holddown = holddown;
            this.flush = flush;
            this.cislo_rozhrania = cislo_rozhrania;
        }
       
    }
}
