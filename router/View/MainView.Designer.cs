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
            this.btn_arp = new System.Windows.Forms.Button();
            this.txt_arp = new System.Windows.Forms.TextBox();
            this.lb_arp_tabulka = new System.Windows.Forms.ListBox();
            this.btn_zmaz_arp = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.txt_casovac = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_adaptery = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btn_nastav
            // 
            this.btn_nastav.Location = new System.Drawing.Point(118, 92);
            this.btn_nastav.Name = "btn_nastav";
            this.btn_nastav.Size = new System.Drawing.Size(75, 23);
            this.btn_nastav.TabIndex = 0;
            this.btn_nastav.Text = "nastav";
            this.btn_nastav.UseVisualStyleBackColor = true;
            this.btn_nastav.Click += new System.EventHandler(this.btn_nastav_Click);
            // 
            // txt_ip_adresa
            // 
            this.txt_ip_adresa.Location = new System.Drawing.Point(103, 27);
            this.txt_ip_adresa.Name = "txt_ip_adresa";
            this.txt_ip_adresa.Size = new System.Drawing.Size(100, 22);
            this.txt_ip_adresa.TabIndex = 2;
            // 
            // txt_maska
            // 
            this.txt_maska.Location = new System.Drawing.Point(103, 64);
            this.txt_maska.Name = "txt_maska";
            this.txt_maska.Size = new System.Drawing.Size(100, 22);
            this.txt_maska.TabIndex = 3;
            // 
            // btn_arp
            // 
            this.btn_arp.Location = new System.Drawing.Point(159, 227);
            this.btn_arp.Name = "btn_arp";
            this.btn_arp.Size = new System.Drawing.Size(75, 23);
            this.btn_arp.TabIndex = 4;
            this.btn_arp.Text = "arp";
            this.btn_arp.UseVisualStyleBackColor = true;
            this.btn_arp.Click += new System.EventHandler(this.btn_arp_Click);
            // 
            // txt_arp
            // 
            this.txt_arp.Location = new System.Drawing.Point(44, 228);
            this.txt_arp.Name = "txt_arp";
            this.txt_arp.Size = new System.Drawing.Size(100, 22);
            this.txt_arp.TabIndex = 5;
            // 
            // lb_arp_tabulka
            // 
            this.lb_arp_tabulka.FormattingEnabled = true;
            this.lb_arp_tabulka.ItemHeight = 16;
            this.lb_arp_tabulka.Location = new System.Drawing.Point(290, 227);
            this.lb_arp_tabulka.Name = "lb_arp_tabulka";
            this.lb_arp_tabulka.Size = new System.Drawing.Size(284, 100);
            this.lb_arp_tabulka.TabIndex = 6;
            // 
            // btn_zmaz_arp
            // 
            this.btn_zmaz_arp.Location = new System.Drawing.Point(314, 343);
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
            this.button2.Location = new System.Drawing.Point(159, 281);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "casovac";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txt_casovac
            // 
            this.txt_casovac.Location = new System.Drawing.Point(44, 281);
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
            this.cb_adaptery.Size = new System.Drawing.Size(387, 24);
            this.cb_adaptery.TabIndex = 13;
            this.cb_adaptery.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 380);
            this.Controls.Add(this.cb_adaptery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_casovac);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_zmaz_arp);
            this.Controls.Add(this.lb_arp_tabulka);
            this.Controls.Add(this.txt_arp);
            this.Controls.Add(this.btn_arp);
            this.Controls.Add(this.txt_maska);
            this.Controls.Add(this.txt_ip_adresa);
            this.Controls.Add(this.btn_nastav);
            this.Name = "MainView";
            this.Text = "SW router";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainView_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_nastav;
        private System.Windows.Forms.TextBox txt_ip_adresa;
        private System.Windows.Forms.TextBox txt_maska;
        private System.Windows.Forms.Button btn_arp;
        private System.Windows.Forms.TextBox txt_arp;
        private System.Windows.Forms.ListBox lb_arp_tabulka;
        private System.Windows.Forms.Button btn_zmaz_arp;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txt_casovac;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_adaptery;
    }
}

