using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace router.Model
{
    static class Praca_s_ip
    {
        public static IPAddress broadcast(IPAddress adresa, IPAddress maska_siete)
        {
            byte[] ip_adresa = adresa.GetAddressBytes();
            byte[] maska = maska_siete.GetAddressBytes();
            byte[] broadcast = new byte[ip_adresa.Length];

            for (int i = 0; i < broadcast.Length; i++)
            {
                broadcast[i] = (byte)(ip_adresa[i] | (maska[i] ^ 255));
            }
            return new IPAddress(broadcast);
        }

        public static IPAddress adresa_siete(IPAddress adresa, IPAddress maska_siete)
        {
            byte[] ip_adresa = adresa.GetAddressBytes();
            byte[] maska = maska_siete.GetAddressBytes();
            byte[] siet = new byte[ip_adresa.Length];

            for (int i = 0; i < siet.Length; i++)
            {
                siet[i] = (byte)(ip_adresa[i] & (maska[i]));
            }
            return new IPAddress(siet);
        }

        public static int sprav_masku(IPAddress maska_siete)
        {
            byte[] maska = maska_siete.GetAddressBytes();
            int maska_slash=0;
            var bit = new BitArray(maska);

            for (int i = 0; i < bit.Length; i++)
            {
                if (bit[i] == true) maska_slash++;
            }
            
            return maska_slash;
        }

        public static bool zisti_podsiet(IPAddress zisti_ip, IPAddress ip_siete, IPAddress maska_siete)
        {
            IPAddress siet1 = adresa_siete(zisti_ip, maska_siete);
            IPAddress siet2 = adresa_siete(ip_siete, maska_siete);

            return siet1.Equals(siet2);
        }

    }
}