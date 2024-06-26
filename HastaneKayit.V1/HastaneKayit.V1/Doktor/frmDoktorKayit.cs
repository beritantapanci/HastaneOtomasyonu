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
    public partial class frmDoktorKayit : Form
    {
        private readonly HastaneDataContext db = new HastaneDataContext();
        public frmDoktorKayit()
        {
            InitializeComponent();
            this.AcceptButton = btn_KaydiTamamla;
        }

   

        

        private void btn_DoktorGiris_Click(object sender, EventArgs e)
        {
            frmDoktorGris frm = new frmDoktorGris();
            frm.Show();
            this.Hide();
        }

        private void btn_KaydiTamamla_Click(object sender, EventArgs e)
        {
            if (kontrol.Validate())
            {

                if (db.tbl_Doktorlars.Any(h => h.DoktorTC == txt_tc.Text))
                {
                    MessageBox.Show("Bu TC ile kayıtlı bir doktor zaten var. Şifrenizi mi unuttunuz?");
                    return;
                }
                tbl_Doktorlar yeniDoktor = new tbl_Doktorlar
                {
                    DoktorAd = txt_isim.Text,
                    DoktorSoyad = txt_soyisim.Text,
                    DoktorTC = txt_tc.Text,
                  DoktorDoğumTarihi = DateTime.Parse(txt_yas.Text),
                    DoktorCinsiyet = comboBox_cinsiyet.Text,
                    DoktorHesKodu = txt_hes_kodu.Text,
                    DoktorTelefon = txt_telefon.Text,
                    DoktorMail = txt_mail.Text,
                    DoktorUzmanlik = combobox_uzmanlık.Text,
                    DoktorSifre = txt_sifre.Text,
                    DoktorAdres = memoEdit_adres.Text,
                    DoktorPolikinik = txt_poliklinik.Text

                };


                db.tbl_Doktorlars.InsertOnSubmit(yeniDoktor);
                db.SubmitChanges();

                MessageBox.Show("Kaydınız Başarıyla Yapıldı Şifreniz: " + txt_sifre.Text);
            }

        }
    }
}
