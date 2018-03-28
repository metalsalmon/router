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
        public List<Smerovaci_zaznam> rip_databaza { get; set; }
        public int update_casovac_hodnota { get; set; }
        public int update_casovac { get; set; }
        public int holddown_casovac { get; set; }
        public int flush_casovac { get; set; }
        public int invalid_casovac { get; set; }
        public bool povolene_rip1 { get; set; }
        public bool povolene_rip2 { get; set; }

        public MainController(IView view)
        {
            main_view = view;
            adaptery = CaptureDeviceList.Instance;
            zoznam_adapterov = new List<ICaptureDevice>();
            zobraz_sietove_adaptery();
            arp_tabulka = new List<Arp>();
            smerovacia_tabulka = new List<Smerovaci_zaznam>();
            rip_databaza = new List<Smerovaci_zaznam>();
            zastav_vlakno = false;
            arp_casovac = 50;
            zmaz_arp_tabulku = false;

            update_casovac_hodnota = 30;
            update_casovac = 0;
            holddown_casovac = 180;
            flush_casovac = 240;
            invalid_casovac = 180;
        }

        public void zobraz_sietove_adaptery()
        {
            foreach (ICaptureDevice adapter in adaptery)
            {
                zoznam_adapterov.Add(adapter);
                main_view.adaptery = adapter.Description;
            }
        }

        public Rozhranie nastav_ip(Rozhranie rozhranie,int cislo_rozhrania)
        {
            rozhranie = new Rozhranie(zoznam_adapterov[main_view.adaptery_index], main_view.ip_adresa, main_view.maska, cislo_rozhrania);
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
                    if (rozhranie==1 && (!eth.SourceHwAddress.ToString().Equals(rozhranie1.adapter.MacAddress.ToString()))&&(eth.DestinationHwAddress.ToString().Equals(rozhranie1.adapter.MacAddress.ToString()) || eth.DestinationHwAddress.ToString().Equals("FFFFFFFFFFFF") || eth.DestinationHwAddress.ToString().Equals("01005E000009")))
                    {

                        analyzuj(rozhranie1, eth, paket);
                    }

                    if (rozhranie==2 && (!eth.SourceHwAddress.ToString().Equals(rozhranie2.adapter.MacAddress.ToString())) && (eth.DestinationHwAddress.ToString().Equals(rozhranie2.adapter.MacAddress.ToString()) || eth.DestinationHwAddress.ToString().Equals("FFFFFFFFFFFF") || eth.DestinationHwAddress.ToString().Equals("01005E000009")))
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
                    if (paket.Bytes[21] == 1)       // PROXY ARP
                    {
                        Smerovaci_zaznam naslo_zaznam = null;
                        naslo_zaznam = najdi_zaznam_v_smerovacej_tabulke(ciel_adres);
                        if (naslo_zaznam != null && naslo_zaznam.exit_interface == -1)
                        {
                            string via = null;
                            naslo_zaznam = rekurzivne_prehladanie(naslo_zaznam, ref via);
                        }
                        if (naslo_zaznam != null && naslo_zaznam.exit_interface != -1 && (rozhranie.cislo_rozhrania!=naslo_zaznam.exit_interface))
                            arp_reply(eth, rozhranie, odosielatel_adres, ciel_adres);
                    }
                }
            }
            else if(eth.Type.ToString().Equals("IpV4"))
            {
                  if ((--paket.Bytes[22]) > 0) {
                
                    IPAddress odosielatel_address = new IPAddress(paket.Bytes.Skip(26).Take(4).ToArray());
                    IPAddress ciel_adres = new IPAddress(paket.Bytes.Skip(30).Take(4).ToArray());
                    Smerovaci_zaznam smerovaci_zaznam = null;
                    string via = null;

                   IPv4Packet ip_pak = (IPv4Packet)eth.PayloadPacket;
                    ip_pak.UpdateIPChecksum();
               
                if ((ciel_adres.ToString()==rozhranie.ip_adresa || ciel_adres.ToString() =="224.0.0.9") && ((int)eth.Bytes[43] == 2)) spracuj_rip(eth, paket,rozhranie,odosielatel_address);
                else
                {

                    smerovaci_zaznam = najdi_zaznam_v_smerovacej_tabulke(ciel_adres);

                    if (smerovaci_zaznam != null && smerovaci_zaznam.exit_interface == -1)
                    {
                        smerovaci_zaznam = rekurzivne_prehladanie(smerovaci_zaznam, ref via);
                    }

                    if (smerovaci_zaznam != null && smerovaci_zaznam.exit_interface != -1 && smerovaci_zaznam.next_hop != "X")
                    {
                        via = smerovaci_zaznam.next_hop;
                    }

                    if (smerovaci_zaznam != null)
                    {
                        if (smerovaci_zaznam.exit_interface == 1) rozhranie = rozhranie1;
                        if (smerovaci_zaznam.exit_interface == 2) rozhranie = rozhranie2;

                        Thread posielanie = new Thread(() => preposli(rozhranie, eth, smerovaci_zaznam, via, ciel_adres));
                        posielanie.Start();
                    }
                }
                }
            }
            if (zastav_vlakno) rozhranie.adapter.Close();
        }

        public void spracuj_rip(EthernetPacket eth,Packet packet,Rozhranie rozhranie, IPAddress oznamovatel)
        {
            Smerovaci_zaznam rip_zaznam;
            int dlzka= eth.Bytes.Length - 46;
            int pocet = dlzka / 20;
            int cislo_rozhrania;
            bool pridaj_do_databazy = true;
            IPAddress adresa_siete_rip;
            IPAddress maska_rip;
            bool pridaj_zaznam = true;
            int slash_maska;
            int posunutie_ip = 0,posunutie_maska=0,posunutie_metrika=0;

            if (rozhranie == rozhranie1) cislo_rozhrania = 1;
            else cislo_rozhrania = 2;
            if((cislo_rozhrania==1 && povolene_rip1) ||(cislo_rozhrania == 2 && povolene_rip2))
            if ((int)eth.Bytes[42]==2) {
                while ((pocet--) > 0)
                {
                    
                    adresa_siete_rip = Praca_s_ip.adresa_siete(new IPAddress(eth.Bytes.Skip(50 + posunutie_ip).Take(4).ToArray()), new IPAddress(eth.Bytes.Skip(54 + posunutie_maska).Take(4).ToArray()));
                    maska_rip = new IPAddress(eth.Bytes.Skip(54 + posunutie_maska).Take(4).ToArray());
                    pridaj_zaznam = true;
                    slash_maska = Praca_s_ip.sprav_masku(maska_rip);

                    rip_zaznam = new Smerovaci_zaznam("R", adresa_siete_rip, maska_rip, 120, (int)eth.Bytes[65 + posunutie_metrika], oznamovatel.ToString(),
                                                         cislo_rozhrania, 0, 0, 0);
                    
                    foreach (var zaznam in smerovacia_tabulka.ToList())
                    {
                        if (adresa_siete_rip.Equals(Praca_s_ip.adresa_siete(zaznam.cielova_siet, zaznam.maska)))
                        {
                            if ((slash_maska == Praca_s_ip.sprav_masku(zaznam.maska))) //prejdem tabulkou a hladam ci je lepsia ako vsetky co tam su doteraz
                            {
                                if (rip_zaznam.metrika == 16)
                                {
                                    if (zaznam.typ == "R")
                                    {
                                        zaznam.metrika = 16;
                                        if (najdi_najlepsiu_v_databaze(zaznam.cielova_siet,zaznam.maska) != null) smerovacia_tabulka.Add(najdi_najlepsiu_v_databaze(zaznam.cielova_siet,zaznam.maska));
                                            //trigger_update(cislo_rozhrania,zaznam);
                                        smerovacia_tabulka.Remove(zaznam);
                                        pridaj_zaznam = false;
                                        break;
                                    }
                                    else
                                    {
                                        pridaj_zaznam = false;
                                        break;
                                    }
                                }
                                if (rip_zaznam.metrika >= zaznam.metrika)
                                {
                                    pridaj_zaznam = false;
                                    break;
                                }
                                else
                                {
                                    smerovacia_tabulka.Remove(zaznam);
                                        break;
                                }

                            }
                        }
                    }
                     //   bool pridavaj = true;

                    foreach (var zaznam in rip_databaza.ToList())
                    {
                        if (adresa_siete_rip.Equals(zaznam.cielova_siet) && zaznam.maska.Equals(rip_zaznam.maska) && (zaznam.next_hop==rip_zaznam.next_hop)) // && zaznam.metrika == rip_zaznam.metrika)
                        {
                            pridaj_do_databazy = false;
                            if (rip_zaznam.metrika == 16)
                            {
                                zaznam.metrika = 16;
                                pridaj_zaznam = false;
                                continue;
                            }
                            else if (rip_zaznam.metrika != zaznam.metrika)
                            {
                                    zaznam.nastav_casovace(0, 0, 0);
                                    pridaj_do_databazy = true;
                            }
                            else if (rip_zaznam.metrika == zaznam.metrika)
                                {
                                    zaznam.nastav_casovace(0, 0, 0);
                                    pridaj_do_databazy = false;
                                    break;
                                }
                          
                        }

                         /*   if (adresa_siete_rip.Equals(zaznam.cielova_siet) && zaznam.maska.Equals(rip_zaznam.maska) && rip_zaznam.exit_interface != zaznam.exit_interface)
                                pridaj_do_databazy = false; */

                    }

                    if (pridaj_zaznam && rip_zaznam.metrika!=16)
                    {
                        smerovacia_tabulka.Add(rip_zaznam);
                    }
                    pridaj_zaznam = true;

                    if (pridaj_do_databazy)
                    {
                        rip_databaza.Add(rip_zaznam);
                    }
                    pridaj_do_databazy = true;

                    posunutie_ip = posunutie_maska = posunutie_metrika = posunutie_ip + 20;
                }
            }else if ((int)eth.Bytes[42] == 1)
            {
                posli_update(rozhranie,cislo_rozhrania,oznamovatel,eth.SourceHwAddress);
            }
                   
        }

        public Smerovaci_zaznam najdi_najlepsiu_v_databaze(IPAddress adresa_siete,IPAddress maska)
        {
            Smerovaci_zaznam smerovaci_zaznam=null;
            int najlepsia = 99;

            foreach (var zaznam in rip_databaza.ToList())
            {
                if (zaznam.cielova_siet.Equals(adresa_siete) && zaznam.maska.Equals(maska))
                {
                    if (zaznam.metrika < najlepsia)
                    {
                        najlepsia = zaznam.metrika;
                        smerovaci_zaznam = zaznam;
                    }
                }          

            }
            if (smerovaci_zaznam == null) return null;
            else   return smerovaci_zaznam;
        }

        public void rip_request(int cislo_rozhrania)
        {
            IPv4Packet ip_paket = null ;
            UdpPacket udp_paket=null;
            EthernetPacket eth = null;

            Byte[] rip_request = new byte[] { 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10};

            if (cislo_rozhrania == 1)
            {
                eth = new EthernetPacket(rozhranie1.adapter.MacAddress, PhysicalAddress.Parse("01005E000009"), EthernetPacketType.IpV4);
                ip_paket = new IPv4Packet(IPAddress.Parse(rozhranie1.ip_adresa), IPAddress.Parse("224.0.0.9"));
            }

            if (cislo_rozhrania == 2)
            {
                eth = new EthernetPacket(rozhranie2.adapter.MacAddress, PhysicalAddress.Parse("01005E000009"), EthernetPacketType.IpV4);
                ip_paket = new IPv4Packet(IPAddress.Parse(rozhranie2.ip_adresa), IPAddress.Parse("224.0.0.9"));
            }
            
            ip_paket.TimeToLive = 2;
            udp_paket = new UdpPacket(520, 520);

            udp_paket.PayloadData = rip_request;
           // udp_paket.UpdateUDPChecksum();

            ip_paket.PayloadPacket = udp_paket;

            ip_paket.UpdateIPChecksum();

            eth.PayloadPacket = ip_paket;

            if (cislo_rozhrania == 1) rozhranie1.adapter.SendPacket(eth);
            if (cislo_rozhrania == 2) rozhranie2.adapter.SendPacket(eth);


        }
        public void trigger_update(int cislo_rozhrania,Smerovaci_zaznam zaznam)
        {
            IPv4Packet ip_paket;
            UdpPacket udp_paket;
            Byte[] ip;
            Byte[] hlava = new byte[] { 0x00, 0x02, 0x00, 0x00 };
            Byte[] rip_hlava = new byte[] { 0x02, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00 };
            Byte[] next_hop = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            Byte[] metrika = new byte[] { 0x00, 0x00, 0x00, 0x10 };
            Rozhranie rozhranie;

            if (cislo_rozhrania == 1) rozhranie = rozhranie2;
            else rozhranie = rozhranie1;

            EthernetPacket eth = new EthernetPacket(rozhranie.adapter.MacAddress, PhysicalAddress.Parse("01005E000009"), EthernetPacketType.IpV4);
            ip_paket = new IPv4Packet(IPAddress.Parse(rozhranie.ip_adresa), IPAddress.Parse("224.0.0.9"));
            ip_paket.TimeToLive = 2;
            udp_paket = new UdpPacket(520, 520);

            rip_hlava = rip_hlava.Concat(zaznam.cielova_siet.GetAddressBytes()).ToArray();
            rip_hlava = rip_hlava.Concat(zaznam.maska.GetAddressBytes()).ToArray();
            rip_hlava = rip_hlava.Concat(next_hop).ToArray();
           // metrika[3] = (byte)(zaznam.metrika + 1);
            rip_hlava = rip_hlava.Concat(metrika).ToArray();

            udp_paket.PayloadData = rip_hlava;
            // udp_paket.UpdateUDPChecksum();

            ip_paket.PayloadPacket = udp_paket;
            ip_paket.UpdateIPChecksum();

            eth.PayloadPacket = ip_paket;

            rozhranie.adapter.SendPacket(eth);

        }

        public void posli_update(Rozhranie rozhranie, int cislo_rozhrania,IPAddress cielova_ip, PhysicalAddress cielova_mac)
        {
            IPv4Packet ip_paket;
            UdpPacket udp_paket;
            Byte[] ip;
            Byte[] hlava = new byte[] { 0x00, 0x02, 0x00, 0x00};
            Byte[] rip_hlava = new byte[] { 0x02, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00};
            Byte[] next_hop = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            Byte[] metrika = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            bool prvy = true;

            EthernetPacket eth = new EthernetPacket(rozhranie.adapter.MacAddress, cielova_mac, EthernetPacketType.IpV4);
            ip_paket= new IPv4Packet(IPAddress.Parse(rozhranie.ip_adresa),cielova_ip);

            if(cielova_mac.ToString().Equals("01005E000009")) ip_paket.TimeToLive = 2;
            else ip_paket.TimeToLive = 255;

            udp_paket = new UdpPacket(520,520); 

            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
                if ((zaznam.typ == "D" || zaznam.typ == "R") && zaznam.exit_interface!= cislo_rozhrania)
                {
                    if(!prvy) rip_hlava = rip_hlava.Concat(hlava).ToArray();
                    prvy = false;
                    rip_hlava = rip_hlava.Concat(zaznam.cielova_siet.GetAddressBytes()).ToArray();
                    rip_hlava = rip_hlava.Concat(zaznam.maska.GetAddressBytes()).ToArray();
                    rip_hlava = rip_hlava.Concat(next_hop).ToArray();
                    if(zaznam.metrika!=16)metrika[3] = (byte)(zaznam.metrika+1);
                    else metrika[3] = (byte)(zaznam.metrika);
                    metrika[3] = (byte)(zaznam.metrika + 1);
                    rip_hlava = rip_hlava.Concat(metrika).ToArray();
                }
            }

            foreach (var zaznam in rip_databaza.ToList())
            {
                if (zaznam.metrika == 16 && zaznam.exit_interface != cislo_rozhrania)
                {
                    if (!prvy) rip_hlava = rip_hlava.Concat(hlava).ToArray();
                    prvy = false;
                    rip_hlava =rip_hlava.Concat(zaznam.cielova_siet.GetAddressBytes()).ToArray();
                    rip_hlava = rip_hlava.Concat(zaznam.maska.GetAddressBytes()).ToArray();
                    rip_hlava = rip_hlava.Concat(next_hop).ToArray();
                    metrika[3] = (byte)zaznam.metrika;
                    if (zaznam.metrika == 0) metrika[3] = 1;
                     rip_hlava = rip_hlava.Concat(metrika).ToArray();
                }

            }

            udp_paket.PayloadData = rip_hlava;
           // udp_paket.UpdateUDPChecksum();

            ip_paket.PayloadPacket = udp_paket;
            ip_paket.UpdateIPChecksum();

            eth.PayloadPacket = ip_paket;

            rozhranie.adapter.SendPacket(eth);

            
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
                if (Praca_s_ip.zisti_podsiet(ciel_adres, zaznam.cielova_siet, zaznam.maska))
                {
                    if (najdlhsi_prefix < Praca_s_ip.sprav_masku(zaznam.maska))
                    {
                        najdlhsi_prefix = Praca_s_ip.sprav_masku(zaznam.maska);
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

            while (pokus_arp > 0)
            {       
                foreach (var zaznam in arp_tabulka.ToList())
                {
                    if (zaznam.ip == via || (via==null && zaznam.ip == ciel_adres.ToString()))
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
                    if (via != null) arp_request(rozhranie, IPAddress.Parse(via));
                    else arp_request(rozhranie, ciel_adres);

                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        public void zmaz_smerovaci_zaznam()
        {
            Smerovaci_zaznam smerovaci_zaznam = najdi_najlepsiu_v_databaze(smerovacia_tabulka.ElementAt(main_view.lb_smerovaci_zaznam_index).cielova_siet, smerovacia_tabulka.ElementAt(main_view.lb_smerovaci_zaznam_index).maska);

            if (smerovaci_zaznam != null) smerovacia_tabulka.Add(smerovaci_zaznam);

            if (smerovacia_tabulka.ElementAt(main_view.lb_smerovaci_zaznam_index).typ!="R")
            smerovacia_tabulka.RemoveAt(main_view.lb_smerovaci_zaznam_index);
        }

        public void updatni_casovace()
        {
            if (povolene_rip1==true || povolene_rip2==true)
            {
                foreach (var zaznam in rip_databaza.ToList())
                {
                    zaznam.flush++;
                    if (zaznam.flush == flush_casovac)
                    {
                        rip_databaza.Remove(zaznam);
                        smerovacia_tabulka.Remove(zaznam);
                        break;
                    }
                    if (zaznam.invalid == invalid_casovac)
                    {
                        zaznam.metrika = 16;
                        zaznam.holddown++;
                    }
                    else zaznam.invalid++;
                }
                update_casovac++;
                if (update_casovac == update_casovac_hodnota)
                {
                    if(povolene_rip1)posli_update(rozhranie1, 1, IPAddress.Parse("224.0.0.9"), PhysicalAddress.Parse("01005E000009"));
                    if(povolene_rip2)posli_update(rozhranie2, 2, IPAddress.Parse("224.0.0.9"), PhysicalAddress.Parse("01005E000009"));
                    update_casovac = 0;
                }
            }
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

            IPAddress siet = Praca_s_ip.adresa_siete(IPAddress.Parse(main_view.ip_adresa), IPAddress.Parse(main_view.maska));
            Smerovaci_zaznam smerovaci_zaznam = new Smerovaci_zaznam("D", siet, IPAddress.Parse(main_view.maska), 1, 0, "X", rozhranie);
            smerovacia_tabulka.Add(smerovaci_zaznam);
            updatni_smerovaciu_tabulku();
        }

        public void updatni_smerovaciu_tabulku()
        {
            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
                if (zaznam.typ == "R") main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + zaznam.cielova_siet + "/" + Praca_s_ip.sprav_masku(zaznam.maska) + " [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.next_hop + ",  "+(zaznam.invalid)+",   " + zaznam.exit_interface;
                if (zaznam.typ == "D") main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + zaznam.cielova_siet + "/" + Praca_s_ip.sprav_masku(zaznam.maska) + "       rozhranie: " + zaznam.exit_interface;
                if (zaznam.typ == "S" && zaznam.exit_interface == -1) main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(zaznam.cielova_siet, zaznam.maska) + "/" + Praca_s_ip.sprav_masku(zaznam.maska) + " [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.next_hop;
                else if (zaznam.typ == "S" && zaznam.next_hop == "X") main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(zaznam.cielova_siet, zaznam.maska) + "/" + Praca_s_ip.sprav_masku(zaznam.maska) + "      [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.exit_interface;
                else if (zaznam.typ == "S" && zaznam.next_hop != "X" && zaznam.exit_interface != -1) main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(zaznam.cielova_siet, zaznam.maska) + "/" + Praca_s_ip.sprav_masku(zaznam.maska) + "         [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.next_hop + "     " + zaznam.exit_interface;
               
            }
        }

        public void pridaj_staticku_cestu(int next_hop)
        {
            Smerovaci_zaznam smerovaci_zaznam = null;
            IPAddress ip = IPAddress.Parse(main_view.staticke_ip);
            IPAddress maska = IPAddress.Parse(main_view.staticke_maska);

            bool vloz = true;
            if (next_hop == 1) smerovaci_zaznam = new Smerovaci_zaznam("S", ip, maska, 1, 0, main_view.staticke_next_hop, -1);
            if (next_hop == 2) smerovaci_zaznam = new Smerovaci_zaznam("S", ip, maska, 1, 0, "X", int.Parse(main_view.staticke_rozhranie));
            if (next_hop == 3) smerovaci_zaznam = new Smerovaci_zaznam("S", ip, maska, 1, 0, main_view.staticke_next_hop, int.Parse(main_view.staticke_rozhranie));

            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
                if (zaznam.Equals(smerovaci_zaznam)) vloz = false;
                if (Praca_s_ip.adresa_siete(zaznam.cielova_siet, zaznam.maska).Equals(Praca_s_ip.adresa_siete(smerovaci_zaznam.cielova_siet, smerovaci_zaznam.maska)) &&
                    zaznam.maska.Equals(smerovaci_zaznam.maska) && zaznam.metrika > smerovaci_zaznam.metrika) smerovacia_tabulka.Remove(zaznam);
            }
           
            if (vloz) smerovacia_tabulka.Add(smerovaci_zaznam);

            updatni_smerovaciu_tabulku();

        }

        public void vypis_rip_databazku()
        {
            foreach (var zaznam in rip_databaza.ToList())
            {
                main_view.vypis(zaznam.cielova_siet.ToString()+"  ,  "+zaznam.next_hop.ToString()+"   ",zaznam.metrika);
            }
        }

        public void vymaz_rip_zaznamy(int cislo_rozhrania)
        {
            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
                if (zaznam.typ == "R" && zaznam.exit_interface == cislo_rozhrania) smerovacia_tabulka.Remove(zaznam);
            }

            foreach (var zaznam in rip_databaza.ToList())
            {
                if (zaznam.typ == "R" && zaznam.exit_interface == cislo_rozhrania) rip_databaza.Remove(zaznam);
            }

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