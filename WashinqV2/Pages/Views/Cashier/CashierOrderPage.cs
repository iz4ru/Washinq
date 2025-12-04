using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WashinqV2.Models;
using WashinqV2.Pages.Views.Admin;
using WashinqV2.Pages.Views.Owner;

namespace WashinqV2.Pages.Views.Cashier
{
    public partial class CashierOrderPage : Form
    {
        private int selectedOrderId;
        private string selectedUserName;
        private string selectedCustomerName;
        private string selectedServiceName;
        private string selectedTotalKg;
        private string selectedTotalPrice;
        private string selectedPaid;
        private string selectedPayment;
        private string selectedSubmittedAt;
        private string selectedTakenAt;
        private string spareChange;

        public CashierOrderPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        public void LoadData()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = @"
                SELECT 
                    o.id,
                    u.name AS user_name,
                    c.name AS customer_name,
                    s.name AS service_name,
                    o.total_qty,
                    o.total_price,
                    o.paid,
                    o.payment,
                    o.submitted_at,
                    o.taken_at
                FROM orders o
                JOIN users u ON o.user_id = u.id
                JOIN customers c ON o.customer_id = c.id
                JOIN services s ON o.service_id = s.id
                ORDER BY o.id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;
                            dgvOrder.Rows.Clear();

                            while (reader.Read())
                            {
                                dgvOrder.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["user_name"],
                                    reader["customer_name"],
                                    reader["service_name"],
                                   Convert.ToInt32(reader["total_qty"]).ToString("N0") + " kg",
                                    "Rp " + Convert.ToInt32(reader["total_price"]).ToString("N0"),
                                    "Rp " + Convert.ToInt32(reader["paid"]).ToString("N0"),
                                    reader["payment"],
                                    Convert.ToDateTime(reader["submitted_at"]).ToString("dd MMM yyyy HH:mm"),
                                    reader["taken_at"] == DBNull.Value ? "-" : Convert.ToDateTime(reader["taken_at"]).ToString("dd MMM yyyy HH:mm")
                                );
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Koneksi ke database gagal: " + ex.Message, "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
        }

        private void CashierOrderPage_Load(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn clid = new DataGridViewTextBoxColumn();
            clid.HeaderText = "ID";
            clid.Name = "id";
            clid.Visible = false;
            DataGridViewTextBoxColumn clnum = new DataGridViewTextBoxColumn();
            clnum.HeaderText = "No";
            clnum.Name = "Nomor";
            clnum.ReadOnly = true;
            DataGridViewTextBoxColumn cl1 = new DataGridViewTextBoxColumn();
            cl1.HeaderText = "Dilayani Oleh";
            cl1.Name = "Dilayani Oleh";
            DataGridViewTextBoxColumn cl2 = new DataGridViewTextBoxColumn();
            cl2.HeaderText = "Pelanggan";
            cl2.Name = "Pelanggan";
            DataGridViewTextBoxColumn cl3 = new DataGridViewTextBoxColumn();
            cl3.HeaderText = "Jenis Layanan";
            cl3.Name = "Jenis Layanan";
            DataGridViewTextBoxColumn cl4 = new DataGridViewTextBoxColumn();
            cl4.HeaderText = "Total Kuantitas";
            cl4.Name = "Total Kuantitas";
            DataGridViewTextBoxColumn cl5 = new DataGridViewTextBoxColumn();
            cl5.HeaderText = "Total Harga";
            cl5.Name = "Total Harga";
            DataGridViewTextBoxColumn cl6 = new DataGridViewTextBoxColumn();
            cl6.HeaderText = "Dibayar";
            cl6.Name = "Dibayar";
            DataGridViewTextBoxColumn cl7 = new DataGridViewTextBoxColumn();
            cl7.HeaderText = "Pembayaran";
            cl7.Name = "Pembayaran";
            DataGridViewTextBoxColumn cl8 = new DataGridViewTextBoxColumn();
            cl8.HeaderText = "Diproses Pada";
            cl8.Name = "Diproses Pada";
            DataGridViewTextBoxColumn cl9 = new DataGridViewTextBoxColumn();
            cl9.HeaderText = "Diambil Pada";
            cl9.Name = "Diambil Pada";
            DataGridViewCheckBoxColumn cl10 = new DataGridViewCheckBoxColumn();
            cl10.HeaderText = "Pilih Aksi";
            cl10.Name = "Pilih Aksi";

            dgvOrder.Columns.Add(clid);
            dgvOrder.Columns.Add(clnum);
            dgvOrder.Columns.Add(cl1);
            dgvOrder.Columns.Add(cl2);
            dgvOrder.Columns.Add(cl3);
            dgvOrder.Columns.Add(cl4);
            dgvOrder.Columns.Add(cl5);
            dgvOrder.Columns.Add(cl6);
            dgvOrder.Columns.Add(cl7);
            dgvOrder.Columns.Add(cl8);
            dgvOrder.Columns.Add(cl9);
            dgvOrder.Columns.Add(cl10);

            dgvOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrder.AllowUserToAddRows = false;

            LoadData();

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CashierAddCustomer form = new CashierAddCustomer();
            form.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Cek apakah ada baris yang dipilih
            bool hasSelected = false;
            List<int> selectedOrderIds = new List<int>();

            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                if (row.Cells["Pilih Aksi"].Value != null &&
                    Convert.ToBoolean(row.Cells["Pilih Aksi"].Value) == true)
                {
                    hasSelected = true;
                    int orderId = Convert.ToInt32(row.Cells["id"].Value);

                    // Cek apakah sudah diambil
                    string takenAt = row.Cells["Diambil Pada"].Value.ToString();
                    if (takenAt != "-")
                    {
                        MessageBox.Show("Order dengan ID " + orderId + " sudah diambil sebelumnya!",
                            "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    selectedOrderIds.Add(orderId);
                }
            }

            if (!hasSelected)
            {
                MessageBox.Show("Silakan pilih order yang ingin ditandai sebagai telah diambil!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Konfirmasi
            var result = MessageBox.Show(
                "Apakah Anda yakin ingin menandai " + selectedOrderIds.Count +
                " order sebagai telah diambil?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var conn = Database.Database.GetConnection())
                    {
                        conn.Open();

                        foreach (int orderId in selectedOrderIds)
                        {
                            string query = "UPDATE orders SET taken_at = @taken_at WHERE id = @id";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@taken_at", DateTime.Now);
                                cmd.Parameters.AddWithValue("@id", orderId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        conn.Close();
                    }

                    MessageBox.Show("Order berhasil ditandai sebagai telah diambil!",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData(); // Refresh data
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message,
                        "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Cek apakah ada baris yang dipilih
            bool hasSelected = false;
            List<int> selectedOrderIds = new List<int>();
            List<string> orderDetails = new List<string>();

            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                if (row.Cells["Pilih Aksi"].Value != null &&
                    Convert.ToBoolean(row.Cells["Pilih Aksi"].Value) == true)
                {
                    hasSelected = true;
                    int orderId = Convert.ToInt32(row.Cells["id"].Value);
                    string customerName = row.Cells["Pelanggan"].Value.ToString();

                    selectedOrderIds.Add(orderId);
                    orderDetails.Add("ID: " + orderId + " | Nama Customer: " + customerName);
                }
            }

            if (!hasSelected)
            {
                MessageBox.Show("Silakan pilih order yang ingin dibatalkan!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Konfirmasi dengan detail
            string detailMessage = "Order yang akan dibatalkan:\n\n" +
                string.Join("\n", orderDetails) +
                "\n\nApakah Anda yakin ingin membatalkan " + selectedOrderIds.Count + " order ini?";

            var result = MessageBox.Show(detailMessage,
                "Konfirmasi Batalkan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var conn = Database.Database.GetConnection())
                    {
                        conn.Open();

                        foreach (int orderId in selectedOrderIds)
                        {
                            string query = "DELETE FROM orders WHERE id = @id";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", orderId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        conn.Close();
                    }

                    MessageBox.Show("Order berhasil dibatalkan!",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData(); // Refresh data
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Gagal membatalkan order: " + ex.Message +
                        "\n\nPastikan tidak ada data terkait yang menghalangi pembatalan.",
                        "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message,
                        "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Konfirmasi logout
            var result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide(); // Menutup form dashboard
                LoginPage LoginPage = new LoginPage();
                LoginPage.ShowDialog(); // Menampilkan form login
                this.Close();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Hide();
            CashierPage register = new CashierPage();
            register.ShowDialog();
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            bool hasSelected = false;
            DataGridViewRow selectedRow = null;

            // Cek baris mana yang pilih aksi nya dicentang
            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                if (row.Cells["Pilih Aksi"].Value != null &&
                    Convert.ToBoolean(row.Cells["Pilih Aksi"].Value) == true)
                {
                    if (hasSelected)
                    {
                        MessageBox.Show("Pilih hanya 1 order yang ingin dicetak",
                            "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    hasSelected = true;
                    selectedRow = row;
                }
            }

            if (!hasSelected)
            {
                MessageBox.Show("Silakan pilih 1 order yang ingin dicetak struk",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                selectedOrderId = Convert.ToInt32(selectedRow.Cells["id"].Value);
                selectedUserName = selectedRow.Cells["Dilayani Oleh"].Value?.ToString() ?? "-";
                selectedCustomerName = selectedRow.Cells["Pelanggan"].Value?.ToString() ?? "-";
                selectedServiceName = selectedRow.Cells["Jenis Layanan"].Value?.ToString() ?? "-";
                selectedTotalKg = selectedRow.Cells["Total Kuantitas"].Value?.ToString() ?? "0";
                selectedTotalPrice = selectedRow.Cells["Total Harga"].Value?.ToString() ?? "0";
                selectedPaid = selectedRow.Cells["Dibayar"].Value?.ToString() ?? "0";
                selectedPayment = selectedRow.Cells["Pembayaran"].Value?.ToString() ?? "-";
                selectedSubmittedAt = selectedRow.Cells["Diproses Pada"].Value?.ToString() ?? "-";
                selectedTakenAt = selectedRow.Cells["Diambil Pada"].Value?.ToString() ?? "-";

                // Hitung kembalian - Bersihkan "Rp " dan titik pemisah ribuan
                decimal totalPrice = 0;
                decimal paid = 0;

                string cleanTotalPrice = selectedRow.Cells["Total Harga"].Value?.ToString()
                    .Replace("Rp ", "").Replace(".", "") ?? "0";
                string cleanPaid = selectedRow.Cells["Dibayar"].Value?.ToString()
                    .Replace("Rp ", "").Replace(".", "") ?? "0";

                decimal.TryParse(cleanTotalPrice, out totalPrice);
                decimal.TryParse(cleanPaid, out paid);

                spareChange = (paid - totalPrice).ToString("N0");

                // SET UKURAN KERTAS DI SINI, BUKAN DI PrintPage!
                printReceipt.DefaultPageSettings.PaperSize = new PaperSize("Struk", 302, 1000);

                PrintPreviewDialog preview = new PrintPreviewDialog();
                preview.Document = printReceipt;
                preview.Width = 400;
                preview.Height = 600;

                preview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat mencetak struk: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printReceipt_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // HAPUS BARIS INI - JANGAN SET PAPERSIZE DI SINI
            // e.PageSettings.PaperSize = new PaperSize("Struk", paperWidth, paperHeight);

            int paperWidth = 302;  // 80mm printer
            int marginLeft = 10;
            int posY = 10;
            int lineHeight = 20;

            // Font lebih kecil biar pas di struk mini
            Font font = new Font("Cascadia Code", 8);
            Font titleFont = new Font("Cascadia Code", 10, FontStyle.Bold);
            Font smallFont = new Font("Cascadia Code", 7);

            // Title - Center aligned
            string title = "WASHINQ";
            SizeF titleSize = e.Graphics.MeasureString(title, titleFont);
            float titleX = (paperWidth - titleSize.Width) / 2;
            e.Graphics.DrawString(title, titleFont, Brushes.Black, titleX, posY);
            posY += 30;

            // Garis pemisah
            e.Graphics.DrawString("===============================================", smallFont, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            // Data order
            e.Graphics.DrawString("ID Order: #" + selectedOrderId, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Kasir: @" + selectedUserName, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Pelanggan:", font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;
            e.Graphics.DrawString("  " + selectedCustomerName, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Layanan:", font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;
            e.Graphics.DrawString("  " + selectedServiceName, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            // Garis pemisah
            e.Graphics.DrawString("-----------------------------------------------", smallFont, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Berat: " + selectedTotalKg, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Harga: " + selectedTotalPrice, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Dibayar: " + selectedPaid, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Kembalian: Rp " + spareChange, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Metode: " + selectedPayment, font, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            // Garis pemisah
            e.Graphics.DrawString("-----------------------------------------------", smallFont, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Masuk: " + selectedSubmittedAt, smallFont, Brushes.Black, marginLeft, posY);
            posY += lineHeight;

            e.Graphics.DrawString("Diambil: " + selectedTakenAt, smallFont, Brushes.Black, marginLeft, posY);
            posY += lineHeight + 10;

            // Footer - Center aligned
            string footer = "Terima Kasih";
            SizeF footerSize = e.Graphics.MeasureString(footer, font);
            float footerX = (paperWidth - footerSize.Width) / 2;
            e.Graphics.DrawString(footer, font, Brushes.Black, footerX, posY);
            posY += lineHeight;

            // Garis penutup
            e.Graphics.DrawString("===============================================", smallFont, Brushes.Black, marginLeft, posY);
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            CashierEditProfile form = new CashierEditProfile(UserSession.id);
            form.ShowDialog();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            this.Hide();
            CashierLogPage register = new CashierLogPage();
            register.ShowDialog();
            this.Close();
        }
    }
}
