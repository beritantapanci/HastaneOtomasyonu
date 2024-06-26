using HastaneKayit.V1.Doktor;
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
    public partial class frmHastaGris : Form
    {
        HastaneDataContext db = new HastaneDataContext();
        public frmHastaGris()
        {
            InitializeComponent();
            this.AcceptButton = btn_girisYap;
        }

       


       

        

        private void btn_girisYap_Click(object sender, EventArgs e)
        {
            if (kontrol.Validate())
            {


                var hasta = (from d in db.tbl_Hastalars
                             where d.HastaTC == txt_tc.Text.Trim() && d.HastaSifre == txt_sifre.Text.Trim()
                             select d).FirstOrDefault();

                //if (textEdit1.Text == string.Empty || textEdit2.Text == string.Empty)
                //{
                //    MessageBox.Show("Boş alanları doldur");
                //}

                if (hasta != null)
                {
                    MessageBox.Show($"Hoş geldiniz  {hasta.HastaAd} {hasta.HastaSoyad}");

                    frmHastaProfil frm = new frmHastaProfil();
                    frm.tcno = hasta.HastaTC;
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Hatalı Giriş.....");
                }
            }
        }

        private void btn_KayitOl_Click(object sender, EventArgs e)
        {
            frmHastaKayit dr = new frmHastaKayit();
            dr.Show();
            this.Hide();
        }

        private void btn_SifremiUnuttum_Click(object sender, EventArgs e)
        {
            frmSifremiUnuttum hst = new frmSifremiUnuttum();
            hst.Show();
            this.Hide();
        }
    }
}
