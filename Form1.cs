using EbubekirBastamatxtokuma;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EBSVCFRead
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        Thread th;
        private string dosyaYolu = "";
        DataTable dt = new DataTable("CompanyName");
        private void Form1_Load(object sender, EventArgs e)
        {
            dosyabul(Application.StartupPath);
            dt.Columns.Add("CompanyName");
            dt.Columns.Add("NumberOne");
            dt.Columns.Add("Numbertwo");
            if (dosyaYolu != "")
            {
                string[] dizi = BekraTxtOkuma.TxtimportDizi(dosyaYolu, false);
                foreach (string item in dizi)
                {
                    string item1 = item.Replace(";", "").Replace("?", "").ToString();
                    if (item1 == "BEGIN:VCARD")
                    {
                        ad = ""; numara1 = ""; numara2 = "";

                    }
                    else
                    {
                        if (item1.IndexOf("N:") != -1)
                        {
                            ad = item1.Split(':')[1];
                        }
                        else if (item1.IndexOf("TELWORKVOICE:") != -1)
                        {
                            numara1 = item1.Split(':')[1];
                        }
                        else if (item1.IndexOf("TELCELLVOICE:") != -1)
                        {
                            numara2 = item1.Split(':')[1];
                        }
                    }
                    if (item1 == "END:VCARD")
                    {
                        DataRow dr = dt.NewRow();
                        dr["CompanyName"] = ad;
                        dr["NumberOne"] = numara1;
                        dr["Numbertwo"] = numara2;
                        dt.Rows.Add(dr);
                        //verikaydet(, , );
                    }
                }
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("VCard Dosyası Bulunamadı.", "EBS VCard Okuyucu", MessageBoxButtons.OK);
            }
        }
        private void dosyabul(string yol)
        {
            string[] isimlistesi = Directory.GetFiles(yol);
            foreach (var dosya in isimlistesi)
            {
                if (dosya.IndexOf(".vcf") != -1)
                {
                    dosyaYolu = dosya;
                    break;
                }
            }


        }
        string ad, numara1, numara2;

        private void textBoxX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("CompanyName Like '%{0}%'", textBoxX1.Text);
                dataGridView1.DataSource = dv.ToTable();
            }
        }

        private void verikaydet(params string[] veri)
        {
            dataGridView1.Rows.Add(
             veri[0], veri[1], veri[2]
            );
            dataGridView1.PerformLayout();
        }

    }
}
