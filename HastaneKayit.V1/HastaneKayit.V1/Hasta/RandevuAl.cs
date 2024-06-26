using DevExpress.XtraEditors;
using System;
using System.Data.Linq;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;

namespace HastaneKayit.V1.Hasta
{
    public partial class RandevuAl : Form
    {
        private readonly HastaneDataContext dataContext = new HastaneDataContext();
        public string tcno;
        public RandevuAl()
        {
            InitializeComponent();
            DoktorlariComboBoxDoldur();
            BolumleriComboBoxDoldur();
            AcceptButton = simpleButton2;
        }

        private void DoktorlariComboBoxDoldur()
        {
            try
            {
                var doktorlar = from doktor in dataContext.tbl_Doktorlars
                                select doktor.DoktorAd;

                comboBoxEdit2.Properties.Items.AddRange(doktorlar.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Doktorlar yüklenirken bir hata oluştu: " + ex.Message);
            }
        }

        private void BolumleriComboBoxDoldur()
        {
            try
            {
                var bolumler = from bolum in dataContext.tbl_Bolums
                               select bolum.BolumAdi;

                comboBoxEdit1.Properties.Items.AddRange(bolumler.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bölümler yüklenirken bir hata oluştu: " + ex.Message);
            }
        }

        private bool RandevuVarMi(int doktorID, DateTime randevuTarihi, TimeSpan randevuSaati)
        {
            var ayniGunRandevular = from randevu in dataContext.tbl_Randevulars
                                    where randevu.DoktorID == doktorID &&
                                          randevu.RandevuTarihi == randevuTarihi
                                    select randevu;

            foreach (var randevu in ayniGunRandevular)
            {
                // Aynı doktorun aynı gün içinde farklı saatlerde randevusu varsa
                // Bu durumda aynı gün içinde başka bir randevu olduğu anlamına gelir.
                if (randevu.RandevuSaati == randevuSaati)
                {
                    return true;
                }
            }

            // Aynı doktorun aynı gün ve aynı saatte randevusu yoksa false döndür
            return false;
        }

        private bool SaatAraligiGecerliMi(TimeSpan randevuSaati)
        {
            return randevuSaati >= TimeSpan.FromHours(8) && randevuSaati <= TimeSpan.FromHours(18);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (kontrol.Validate())
            {
                string hastaTc = textEdit1.Text;
                string doktorAdi = comboBoxEdit2.SelectedItem?.ToString();
                string bolumAdi = comboBoxEdit1.SelectedItem?.ToString();
                DateTime randevuTarihi = dateEdit1.DateTime.Date;
                TimeSpan randevuSaati = timeEdit1.Time.TimeOfDay;
                try
                {
                    if (!SaatAraligiGecerliMi(randevuSaati))
                    {
                        MessageBox.Show("Randevu saati 08:00 ile 18:00 arasında olmalıdır.");
                        return;
                    }

                    int doktorID = (from doktor in dataContext.tbl_Doktorlars
                                    where doktor.DoktorAd == doktorAdi
                                    select doktor.DoktorID).FirstOrDefault();

                    int hastaID = (from hasta in dataContext.tbl_Hastalars
                                   where hasta.HastaTC == hastaTc
                                   select hasta.HastaID).FirstOrDefault();

                    if (doktorID == 0 || hastaID == 0)
                    {
                        MessageBox.Show("Geçerli bir doktor ve hasta seçmelisiniz.");
                        return;
                    }

                    if (RandevuVarMi(doktorID, randevuTarihi, randevuSaati))
                    {
                        MessageBox.Show("Bu saatte zaten bir randevu mevcut. Lütfen başka bir saat seçin.");
                        return;
                    }

                    var yeniRandevu = new tbl_Randevular
                    {
                        RandevuSaati = randevuSaati,
                        RandevuTarihi = randevuTarihi,
                        DoktorID = doktorID,
                        HastaID = hastaID
                    };

                    dataContext.tbl_Randevulars.InsertOnSubmit(yeniRandevu);
                    dataContext.SubmitChanges();
                   
                    //RandevuBilgisiEpostaGonder(hastaTc, doktorAdi, randevuTarihi, randevuSaati);

                    MessageBox.Show("Randevu başarıyla oluşturuldu! Randevu bilgileriniz mail adresinize gönderilmiştir.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Randevu oluşturulurken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void RandevuAl_Load(object sender, EventArgs e)
        {

            var yonetici = dataContext.tbl_Yoneticis.FirstOrDefault(d => d.TC == tcno);
            if (yonetici != null)
            {
                
               textEdit1.Text = yonetici.TC;
            }
            else
            {
                MessageBox.Show("Yonetici bulunamadı!");
            }


           


        }
    }
        //private void RandevuBilgisiEpostaGonder(string hastaTc, string doktorAdi, DateTime randevuTarihi, TimeSpan randevuSaati)
        //{
        //    try
        //    {
        //        var hasta = (from h in dataContext.tbl_Hastalars
        //                     where h.HastaTC == hastaTc
        //                     select h).FirstOrDefault();

        //        if (hasta != null)
        //        {
        //            MailMessage mail = new MailMessage();
        //            SmtpClient smtpSunucusu = new SmtpClient("smtp.gmail.com");

        //            mail.From = new MailAddress("sizin_email@gmail.com");
        //            mail.To.Add(hasta.HastaMail);
        //            mail.Subject = "Randevu Bilgileriniz";
        //            mail.Body = $"Merhaba {hasta.HastaAd},\n\nRandevunuz başarıyla oluşturulmuştur. İşte detaylar:\n\nDoktor: {doktorAdi}\nRandevu Tarihi: {randevuTarihi.ToShortDateString()}\nRandevu Saati: {randevuSaati.ToString(@"hh\:mm")}\n\nSağlıklı günler dileriz.";

        //            smtpSunucusu.Port = 587;
        //            smtpSunucusu.Credentials = new System.Net.NetworkCredential("sizin_email@gmail.com", "sizin_sifreniz");
        //            smtpSunucusu.EnableSsl = true;

        //            smtpSunucusu.Send(mail);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Hasta bulunamadı!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Email gönderilirken bir hata oluştu: " + ex.Message);
        //    }
        //}
    
}
