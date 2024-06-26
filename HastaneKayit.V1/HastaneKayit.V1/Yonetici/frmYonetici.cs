using DevExpress.Utils.DirectXPaint;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
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
using DevExpress.XtraBars;

namespace HastaneKayit.V1.NewFolder1
{

    public partial class frmYonetici : Form
    {
        private readonly HastaneDataContext db = new HastaneDataContext();
        public string tcno;
        //public class Duyuru
        //{
        //    public int DuyuruID { get; set; }
        //    public string Baslik { get; set; }
        //    public string Icerik { get; set; }
        //    public DateTime Tarih { get; set; }
        //}
        public frmYonetici()
        {
            InitializeComponent();
           

        }

        private void frmYonetici_Load(object sender, EventArgs e)
        { 

            var yonetici = db.tbl_Yoneticis.FirstOrDefault(d => d.TC == tcno);
            if (yonetici != null)
            {
                labelControl1.Text = yonetici.Ad + " " + yonetici.Soyad;
                labelControl2.Text = yonetici.TC;
            }
            else
            {
                MessageBox.Show("Yonetici bulunamadı!");
            }
            gridView1.Columns.AddVisible("PersonelID", "Personel ID");
            gridView1.Columns.AddVisible("Ad", "Adı");
            gridView1.Columns.AddVisible("Soyad", "Soyadı");
            gridView1.Columns.AddVisible("Departman", "Departman");


            RefreshGrid();
            RefreshRandevuGrid();
            RefreshFaturaGrid();

        }
        private void RefreshGrid()
        {
            var personeller = db.tbl_Personels.ToList();


            gridControl1.DataSource = personeller;

        }
        private void RefreshRandevuGrid()
        {
            var randevular = (from r in db.tbl_Randevulars
                              join d in db.tbl_Doktorlars on r.DoktorID equals d.DoktorID
                              join h in db.tbl_Hastalars on r.HastaID equals h.HastaID
                              select new
                              {
                                  RandevuID = r.RandevuID,
                                  RandevuTarihi = r.RandevuTarihi,
                                  RandevuSaati = r.RandevuSaati,
                                  DoktorUzmanlık = d.DoktorUzmanlik,
                                  Doktor = d.DoktorAd + " " + d.DoktorSoyad,
                                  Hasta = h.HastaAd + " " + h.HastaSoyad,
                                  HastaYasi = YasHesapla(h.HastaDogumTarihi),
                              }).ToList();

            gridControl2.DataSource = randevular;
        }
        private void RefreshFaturaGrid()
        {
            var faturalar = db.tbl_FaturaBilgileris.ToList();

            gridControl3.DataSource = faturalar;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as tbl_Personel;

            if (selectedRow != null)
            {

                DialogResult result = MessageBox.Show("Seçili personeli silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Eğer kullanıcı "Evet" derse
                if (result == DialogResult.Yes)
                {
                    // Seçili personeli veritabanından sil
                    db.tbl_Personels.DeleteOnSubmit(selectedRow); // Silme işlemi
                    db.SubmitChanges(); // Değişiklikleri kaydet

                    // Grid'i güncelle
                    RefreshGrid();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz personeli seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            tbl_Personel yeniPersonel = new tbl_Personel
            {
                Ad = "",
                Soyad = "",
                Departman = "",


            };


            db.tbl_Personels.InsertOnSubmit(yeniPersonel);
            db.SubmitChanges();


            RefreshGrid();

            MessageBox.Show("Yeni personel başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView2.GetFocusedRow() as dynamic;

            if (selectedRow != null)
            {
                DialogResult result = MessageBox.Show("Seçili randevuyu silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int randevuID = selectedRow.RandevuID; // RandevuID'nin birincil anahtar olduğunu varsayıyoruz
                    var randevuToDelete = db.tbl_Randevulars.FirstOrDefault(r => r.RandevuID == randevuID);

                    if (randevuToDelete != null)
                    {
                        db.tbl_Randevulars.DeleteOnSubmit(randevuToDelete);
                        db.SubmitChanges();
                        RefreshRandevuGrid();
                    }

                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz randevuyu seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

            tbl_Randevular yeniRandevu = new tbl_Randevular
            {

                RandevuTarihi = DateTime.Now, // Örnek: Randevu tarihini mevcut tarih olarak ayarlıyoruz
                RandevuSaati = TimeSpan.FromHours(10), // Örnek: Randevu saati 10:00 olarak ayarlanıyor
                DoktorID = 1, // Örnek: Doktor ID'sini 1 olarak varsayıyoruz
                HastaID = 1 // Örnek: Hasta ID'sini 1 ol

            };


            db.tbl_Randevulars.InsertOnSubmit(yeniRandevu);
            db.SubmitChanges();


            RefreshRandevuGrid();

            MessageBox.Show("Yeni randevu başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            tbl_FaturaBilgileri yeniFatura = new tbl_FaturaBilgileri
            {


                HastaID = 1 // Örnek: Hasta ID'sini 1 ol

            };


            db.tbl_FaturaBilgileris.InsertOnSubmit(yeniFatura);
            db.SubmitChanges();


            RefreshFaturaGrid();

            MessageBox.Show("Yeni fatura başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {

            var selectedRow = gridView3.GetFocusedRow() as dynamic;

            if (selectedRow != null)
            {
                DialogResult result = MessageBox.Show("Seçili faturayı silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int faturaID = selectedRow.FaturaID; // FaturaID'nin birincil anahtar olduğunu varsayıyoruz
                    var faturaToDelete = db.tbl_FaturaBilgileris.FirstOrDefault(f => f.FaturaID == faturaID);

                    if (faturaToDelete != null)
                    {
                        db.tbl_FaturaBilgileris.DeleteOnSubmit(faturaToDelete);
                        db.SubmitChanges();
                        RefreshFaturaGrid();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz faturayı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {

            string duyuruMetni = memoEdit2.Text;

            if (!string.IsNullOrEmpty(duyuruMetni))
            {
                tbl_Duyuru yeniDuyuru = new tbl_Duyuru
                {
                    Baslik = "Yeni Duyuru",
                    Icerik = duyuruMetni,
                    Tarih = DateTime.Now
                };

                db.tbl_Duyurus.InsertOnSubmit(yeniDuyuru);
                db.SubmitChanges();
                MessageBox.Show("Duyuru başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (Application.OpenForms["frmAnaSayfa"] != null)
                {
                    frmAnaSayfa anaSayfaForm = (frmAnaSayfa)Application.OpenForms["frmAnaSayfa"];
                    anaSayfaForm.GosterMesaj("Yeni Duyuru", duyuruMetni);
                }



            }
            else
            {
                MessageBox.Show("Lütfen bir duyuru metni girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {

        }
        private int YasHesapla(DateTime dogumTarihi)
        {
            DateTime bugun = DateTime.Today;
            int yas = bugun.Year - dogumTarihi.Year;
            if (dogumTarihi.Date > bugun.AddYears(-yas))
                yas--;
            return yas;
        }

        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            var focusedRow = gridView1.GetFocusedRow() as tbl_Personel;

            if (focusedRow != null)
            {

                if (XtraMessageBox.Show($"Silmek istediğinize emin misiniz? {focusedRow.PersonelID}, {focusedRow.Ad}", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.tbl_Personels.DeleteOnSubmit(focusedRow);
                    db.SubmitChanges();
                    RefreshGrid();
                    XtraMessageBox.Show("Personel başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                XtraMessageBox.Show("Lütfen silmek istediğiniz personeli seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private int selectedRowHandle = GridControl.InvalidRowHandle;
        private void gridView2_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            //var p = MousePosition;
            //popupMenu1.ShowPopup(p);
            GridView view = sender as GridView;
            if (view != null)
            {
                // Seçili satırın indeksini al
                selectedRowHandle = view.FocusedRowHandle;

                // Popup menüyü göster
                Point pt = view.GridControl.PointToScreen(e.Point);
                popupMenu1.ShowPopup(pt);
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedRow = gridView2.GetFocusedRow() as dynamic;

            if (selectedRow != null)
            {
                DialogResult result = MessageBox.Show("Seçili randevuyu silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int randevuID = selectedRow.RandevuID;
                    var randevuToDelete = db.tbl_Randevulars.FirstOrDefault(r => r.RandevuID == randevuID);

                    if (randevuToDelete != null)
                    {
                        db.tbl_Randevulars.DeleteOnSubmit(randevuToDelete);
                        db.SubmitChanges();
                        RefreshRandevuGrid(); // Grid'i güncelle
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz randevuyu seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       

        private void gridView2_CustomDrawCell_1(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            GridView view = sender as GridView;
            if (e.RowHandle == view.FocusedRowHandle && view.IsDataRow(e.RowHandle))
            {
                e.Appearance.BackColor = Color.DarkOrange; // Seçili satırın arka plan rengi
                e.Appearance.ForeColor = Color.Black; // Seçili satırın metin rengi
            }
            else
            {
                e.Appearance.BackColor = Color.Empty; // Seçili olmayan satırların arka plan rengi
                e.Appearance.ForeColor = Color.Empty; // Seçili olmayan satırların metin rengi
            }
        }
    }

}