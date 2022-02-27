using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Console;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Environment;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace WinFormsApp5
{
    public class oyun
    {
        //AHMET MERT ÖZ 
        //18360859037
        //Görsel Programlama Ödev 7
        string hash = "@hm3t";

        private string _path = Combine(CurrentDirectory, "yedek.json");
        OyunVeri oyunVeriFromJson;


        //Oyun sonunda kazanınca ya da kaybedince gelen label fontu
        Font SonucFont = new Font("Snap ITC", 16);


        //klavyeden sağ ok ya da sol ok tuşuna basılı kontrol
        bool saga_git;
        bool sola_git;

        Label sonuc_yazisi; 

        public int score = 0;

        //topların düşmesini engelleyen picBox'ın konum değişim hızı
        private int tahta_hizi = 10;
        //10 saniyede bir üretilen topların kontrolü
        public int top_10sn_sayi = 0;

        //10 sn de bir oluşturulan topların sayısı
        private int top_sayisi = 0;

        Random rastgele_sayi = new Random();

        private int top_yukseklik = 70;
        private int top_genislik = 70;

        //Y ekseni Hızı
        public int[] y_ekseni_hizi = new int[70];
        //X ekseni Hızı
        public int[] x_ekseni_hizi = new int[70];

        //ekranda mevcut bulunan top sayısı
        public int ekrandaki_top = 0;

        //Toplara rastgele renk vermemizi sağlayacak olan renk listesi
        List<Color> renkler = new List<Color>();

        //Oluştuduğumuz topları tuttuğumuz Liste
        public List<PictureBox> toplar = new List<PictureBox>();

        Form form;
        public oyun(Form form)
        {


            this.form = form;

            sonuc_yazisi = new Label();

            for (int i = 0; i < 70; i++)
            {
                y_ekseni_hizi[i] = 2 * bir_yada_eksibir_oyun();
            }


            //X eksenindeki hız

            for (int i = 0; i < 70; i++)
            {
                x_ekseni_hizi[i] = 2 * bir_yada_eksibir_oyun();
            }


            renkler.Add(Color.Gray);
            //renkler.Add(Color.AliceBlue);
            renkler.Add(Color.Aquamarine);
            //renkler.Add(Color.Black);
            renkler.Add(Color.Brown);
            renkler.Add(Color.DarkCyan);
            renkler.Add(Color.Red);
            renkler.Add(Color.PaleGreen);
            renkler.Add(Color.DarkGreen);
            renkler.Add(Color.Pink);
            renkler.Add(Color.Fuchsia);
            renkler.Add(Color.Lime);
            renkler.Add(Color.Lavender);
            renkler.Add(Color.LightSeaGreen);
            renkler.Add(Color.Azure);
            renkler.Add(Color.PowderBlue);
            renkler.Add(Color.Tan);


            top_olustur_oyun_json();
        }

        public void tahtaHareketThread(ref PictureBox pictureBox1)
        {
            while(true)
            {
                Thread.Sleep(10);
                try
                {
                    if (sola_git == true && pictureBox1.Left > 30)
                    {
                        pictureBox1.Left -= tahta_hizi;
                    }
                    if (saga_git == true && pictureBox1.Left < 620)
                    {
                        pictureBox1.Left += tahta_hizi;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
               
            }
        }

        public void tusa_basildi_oyun(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                sola_git = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                saga_git = true;
            }
        }

        public void tus_cekildi_oyun(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                sola_git = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                saga_git = false;
            }
        }

        public void hareket_ettir_oyun(System.Windows.Forms.Timer timer1, System.Windows.Forms.Timer timer2, System.Windows.Forms.Timer timer3,PictureBox pictureBox1, PictureBox pictureBox2, PictureBox pictureBox3, PictureBox pictureBox4, PictureBox pictureBox5, PictureBox pictureBox6, PictureBox pictureBox7)
        {
            if (ekrandaki_top == 10)
            {
                timer1.Stop();
                timer2.Stop();
                timer3.Stop();
                //Oyun sonunda sonucu yazan label
                //Label sonuc_yazisi = new Label();
                sonuc_yazisi.Text = "KAYBETTİNİZ";
                sonuc_yazisi.ForeColor = Color.Black;
                sonuc_yazisi.BackColor = Color.Transparent;
                sonuc_yazisi.Font = SonucFont;
                sonuc_yazisi.Size = new Size(200, 100);
                sonuc_yazisi.Location = new Point(300, 200);
                sonuc_yazisi.Enabled = true;
                form.Controls.Add(sonuc_yazisi);
            }
            else if (4 < 5)
            {
                for (int i = 0; i < toplar.Count; i++)
                {
                    toplar[i].Left = toplar[i].Left + x_ekseni_hizi[i];
                    toplar[i].Top = toplar[i].Top + y_ekseni_hizi[i];



                    ////YUKARIDAN YOK OLAN TOP
                    if (toplar[i].Top < 0)
                    {
                        form.Controls.Remove(toplar[i]);
                        toplar.RemoveAt(i);
                        score = score + 10;
                        form.Controls["label5"].Text = "SCORE : " + score.ToString();


                        ekrandaki_top--;
                        if (ekrandaki_top == 0 && top_10sn_sayi > 5)
                        {
                            timer1.Stop();
                            timer2.Stop();
                            timer3.Stop();
                            //Label sonuc_yazisi = new Label();
                            sonuc_yazisi.Text = "KAZANDINIZ";
                            sonuc_yazisi.ForeColor = Color.Black;
                            sonuc_yazisi.Font = SonucFont;
                            sonuc_yazisi.Size = new Size(200, 100);
                            sonuc_yazisi.Location = new Point(300, 200);
                            sonuc_yazisi.Enabled = true;
                            form.Controls.Add(sonuc_yazisi);
                        }
                        form.Controls["label1"].Text = "Mevcut Top : " + ekrandaki_top.ToString();
                        break;
                    }


                    ////AŞAĞIDAN YOK OLAN TOP
                    if (toplar[i].Top > form.ClientSize.Height)
                    {
                        form.Controls.Remove(toplar[i]);
                        toplar.RemoveAt(i);
                        score = score - 20;
                        form.Controls["label5"].Text = "SCORE : " + score.ToString();

                        ekrandaki_top--;
                        if (ekrandaki_top == 0 && top_10sn_sayi >= 5)
                        {
                            timer1.Stop();
                            timer2.Stop();
                            timer3.Stop();
                            //Label sonuc_yazisi = new Label();
                            sonuc_yazisi.Text = "KAZANDINIZ";
                            sonuc_yazisi.ForeColor = Color.Black;
                            sonuc_yazisi.Font = SonucFont;
                            sonuc_yazisi.Size = new Size(200, 100);
                            sonuc_yazisi.Location = new Point(300, 200);
                            sonuc_yazisi.Enabled = true;
                            form.Controls.Add(sonuc_yazisi);
                        }
                        form.Controls["label1"].Text = "Mevcut Top : " + ekrandaki_top.ToString();

                        ikili_top_olustur_oyun();

                    }

                    //// KENARLARLA ETKİLEŞİM
                    if (toplar[i].Bounds.IntersectsWith(pictureBox1.Bounds))
                    {
                        y_ekseni_hizi[i] = -y_ekseni_hizi[i];
                        score = score + 1;
                        form.Controls["label5"].Text = "SCORE : " + score.ToString();
                    }

                    if (toplar[i].Bounds.IntersectsWith(pictureBox4.Bounds))
                    {
                        x_ekseni_hizi[i] = -x_ekseni_hizi[i];
                    }

                    if (toplar[i].Bounds.IntersectsWith(pictureBox2.Bounds))
                    {
                        y_ekseni_hizi[i] = -y_ekseni_hizi[i];
                    }

                    if (toplar[i].Bounds.IntersectsWith(pictureBox3.Bounds))
                    {
                        y_ekseni_hizi[i] = -y_ekseni_hizi[i];
                    }

                    if (toplar[i].Bounds.IntersectsWith(pictureBox5.Bounds))
                    {
                        x_ekseni_hizi[i] = -x_ekseni_hizi[i];
                    }

                    if (toplar[i].Bounds.IntersectsWith(pictureBox6.Bounds))
                    {
                        x_ekseni_hizi[i] = -x_ekseni_hizi[i];
                    }

                    if (toplar[i].Bounds.IntersectsWith(pictureBox7.Bounds))
                    {
                        x_ekseni_hizi[i] = -x_ekseni_hizi[i];
                    }




                }
            }
        }

        public void top_olustur_oyun()
        {
            if (top_sayisi < 5)
            {


                PictureBox top = new Class2();
                top.BackColor = renkler[rastgele_sayi.Next(0, 10)];
                top.Size = new Size(top_genislik, top_yukseklik);
                top.Location = new Point(rastgele_sayi.Next(40, 700), rastgele_sayi.Next(35, 300));
                top.Enabled = true;
                form.Controls.Add(top);
                toplar.Add(top);
                top_10sn_sayi++;
                top_sayisi++;
                ekrandaki_top++;
                form.Controls["label1"].Text = "Mevcut Top : " + toplar.Count.ToString();
                form.Controls["label2"].Text = "10 saniyede gelen top : " + top_10sn_sayi.ToString();


            }
        }

        public void tahta_hareket_oyun(PictureBox pictureBox1)
        {
            if (sola_git == true && pictureBox1.Left > 30)
            {
                pictureBox1.Left -= tahta_hizi;
            }
            if (saga_git == true && pictureBox1.Left < 620)
            {
                pictureBox1.Left += tahta_hizi;
            }
        }

        public void ikili_top_olustur_oyun()
        {
            for (int i = 0; i < 2; i++)
            {
                PictureBox top = new Class2();
                top.BackColor = renkler[rastgele_sayi.Next(0, 10)];
                top.Size = new Size(top_genislik, top_yukseklik);
                top.Location = new Point(rastgele_sayi.Next(35, 700), rastgele_sayi.Next(35, 350));
                top.Enabled = true;
                form.Controls.Add(top);
                toplar.Add(top);
                //top_sayisi++;
                ekrandaki_top++;
                form.Controls["label1"].Text = "Mevcut Top : " + toplar.Count.ToString();
            }
        }


        public void top_olustur_oyun_json()
        {


            if (File.Exists(_path))
            {
                

                var result = MessageBox.Show("Kayıtlı Yerdem Devam Edilsin mi ?", "UYARI",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

               
                if (result == DialogResult.Yes)
                {
                   
                    restoreTikleRead();

                    for (int i = 0; i < toplar.Count; i++)
                    {
                        form.Controls.Remove(toplar[i]);
                    }

                    toplar.Clear();

                    for (int i = 0; i < oyunVeriFromJson.Toplar.Count; i++)
                    {
                        PictureBox top = new Class2();
                        top.BackColor = oyunVeriFromJson.Toplar[i].Renk;
                        top.Size = new Size(top_genislik, top_yukseklik);
                        top.Location = new Point(oyunVeriFromJson.Toplar[i].Konum_x, oyunVeriFromJson.Toplar[i].Konum_y);
                        top.Enabled = true;
                        form.Controls.Add(top);
                        toplar.Add(top);



                        //form.Controls["label1"].Text = "Mevcut Top : " + oyunVeriFromJson.Ekran_top_say.ToString();
                        //form.Controls["label2"].Text = "10 saniyede gelen top : " + oyunVeriFromJson.Top_sa10sn.ToString();

                        //form.Controls["label5"].Text = "SCORE : " + oyunVeriFromJson.Skor.ToString();



                        ekrandaki_top = oyunVeriFromJson.Ekran_top_say;
                        top_10sn_sayi = oyunVeriFromJson.Top_sa10sn;
                        score = oyunVeriFromJson.Skor;
                    }

                    
                }

                
            }
            


        }

        public int bir_yada_eksibir_oyun()
        {
            int sayi = rastgele_sayi.Next(-100, 100);
            if (sayi > 0)
            {
                return 1;
            }
            else if (sayi < 0)
            {
                return -1;
            }
            else
            {
                //0 olma durumu
                return 1;
            }
        }

        public void devam_et_oyun(System.Windows.Forms.Timer timer1, System.Windows.Forms.Timer timer2, System.Windows.Forms.Timer timer3)
        {
            timer1.Start();
            timer2.Start();
            timer3.Start();
            //Label sonuc_yazisi = new Label();
            //sonuc_yazisi.Text = "Oyun Duraklatıldı";
            //sonuc_yazisi.ForeColor = Color.Black;
            //sonuc_yazisi.Font = SonucFont;
            //sonuc_yazisi.Size = new Size(200, 100);
            //sonuc_yazisi.Location = new Point(300, 200);
            //sonuc_yazisi.Enabled = true;
            //Controls.Add(sonuc_yazisi);
            form.Controls.Remove(sonuc_yazisi);
        }

        public void durdur_oyun(System.Windows.Forms.Timer timer1, System.Windows.Forms.Timer timer2, System.Windows.Forms.Timer timer3)
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            //Label sonuc_yazisi = new Label();
            sonuc_yazisi.Text = "Oyun Duraklatıldı";
            sonuc_yazisi.ForeColor = Color.Black;
            sonuc_yazisi.Font = SonucFont;
            sonuc_yazisi.Size = new Size(200, 100);
            sonuc_yazisi.Location = new Point(300, 200);
            sonuc_yazisi.Enabled = true;
            form.Controls.Add(sonuc_yazisi);
        }

        /// <summary>
        /// ////
        /// </summary>
        /// <returns></returns>
        

        public OyunVeri GetOyunVeri()
        {

            List<Top> ToplarVeri = new List<Top>();
            for (int i = 0; i < toplar.Count; i++)
            {
                Top top_gecici = new Top
                {
                    Renk = toplar[i].BackColor,
                    Konum_x = toplar[i].Location.X,
                    Konum_y = toplar[i].Location.Y,
                    top_index = i,
                    X_eksen_hizi = x_ekseni_hizi[i],
                    Y_eksen_hizi = y_ekseni_hizi[i]


                };

                ToplarVeri.Add(top_gecici);
            }

            var veri = new OyunVeri
            {
                Skor = score,
                Ekran_top_say = ekrandaki_top,
                Top_sa10sn = top_10sn_sayi,
                Toplar = ToplarVeri
            };


            return veri;
        }

        public string sifrele(string gelenString)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(gelenString);
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider() )
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));

                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() {Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 } )
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    gelenString = Convert.ToBase64String(results, 0, results.Length);
                }
                
            }

            return gelenString;
        }

        public string sifreyi_coz(string gelenSifre)
        {
            byte[] data = Convert.FromBase64String(gelenSifre);
            
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));

                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    gelenSifre = UTF8Encoding.UTF8.GetString(results);
                }

            }

            return gelenSifre;


        }

        public void backupTiklaWrite()
        {
            try
            {
                OyunVeri oyunVeri = GetOyunVeri();

                string jsonToWrite = JsonConvert.SerializeObject(oyunVeri, Formatting.Indented);

                string sifrelenmis_jsonToWrite = sifrele(jsonToWrite);

                using (var writer = new StreamWriter(_path))
                {
                    System.Console.WriteLine("ahmet");
                    writer.Write(sifrelenmis_jsonToWrite);
                    
                }
            }
            catch (Exception )
            {
                
            }
        }

        public void restoreTikleRead()
        {
            try
            {
                string jsonFromFile;
                string sifre_cozulu;
                using (var reader = new StreamReader(_path))
                {
                    jsonFromFile = reader.ReadToEnd();
                }

                sifre_cozulu = sifreyi_coz(jsonFromFile);

                 oyunVeriFromJson = JsonConvert.DeserializeObject<OyunVeri>(sifre_cozulu);

            }
            catch (Exception )
            {
                // ignored
            }
        }

        






    }


    public class Top
    {
        public int X_eksen_hizi { get; set; }
        public int Y_eksen_hizi { get; set; }
        public Color Renk { get; set; }
        public int Konum_x { get; set; }
        public int Konum_y { get; set; }

        public int top_index { get; set; }
    }
    public class OyunVeri
    {
        public int Skor { get; set; }
        public int Ekran_top_say { get; set; }
        public int Top_sa10sn { get; set; }
        public List<Top> Toplar { get; set; }
    }

}
