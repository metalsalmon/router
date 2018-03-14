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
        public Rozhranie rozhranie1 {get; set;}
        public Rozhranie rozhranie2 {get; set;}
        List<ICaptureDevice> zoznam_adapterov;
        public CaptureDeviceList adaptery { get; set; }
        private byte[] byteData = new byte[4096];
        private byte[] receiveBuffer = new byte[4096];
        Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
        public List<Arp> arp_tabulka;
        public ARPPacket arp_packet { get; set; }
        //public EthernetPacket eth { get; set; }
        public IPAddress odosielatel_address, ciel_address;
        public int omg = 5; 
        private bool pridaj_arp_zaznam = true;
        public bool zastav_vlakno { get; set; }
        private Arp arp;
        public int casovac {get;set;}
        public bool zmaz_arp_tabulku { get; set; }
        public List<Smerovaci_zaznam> smerovacia_tabulka { get; set; }
        private Smerovaci_zaznam smerovaci_zaznam;
      //  private Packet paket;

        public MainController(IView view)
        {
            main_view = view;
            adaptery = CaptureDeviceList.Instance;
            zoznam_adapterov = new List<ICaptureDevice>();
            zobraz_sietove_adaptery();
            arp_tabulka = new List<Arp>();
            smerovacia_tabulka = new List<Smerovaci_zaznam>();
            zastav_vlakno = false;
            casovac = 50;
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

        public void pocuvaj(Rozhranie rozhranie)
        {
            rozhranie.adapter.OnPacketArrival +=new PacketArrivalEventHandler(zachytenie);
            rozhranie.adapter.Open(DeviceMode.Promiscuous);
            rozhranie.adapter.StartCapture();                     
        }

        public void zachytenie(object sender, CaptureEventArgs e) 
        {
            Packet paket = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            if (paket is EthernetPacket)
            {
                EthernetPacket eth = ((EthernetPacket)paket);

                if (rozhranie1 != null)
                {
                    if (eth.DestinationHwAddress.ToString().Equals(rozhranie1.adapter.MacAddress.ToString()) || eth.DestinationHwAddress.ToString().Equals("FFFFFFFFFFFF"))
                    {
                        if (rozhranie2 != null)   analyzuj(rozhranie1, eth, paket);
                    }
                }

                if (rozhranie2 != null)
                {
                    if (eth.DestinationHwAddress.ToString().Equals(rozhranie2.adapter.MacAddress.ToString()) || eth.DestinationHwAddress.ToString().Equals("FFFFFFFFFFFF"))
                    {
                       if(rozhranie1!=null) analyzuj(rozhranie2, eth, paket);
                    }
                }
            }
        }
        public void analyzuj(Rozhranie rozhranie, EthernetPacket eth, Packet paket)
        {
            if (eth.Type.ToString().Equals("Arp"))

            {
                odosielatel_address = new IPAddress(paket.Bytes.Skip(28).Take(4).ToArray());
                ciel_address = new IPAddress(paket.Bytes.Skip(38).Take(4).ToArray());

                if (ciel_address.ToString().Equals("255.255.255.255") || ciel_address.ToString().Equals(rozhranie.ip_adresa))
                {


                    if (paket.Bytes[21] == 1)
                    {

                        arp_reply(eth, rozhranie);  //dosla mi request tak odpovedam
                    }
                    else if (paket.Bytes[21] == 2)
                    {
                        arp = new Arp(odosielatel_address.ToString(), eth.SourceHwAddress.ToString(), casovac);

                        foreach (var zaznam in arp_tabulka)
                        {
                            if (zaznam.ip.Equals(arp.ip) && zaznam.mac.Equals(arp.mac))
                            {
                                zaznam.casovac = casovac;
                                pridaj_arp_zaznam = false;
                            }
                        }

                        if (pridaj_arp_zaznam)
                        {
                           // main_view.lb_arp_zaznam = arp.ip + "      " + arp.mac + "    " + arp.casovac;
                       //     string text=arp.ip + "      " + arp.mac + "    " + arp.casovac;
                        //    main_view.vypisomg(text);
                            arp_tabulka.Add(arp);
                         //   updatni_arp_tabulku();
                        }
                        pridaj_arp_zaznam = true;
                    }
                }
            }

            else if (!eth.Type.ToString().Equals("Arp"))
            {
                odosielatel_address = new IPAddress(paket.Bytes.Skip(26).Take(4).ToArray());
                ciel_address = new IPAddress(paket.Bytes.Skip(30).Take(4).ToArray());

                Smerovaci_zaznam smerovaci_zaznam = null;
                int najdlhsi_prefix = 0;

                foreach (var zaznam in smerovacia_tabulka.ToList())
                {

                    if (Praca_s_ip.zisti_podsiet(ciel_address, IPAddress.Parse(zaznam.cielova_siet), IPAddress.Parse(zaznam.maska)))
                    {
                        main_view.vypis("true",1);
                        if (najdlhsi_prefix < Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska)))
                        {
                            najdlhsi_prefix = Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska));
                            main_view.vypis("prefix: ",najdlhsi_prefix);
                            main_view.vypis(zaznam.cielova_siet, 55);
                            main_view.vypis(ciel_address.ToString(), 55);
                            main_view.vypis("*--------------------", 0);
                            smerovaci_zaznam = zaznam;
                        }
                    }
                }
               
                if (smerovaci_zaznam != null)
                {
                    main_view.vypis(smerovaci_zaznam.cielova_siet, -1);

                    if (rozhranie == rozhranie1) rozhranie = rozhranie2;
                    else rozhranie = rozhranie1;
                    Thread posielanie = new Thread(() => preposli(rozhranie, eth, smerovaci_zaznam));
                    posielanie.Start();

                }

            }
            if (zastav_vlakno) rozhranie.adapter.Close();
        }

        public void preposli(Rozhranie rozhranie, EthernetPacket eth, Smerovaci_zaznam smerovaci_zaznam)
        {
            bool naslo = false;
            int pokus_arp = 3;

            while (pokus_arp-- > 0)
            {

                foreach (var zaznam in arp_tabulka.ToList())
                {
                    if (zaznam.ip == smerovaci_zaznam.cielova_siet || zaznam.ip == smerovaci_zaznam.next_hop)
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
                        if(smerovaci_zaznam.typ!="D") arp_request(rozhranie, IPAddress.Parse(smerovaci_zaznam.next_hop));
                        else arp_request(rozhranie, IPAddress.Parse(smerovaci_zaznam.cielova_siet));

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
                if(zaznam.typ=="D" && rozhranie==zaznam.exit_interface) smerovacia_tabulka.Remove(zaznam);
            }


            string siet = Praca_s_ip.adresa_siete(IPAddress.Parse(main_view.ip_adresa), IPAddress.Parse(main_view.maska)).ToString();
            smerovaci_zaznam = new Smerovaci_zaznam("D",siet,main_view.maska,1,0,"X",-1,rozhranie);
            smerovacia_tabulka.Add(smerovaci_zaznam);
            updatni_smerovaciu_tabulku();
        }

        public void updatni_smerovaciu_tabulku()
        {
            foreach (var zaznam in smerovacia_tabulka.ToList())
            {
               if(zaznam.typ=="D") main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + zaznam.cielova_siet+"/"+Praca_s_ip.sprav_masku(IPAddress.Parse(zaznam.maska)) + "       rozhranie: " + zaznam.exit_interface;
                if (zaznam.typ == "S" && zaznam.exit_interface==-1) main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(IPAddress.Parse(zaznam.cielova_siet), IPAddress.Parse(zaznam.maska)) + " [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.next_hop;
                else if (zaznam.typ == "S" && zaznam.next_hop == "X") main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(IPAddress.Parse(zaznam.cielova_siet), IPAddress.Parse(zaznam.maska)) + "      [" +zaznam.ad+"/"+zaznam.metrika+"]       cez: " + zaznam.exit_interface;
                else if (zaznam.typ == "S" && zaznam.next_hop != "X" && zaznam.exit_interface!=-1) main_view.lb_smerovacia_tabulka = zaznam.typ + "         " + Praca_s_ip.adresa_siete(IPAddress.Parse(zaznam.cielova_siet), IPAddress.Parse(zaznam.maska)) + "         [" + zaznam.ad + "/" + zaznam.metrika + "]       cez: " + zaznam.next_hop + "     " + zaznam.exit_interface;
            }
            
        }

        public void pridaj_staticku_cestu(int next_hop)
        {
            bool vloz = true;
            if (next_hop == 1) smerovaci_zaznam = new Smerovaci_zaznam("S",main_view.staticke_ip,main_view.staticke_maska,1,0,main_view.staticke_next_hop,-1,-1);
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

        public void arp_reply(EthernetPacket eth, Rozhranie rozhranie)
        {
            if (rozhranie.adapter.MacAddress != null)
            {
                EthernetPacket ethernet_packet = new EthernetPacket(rozhranie.adapter.MacAddress, eth.SourceHwAddress, EthernetPacketType.Arp);
                arp_packet = new ARPPacket(ARPOperation.Response, eth.SourceHwAddress, IPAddress.Parse(odosielatel_address.ToString()), rozhranie.adapter.MacAddress, IPAddress.Parse(rozhranie.ip_adresa));

                ethernet_packet.PayloadPacket = arp_packet;
                rozhranie.adapter.SendPacket(ethernet_packet);
            }
        }

    }
}
