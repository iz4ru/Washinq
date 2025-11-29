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

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminAddCategory : Form
    {
        private AdminCategoryPage parentForm;

        public AdminAddCategory(AdminCategoryPage parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void AdminAddCategory_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCategory.Content) ||
                string.IsNullOrWhiteSpace(tbUnitType.Content))
            {
                MessageBox.Show("Semua field harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string categoryName = tbCategory.Content;
            string unitType = tbUnitType.Content;

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = "INSERT INTO categories (name, unit_type) VALUES (@name, @unit_type)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", categoryName);
                        cmd.Parameters.AddWithValue("@unit_type", unitType);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Data berhasil ditambahkan!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    parentForm.LoadData();
                    this.Close();
                }
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
