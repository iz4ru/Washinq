using Guna.UI2.WinForms.Suite;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminEditService : Form
    {
        public int ID { get; set; }
        public string ServiceName { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

        private AdminServicePage parentForm;
        public AdminEditService(int id, string serviceName, int price, string description, AdminServicePage parent)
        {
            InitializeComponent();

            ID = id;
            ServiceName = serviceName;
            Price = price;
            Description = description;

            // Value awal untuk text box
            tbService.Content = serviceName;
            tbPrice.Content = price.ToString();
            tbDescription.Content = description;

            parentForm = parent;
        }

        private void AdminEditService_Load(object sender, EventArgs e)
        {
            tbService.Content = ServiceName;
            tbPrice.Content = Price.ToString();
            tbDescription.Content = Description;

            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbService.Content) ||
                string.IsNullOrWhiteSpace(tbPrice.Content) ||
                string.IsNullOrWhiteSpace(tbDescription.Content))
            {
                MessageBox.Show("Semua field harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi input harga agar hanya angka
            if (!int.TryParse(tbPrice.Content, out int validPrice))
            {
                MessageBox.Show("Harga harus berupa angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ServiceName = tbService.Content;
            Price = validPrice;
            Description = tbDescription.Content;

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = "UPDATE services SET name = @name, price_per_kg = @price_per_kg, description = @description WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", ServiceName);
                        cmd.Parameters.AddWithValue("@price_per_kg", Price);
                        cmd.Parameters.AddWithValue("@description", Description);
                        cmd.Parameters.AddWithValue("@id", ID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                MessageBox.Show("Data berhasil diperbarui!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
