using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HastaneKayit.V1.Doktor
{
    public partial class frmDoktorProfil : Form
    {
        private readonly HastaneDataContext db = new HastaneDataContext();
        public string tcno;
        public frmDoktorProfil()
        {
            InitializeComponent();
        }

      

       

        
     

        private void btn_Guncelle_Click(object sender, EventArgs e)
        {
            var doktor = db.tbl_Doktorlars.FirstOrDefault(d => d.DoktorTC == tcno);
            if (doktor != null)
            {
                doktor.DoktorHesKodu = txt_hes_kodu.Text;
                doktor.DoktorMail = txt_mail.Text;
                doktor.DoktorDoğumTarihi = DateTime.Parse(txt_yas.Text); 
                doktor.DoktorTelefon = txt_telefon.Text;
                doktor.DoktorSifre = txt_sifre.Text;
                doktor.DoktorTC = txt_tc.Text;
                doktor.DoktorPolikinik = comboBoxEdit_poliklinik.Text;
                doktor.DoktorCinsiyet = comboBoxEdit_cinsiyet.Text;
                db.SubmitChanges();
                MessageBox.Show("Bilgileriniz Güncellendi...");
            }
            else
            {
                MessageBox.Show("Doktor bulunamadı!");
            }
        }

        private void btn_ana_sayfa_Click(object sender, EventArgs e)
        {
            frmAnaSayfa frm = new frmAnaSayfa();
            frm.Show();
            this.Hide();
        }

        private void btn_Randevular_Click(object sender, EventArgs e)
        {
            frmRandevular frm = new frmRandevular();
            frm.Show();
            this.Hide();
        }
        private void frmDoktorProfil_Load(object sender, EventArgs e)
        {
            var doktor = db.tbl_Doktorlars.FirstOrDefault(d => d.DoktorTC == tcno);
            if (doktor != null)
            {
                labelControl1.Text = doktor.DoktorAd + " " + doktor.DoktorSoyad;
                txt_hes_kodu.Text = doktor.DoktorHesKodu;
                txt_mail.Text = doktor.DoktorMail;
                txt_yas.Text = doktor.DoktorDoğumTarihi.ToString();
                txt_telefon.Text = doktor.DoktorTelefon;
                comboBoxEdit_cinsiyet.Text = doktor.DoktorCinsiyet;
                txt_sifre.Text = doktor.DoktorSifre;
                comboBoxEdit_poliklinik.Text = doktor.DoktorPolikinik;
                txt_uzmanlik.Text = doktor.DoktorUzmanlik;

                txt_tc.Text = doktor.DoktorTC;
            }
            else
            {
                MessageBox.Show("Doktor bulunamadı!");
            }
        }

    }
}
