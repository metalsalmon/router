using PacketDotNet;
using router.Model;
using router.View;
using SharpPcap;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

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
      //  private Packet paket;

        public MainController(IView view)
        {
            main_view = view;
            adaptery = CaptureDeviceList.Instance;
            zoznam_adapterov = new List<ICaptureDevice>();
            zobraz_sietove_adaptery();
            arp_tabulka = new List<Arp>();
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
                        analyzuj(rozhranie1, eth, paket);
                    }
                }

                if (rozhranie2 != null)
                {main_view.vypis(eth.DestinationHwAddress.ToString());
                    if (eth.DestinationHwAddress.ToString().Equals(rozhranie2.adapter.MacAddress.ToString()) || eth.DestinationHwAddress.ToString().Equals("FFFFFFFFFFFF"))
                    {
                        analyzuj(rozhranie2, eth, paket);
                    }
                }
            }
        }
        public void analyzuj(Rozhranie rozhranie, EthernetPacket eth, Packet paket)
        {
                    odosielatel_address = new IPAddress(paket.Bytes.Skip(28).Take(4).ToArray());
                    ciel_address = new IPAddress(paket.Bytes.Skip(38).Take(4).ToArray());
                     

                if (eth.Type.ToString().Equals("Arp") && (ciel_address.ToString().Equals("255.255.255.255") || ciel_address.ToString().Equals(rozhranie.ip_adresa)))
                    {
                    
                        if (paket.Bytes[21] == 1)
                        {
                            
                            arp_reply(eth,rozhranie);  //dosla mi request tak odpovedam
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
                            main_view.lb_arp_zaznam = arp.ip + "      " + arp.mac + "    " + arp.casovac;
                            arp_tabulka.Add(arp);
                        }
                        pridaj_arp_zaznam = true;
                    }
                   }

            if (zastav_vlakno) rozhranie.adapter.Close();
        }

        public void zmaz_arp_zaznam()
        {  
            arp_tabulka.RemoveAt(main_view.lb_arp_zaznam_index);
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

        public void arp_request(Rozhranie rozhranie)
        {
           
            if (rozhranie.adapter.MacAddress != null)
            {
                EthernetPacket ethernet_packet = new EthernetPacket(rozhranie.adapter.MacAddress, PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF"), EthernetPacketType.Arp);
                arp_packet = new ARPPacket(ARPOperation.Request, PhysicalAddress.Parse("00-00-00-00-00-00"), IPAddress.Parse(main_view.arp), rozhranie.adapter.MacAddress, IPAddress.Parse(rozhranie.ip_adresa));

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
