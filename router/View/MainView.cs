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

namespace router
{
    public partial class MainView : Form, IView
    {
        private MainController presenter;
        public MainView()
        {
            InitializeComponent();
            presenter = new MainController(this);

        }

        public string ip_adresa { get => txt_ip_adresa.Text; set => txt_ip_adresa.Text = value; }
        public string maska { get =>txt_maska.Text; set => txt_maska.Text = value; }
       // public string adaptery { get => lb_rozhrania.SelectedItem.ToString(); set => lb_rozhrania.Items.Add(value); }
        public string adaptery { get => cb_adaptery.SelectedItem.ToString(); set => cb_adaptery.Items.Add(value); }
        //public int lb_adaptery_index { get =>lb_rozhrania.SelectedIndex; set => throw new NotImplementedException(); }
        public int lb_adaptery_index { get => cb_adaptery.SelectedIndex; set => throw new NotImplementedException(); }
        internal MainController Presenter { get => presenter; set => presenter = value; }
        public string arp { get => txt_arp.Text.ToString(); set => txt_arp.Text = value; }
        public string lb_arp_zaznam { get => lb_arp_tabulka.SelectedItem.ToString(); set => lb_arp_tabulka.Items.Add(value); }
        public int lb_arp_zaznam_index { get => lb_arp_tabulka.SelectedIndex; set => throw new NotImplementedException(); }

        public int casovac { get => Int32.Parse(txt_casovac.Text); set => txt_casovac.Text=value.ToString(); }

        public Thread t = null;
        private void btn_nastav_Click(object sender, EventArgs e)
        {
            presenter.zvoleny_adapter = null;
            if (lb_adaptery_index >= 0)
            {

                presenter.nastav_ip();
                try
                {
                    if (t == null)
                    {
                        t = new Thread(new ThreadStart(presenter.pocuvaj));
                        t.Start();
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
              presenter.arp_request();

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
            catch(Exception ee)
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
            if (t != null)
            {
                presenter.zastav_vlakno = true;
                t.Join();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
