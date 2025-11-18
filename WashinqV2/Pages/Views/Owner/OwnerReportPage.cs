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
using ClosedXML.Excel;
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Owner
{
    public partial class OwnerReportPage : Form
    {
        public OwnerReportPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void OwnerReportPage_Load(object sender, EventArgs e)
        {
            // ✅ Set default tanggal
            cdpStart.Content = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            cdpEnd.Content = DateTime.Now;

            LoadDataGridViewReport();
            LoadDataGridViewIncome();
            LoadDataGridViewTotal();
        }

        private void LoadDataGridViewReport()
        {
            dgvReport.Columns.Clear();

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
            cl4.HeaderText = "Total Kilogram";
            cl4.Name = "Total Kilogram";

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

            dgvReport.Columns.Add(clid);
            dgvReport.Columns.Add(clnum);
            dgvReport.Columns.Add(cl1);
            dgvReport.Columns.Add(cl2);
            dgvReport.Columns.Add(cl3);
            dgvReport.Columns.Add(cl4);
            dgvReport.Columns.Add(cl5);
            dgvReport.Columns.Add(cl6);
            dgvReport.Columns.Add(cl7);
            dgvReport.Columns.Add(cl8);
            dgvReport.Columns.Add(cl9);

            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.ReadOnly = true;
            dgvReport.RowHeadersVisible = false;

            LoadData();
        }

        private void LoadDataGridViewIncome()
        {
            dgvIncome.Columns.Clear();

            DataGridViewTextBoxColumn clIncome = new DataGridViewTextBoxColumn();
            clIncome.HeaderText = "Pendapatan Per";
            clIncome.Name = "Pendapatan Per";

            DataGridViewTextBoxColumn clTotal = new DataGridViewTextBoxColumn();
            clTotal.HeaderText = "Total Pendapatan";
            clTotal.Name = "Total Pendapatan";

            dgvIncome.Columns.Add(clIncome);
            dgvIncome.Columns.Add(clTotal);

            dgvIncome.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIncome.AllowUserToAddRows = false;
            dgvIncome.ReadOnly = true;
            dgvIncome.RowHeadersVisible = false;

            LoadIncomeData();
        }

        private void LoadDataGridViewTotal()
        {
            dgvTotal.Columns.Clear();

            DataGridViewTextBoxColumn clSummary = new DataGridViewTextBoxColumn();
            clSummary.HeaderText = "Ringkasan";
            clSummary.Name = "Ringkasan";

            DataGridViewTextBoxColumn clTotal = new DataGridViewTextBoxColumn();
            clTotal.HeaderText = "Total";
            clTotal.Name = "Total";

            dgvTotal.Columns.Add(clSummary);
            dgvTotal.Columns.Add(clTotal);

            dgvTotal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTotal.AllowUserToAddRows = false;
            dgvTotal.ReadOnly = true;
            dgvTotal.RowHeadersVisible = false;

            LoadTotalData();
        }

        private void LoadTotalData()
        {
            try
            {
                dgvTotal.Rows.Clear();

                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();

                    // 1. Total Pendapatan (Semua Waktu)
                    string queryTotalIncome = @"SELECT COALESCE(SUM(total_price), 0) FROM orders";
                    using (MySqlCommand cmd = new MySqlCommand(queryTotalIncome, conn))
                    {
                        decimal totalIncome = Convert.ToDecimal(cmd.ExecuteScalar());
                        dgvTotal.Rows.Add("Total Pendapatan", "Rp " + totalIncome.ToString("N0"));
                    }

                    // 2. Total Pesanan
                    string queryTotalOrders = @"SELECT COUNT(*) FROM orders";
                    using (MySqlCommand cmd = new MySqlCommand(queryTotalOrders, conn))
                    {
                        int totalOrders = Convert.ToInt32(cmd.ExecuteScalar());
                        dgvTotal.Rows.Add("Total Pesanan", totalOrders.ToString("N0") + " pesanan");
                    }

                    // 3. Total Layanan
                    string queryTotalServices = @"SELECT COUNT(*) FROM services";
                    using (MySqlCommand cmd = new MySqlCommand(queryTotalServices, conn))
                    {
                        int totalServices = Convert.ToInt32(cmd.ExecuteScalar());
                        dgvTotal.Rows.Add("Total Layanan", totalServices.ToString() + " layanan");
                    }

                    conn.Close();
                }
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

        private void LoadIncomeData()
        {
            try
            {
                dgvIncome.Rows.Clear();

                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();

                    // 1. Pendapatan Hari Ini
                    string queryToday = @"SELECT COALESCE(SUM(total_price), 0) 
                                 FROM orders 
                                 WHERE DATE(submitted_at) = CURDATE()";
                    using (MySqlCommand cmd = new MySqlCommand(queryToday, conn))
                    {
                        decimal todayIncome = Convert.ToDecimal(cmd.ExecuteScalar());
                        dgvIncome.Rows.Add("Hari ini", "Rp " + todayIncome.ToString("N0"));
                    }

                    // 2. Pendapatan Minggu Ini (Senin - Minggu)
                    string queryWeek = @"SELECT COALESCE(SUM(total_price), 0) 
                                FROM orders 
                                WHERE YEARWEEK(submitted_at, 1) = YEARWEEK(CURDATE(), 1)";
                    using (MySqlCommand cmd = new MySqlCommand(queryWeek, conn))
                    {
                        decimal weekIncome = Convert.ToDecimal(cmd.ExecuteScalar());
                        dgvIncome.Rows.Add("Minggu ini", "Rp " + weekIncome.ToString("N0"));
                    }

                    // 3. Pendapatan Bulan Ini
                    string queryMonth = @"SELECT COALESCE(SUM(total_price), 0) 
                                 FROM orders 
                                 WHERE MONTH(submitted_at) = MONTH(CURDATE()) 
                                 AND YEAR(submitted_at) = YEAR(CURDATE())";
                    using (MySqlCommand cmd = new MySqlCommand(queryMonth, conn))
                    {
                        decimal monthIncome = Convert.ToDecimal(cmd.ExecuteScalar());
                        dgvIncome.Rows.Add("Bulan ini", "Rp " + monthIncome.ToString("N0"));
                    }

                    // 4. Pendapatan Tahun Ini
                    string queryYear = @"SELECT COALESCE(SUM(total_price), 0) 
                                FROM orders 
                                WHERE YEAR(submitted_at) = YEAR(CURDATE())";
                    using (MySqlCommand cmd = new MySqlCommand(queryYear, conn))
                    {
                        decimal yearIncome = Convert.ToDecimal(cmd.ExecuteScalar());
                        dgvIncome.Rows.Add("Tahun ini", "Rp " + yearIncome.ToString("N0"));
                    }

                    conn.Close();
                }
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

        public void LoadData(DateTime? startDate = null, DateTime? endDate = null)
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
                    o.total_kg,
                    o.total_price,
                    o.paid,
                    o.payment,
                    o.submitted_at,
                    o.taken_at
                FROM orders o
                JOIN users u ON o.user_id = u.id
                JOIN customers c ON o.customer_id = c.id
                JOIN services s ON o.service_id = s.id";

                    if (startDate.HasValue && endDate.HasValue)
                    {
                        query += @" WHERE DATE(o.submitted_at) BETWEEN @startDate AND @endDate";
                    }

                    query += " ORDER BY o.id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (startDate.HasValue && endDate.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@startDate", startDate.Value.Date);
                            cmd.Parameters.AddWithValue("@endDate", endDate.Value.Date);
                        }

                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;
                            dgvReport.Rows.Clear();

                            while (reader.Read())
                            {
                                dgvReport.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["user_name"],
                                    reader["customer_name"],
                                    reader["service_name"],
                                    Convert.ToInt32(reader["total_kg"]).ToString("N0") + " kg",
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshAllData()
        {
            LoadIncomeData();
            LoadTotalData();
            LoadData();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // ✅ Gunakan property Content, bukan Value
            if (cdpStart.Content > cdpEnd.Content)
            {
                MessageBox.Show("Tanggal mulai tidak boleh lebih besar dari tanggal akhir!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Load data dengan filter tanggal
            LoadData(cdpStart.Content, cdpEnd.Content);

            MessageBox.Show($"Data berhasil difilter dari {cdpStart.Content:dd MMM yyyy} hingga {cdpEnd.Content:dd MMM yyyy}",
                "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvReport.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk di-export!",
                        "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Files|*.xlsx";
                    sfd.FileName = $"Laporan_Pesanan_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Laporan Pesanan");

                            // Header
                            int col = 1;
                            for (int i = 1; i < dgvReport.Columns.Count; i++) // Skip kolom ID
                            {
                                if (dgvReport.Columns[i].Visible)
                                {
                                    worksheet.Cell(1, col).Value = dgvReport.Columns[i].HeaderText;
                                    worksheet.Cell(1, col).Style.Font.Bold = true;
                                    worksheet.Cell(1, col).Style.Fill.BackgroundColor = XLColor.LightGray;
                                    col++;
                                }
                            }

                            // Data
                            for (int i = 0; i < dgvReport.Rows.Count; i++)
                            {
                                col = 1;
                                for (int j = 1; j < dgvReport.Columns.Count; j++) // Skip kolom ID
                                {
                                    if (dgvReport.Columns[j].Visible)
                                    {
                                        var cellValue = dgvReport.Rows[i].Cells[j].Value;
                                        worksheet.Cell(i + 2, col).Value = cellValue?.ToString() ?? "";
                                        col++;
                                    }
                                }
                            }

                            // Auto-fit kolom
                            worksheet.Columns().AdjustToContents();

                            // Save file
                            workbook.SaveAs(sfd.FileName);

                            MessageBox.Show($"Data berhasil di-export ke:\n{sfd.FileName}",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Buka file (optional)
                            var result = MessageBox.Show("Apakah Anda ingin membuka file?",
                                "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(sfd.FileName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal export data: " + ex.Message,
                    "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnProfile_Click(object sender, EventArgs e)
        {
            OwnerEditProfile form = new OwnerEditProfile(UserSession.id);
            form.ShowDialog();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Hide();
            OwnerPage register = new OwnerPage();
            register.ShowDialog();
            this.Close();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            OwnerUserPage register = new OwnerUserPage();
            register.ShowDialog();
            this.Close();
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            this.Hide();
            OwnerServicePage register = new OwnerServicePage();
            register.ShowDialog();
            this.Close();
        }


    }
}
