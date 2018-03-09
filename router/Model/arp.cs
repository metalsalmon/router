using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace router.Model
{
    class Arp
    {
        public string ip { get; set; }
        public string mac { get; set; }
        public int casovac { get; set; }

        public Arp(string ip, string mac, int casovac)
        {
            this.ip = ip;
            this.mac = mac;
            this.casovac = casovac;

        }
    }
}
