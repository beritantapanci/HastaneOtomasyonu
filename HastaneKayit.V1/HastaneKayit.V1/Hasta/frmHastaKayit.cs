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
    public partial class frmHastaKayit : Form
    {
        private readonly HastaneDataContext db = new HastaneDataContext();
        public frmHastaKayit()
        {
            InitializeComponent();
            this.AcceptButton = btn_KaydiTamamla;
        }

  

       
        private void btn_HastaGris_Click(object sender, EventArgs e)
        {
            frmHastaGris frm = new frmHastaGris();
            frm.Show();
            this.Hide();
        }

        private void btn_KaydiTamamla_Click(object sender, EventArgs e)
        {
            if (kontrol.Validate())

            {
                if (db.tbl_Hastalars.Any(h => h.HastaTC == txt_tc.Text))
                {
                    MessageBox.Show("Bu TC ile kayıtlı bir hasta zaten var. Şifrenizi mi unuttunuz?");
                    return;
                }
                //DateTime dogumTarihi = DateTime.Today.AddYears(-Convert.ToInt32(txt_yas.Text));
                tbl_Hastalar yeniHasta = new tbl_Hastalar
                {
                    HastaAd = txt_isim.Text,
                    HastaSoyad = txt_soyisim.Text,
                    HastaTC = txt_tc.Text,
                    HastaDogumTarihi = DateTime.Parse(txt_yas.Text),
                    HastaCinsiyet = comboBoxEdit_cinsiyet.Text,
                    HastaHesKodu = txt_hes_kodu.Text,
                    HastaTelefon = txt_telefon.Text,
                    HastaMail = txt_mail.Text,
                    HastaSifre = txt_sifre.Text,
                    HastaAdres = memoEdit_adres.Text
                };

                
                db.tbl_Hastalars.InsertOnSubmit(yeniHasta);
                db.SubmitChanges();

                MessageBox.Show("Kaydınız Başarıyla Yapıldı Şifreniz: " + txt_sifre.Text);
                
                txt_isim.Clear();
                txt_soyisim.Clear();
                txt_yas.Clear();
                comboBoxEdit_cinsiyet.SelectedIndex = -1; // veya isteğe bağlı bir başlangıç değeri
                txt_hes_kodu.Clear();
                txt_mail.Clear();
                txt_telefon.Clear();
                txt_sifre.Clear();
                txt_tc.Clear();

            }
        }
    }
}
