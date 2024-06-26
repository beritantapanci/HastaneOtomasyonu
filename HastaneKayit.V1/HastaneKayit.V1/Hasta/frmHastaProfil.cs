using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HastaneKayit.V1.Hasta
{
    public partial class frmHastaProfil : Form
    {
        private readonly HastaneDataContext db = new HastaneDataContext();
        public string tcno;
        public frmHastaProfil()
        {
            InitializeComponent();
        }







        private void btn_Guncellle_Click(object sender, EventArgs e)
        {
            var hasta = db.tbl_Hastalars.FirstOrDefault(h => h.HastaTC == tcno);
            if (hasta != null)
            {
                hasta.HastaHesKodu = txt_hes_kodu.Text;
                hasta.HastaMail = txt_mail.Text;
                hasta.HastaDogumTarihi = DateTime.Parse(txt_yas.Text);
                hasta.HastaTelefon = txt_telefon.Text;
                hasta.HastaSifre = txt_sifre.Text;

                db.SubmitChanges();
                MessageBox.Show("Bilgileriniz Güncellendi...");
            }
        }

        private void btn_ana_sayfa_Click(object sender, EventArgs e)
        {
            frmAnaSayfa frm = new frmAnaSayfa();
            frm.Show();
            this.Hide();
        }

        private void btn_Randevu_Al_Click(object sender, EventArgs e)
        {
            RandevuAl frm = new RandevuAl();
        
            frm.Show();
            this.Hide();
        }



        private void frmHastaProfil_Load(object sender, EventArgs e)
        {

            var hasta = db.tbl_Hastalars.FirstOrDefault(h => h.HastaTC == tcno);
            if (hasta != null)
            {
                labelControl1.Text = hasta.HastaAd + " " + hasta.HastaSoyad;
                txt_hes_kodu.Text = hasta.HastaHesKodu;
                txt_mail.Text = hasta.HastaMail;
                txt_yas.Text = hasta.HastaDogumTarihi.ToString();
                txt_telefon.Text = hasta.HastaTelefon;
                comboBoxEdit_cinsiyet.Text = hasta.HastaCinsiyet;
                txt_sifre.Text = hasta.HastaSifre;
            }
            var hastaRandevulari = from randevu in db.tbl_Randevulars
                                   join hastalar in db.tbl_Hastalars on randevu.HastaID equals hastalar.HastaID
                                   join doktorlar in db.tbl_Doktorlars on randevu.DoktorID equals doktorlar.DoktorID
                                   where hastalar.HastaTC == tcno
                                   select new
                                   {
                                       randevu.RandevuTarihi,
                                       randevu.RandevuSaati,
                                       DoktorAdSoyad = doktorlar.DoktorAd + " " + doktorlar.DoktorSoyad, 
                                       doktorlar.DoktorUzmanlik
                                   };


            var hastaRandevuListesi = hastaRandevulari.ToList();


      

            gridControl1.DataSource = hastaRandevuListesi;


            //var hastaMuayeneleri = from muayene in db.tbl_MuayeneBilgileris
            //                       join hastalar in db.tbl_Hastalars on muayene.HastaID equals hastalar.HastaID
            //                       where hastalar.HastaTC == tcno
            //                       select new
            //                       {
            //                           muayene.MuayeneID,

            //                           muayene.MuayeneTarihi,
            //                           muayene.Teşhis,
            //                           hastalar.HastaAd, // Hasta adını da alabilirsiniz
            //                           hastalar.HastaSoyad // Hasta soyadını da alabilirsiniz
            //                       };

            //gridControl1.DataSource = hastaMuayeneleri.ToList();

            //var randevular = from randevu in db.tbl_Randevulars
            //                 join doktor in db.tbl_Doktorlars on randevu.DoktorID equals doktor.DoktorID
            //                 where randevu.HastaID == hasta.HastaID
            //                 select new
            //                 {
            //                     randevu.RandevuID,
            //                     RandevuTarihi = randevu.RandevuTarihi.HasValue ? randevu.RandevuTarihi.Value.ToShortDateString() : string.Empty,
            //                     RandevuSaati = randevu.RandevuSaati.ToString(),
            //                     DoktorAdi = doktor.DoktorAd + " " + doktor.DoktorSoyad
            //                 };

            //gridControl2.DataSource = randevular.ToList();

        }






    }
}
