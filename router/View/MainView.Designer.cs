namespace router
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_nastav = new System.Windows.Forms.Button();
            this.txt_ip_adresa = new System.Windows.Forms.TextBox();
            this.txt_maska = new System.Windows.Forms.TextBox();
            this.lb_arp_tabulka = new System.Windows.Forms.ListBox();
            this.btn_zmaz_arp = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.txt_casovac = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_adaptery = new System.Windows.Forms.ComboBox();
            this.rb_rozhranie1 = new System.Windows.Forms.RadioButton();
            this.rb_rozhranie2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_smerovacia_tabulka = new System.Windows.Forms.ListBox();
            this.btnpridaj = new System.Windows.Forms.Button();
            this.txt_ip_staticke = new System.Windows.Forms.TextBox();
            this.txt_maska_staticke = new System.Windows.Forms.TextBox();
            this.lbl_ciel_ip = new System.Windows.Forms.Label();
            this.lbl_maska = new System.Windows.Forms.Label();
            this.txt_next_hop = new System.Windows.Forms.TextBox();
            this.lbl_next_hop = new System.Windows.Forms.Label();
            this.lbl_rozhranie = new System.Windows.Forms.Label();
            this.txt_rozhranie_staticke = new System.Windows.Forms.TextBox();
            this.btn_zmaz_cestu = new System.Windows.Forms.Button();
            this.rip_rozhranie_2 = new System.Windows.Forms.CheckBox();
            this.rip_rozhranie_1 = new System.Windows.Forms.CheckBox();
            this.btn_nastav_casovace = new System.Windows.Forms.Button();
            this.invalid = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_invalid = new System.Windows.Forms.TextBox();
            this.txt_holddown = new System.Windows.Forms.TextBox();
            this.txt_flush = new System.Windows.Forms.TextBox();
            this.txt_update = new System.Windows.Forms.TextBox();
            this.txt_ping = new System.Windows.Forms.TextBox();
            this.btn_ping = new System.Windows.Forms.Button();
            this.lbl_ping = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_nastav
            // 
            this.btn_nastav.Location = new System.Drawing.Point(103, 92);
            this.btn_nastav.Name = "btn_nastav";
            this.btn_nastav.Size = new System.Drawing.Size(100, 23);
            this.btn_nastav.TabIndex = 0;
            this.btn_nastav.Text = "nastav";
            this.btn_nastav.UseVisualStyleBackColor = true;
            this.btn_nastav.Click += new System.EventHandler(this.btn_nastav_Click);
            // 
            // txt_ip_adresa
            // 
            this.txt_ip_adresa.Location = new System.Drawing.Point(103, 27);
            this.txt_ip_adresa.Name = "txt_ip_adresa";
            this.txt_ip_adresa.Size = new System.Drawing.Size(111, 22);
            this.txt_ip_adresa.TabIndex = 2;
            // 
            // txt_maska
            // 
            this.txt_maska.Location = new System.Drawing.Point(103, 64);
            this.txt_maska.Name = "txt_maska";
            this.txt_maska.Size = new System.Drawing.Size(111, 22);
            this.txt_maska.TabIndex = 3;
            // 
            // lb_arp_tabulka
            // 
            this.lb_arp_tabulka.FormattingEnabled = true;
            this.lb_arp_tabulka.ItemHeight = 16;
            this.lb_arp_tabulka.Location = new System.Drawing.Point(263, 227);
            this.lb_arp_tabulka.Name = "lb_arp_tabulka";
            this.lb_arp_tabulka.Size = new System.Drawing.Size(397, 100);
            this.lb_arp_tabulka.TabIndex = 6;
            // 
            // btn_zmaz_arp
            // 
            this.btn_zmaz_arp.Location = new System.Drawing.Point(263, 344);
            this.btn_zmaz_arp.Name = "btn_zmaz_arp";
            this.btn_zmaz_arp.Size = new System.Drawing.Size(75, 23);
            this.btn_zmaz_arp.TabIndex = 8;
            this.btn_zmaz_arp.Text = "zmaz";
            this.btn_zmaz_arp.UseVisualStyleBackColor = true;
            this.btn_zmaz_arp.Click += new System.EventHandler(this.btn_zmaz_arp_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(162, 227);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "casovac";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txt_casovac
            // 
            this.txt_casovac.Location = new System.Drawing.Point(44, 228);
            this.txt_casovac.Name = "txt_casovac";
            this.txt_casovac.Size = new System.Drawing.Size(100, 22);
            this.txt_casovac.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "ip adresa";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "maska";
            // 
            // cb_adaptery
            // 
            this.cb_adaptery.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cb_adaptery.AllowDrop = true;
            this.cb_adaptery.Cursor = System.Windows.Forms.Cursors.Default;
            this.cb_adaptery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_adaptery.FormattingEnabled = true;
            this.cb_adaptery.Location = new System.Drawing.Point(220, 27);
            this.cb_adaptery.Name = "cb_adaptery";
            this.cb_adaptery.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_adaptery.Size = new System.Drawing.Size(440, 24);
            this.cb_adaptery.TabIndex = 13;
            // 
            // rb_rozhranie1
            // 
            this.rb_rozhranie1.AutoSize = true;
            this.rb_rozhranie1.Checked = true;
            this.rb_rozhranie1.Location = new System.Drawing.Point(23, 30);
            this.rb_rozhranie1.Name = "rb_rozhranie1";
            this.rb_rozhranie1.Size = new System.Drawing.Size(101, 21);
            this.rb_rozhranie1.TabIndex = 14;
            this.rb_rozhranie1.TabStop = true;
            this.rb_rozhranie1.Text = "rozhranie 1";
            this.rb_rozhranie1.UseVisualStyleBackColor = true;
            this.rb_rozhranie1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rb_rozhranie2
            // 
            this.rb_rozhranie2.AutoSize = true;
            this.rb_rozhranie2.Location = new System.Drawing.Point(23, 57);
            this.rb_rozhranie2.Name = "rb_rozhranie2";
            this.rb_rozhranie2.Size = new System.Drawing.Size(101, 21);
            this.rb_rozhranie2.TabIndex = 15;
            this.rb_rozhranie2.Text = "rozhranie 2";
            this.rb_rozhranie2.UseVisualStyleBackColor = true;
            this.rb_rozhranie2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_rozhranie1);
            this.groupBox1.Controls.Add(this.rb_rozhranie2);
            this.groupBox1.Location = new System.Drawing.Point(220, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 97);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "rozhrania";
            // 
            // lb_smerovacia_tabulka
            // 
            this.lb_smerovacia_tabulka.FormattingEnabled = true;
            this.lb_smerovacia_tabulka.ItemHeight = 16;
            this.lb_smerovacia_tabulka.Location = new System.Drawing.Point(666, 12);
            this.lb_smerovacia_tabulka.Name = "lb_smerovacia_tabulka";
            this.lb_smerovacia_tabulka.Size = new System.Drawing.Size(509, 340);
            this.lb_smerovacia_tabulka.TabIndex = 17;
            this.lb_smerovacia_tabulka.SelectedIndexChanged += new System.EventHandler(this.lb_smerovacia_tabulka_SelectedIndexChanged);
            // 
            // btnpridaj
            // 
            this.btnpridaj.Location = new System.Drawing.Point(994, 436);
            this.btnpridaj.Name = "btnpridaj";
            this.btnpridaj.Size = new System.Drawing.Size(75, 41);
            this.btnpridaj.TabIndex = 18;
            this.btnpridaj.Text = "pridaj";
            this.btnpridaj.UseVisualStyleBackColor = true;
            this.btnpridaj.Click += new System.EventHandler(this.btnpridaj_Click);
            // 
            // txt_ip_staticke
            // 
            this.txt_ip_staticke.Location = new System.Drawing.Point(629, 407);
            this.txt_ip_staticke.Name = "txt_ip_staticke";
            this.txt_ip_staticke.Size = new System.Drawing.Size(100, 22);
            this.txt_ip_staticke.TabIndex = 19;
            // 
            // txt_maska_staticke
            // 
            this.txt_maska_staticke.Location = new System.Drawing.Point(748, 407);
            this.txt_maska_staticke.Name = "txt_maska_staticke";
            this.txt_maska_staticke.Size = new System.Drawing.Size(100, 22);
            this.txt_maska_staticke.TabIndex = 20;
            // 
            // lbl_ciel_ip
            // 
            this.lbl_ciel_ip.AutoSize = true;
            this.lbl_ciel_ip.Location = new System.Drawing.Point(626, 387);
            this.lbl_ciel_ip.Name = "lbl_ciel_ip";
            this.lbl_ciel_ip.Size = new System.Drawing.Size(67, 17);
            this.lbl_ciel_ip.TabIndex = 21;
            this.lbl_ciel_ip.Text = "cielova ip";
            // 
            // lbl_maska
            // 
            this.lbl_maska.AutoSize = true;
            this.lbl_maska.Location = new System.Drawing.Point(748, 387);
            this.lbl_maska.Name = "lbl_maska";
            this.lbl_maska.Size = new System.Drawing.Size(49, 17);
            this.lbl_maska.TabIndex = 22;
            this.lbl_maska.Text = "maska";
            // 
            // txt_next_hop
            // 
            this.txt_next_hop.Location = new System.Drawing.Point(870, 407);
            this.txt_next_hop.Name = "txt_next_hop";
            this.txt_next_hop.Size = new System.Drawing.Size(100, 22);
            this.txt_next_hop.TabIndex = 23;
            // 
            // lbl_next_hop
            // 
            this.lbl_next_hop.AutoSize = true;
            this.lbl_next_hop.Location = new System.Drawing.Point(867, 387);
            this.lbl_next_hop.Name = "lbl_next_hop";
            this.lbl_next_hop.Size = new System.Drawing.Size(62, 17);
            this.lbl_next_hop.TabIndex = 24;
            this.lbl_next_hop.Text = "next hop";
            // 
            // lbl_rozhranie
            // 
            this.lbl_rozhranie.AutoSize = true;
            this.lbl_rozhranie.Location = new System.Drawing.Point(866, 448);
            this.lbl_rozhranie.Name = "lbl_rozhranie";
            this.lbl_rozhranie.Size = new System.Drawing.Size(68, 17);
            this.lbl_rozhranie.TabIndex = 25;
            this.lbl_rozhranie.Text = "rozhranie";
            // 
            // txt_rozhranie_staticke
            // 
            this.txt_rozhranie_staticke.Location = new System.Drawing.Point(940, 445);
            this.txt_rozhranie_staticke.Name = "txt_rozhranie_staticke";
            this.txt_rozhranie_staticke.Size = new System.Drawing.Size(30, 22);
            this.txt_rozhranie_staticke.TabIndex = 26;
            // 
            // btn_zmaz_cestu
            // 
            this.btn_zmaz_cestu.Location = new System.Drawing.Point(1063, 373);
            this.btn_zmaz_cestu.Name = "btn_zmaz_cestu";
            this.btn_zmaz_cestu.Size = new System.Drawing.Size(75, 23);
            this.btn_zmaz_cestu.TabIndex = 27;
            this.btn_zmaz_cestu.Text = "zmaz";
            this.btn_zmaz_cestu.UseVisualStyleBackColor = true;
            this.btn_zmaz_cestu.Click += new System.EventHandler(this.btn_zmaz_cestu_Click);
            // 
            // rip_rozhranie_2
            // 
            this.rip_rozhranie_2.AutoSize = true;
            this.rip_rozhranie_2.Location = new System.Drawing.Point(427, 121);
            this.rip_rozhranie_2.Name = "rip_rozhranie_2";
            this.rip_rozhranie_2.Size = new System.Drawing.Size(122, 21);
            this.rip_rozhranie_2.TabIndex = 28;
            this.rip_rozhranie_2.Text = "rip rozhranie 2";
            this.rip_rozhranie_2.UseVisualStyleBackColor = true;
            this.rip_rozhranie_2.CheckedChanged += new System.EventHandler(this.rip_rozhranie_2_CheckedChanged);
            // 
            // rip_rozhranie_1
            // 
            this.rip_rozhranie_1.AutoSize = true;
            this.rip_rozhranie_1.Location = new System.Drawing.Point(427, 92);
            this.rip_rozhranie_1.Name = "rip_rozhranie_1";
            this.rip_rozhranie_1.Size = new System.Drawing.Size(122, 21);
            this.rip_rozhranie_1.TabIndex = 29;
            this.rip_rozhranie_1.Text = "rip rozhranie 1";
            this.rip_rozhranie_1.UseVisualStyleBackColor = true;
            this.rip_rozhranie_1.CheckedChanged += new System.EventHandler(this.rip_rozhranie_1_CheckedChanged);
            // 
            // btn_nastav_casovace
            // 
            this.btn_nastav_casovace.Location = new System.Drawing.Point(412, 442);
            this.btn_nastav_casovace.Name = "btn_nastav_casovace";
            this.btn_nastav_casovace.Size = new System.Drawing.Size(122, 23);
            this.btn_nastav_casovace.TabIndex = 30;
            this.btn_nastav_casovace.Text = "nastav casovace";
            this.btn_nastav_casovace.UseVisualStyleBackColor = true;
            this.btn_nastav_casovace.Click += new System.EventHandler(this.btn_nastav_casovace_Click);
            // 
            // invalid
            // 
            this.invalid.AutoSize = true;
            this.invalid.Location = new System.Drawing.Point(49, 406);
            this.invalid.Name = "invalid";
            this.invalid.Size = new System.Drawing.Size(48, 17);
            this.invalid.TabIndex = 31;
            this.invalid.Text = "invalid";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(113, 406);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 32;
            this.label3.Text = "holddown";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(199, 406);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 33;
            this.label4.Text = "flush";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(260, 406);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 17);
            this.label5.TabIndex = 34;
            this.label5.Text = "update";
            // 
            // txt_invalid
            // 
            this.txt_invalid.Location = new System.Drawing.Point(52, 436);
            this.txt_invalid.Name = "txt_invalid";
            this.txt_invalid.Size = new System.Drawing.Size(45, 22);
            this.txt_invalid.TabIndex = 35;
            // 
            // txt_holddown
            // 
            this.txt_holddown.Location = new System.Drawing.Point(116, 436);
            this.txt_holddown.Name = "txt_holddown";
            this.txt_holddown.Size = new System.Drawing.Size(45, 22);
            this.txt_holddown.TabIndex = 36;
            // 
            // txt_flush
            // 
            this.txt_flush.Location = new System.Drawing.Point(192, 436);
            this.txt_flush.Name = "txt_flush";
            this.txt_flush.Size = new System.Drawing.Size(45, 22);
            this.txt_flush.TabIndex = 37;
            // 
            // txt_update
            // 
            this.txt_update.Location = new System.Drawing.Point(263, 436);
            this.txt_update.Name = "txt_update";
            this.txt_update.Size = new System.Drawing.Size(45, 22);
            this.txt_update.TabIndex = 38;
            // 
            // txt_ping
            // 
            this.txt_ping.Location = new System.Drawing.Point(44, 277);
            this.txt_ping.Name = "txt_ping";
            this.txt_ping.Size = new System.Drawing.Size(100, 22);
            this.txt_ping.TabIndex = 39;
            // 
            // btn_ping
            // 
            this.btn_ping.Location = new System.Drawing.Point(162, 277);
            this.btn_ping.Name = "btn_ping";
            this.btn_ping.Size = new System.Drawing.Size(75, 27);
            this.btn_ping.TabIndex = 40;
            this.btn_ping.Text = "ping";
            this.btn_ping.UseVisualStyleBackColor = true;
            this.btn_ping.Click += new System.EventHandler(this.btn_ping_Click);
            // 
            // lbl_ping
            // 
            this.lbl_ping.AutoSize = true;
            this.lbl_ping.Location = new System.Drawing.Point(44, 334);
            this.lbl_ping.Name = "lbl_ping";
            this.lbl_ping.Size = new System.Drawing.Size(0, 17);
            this.lbl_ping.TabIndex = 41;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 560);
            this.Controls.Add(this.lbl_ping);
            this.Controls.Add(this.btn_ping);
            this.Controls.Add(this.txt_ping);
            this.Controls.Add(this.txt_update);
            this.Controls.Add(this.txt_flush);
            this.Controls.Add(this.txt_holddown);
            this.Controls.Add(this.txt_invalid);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.invalid);
            this.Controls.Add(this.btn_nastav_casovace);
            this.Controls.Add(this.rip_rozhranie_1);
            this.Controls.Add(this.rip_rozhranie_2);
            this.Controls.Add(this.btn_zmaz_cestu);
            this.Controls.Add(this.txt_rozhranie_staticke);
            this.Controls.Add(this.lbl_rozhranie);
            this.Controls.Add(this.lbl_next_hop);
            this.Controls.Add(this.txt_next_hop);
            this.Controls.Add(this.lbl_maska);
            this.Controls.Add(this.lbl_ciel_ip);
            this.Controls.Add(this.txt_maska_staticke);
            this.Controls.Add(this.txt_ip_staticke);
            this.Controls.Add(this.btnpridaj);
            this.Controls.Add(this.lb_smerovacia_tabulka);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cb_adaptery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_casovac);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_zmaz_arp);
            this.Controls.Add(this.lb_arp_tabulka);
            this.Controls.Add(this.txt_maska);
            this.Controls.Add(this.txt_ip_adresa);
            this.Controls.Add(this.btn_nastav);
            this.Name = "MainView";
            this.Text = "SW router";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainView_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_nastav;
        private System.Windows.Forms.TextBox txt_ip_adresa;
        private System.Windows.Forms.TextBox txt_maska;
        private System.Windows.Forms.ListBox lb_arp_tabulka;
        private System.Windows.Forms.Button btn_zmaz_arp;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txt_casovac;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_adaptery;
        private System.Windows.Forms.RadioButton rb_rozhranie1;
        private System.Windows.Forms.RadioButton rb_rozhranie2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lb_smerovacia_tabulka;
        private System.Windows.Forms.Button btnpridaj;
        private System.Windows.Forms.TextBox txt_ip_staticke;
        private System.Windows.Forms.TextBox txt_maska_staticke;
        private System.Windows.Forms.Label lbl_ciel_ip;
        private System.Windows.Forms.Label lbl_maska;
        private System.Windows.Forms.TextBox txt_next_hop;
        private System.Windows.Forms.Label lbl_next_hop;
        private System.Windows.Forms.Label lbl_rozhranie;
        private System.Windows.Forms.TextBox txt_rozhranie_staticke;
        private System.Windows.Forms.Button btn_zmaz_cestu;
        private System.Windows.Forms.CheckBox rip_rozhranie_2;
        private System.Windows.Forms.CheckBox rip_rozhranie_1;
        private System.Windows.Forms.Button btn_nastav_casovace;
        private System.Windows.Forms.Label invalid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_invalid;
        private System.Windows.Forms.TextBox txt_holddown;
        private System.Windows.Forms.TextBox txt_flush;
        private System.Windows.Forms.TextBox txt_update;
        private System.Windows.Forms.TextBox txt_ping;
        private System.Windows.Forms.Button btn_ping;
        private System.Windows.Forms.Label lbl_ping;
    }
}

