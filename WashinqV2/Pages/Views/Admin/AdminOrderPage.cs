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
    public partial class AdminOrderPage : Form
    {
        public AdminOrderPage()
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

        private void AdminOrderPage_Load(object sender, EventArgs e)
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
            AdminAddCustomer form = new AdminAddCustomer();
            form.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

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

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminPage register = new AdminPage();
            register.ShowDialog();
            this.Close();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {

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
