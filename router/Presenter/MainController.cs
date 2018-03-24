using PacketDotNet;
using router.Model;
using router.View;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace router.Presenter
{

    class MainController
    {
        IView main_view;
        public Rozhranie rozhranie1 { get; set; }
        public Rozhranie rozhranie2 { get; set; }
        List<ICaptureDevice> zoznam_adapterov;
        public CaptureDeviceList adaptery { get; set; }
        public List<Arp> arp_tabulka;
        public ARPPacket arp_packet { get; set; }
        public bool zastav_vlakno { get; set; }
        private Arp arp;
        public int arp_casovac { get; set; }
        public bool zmaz_arp_tabulku { get; set; }
        public List<Smerovaci_zaznam> smerovacia_tabulka { get; set; }

        public MainController(IView view)
        {
            main_view = view;
            adaptery = CaptureDeviceList.Instance;
            zoznam_adapterov = new List<ICaptureDevice>();
            zobraz_sietove_adaptery();
            arp_tabulka = new List<Arp>();
            smerovacia_tabulka = new List<Smerovaci_zaznam>();
            zastav_vlakno = false;
            arp_casovac = 50;
            zmaz_arp_tabulku = false;
        }

        public void zobraz_sietove_adaptery()
        {
            foreach (ICaptureDevice adapter in adaptery)
            {
                zoznam_adapterov.Add(adapter);
                main_view.adaptery = adapter.Description;
            }
        }

        public Rozhranie nastav_ip(Rozhranie rozhranie)
        {
            rozhranie = new Rozhranie(zoznam_adapterov[main_view.adaptery_index], main_view.ip_adresa, main_view.maska);
            return rozhranie;
        }

        public void pocuvaj_rozhranie1(Rozhranie rozhranie)
        {
            rozhranie.adapter.OnPacketArrival += new PacketArrivalEventHandler(zachytenie_rozhranie1);
            rozhranie.adapter.Open(DeviceMode.Promiscuous);
            rozhranie.adapter.StartCapture();
        }

        public void pocuvaj_rozhranie2(Rozhranie rozhranie)
        {
            rozhranie.adapter.OnPacketArrival += new PacketArrivalEventHandler(zachytenie_rozhranie2);
            rozhranie.adapter.Open(DeviceMode.Promiscuous);
            rozhranie.adapter.StartCapture();
        }

        public void zachytenie_rozhranie1(object sender, CaptureEventArgs e)
        {
            Packet paket = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            zachytenie(paket,1);
            
        }
        public void zachytenie_rozhranie2(object sender, CaptureEventArgs e)
        {
            Packet paket = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            zachytenie(paket,2);
        }

        public void zachytenie(Packet paket, int rozhranie)
        {
            if (paket is EthernetPacket)
            {
                EthernetPacket eth = (EthernetPacket)paket;

                if (rozhranie1 != null && rozhranie2 !=null)
                {
                    if (rozhranie==1 && (eth.DestinationHwAddress.ToString().Equals(rozhranie1.adapter.MacAddress.ToString()) || eth.DestinationHwAddress.ToString().Equals("FFFFFFFFFFFF")))
                    {
                        analyzuj(rozhranie1, eth, paket);
                    }

                    if (rozhranie==2 && (eth.DestinationHwAddress.ToString().Equals(rozhranie2.adapter.MacAddress.ToString()) || eth.DestinationHwAddress.ToString().Equals("FFFFFFFFFFFF")))
                    {
                        analyzuj(rozhranie2, eth, paket);
                    }
                }
            }
        }
        public void analyzuj(Rozhranie rozhranie, EthernetPacket eth, Packet paket)
        {
            if (eth.Type.ToString().Equals("Arp"))
            {
                IPAddress odosielatel_adres = new IPAddress(paket.Bytes.Skip(28).Take(4).ToArray());
                IPAddress ciel_adres = new IPAddress(paket.Bytes.Skip(38).Take(4).ToArray());

                if (ciel_adres.ToString().Equals("255.255.255.255") || ciel_adres.ToString().Equals(rozhranie.ip_adresa))
                {
                    if (paket.Bytes[21] == 1)
                    {
                        arp_reply(eth, rozhranie, odosielatel_adres,ciel_adres);  //dosla mi request tak odpovedam
                    }
                    else if (paket.Bytes[21] == 2 && !eth.SourceHwAddress.ToString().Equals("02004C4F4F50"))  //IGNORUJEM LOOPBACK
                    {
                        bool pridaj_arp_zaznam = true;
                        arp = new Arp(odosielatel_adres.ToString(), eth.SourceHwAddress.ToString(), arp_casovac);

                        foreach (var zaznam in arp_tabulka)
                        {
                            if (zaznam.ip.Equals(arp.ip) && zaznam.mac.Equals(arp.mac))
                            {
                                zaznam.casovac = arp_casovac;
                                pridaj_arp_zaznam = false;
                            }
                        }

                        if (pridaj_arp_zaznam)
                        {
                            arp_tabulka.Add(arp);
                        }
                    }
                }
                else 
                {
                    if (paket.Bytes[21] == 1)
                    {
                        Smerovaci_zaznam naslo_zaznam = null;
                        naslo_zaznam = najdi_zaznam_v_smerovacej_tabulke(ciel_adres);
                        if(naslo_zaznam != null) arp_reply(eth, rozhranie, odosielatel_adres, ciel_adres);
                    }
                }
            }

            else if (!eth.Type.ToString().Equals("Arp"))
            {
                IPAddress odosielatel_address = new IPAddress(paket.Bytes.Skip(26).Take(4).ToArray());
                IPAddress ciel_adres = new IPAddress(paket.Bytes.Skip(30).Take(4).ToArray());
                Smerovaci_zaznam smerovaci_zaznam = null;
                string via = null;

                smerovaci_zaznam = najdi_zaznam_v_smerovacej_tabulke(ciel_adres);

                if (smerovaci_zaznam != null && smerovaci_zaznam.exit_interface == -1)
                {
                    smerovaci_zaznam = rekurzivne_prehladanie(smerovaci_zaznam,ref via);
                }

                if (smerovaci_zaznam != null)
                { 
                    if (smerovaci_zaznam.exit_interface == 1) rozhranie = rozhranie1;
                    if (smerovaci_zaznam.exit_interface == 2) rozhranie = rozhranie2;

                    Thread posielanie = new Thread(() => preposli(rozhranie, eth, smerovaci_zaznam, via, ciel_adres));
                    posielanie.Start();
                }
            }
            if (zastav_vlakno) rozhranie.adapter.Close();
        }

        public Smerovaci_zaznam rekurzivne_prehladanie(Smerovaci_zaznam smerovaci_zaznam ,ref string via)
        {
            while (true)
            {
                if (smerovaci_zaznam.exit_interface == -1)
                {
                    via = smerovaci_zaznam.next_hop;
                    smerovaci_zaznam = najdi_zaznam_v_smerovacej_tabulke(IPAddress.Parse(smerovaci_zaznam.next_hop));
                    if (smerovaci_zaznam == null) return null;
                }
                else return smerovaci_zaznam;
            }
        }

        public Smerovaci_zaznam najdi_zaznam_v_smerovacej_tabulke(IPAddress ciel_adres)
        {
            int najdlhsi_prefix = -1;
            Smerovaci_zaznam smerovaci_zaznam = null;

            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
                if (Praca_s_ip.zisti_podsiet(ciel_adres, IPAddress.Parse(zaznam.cielova_siet), IPAddress.Parse(zaznam.maska)))
                {
                    if (najdlhsi_prefix < Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska)))
                    {
                        najdlhsi_prefix = Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska));
                        smerovaci_zaznam = zaznam;
                    }
                }

            }
            if (smerovaci_zaznam != null) return smerovaci_zaznam;
            else return null;
        }

        public void preposli(Rozhranie rozhranie, EthernetPacket eth, Smerovaci_zaznam smerovaci_zaznam, string via, IPAddress ciel_adres)
        {
            bool naslo = false;
            int pokus_arp = 3;

            while (pokus_arp-- > 0)
            {

                foreach (var zaznam in arp_tabulka.ToList())
                {
                    if (zaznam.ip == via || zaznam.ip == ciel_adres.ToString())
                    {
                        naslo = true;
                        eth.DestinationHwAddress = PhysicalAddress.Parse(zaznam.mac);
                        eth.SourceHwAddress = rozhranie.adapter.MacAddress;
                        rozhranie.adapter.SendPacket(eth);
                        pokus_arp = 0;
                        break;
                    }
                }
                if (!naslo)
                {
                    if(via!=null)  main_view.vypis(via, 2);

                    if (via != null) arp_request(rozhranie, IPAddress.Parse(via));
                    else arp_request(rozhranie, ciel_adres);   

                    System.Threading.Thread.Sleep(1000);
                }
            }
            pokus_arp = 3;

        }

        public void zmaz_smerovaci_zaznam()
        {
            smerovacia_tabulka.RemoveAt(main_view.lb_smerovaci_zaznam_index);
        }

        public void updatni_arp_tabulku()
        {
            if (zmaz_arp_tabulku)
            {
                foreach (var zaznam in arp_tabulka.ToList())
                {
                    arp_tabulka.Remove(zaznam);
                }
            }

            foreach (var zaznam in arp_tabulka.ToList())
            {
                zaznam.casovac--;
                main_view.lb_arp_zaznam = zaznam.ip + "         " + zaznam.mac + "       " + zaznam.casovac;

                if (zaznam.casovac <= 0)
                {
                    arp_tabulka.Remove(zaznam);

                }
            }


        }

        public void priamo_pripojena_siet(int rozhranie)
        {
            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
                if (zaznam.typ == "D" && rozhranie == zaznam.exit_interface) smerovacia_tabulka.Remove(zaznam);
            }

            string siet = Praca_s_ip.adresa_siete(IPAddress.Parse(main_view.ip_adresa), IPAddress.Parse(main_view.maska)).ToString();
            Smerovaci_zaznam smerovaci_zaznam = new Smerovaci_zaznam("D", siet, main_view.maska, 1, 0, "X", -1, rozhranie);
            smerovacia_tabulka.Add(smerovaci_zaznam);
            updatni_smerovaciu_tabulku();
        }

        public void updatni_smerovaciu_tabulku()
        {
            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
                if (zaznam.typ == "D") main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + zaznam.cielova_siet + "/" + Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska)) + "       rozhranie: " + zaznam.exit_interface;
                if (zaznam.typ == "S" && zaznam.exit_interface == -1) main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(IPAddress.Parse(zaznam.cielova_siet), IPAddress.Parse(zaznam.maska)) + "/" + Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska)) + " [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.next_hop;
                else if (zaznam.typ == "S" && zaznam.next_hop == "X") main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(IPAddress.Parse(zaznam.cielova_siet), IPAddress.Parse(zaznam.maska)) + "/" + Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska)) + "      [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.exit_interface;
                else if (zaznam.typ == "S" && zaznam.next_hop != "X" && zaznam.exit_interface != -1) main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(IPAddress.Parse(zaznam.cielova_siet), IPAddress.Parse(zaznam.maska)) + "/" + Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska)) + "         [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.next_hop + "     " + zaznam.exit_interface;
            }
        }

        public void pridaj_staticku_cestu(int next_hop)
        {
            Smerovaci_zaznam smerovaci_zaznam = null;
            bool vloz = true;
            if (next_hop == 1) smerovaci_zaznam = new Smerovaci_zaznam("S", main_view.staticke_ip, main_view.staticke_maska, 1, 0, main_view.staticke_next_hop, -1, -1);
            if (next_hop == 2) smerovaci_zaznam = new Smerovaci_zaznam("S", main_view.staticke_ip, main_view.staticke_maska, 1, 0, "X", -1, int.Parse(main_view.staticke_rozhranie));
            if (next_hop == 3) smerovaci_zaznam = new Smerovaci_zaznam("S", main_view.staticke_ip, main_view.staticke_maska, 1, 0, main_view.staticke_next_hop, -1, int.Parse(main_view.staticke_rozhranie));

            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
                if (zaznam.Equals(smerovaci_zaznam)) vloz = false;
            }

            if (vloz) smerovacia_tabulka.Add(smerovaci_zaznam);

            updatni_smerovaciu_tabulku();

        }

        public void arp_request(Rozhranie rozhranie, IPAddress hladana_ip)
        {

            if (rozhranie.adapter.MacAddress != null)
            {
                EthernetPacket ethernet_packet = new EthernetPacket(rozhranie.adapter.MacAddress, PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF"), EthernetPacketType.Arp);
                arp_packet = new ARPPacket(ARPOperation.Request, PhysicalAddress.Parse("00-00-00-00-00-00"), hladana_ip, rozhranie.adapter.MacAddress, IPAddress.Parse(rozhranie.ip_adresa));

                ethernet_packet.PayloadPacket = arp_packet;
                rozhranie.adapter.SendPacket(ethernet_packet);
            }
        }

        public void arp_reply(EthernetPacket eth, Rozhranie rozhranie, IPAddress odosielatel_address, IPAddress ciel_adres)
        {
            if (rozhranie.adapter.MacAddress != null)
            {
                EthernetPacket ethernet_packet = new EthernetPacket(rozhranie.adapter.MacAddress, eth.SourceHwAddress, EthernetPacketType.Arp);
               arp_packet = new ARPPacket(ARPOperation.Response, eth.SourceHwAddress, odosielatel_address, rozhranie.adapter.MacAddress, ciel_adres);
            
                ethernet_packet.PayloadPacket = arp_packet;
                rozhranie.adapter.SendPacket(ethernet_packet);
            }
        }

    }
}