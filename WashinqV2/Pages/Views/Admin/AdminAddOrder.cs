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
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminAddOrder : Form
    {
        private int customerId;
        public AdminAddOrder(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
        }

        private void AdminAddOrder_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Disable close button
            this.ControlBox = false;

            //LoadCustomerInfo(customerId);
            LoadService();
            LoadCustomerInfo(customerId);
        }

        private void LoadService()
        {
            cbxService.Items = new string[0];

            using (var conn = Database.Database.GetConnection())
            {
                string query = "SELECT DISTINCT name FROM services ORDER BY name ASC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    var list = new List<string>();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["name"].ToString());
                        }
                    }

                    cbxService.Items = list.ToArray();
                }
            }
        }

        private void LoadCustomerInfo(int customerId)
        {
            using (var conn = Database.Database.GetConnection())
            {
                string query = "SELECT name FROM customers WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", customerId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lbCustName.Text = reader["name"].ToString();
                        }
                    }
                }
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (cbxService.SelectedItem == null)
            {
                MessageBox.Show("Pilih layanan terlebih dahulu.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(tbTotalKg.Content.ToString(), out decimal totalKg) || totalKg <= 0)
            {
                MessageBox.Show("Masukkan total kilogram yang valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string serviceName = cbxService.SelectedItem.ToString();
            decimal pricePerKg = 0;

            // Ambil service_id dulu
            int serviceId = 0;
            using (var conn = Database.Database.GetConnection())
            {
                string queryService = "SELECT id, price_per_kg FROM services WHERE name = @name LIMIT 1";
                using (var cmd = new MySqlCommand(queryService, conn))
                {
                    cmd.Parameters.AddWithValue("@name", serviceName);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            serviceId = Convert.ToInt32(reader["id"]);
                            pricePerKg = Convert.ToDecimal(reader["price_per_kg"]);
                        }
                        else
                        {
                            MessageBox.Show("Service tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            // Hitung total
            decimal totalPrice = pricePerKg * totalKg;

            // Insert ke orders
            using (var conn = Database.Database.GetConnection())
            {
                string insertQuery = @"
        INSERT INTO orders 
        (user_id, customer_id, service_id, total_kg, total_price, paid, payment, notes, submitted_at, taken_at)
        VALUES 
        (@userId, @customerId, @serviceId, @totalKg, @totalPrice, @paid, @payment, @notes, @submittedAt, @takenAt)
    ";

                using (var cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", UserSession.id);
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@serviceId", serviceId);
                    cmd.Parameters.AddWithValue("@totalKg", totalKg);
                    cmd.Parameters.AddWithValue("@totalPrice", totalPrice);
                    cmd.Parameters.AddWithValue("@paid", 0); // belum dibayar
                    cmd.Parameters.AddWithValue("@payment", "cash");
                    cmd.Parameters.AddWithValue("@notes", DBNull.Value); // kosong
                    cmd.Parameters.AddWithValue("@submittedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@takenAt", DBNull.Value);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Order berhasil ditambahkan. Total: " + totalPrice.ToString("C"), "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var payForm = new AdminCustomerPay(customerId, totalPrice);
                        payForm.ShowDialog();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menyimpan order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Membatalkan transaksi akan menghapus data customer yang baru diinput. Lanjutkan?",
                "Batalkan Transaksi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var conn = Database.Database.GetConnection())
                    {
                        string query = "DELETE FROM customers WHERE id = @id";
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", customerId);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Data customer telah dihapus.", "Dibatalkan", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Balik ke form sebelumnya
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Koneksi ke database gagal: " + ex.Message, "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
