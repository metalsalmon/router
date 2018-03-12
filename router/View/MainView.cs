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
        public MainView()
        {
            AllocConsole();
            InitializeComponent(); 
            presenter = new MainController(this);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public string ip_adresa { get => txt_ip_adresa.Text; set => txt_ip_adresa.Text = value; }
        public string maska { get => txt_maska.Text; set => txt_maska.Text = value; }
        public string adaptery { get => cb_adaptery.SelectedItem.ToString(); set => cb_adaptery.Items.Add(value); }
        public int adaptery_index { get => cb_adaptery.SelectedIndex; set => throw new NotImplementedException(); }
        internal MainController Presenter { get => presenter; set => presenter = value; }
        public string arp { get => txt_arp.Text.ToString(); set => txt_arp.Text = value; }
        public string lb_arp_zaznam { get => lb_arp_tabulka.SelectedItem.ToString(); set => lb_arp_tabulka.Items.Add(value); }
        public int lb_arp_zaznam_index { get => lb_arp_tabulka.SelectedIndex; set => throw new NotImplementedException(); }

        public int casovac { get => Int32.Parse(txt_casovac.Text); set => txt_casovac.Text = value.ToString(); }

        public Thread vlakno_rozhranie1 = null, vlakno_rozhranie2 = null;
        private void btn_nastav_Click(object sender, EventArgs e)
        {
            if (adaptery_index >= 0 && (rb_rozhranie1.Checked || rb_rozhranie2.Checked))
            {
                if (rb_rozhranie1.Checked) presenter.rozhranie1 = presenter.nastav_ip(presenter.rozhranie1);
                else presenter.rozhranie2 = presenter.nastav_ip(presenter.rozhranie2);

                try
                {
                    if (vlakno_rozhranie1 == null && rb_rozhranie1.Checked)
                    {
                        vlakno_rozhranie1 = new Thread(() => presenter.pocuvaj(presenter.rozhranie1));
                        vlakno_rozhranie1.Start();
                    }

                    if (vlakno_rozhranie2 == null && rb_rozhranie2.Checked)
                    {
                        vlakno_rozhranie2 = new Thread(() => presenter.pocuvaj(presenter.rozhranie2));
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

        public void vypis(string omg)
        {
            Console.Write(omg);
        }
        public void vymaz_arp()
        {
            lb_arp_tabulka.Items.Clear();
        }

        private void btn_arp_Click(object sender, EventArgs e)
        {
              presenter.arp_request(presenter.rozhranie2);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lb_arp_tabulka.Items.Clear();
            presenter.updatni_arp_tabulku();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                presenter.casovac = Int32.Parse(txt_casovac.Text);
            }
            catch(Exception)
            {
                presenter.casovac = 50;
               
            }
        }

        private void btn_zmaz_arp_Click(object sender, EventArgs e)
        {
            /*    if (lb_arp_tabulka.SelectedIndex == -1){
                    MessageBox.Show("vyber zaznam!");
                }else
                {            
                    presenter.zmaz_arp_zaznam();
                    lb_arp_tabulka.Items.RemoveAt(lb_arp_zaznam_index);
                    */
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
    }
}
