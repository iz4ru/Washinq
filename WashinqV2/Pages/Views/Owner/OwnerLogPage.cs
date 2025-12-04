using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Owner
{
    public partial class OwnerLogPage : Form
    {
        public OwnerLogPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

        }

        private void OwnerLogPage_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadData();
        }

        private void SetupDataGridView()
        {
            dgvLog.Columns.Clear();

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
            cl2.HeaderText = "Role";
            cl2.Name = "Role";
            cl2.ReadOnly = true;
            cl2.Width = 100;

            DataGridViewTextBoxColumn cl3 = new DataGridViewTextBoxColumn();
            cl3.HeaderText = "Aksi";
            cl3.Name = "Aksi";
            cl3.ReadOnly = true;
            cl3.Width = 120;

            DataGridViewTextBoxColumn cl4 = new DataGridViewTextBoxColumn();
            cl4.HeaderText = "Deskripsi";
            cl4.Name = "Deskripsi";
            cl4.ReadOnly = true;
            cl4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewTextBoxColumn cl5 = new DataGridViewTextBoxColumn();
            cl5.HeaderText = "Dibuat Pada";
            cl5.Name = "Dibuat Pada";
            cl5.ReadOnly = true;
            cl5.Width = 150;

            dgvLog.Columns.Add(clid);
            dgvLog.Columns.Add(clnum);
            dgvLog.Columns.Add(cl1);
            dgvLog.Columns.Add(cl2);
            dgvLog.Columns.Add(cl3);
            dgvLog.Columns.Add(cl4);
            dgvLog.Columns.Add(cl5);

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
                    // Query base - tampilkan semua log dari semua user
                    string query = @"
                        SELECT 
                            l.id,
                            u.name AS user_name,
                            u.role,
                            l.action,
                            l.description,
                            l.created_at
                        FROM logs l
                        JOIN users u ON l.user_id = u.id
                        WHERE 1=1";

                    // Filter search jika ada keyword
                    if (!string.IsNullOrWhiteSpace(tbSearch.Content))
                    {
                        query += @" AND (
                            u.name LIKE @search 
                            OR l.action LIKE @search 
                            OR l.description LIKE @search
                            OR u.role LIKE @search
                        )";
                    }

                    // Order by newest first
                    query += " ORDER BY l.created_at DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameter search jika ada
                        if (!string.IsNullOrWhiteSpace(tbSearch.Content))
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + tbSearch.Content.Trim() + "%");
                        }

                        conn.Open();

                        dgvLog.Rows.Clear();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;

                            while (reader.Read())
                            {
                                // Format role untuk display (capitalize)
                                string roleDb = reader["role"].ToString();
                                string roleDisplay = "";

                                if (roleDb == "owner") roleDisplay = "Owner";
                                else if (roleDb == "admin") roleDisplay = "Admin";
                                else if (roleDb == "cashier") roleDisplay = "Kasir";

                                dgvLog.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["user_name"],
                                    roleDisplay,
                                    reader["action"],
                                    reader["description"],
                                    Convert.ToDateTime(reader["created_at"]).ToString("dd MMM yyyy HH:mm", new CultureInfo("id-ID"))
                                );
                            }
                        }
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(); // Reload dengan filter search
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tbSearch.Content = string.Empty; // Clear search box
            LoadData(); // Reload semua data
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

        private void btnReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            OwnerReportPage register = new OwnerReportPage();
            register.ShowDialog();
            this.Close();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            this.Hide();
            OwnerCategoryPage register = new OwnerCategoryPage();
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
            OwnerEditProfile form = new OwnerEditProfile(UserSession.id);
            form.ShowDialog();
        }
    }
}
