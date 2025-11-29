using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminAddOrder : Form
    {
        private int customerId;

        private int selectedCategoryId;
        private int selectedServiceId;
        private decimal selectedServicePrice;
        private string selectedUnitType;

        // Kategori: nama -> (id, unit_type)
        private readonly Dictionary<string, (int Id, string UnitType)> categoryMap =
            new Dictionary<string, (int, string)>();

        // Service: nama -> (id, price)
        private readonly Dictionary<string, (int Id, decimal Price)> serviceMap =
            new Dictionary<string, (int, decimal)>();

        private readonly CultureInfo idCulture = new CultureInfo("id-ID");

        public AdminAddOrder(int customerId)
        {
            InitializeComponent();
            this.ClientSize = new Size(480, 600);
            this.customerId = customerId;
        }

        private void AdminAddOrder_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Default label
            lbUnitType.Text = "none";
            lbTotalPrice.Text = "Rp 0";

            // Hanya angka + koma di kuantitas
            tbTotalQty.KeyPress += tbTotalQty_KeyPress;
            tbTotalQty.ContentChanged += tbTotalQty_ContentChanged;

            LoadCategory();
            LoadCustomerInfo(customerId);
        }

        private void LoadCategory()
        {
            categoryMap.Clear();
            cbxCategory.Items = new string[0];

            using (var conn = Database.Database.GetConnection())
            {
                string query = @"SELECT id, name, unit_type 
                                 FROM categories 
                                 ORDER BY name ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    var list = new List<string>();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string name = reader["name"].ToString();
                            string unitType = reader["unit_type"].ToString();

                            list.Add(name);
                            categoryMap[name] = (id, unitType);
                        }
                    }

                    cbxCategory.Items = list.ToArray();
                }
            }

            cbxCategory.SelectedIndexChanged += cbxCategory_SelectedIndexChanged;
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCategory.SelectedItem == null) return;

            string selectedName = cbxCategory.SelectedItem.ToString();
            if (!categoryMap.TryGetValue(selectedName, out var info))
                return;

            selectedCategoryId = info.Id;
            selectedUnitType = info.UnitType;

            lbUnitType.Text = selectedUnitType;
            tbTotalQty.Content = string.Empty;
            lbTotalPrice.Text = "Rp 0";

            LoadServiceByCategory(selectedCategoryId);
        }

        private void LoadServiceByCategory(int categoryId)
        {
            serviceMap.Clear();
            cbxService.Items = new string[0];

            using (var conn = Database.Database.GetConnection())
            {
                string query = @"SELECT id, name, price 
                                 FROM services 
                                 WHERE category_id = @category_id
                                 ORDER BY name ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@category_id", categoryId);
                    conn.Open();

                    var list = new List<string>();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string name = reader["name"].ToString();
                            decimal price = Convert.ToDecimal(reader["price"]);

                            list.Add(name);
                            serviceMap[name] = (id, price);
                        }
                    }

                    cbxService.Items = list.ToArray();
                }
            }

            cbxService.SelectedIndexChanged += cbxService_SelectedIndexChanged;
        }

        private void cbxService_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxService.SelectedItem == null) return;

            string selectedName = cbxService.SelectedItem.ToString();
            if (!serviceMap.TryGetValue(selectedName, out var info))
                return;

            selectedServiceId = info.Id;
            selectedServicePrice = info.Price;

            // Hitung ulang jika qty sudah diisi
            if (decimal.TryParse(tbTotalQty.Content, NumberStyles.Number, idCulture, out decimal qty) && qty > 0)
            {
                decimal total = selectedServicePrice * qty;
                lbTotalPrice.Text = "Rp " + total.ToString("N0", idCulture);
            }
            else
            {
                lbTotalPrice.Text = "Rp 0";
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

        private void tbTotalQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            char decimalSep = idCulture.NumberFormat.NumberDecimalSeparator[0];

            // Hanya angka, backspace, dan tanda desimal
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != decimalSep)
            {
                e.Handled = true;
                return;
            }

            // Ambil isi sekarang dari Content (string)
            string currentText = tbTotalQty.Content ?? string.Empty;

            // Hanya boleh satu tanda desimal
            if (e.KeyChar == decimalSep && currentText.Contains(decimalSep))
            {
                e.Handled = true;
            }
        }

        private void tbTotalQty_ContentChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(tbTotalQty.Content, NumberStyles.Number, idCulture, out decimal qty) &&
                qty > 0 && selectedServicePrice > 0)
            {
                decimal total = selectedServicePrice * qty;
                lbTotalPrice.Text = "Rp " + total.ToString("N0", idCulture);
            }
            else
            {
                lbTotalPrice.Text = "Rp 0";
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (cbxCategory.SelectedItem == null || selectedCategoryId == 0)
            {
                MessageBox.Show("Pilih tipe layanan terlebih dahulu.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbxService.SelectedItem == null || selectedServiceId == 0)
            {
                MessageBox.Show("Pilih jenis layanan terlebih dahulu.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(tbTotalQty.Content, NumberStyles.Number, idCulture, out decimal qty) || qty <= 0)
            {
                MessageBox.Show($"Masukkan total {selectedUnitType} yang valid.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalPrice = selectedServicePrice * qty;
            lbTotalPrice.Text = "Rp " + totalPrice.ToString("N0", idCulture);

            string notes = string.IsNullOrWhiteSpace(tbNotes.Content)
                ? null
                : tbNotes.Content.Trim();

            using (var conn = Database.Database.GetConnection())
            {
                string insertQuery = @"
INSERT INTO orders 
(user_id, customer_id, service_id, total_qty, total_price, paid, payment, notes, submitted_at, taken_at)
VALUES 
(@userId, @customerId, @serviceId, @totalQty, @totalPrice, @paid, @payment, @notes, @submittedAt, @takenAt)";

                using (var cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", UserSession.id);
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@serviceId", selectedServiceId);
                    cmd.Parameters.AddWithValue("@totalQty", qty);
                    cmd.Parameters.AddWithValue("@totalPrice", totalPrice);
                    cmd.Parameters.AddWithValue("@paid", 0);
                    cmd.Parameters.AddWithValue("@payment", "cash");
                    cmd.Parameters.AddWithValue("@notes",
                        string.IsNullOrEmpty(notes) ? (object)DBNull.Value : notes);
                    cmd.Parameters.AddWithValue("@submittedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@takenAt", DBNull.Value);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show(
                            $"Order berhasil ditambahkan.\nTotal: Rp {totalPrice:N0}",
                            "Sukses",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        var payForm = new AdminCustomerPay(customerId, totalPrice);
                        payForm.ShowDialog();

                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menyimpan order: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    MessageBox.Show("Data customer telah dihapus.",
                        "Dibatalkan", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Koneksi ke database gagal: " + ex.Message,
                        "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message,
                        "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
