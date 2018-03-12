using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace router.View
{
    interface IView
    {
        string ip_adresa { get; set; }
        string maska { get; set; }
        string adaptery { get; set; }
        int adaptery_index { get; set; }
        int lb_arp_zaznam_index { get; set; }
        string lb_arp_zaznam { get; set; }
        string arp { get; set; }
        int casovac { get; set; }
        string lb_smerovacia_tabulka { get; set; }
        void vymaz_arp();
        void vypis(string omg);
    }
}
