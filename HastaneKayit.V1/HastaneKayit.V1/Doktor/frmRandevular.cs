using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HastaneKayit.V1
{
    public partial class frmRandevular : Form
    {
        private readonly HastaneDataContext db = new HastaneDataContext();
        public frmRandevular()
        {
            InitializeComponent();
        }

        private void frmRandevular_Load(object sender, EventArgs e)
        {
          
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (kontrol.Validate())
            {
                string doctorTC = textEdit1.Text.Trim();
                if (!string.IsNullOrEmpty(doctorTC))
                {
                    LoadAppointments(doctorTC);
                }
                else
                {
                    XtraMessageBox.Show("Lütfen bir doktor TC girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void LoadAppointments(string doctorTC)
        {
            var doctor = db.tbl_Doktorlars.FirstOrDefault(d => d.DoktorTC == doctorTC);
            if (doctor != null)
            {
                var appointments = db.tbl_Randevulars
                                      .Where(r => r.DoktorID == doctor.DoktorID)
                                      .Select(r => new
                                      {
                                          r.RandevuID,
                                          r.RandevuTarihi,
                                          r.RandevuSaati,
                                          HastaAdi = r.tbl_Hastalar.HastaAd,// Hasta adını al
                                          HastaSoyadi = r.tbl_Hastalar.HastaSoyad // Hasta soyadını al
                                      })
                                      .ToList();

                gridControl1.DataSource = appointments;
            }
            else
            {
                XtraMessageBox.Show("Girilen TC numarasına sahip bir doktor bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

