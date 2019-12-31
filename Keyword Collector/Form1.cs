using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Collections;

namespace Keyword_Collector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int aratilan;
        IWebDriver driver = new ChromeDriver();
        Thread ara,say;
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog kaydet = new SaveFileDialog();
            kaydet.Filter = "Metin Dosyası|*.txt";
            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(kaydet.FileName);
                foreach (string item in listBox1.Items)
                {
                    sw.WriteLine(item);
                }
                sw.Close();
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Remove(listBox2.SelectedItem);
                label4.Text = "Üretilen : " + listBox2.Items.Count + " Kelime";
            }
            catch
            {
                MessageBox.Show("Kayıt Seçilmedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                label4.Text = "Üretilen : 0 Kelime";
            }
            catch
            {
                MessageBox.Show("Temizlenecek Kayıt Bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int ayni = 0;
            foreach (string item in listBox2.Items)
            {
                foreach(string kontrol in listBox1.Items)
                {
                    if(kontrol==item)
                    {
                        ayni++;
                    }
                }
                if(ayni==0)
                {
                    listBox1.Items.Add(item);      
                }
                else
                {
                    ayni = 0;
                }
                
            }
            label5.Text = "Arınmış Toplam Kelime : " + listBox1.Items.Count;
            int[] sira = new int[listBox1.Items.Count];
            int yer = 0;
            foreach(string item in listBox1.Items)
            {
                int denk = 0;
                foreach(string kontrol in listBox2.Items)
                {
                    if(kontrol==item)
                    {
                        denk++;
                    }
                }
                sira[yer] = denk;
                yer++;
            }
            for(int i=0;i<listBox1.Items.Count;i++)
            {
                listBox1.Items[i]=listBox1.Items[i]+"( "+sira[i]+" )";
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                label5.Text = "Arınmış Toplam Kelime : " + listBox1.Items.Count;
            }
            catch
            {
                MessageBox.Show("Temizlenecek Kayıt Bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
                label5.Text = "Arınmış Toplam Kelime : " + listBox1.Items.Count;
            }
            catch
            {
                MessageBox.Show("Kayıt Seçilmedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ara = new Thread(new ThreadStart(doldur));
            say = new Thread(new ThreadStart(kontrol));
            ara.Start();            
            say.Start();
        }
        
        void doldur()
        {
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button2.Enabled = false;
            button8.Enabled = true;
            int sayi = Convert.ToInt32(textBox2.Text);
            aratilan = 1;
            listBox2.Items.Clear();
            listBox2.Items.Add(textBox1.Text);
            string link = "https://www.google.com/search?q=" + textBox1.Text;
            driver.Navigate().GoToUrl(link);
            for (int i = 1; i < 5; i++)
            {
                try
                {
                    string metin = driver.FindElement(By.XPath("//*[@id='brs']/g-section-with-header/div[2]/div[1]/p[" + i + "]/a")).Text.Trim().ToString();
                    listBox2.Items.Add(metin);
                }
                catch { }
            }
            for (int i = 1; i < 5; i++)
            {
                try
                {
                    string metin = driver.FindElement(By.XPath("//*[@id='brs']/g-section-with-header/div[2]/div[2]/p[" + i + "]/a")).Text.Trim().ToString();
                    listBox2.Items.Add(metin);
                }
                catch { }
            }
            if(listBox2.Items.Count<2)
            {
                MessageBox.Show("Arama metnini daha sade ve anlaşılır olacak şekilde değiştirin", "Arama Başarısız!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return;
            }
            while (listBox2.Items.Count < sayi)
            {
                link = "https://www.google.com/search?q=" + listBox2.Items[aratilan];
                driver.Navigate().GoToUrl(link);
                for (int i = 1; i < 5; i++)
                {
                    if (listBox2.Items.Count < sayi)
                    {
                        try
                        {
                            string metin = driver.FindElement(By.XPath("//*[@id='brs']/g-section-with-header/div[2]/div[1]/p[" + i + "]/a")).Text.Trim().ToString();
                            listBox2.Items.Add(metin);
                        }
                        catch { }
                    }
                    else
                    {                      
                        return;
                    }

                }
                for (int i = 1; i < 5; i++)
                {
                    if (listBox2.Items.Count < sayi)
                    {
                        try
                        {
                            string metin = driver.FindElement(By.XPath("//*[@id='brs']/g-section-with-header/div[2]/div[2]/p[" + i + "]/a")).Text.Trim().ToString();
                            listBox2.Items.Add(metin);
                        }
                        catch { }
                    }
                    else
                    {                       
                        return;
                    }
                }
                aratilan++;
            }
        }
        void kontrol()
        {
            while(true)
            {
                label4.Text = "Üretilen : " + listBox2.Items.Count + " Kelime";
                if(ara.ThreadState!=ThreadState.Running)
                {
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    button2.Enabled = true;
                    button8.Enabled = false;
                    return;
                }
            }
        }

        private void listBox1_SizeChanged(object sender, EventArgs e)
        {
            label5.Text = "Arınmış Toplam Kelime : " + listBox1.Items.Count;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try { ara.Abort(); }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            driver.Navigate().GoToUrl("https://google.com");
        }
    }
}
