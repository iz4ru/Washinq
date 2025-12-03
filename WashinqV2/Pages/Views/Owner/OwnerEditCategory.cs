using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WashinqV2.Helpers;

namespace WashinqV2.Pages.Views.Owner
{
    public partial class OwnerEditCategory : Form
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string UnitType { get; set; }

        private OwnerCategoryPage parentForm;

        public OwnerEditCategory(int id, string categoryName, string unitType, OwnerCategoryPage parentForm)
        {
            InitializeComponent();
            this.ClientSize = new Size(480, 600);
            ID = id;
            CategoryName = categoryName;
            UnitType = unitType;

            tbCategory.Content = categoryName;
            tbUnitType.Content = unitType;

            this.parentForm = parentForm;
        }

        private void OwnerEditCategory_Load(object sender, EventArgs e)
        {
            tbCategory.Content = CategoryName;
            tbUnitType.Content = UnitType;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCategory.Content) ||
                string.IsNullOrWhiteSpace(tbUnitType.Content))
            {
                MessageBox.Show("Semua field harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CategoryName = tbCategory.Content;
            UnitType = tbUnitType.Content;

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = "UPDATE categories SET name = @name, unit_type = @unit_type WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", CategoryName);
                        cmd.Parameters.AddWithValue("@unit_type", UnitType);
                        cmd.Parameters.AddWithValue("@id", ID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                MessageBox.Show("Data berhasil diperbarui!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogActivity.Insert("Edit Data",
       $"Mengubah kategori '{tbCategory.Content} ' (ID:  {tbUnitType.Content})");

                parentForm.LoadData();
                this.Close();
            }
            catch (MySqlException ex) // Menangkap kesalahan MySQL
            {
                MessageBox.Show("Koneksi ke database gagal: " + ex.Message, "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit(); // Menutup aplikasi jika tidak dapat terhubung
                return; // Keluar dari metode jika koneksi gagal
            }
            catch (Exception ex) // Menangkap kesalahan umum lainnya
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit(); // Menutup aplikasi jika terjadi kesalahan
                return; // Keluar dari metode jika terjadi kesalahan
            }
        }

        private void llbBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
