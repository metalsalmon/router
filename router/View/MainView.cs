using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using router.View;
using router.Presenter;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;

namespace router
{
    public partial class MainView : Form, IView
    {
        private MainController presenter;
        internal MainController Presenter { get => presenter; set => presenter = value; }
        public string ip_adresa { get => txt_ip_adresa.Text; set => txt_ip_adresa.Text = value; }
        public string maska { get => txt_maska.Text; set => txt_maska.Text = value; }
        public string adaptery { get => cb_adaptery.SelectedItem.ToString(); set => cb_adaptery.Items.Add(value); }
        public int adaptery_index { get => cb_adaptery.SelectedIndex; set => throw new NotImplementedException(); }
        public string arp { get => txt_arp.Text.ToString(); set => txt_arp.Text = value; }
        public string lb_arp_zaznam { get => lb_arp_tabulka.SelectedItem.ToString(); set => lb_arp_tabulka.Items.Add(value); }
        public int lb_smerovaci_zaznam_index { get => lb_smerovacia_tabulka.SelectedIndex; set => throw new NotImplementedException(); }
        public int casovac { get => Int32.Parse(txt_casovac.Text); set => txt_casovac.Text = value.ToString(); }
        string IView.lb_smerovacia_tabulka { get => throw new NotImplementedException(); set => lb_smerovacia_tabulka.Items.Add(value); }
        public string staticke_ip { get => txt_ip_staticke.Text; set => txt_ip_staticke.Text = value; }
        public string staticke_maska { get => txt_maska_staticke.Text; set => txt_maska_staticke.Text = value; }
        public string staticke_next_hop { get => txt_next_hop.Text; set => txt_next_hop.Text = value; }
        public string staticke_rozhranie { get => txt_rozhranie_staticke.Text; set => txt_rozhranie_staticke.Text = value; }

        private Thread vlakno_rozhranie1 = null, vlakno_rozhranie2 = null;


        public MainView()
        {
            AllocConsole();
            InitializeComponent(); 
            presenter = new MainController(this);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();


        private void btn_nastav_Click(object sender, EventArgs e)
        {

            if (adaptery_index >= 0 && (rb_rozhranie1.Checked || rb_rozhranie2.Checked))
            {
                lb_smerovacia_tabulka.Items.Clear();
                if (rb_rozhranie1.Checked)
                {
                    presenter.rozhranie1 = presenter.nastav_ip(presenter.rozhranie1,1);
                    presenter.priamo_pripojena_siet(1);
                }
                else
                {
                    presenter.rozhranie2 = presenter.nastav_ip(presenter.rozhranie2,2);
                    presenter.priamo_pripojena_siet(2);
                }

                try
                {
                    if (vlakno_rozhranie1 == null && rb_rozhranie1.Checked)
                    {
                        vlakno_rozhranie1 = new Thread(() => presenter.pocuvaj_rozhranie1(presenter.rozhranie1));
                        vlakno_rozhranie1.Start();
                    }

                    if (vlakno_rozhranie2 == null && rb_rozhranie2.Checked)
                    {
                        vlakno_rozhranie2 = new Thread(() => presenter.pocuvaj_rozhranie2(presenter.rozhranie2));
                        vlakno_rozhranie2.Start();
                    }

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }
            }
            else
            {
                MessageBox.Show("zvol adatper!");
            }
        }

        public void vymaz_arp()
        {
            lb_arp_tabulka.Items.Clear();
        }

        private void btn_arp_Click(object sender, EventArgs e)
        {
              presenter.arp_request(presenter.rozhranie2, IPAddress.Parse(arp));

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_arp_tabulka.Items.Clear();
            presenter.updatni_arp_tabulku();

            presenter.zniz_casovace();
            lb_smerovacia_tabulka.Items.Clear();
            presenter.updatni_smerovaciu_tabulku();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            presenter.vypis_rip_databazku();
            try
            {
                presenter.arp_casovac = Int32.Parse(txt_casovac.Text);
            }
            catch(Exception)
            {
                presenter.arp_casovac = 50;
               
            }
        }

        private void btn_zmaz_arp_Click(object sender, EventArgs e)
        {
            lb_arp_tabulka.Items.Clear();
            presenter.zmaz_arp_tabulku = true;
            presenter.updatni_arp_tabulku();
            presenter.zmaz_arp_tabulku = false;

        }

        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vlakno_rozhranie1 != null)
            {
                presenter.zastav_vlakno = true;
                vlakno_rozhranie1.Join();
            }
            if (vlakno_rozhranie2 != null)
            {
                presenter.zastav_vlakno = true;
                vlakno_rozhranie2.Join();
            }
        }
    

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(presenter.rozhranie1!= null)
            {
                txt_ip_adresa.Text = presenter.rozhranie1.ip_adresa;
                txt_maska.Text = presenter.rozhranie1.maska;
            }
            else
            {
                txt_ip_adresa.Text = String.Empty;
                txt_maska.Text = String.Empty;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (presenter.rozhranie2 != null)
            {
                txt_ip_adresa.Text = presenter.rozhranie2.ip_adresa;
                txt_maska.Text = presenter.rozhranie2.maska;
            }
            else
            {
                txt_ip_adresa.Text = String.Empty;
                txt_maska.Text = String.Empty;
            }
        }

        private void btn_zmaz_cestu_Click(object sender, EventArgs e)
        {
            if (lb_smerovacia_tabulka.SelectedIndex == -1)
            {
                MessageBox.Show("vyber zaznam!");
            }
            else
            {
                presenter.zmaz_smerovaci_zaznam();
                lb_smerovacia_tabulka.Items.RemoveAt(lb_smerovaci_zaznam_index);
            }
            }

        private void btnpridaj_Click(object sender, EventArgs e)
        {
            lb_smerovacia_tabulka.Items.Clear();
            if (txt_rozhranie_staticke.Text=="")presenter.pridaj_staticku_cestu(1);
            else if (txt_next_hop.Text == "") presenter.pridaj_staticku_cestu(2);
            else if((txt_rozhranie_staticke.Text != "") &&(txt_rozhranie_staticke.Text != "")) presenter.pridaj_staticku_cestu(3);

            txt_ip_staticke.Text = "";
            txt_maska_staticke.Text = "";
            txt_rozhranie_staticke.Text = "";
            txt_next_hop.Text = "";
        }
        public void vypis(string text,int cislo)
        {
            Console.WriteLine(text+"  "+cislo);

        }

        private void lb_smerovacia_tabulka_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*presenter.zmaz_smerovaci_zaznam();
            lb_smerovacia_tabulka.Items.RemoveAt(lb_smerovaci_zaznam_index);*/
        }

        public void vymaz_lb_smerovacia_tabulka()
        {
            lb_smerovacia_tabulka.Items.Clear();
        }

    }
}
