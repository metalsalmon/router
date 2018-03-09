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
        int lb_adaptery_index { get; set; }
        int lb_arp_zaznam_index { get; set; }
        string lb_arp_zaznam { get; set; }
        string arp { get; set; }
        int casovac { get; set; }
        void vymaz_arp();
    }
}
