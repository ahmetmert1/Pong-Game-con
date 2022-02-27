using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {

        public List<Color> toplarRenk = new List<Color>();
        public List<int> toplarX = new List<int>();
        public List<int> toplarY = new List<int>();
        

        oyun oyun_nesne;
        public Form1()
        {
            //PictureBox ziplatmaTahtasi = new PictureBox();
            //ziplatmaTahtasi.Size = new Size(140, 40);
            //ziplatmaTahtasi.Location = new Point(314, 378);
            //ziplatmaTahtasi.BackColor = Color.Aqua;
            //ziplatmaTahtasi.Name = "pictureBox1";
            //Controls.Add(ziplatmaTahtasi);


            oyun_nesne = new oyun(this);
            Task gorev1 = Task.Run(new Action(hareketEttirYontemThread));
            InitializeComponent();
            

        }

        private void tusa_basildi(object sender, KeyEventArgs e)
        {
            oyun_nesne.tusa_basildi_oyun(e);
        }

        private void tus_cekildi(object sender, KeyEventArgs e)
        {
            oyun_nesne.tus_cekildi_oyun(e);
        }

        //timer1 'e bağlı 10 ms de bir tetiklenen topların hareketini sağlayan fonksiyon
        private void hareket_ettir(object sender, EventArgs e)
        {

            oyun_nesne.hareket_ettir_oyun(timer1, timer2, timer3, pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7);
        }

        //10 saniyede 1 top oluşturan fonksiyon
        private void top_olustur(object sender, EventArgs e)
        {
            oyun_nesne.top_olustur_oyun();
        }

        //private void tahta_hareket(object sender, EventArgs e)
        //{
        //    oyun_nesne.tahta_hareket_oyun(pictureBox1);
        //}
        private void hareketEttirYontemThread()
        {
            oyun_nesne.tahtaHareketThread(ref pictureBox1);
        }

        

        private void devam_et(object sender, EventArgs e)
        {

            oyun_nesne.devam_et_oyun(timer1,timer2,timer3);
        }

        private void durdur(object sender, EventArgs e)
        {
            oyun_nesne.durdur_oyun(timer1, timer2, timer3);
            
        }

        /// <summary>
        /// /////////////////////////////////////////////////////
        /// </summary>
     

        

        private void label6_Click(object sender, EventArgs e)
        {
            //BACKUP - OTOMATİK YEDEKLEME -


            oyun_nesne.backupTiklaWrite();



            MessageBox.Show("Verileriniz Yedeklendi");
            
        }

        private void label7_Click(object sender, EventArgs e)
        {
            oyun_nesne.top_olustur_oyun_json();

            //RESTORE - GERİ YÜKLEME -
        }

        private void kayit_al(object sender, EventArgs e)
        {
            oyun_nesne.backupTiklaWrite();
            MessageBox.Show("Verileriniz Yedeklendi");
        }
    }
}
