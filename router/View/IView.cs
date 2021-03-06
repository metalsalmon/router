﻿using System.Windows.Forms;

namespace router.View
{
    interface IView
    {
        string ip_adresa { get; set; }
        string maska { get; set; }
        string adaptery { get; set; }
        int adaptery_index { get; set; }
        int lb_smerovaci_zaznam_index { get; set; }
        string lb_arp_zaznam { get; set; }
        int casovac { get; set; }
        string lb_smerovacia_tabulka { get; set; }
        string staticke_ip { get; set; }
        string staticke_maska { get; set; }
        string staticke_next_hop { get; set; }
        string staticke_rozhranie { get; set; }
        string lbl_ping { get; set; }
        void vymaz_arp();
        void vypis(string text,int cislo);
        void vymaz_lb_smerovacia_tabulka();
    }
}
