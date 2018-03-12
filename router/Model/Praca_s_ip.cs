using System;
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

        public static IPAddress adresa_siete(this IPAddress adresa, IPAddress maska_siete)
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

    }
}