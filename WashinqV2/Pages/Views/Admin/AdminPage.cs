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
using WashinqV2.Pages.Views.Admin;

namespace WashinqV2.Pages.Views
{
    public partial class AdminPage : Form
    {
        public AdminPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void AdminPage_Load(object sender, EventArgs e)
        {
            SetupDataGridView();

            LoadDashboardStatistics();
            LoadRecentOrders();
        }

        private void SetupDataGridView()
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
            cl1.ReadOnly = true;

            DataGridViewTextBoxColumn cl2 = new DataGridViewTextBoxColumn();
            cl2.HeaderText = "Pelanggan";
            cl2.Name = "Pelanggan";
            cl2.ReadOnly = true;

            DataGridViewTextBoxColumn cl3 = new DataGridViewTextBoxColumn();
            cl3.HeaderText = "Jenis Layanan";
            cl3.Name = "Jenis Layanan";
            cl3.ReadOnly = true;

            DataGridViewTextBoxColumn cl4 = new DataGridViewTextBoxColumn();
            cl4.HeaderText = "Total Kilogram";
            cl4.Name = "Total Kilogram";
            cl4.ReadOnly = true;

            DataGridViewTextBoxColumn cl5 = new DataGridViewTextBoxColumn();
            cl5.HeaderText = "Total Harga";
            cl5.Name = "Total Harga";
            cl5.ReadOnly = true;

            DataGridViewTextBoxColumn cl6 = new DataGridViewTextBoxColumn();
            cl6.HeaderText = "Dibayar";
            cl6.Name = "Dibayar";
            cl6.ReadOnly = true;

            DataGridViewTextBoxColumn cl7 = new DataGridViewTextBoxColumn();
            cl7.HeaderText = "Pembayaran";
            cl7.Name = "Pembayaran";
            cl7.ReadOnly = true;

            DataGridViewTextBoxColumn cl8 = new DataGridViewTextBoxColumn();
            cl8.HeaderText = "Diproses Pada";
            cl8.Name = "Diproses Pada";
            cl8.ReadOnly = true;

            DataGridViewTextBoxColumn cl9 = new DataGridViewTextBoxColumn();
            cl9.HeaderText = "Diambil Pada";
            cl9.Name = "Diambil Pada";
            cl9.ReadOnly = true;

            dgvBeranda.Columns.Add(clid);
            dgvBeranda.Columns.Add(clnum);
            dgvBeranda.Columns.Add(cl1);
            dgvBeranda.Columns.Add(cl2);
            dgvBeranda.Columns.Add(cl3);
            dgvBeranda.Columns.Add(cl4);
            dgvBeranda.Columns.Add(cl5);
            dgvBeranda.Columns.Add(cl6);
            dgvBeranda.Columns.Add(cl7);
            dgvBeranda.Columns.Add(cl8);
            dgvBeranda.Columns.Add(cl9);

            dgvBeranda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBeranda.AllowUserToAddRows = false;
            dgvBeranda.ReadOnly = true;
        }

        private void LoadDashboardStatistics()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();

                    string queryTotalOrder = "SELECT COUNT(*) FROM orders";
                    using (MySqlCommand cmd = new MySqlCommand(queryTotalOrder, conn))
                    {
                        int totalOrder = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalOrder.Text = totalOrder.ToString();
                    }

                    string queryCompletedOrder = "SELECT COUNT(*) FROM orders WHERE taken_at IS NOT NULL";
                    using (MySqlCommand cmd = new MySqlCommand(queryCompletedOrder, conn))
                    {
                        int completedOrder = Convert.ToInt32(cmd.ExecuteScalar());
                        lblPesananSelesai.Text = completedOrder.ToString();
                    }

                    string queryTotalService = "SELECT COUNT(*) FROM services";
                    using (MySqlCommand cmd = new MySqlCommand(queryTotalService, conn))
                    {
                        int totalService = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalLayanan.Text = totalService.ToString();
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

        private void LoadRecentOrders()
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
                        JOIN services s ON o.service_id = s.id
                        ORDER BY o.id DESC
                        LIMIT 10";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;
                            dgvBeranda.Rows.Clear();

                            while (reader.Read())
                            {
                                dgvBeranda.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["user_name"],
                                    reader["customer_name"],
                                    reader["service_name"],
                                    Convert.ToDouble(reader["total_kg"]).ToString("N1") + " kg",
                                    "Rp " + Convert.ToInt32(reader["total_price"]).ToString("N0"),
                                    "Rp " + Convert.ToInt32(reader["paid"]).ToString("N0"),
                                    reader["payment"],
                                    Convert.ToDateTime(reader["submitted_at"]).ToString("dd MMM yyyy HH:mm"),
                                    reader["taken_at"] == DBNull.Value ? "-" :
                                        Convert.ToDateTime(reader["taken_at"]).ToString("dd MMM yyyy HH:mm")
                                );
                            }
                        }
                        conn.Close();
                    }
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Apakah Anda yakin ingin logout?",
                "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginPage LoginPage = new LoginPage();
                LoginPage.ShowDialog();
                this.Close();
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {

        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminOrderPage register = new AdminOrderPage();
            register.ShowDialog();
            this.Close();
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminServicePage register = new AdminServicePage();
            register.ShowDialog();
            this.Close();
        }
    }
}
