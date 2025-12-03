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
    public partial class OwnerAddService : Form
    {
        private OwnerServicePage parentForm;

        public OwnerAddService(OwnerServicePage parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void OwnerAddService_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Tambah event biar tbPrice cuma bisa angka
            tbPrice.KeyPress += TbPrice_KeyPress;
        }

        private void TbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Hanya izinkan angka dan tombol kontrol seperti Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Kolom harga hanya boleh angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbService.Content) ||
                string.IsNullOrWhiteSpace(tbPrice.Content) ||
                string.IsNullOrWhiteSpace(tbDescription.Content))
            {
                MessageBox.Show("Semua field harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string serviceName = tbService.Content;
            string price = tbPrice.Content;
            string description = tbDescription.Content;

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = "INSERT INTO services (name, price, description) VALUES (@name,  @price, @description)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", serviceName);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@description", description);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Data berhasil ditambahkan!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogActivity.Insert("Tambah Data",
                        $"Menambahkan layanan '{tbService.Content}' dengan harga Rp {tbPrice.Content}");

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
