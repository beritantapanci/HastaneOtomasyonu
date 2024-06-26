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
    public partial class frmSifremiUnuttum : Form
    {
        private readonly HastaneDataContext db = new HastaneDataContext();
        public frmSifremiUnuttum()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text == "" || textEdit2.Text == "")
            {
                MessageBox.Show("TC ve Şifre Boş Bırakılamaz!.....");
            }
            else
            {
                var yonetici = (from y in db.tbl_Yoneticis
                                where y.TC == textEdit1.Text.Trim()
                                select y).FirstOrDefault();

                if (yonetici != null) 
                {
                    yonetici.Sifre = textEdit2.Text.Trim(); 
                    db.SubmitChanges(); 
                    MessageBox.Show("Yönetici Şifresi Güncellendi...");
                }
                else 
                {
                    var doktor = (from d in db.tbl_Doktorlars
                                  where d.DoktorTC == textEdit1.Text.Trim()
                                  select d).FirstOrDefault();

                    if (doktor == null) 
                    {
                        var hasta = (from h in db.tbl_Hastalars
                                     where h.HastaTC == textEdit1.Text.Trim()
                                     select h).FirstOrDefault();

                        if (hasta != null) 
                        {
                            hasta.HastaSifre = textEdit2.Text.Trim(); 
                            db.SubmitChanges(); 
                            MessageBox.Show("Hasta Şifresi Güncellendi...");
                        }
                        else
                        {
                            MessageBox.Show("Hasta bulunamadı!");
                        }
                    }
                    else 
                    {
                        doktor.DoktorSifre = textEdit2.Text.Trim(); 
                        db.SubmitChanges();
                        MessageBox.Show("Doktor Şifresi Güncellendi...");
                    }
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
          frmAnaSayfa hst = new frmAnaSayfa();
            hst.Show();
            this.Hide();
        }
    }
}
