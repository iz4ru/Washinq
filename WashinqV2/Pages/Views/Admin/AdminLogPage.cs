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
    public partial class AdminLogPage : Form
    {
        public AdminLogPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void AdminLogPage_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadData();
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
            clnum.Width = 60;

            DataGridViewTextBoxColumn cl1 = new DataGridViewTextBoxColumn();
            cl1.HeaderText = "User";
            cl1.Name = "User";
            cl1.ReadOnly = true;
            cl1.Width = 150;

            DataGridViewTextBoxColumn cl2 = new DataGridViewTextBoxColumn();
            cl2.HeaderText = "Aksi";
            cl2.Name = "Aksi";
            cl2.ReadOnly = true;
            cl2.Width = 100;

            DataGridViewTextBoxColumn cl3 = new DataGridViewTextBoxColumn();
            cl3.HeaderText = "Deskripsi";
            cl3.Name = "Deskripsi";
            cl3.ReadOnly = true;
            cl3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewTextBoxColumn cl4 = new DataGridViewTextBoxColumn();
            cl4.HeaderText = "Dibuat Pada";
            cl4.Name = "Dibuat Pada";
            cl4.ReadOnly = true;
            cl4.Width = 150;

            dgvLog.Columns.Add(clid);
            dgvLog.Columns.Add(clnum);
            dgvLog.Columns.Add(cl1);
            dgvLog.Columns.Add(cl2);
            dgvLog.Columns.Add(cl3);
            dgvLog.Columns.Add(cl4);

            dgvLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvLog.AllowUserToAddRows = false;
            dgvLog.ReadOnly = true;
            dgvLog.RowHeadersVisible = false;
            dgvLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadData()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = @"
                SELECT 
                    l.id,
                    u.name AS user_name,
                    l.action,
                    l.description,
                    l.created_at
                FROM logs l
                JOIN users u ON l.user_id = u.id
                WHERE l.user_id = @userId
                ORDER BY l.id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", UserSession.id);

                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;
                            dgvLog.Rows.Clear();

                            while (reader.Read())
                            {
                                dgvLog.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["user_name"],
                                    reader["action"],
                                    reader["description"],
                                    Convert.ToDateTime(reader["created_at"]).ToString("dd MMM yyyy HH:mm")
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


        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminPage register = new AdminPage();
            register.ShowDialog();
            this.Close();
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
            AdminEditProfile form = new AdminEditProfile(UserSession.id);
            form.ShowDialog();
        }

        private void btnCashier_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminCashierPage register = new AdminCashierPage();
            register.ShowDialog();
            this.Close();
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

        private void btnCategory_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminCategoryPage register = new AdminCategoryPage();
            register.ShowDialog();
            this.Close();
        }
    }
}
