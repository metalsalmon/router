using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace router.Model
{
    class Rozhranie
    {
        public string ip_adresa { get; set; }
        public string maska { get; set; }
        public ICaptureDevice adapter { get; set; }
        public int cislo_rozhrania { get; set; }

        public Rozhranie(ICaptureDevice adapter, string ip_adresa, string maska, int cislo_rozhrania)
        {
            this.ip_adresa = ip_adresa;
            this.maska = maska;
            this.adapter = adapter;
            this.cislo_rozhrania = cislo_rozhrania;
            

        }
        
        public void nastav_IP(string ip, string maska,ICaptureDevice adapter, int cislo_rozhrania)
        {
            this.ip_adresa = ip;
            this.maska = maska;
            this.adapter = adapter;
            this.cislo_rozhrania = cislo_rozhrania;
        }

    }
}
