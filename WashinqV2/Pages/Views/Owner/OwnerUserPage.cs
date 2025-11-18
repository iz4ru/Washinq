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

namespace WashinqV2.Pages.Views.Owner
{
    public partial class OwnerUserPage : Form
    {
        public OwnerUserPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void OwnerUserPage_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadData();
        }

        private void SetupDataGridView()
        {
            dgvUser.Columns.Clear();

            DataGridViewTextBoxColumn clId = new DataGridViewTextBoxColumn();
            clId.HeaderText = "ID";
            clId.Name = "id";
            clId.Visible = false;

            DataGridViewTextBoxColumn clNo = new DataGridViewTextBoxColumn();
            clNo.HeaderText = "No";
            clNo.Name = "Nomor";
            clNo.ReadOnly = true;

            DataGridViewTextBoxColumn clName = new DataGridViewTextBoxColumn();
            clName.HeaderText = "Nama Pengguna";
            clName.Name = "Nama Pengguna";
            clName.ReadOnly = true;

            DataGridViewTextBoxColumn clUsername = new DataGridViewTextBoxColumn();
            clUsername.HeaderText = "Username";
            clUsername.Name = "Username";
            clUsername.ReadOnly = true;

            DataGridViewTextBoxColumn clEmail = new DataGridViewTextBoxColumn();
            clEmail.HeaderText = "Email";
            clEmail.Name = "Email";
            clEmail.ReadOnly = true;

            DataGridViewTextBoxColumn clRole = new DataGridViewTextBoxColumn();
            clRole.HeaderText = "Role";
            clRole.Name = "Role";
            clRole.ReadOnly = true;

            DataGridViewTextBoxColumn clPhone = new DataGridViewTextBoxColumn();
            clPhone.HeaderText = "Nomor Telepon";
            clPhone.Name = "Nomor Telepon";
            clPhone.ReadOnly = true;

            DataGridViewTextBoxColumn clAddress = new DataGridViewTextBoxColumn();
            clAddress.HeaderText = "Alamat";
            clAddress.Name = "Alamat";
            clAddress.ReadOnly = true;

            DataGridViewTextBoxColumn clCreatedAt = new DataGridViewTextBoxColumn();
            clCreatedAt.HeaderText = "Dibuat Pada";
            clCreatedAt.Name = "Dibuat Pada";
            clCreatedAt.ReadOnly = true;

            DataGridViewCheckBoxColumn clCheckbox = new DataGridViewCheckBoxColumn();
            clCheckbox.HeaderText = "Pilih Aksi";
            clCheckbox.Name = "Pilih Aksi";

            dgvUser.Columns.Add(clId);
            dgvUser.Columns.Add(clNo);
            dgvUser.Columns.Add(clName);
            dgvUser.Columns.Add(clUsername);
            dgvUser.Columns.Add(clEmail);
            dgvUser.Columns.Add(clRole);
            dgvUser.Columns.Add(clPhone);
            dgvUser.Columns.Add(clAddress);
            dgvUser.Columns.Add(clCreatedAt);
            dgvUser.Columns.Add(clCheckbox);

            dgvUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUser.AllowUserToAddRows = false;
        }

        public void LoadData()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = @"SELECT 
                id, name, username, email, role, phone, address, created_at 
                FROM users 
                WHERE role IN ('cashier', 'admin') 
                ORDER BY id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;
                            dgvUser.Rows.Clear();

                            while (reader.Read())
                            {
                                dgvUser.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["name"],
                                    reader["username"],
                                    reader["email"],
                                    reader["role"],
                                    reader["phone"] == DBNull.Value ? "-" : reader["phone"].ToString(),
                                    reader["address"] == DBNull.Value ? "-" : reader["address"].ToString(),
                                    Convert.ToDateTime(reader["created_at"]).ToString("dd MMM yyyy HH:mm"),
                                    false
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OwnerAddUser form = new OwnerAddUser();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool hasSelected = false;
            int selectedUserId = 0;
            string selectedUserName = "";

            for (int i = 0; i < dgvUser.Rows.Count; i++)
            {
                DataGridViewRow row = dgvUser.Rows[i];
                if (row.Cells["Pilih Aksi"].Value != null &&
                    Convert.ToBoolean(row.Cells["Pilih Aksi"].Value) == true)
                {
                    if (hasSelected)
                    {
                        MessageBox.Show("Silakan pilih hanya satu pengguna untuk diedit!",
                            "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    hasSelected = true;
                    selectedUserId = Convert.ToInt32(row.Cells["id"].Value);
                    selectedUserName = row.Cells["Nama Pengguna"].Value.ToString();
                }
            }

            if (!hasSelected)
            {
                MessageBox.Show("Silakan pilih pengguna yang ingin diedit dengan mencentang checkbox!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OwnerEditUser editForm = new OwnerEditUser(selectedUserId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> selectedUserIds = new List<int>();
            List<string> selectedUserDetails = new List<string>();

            foreach (DataGridViewRow row in dgvUser.Rows)
            {
                if (row.Cells["Pilih Aksi"].Value != null &&
                    Convert.ToBoolean(row.Cells["Pilih Aksi"].Value) == true)
                {
                    int userId = Convert.ToInt32(row.Cells["id"].Value);
                    string name = row.Cells["Nama Pengguna"].Value.ToString();
                    string username = row.Cells["Username"].Value.ToString();

                    selectedUserIds.Add(userId);
                    selectedUserDetails.Add($"- {name} ({username})");
                }
            }

            if (selectedUserIds.Count == 0)
            {
                MessageBox.Show("Silakan pilih pengguna yang ingin dihapus dengan mencentang checkbox!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string detailMessage = "Pengguna yang akan dihapus:\n\n" +
                string.Join("\n", selectedUserDetails) +
                $"\n\nTotal: {selectedUserIds.Count} pengguna\n\n" +
                "Apakah Anda yakin ingin menghapus data pengguna ini?\n" +
                "Data pengguna dan semua order terkait akan dihapus!";

            var result = MessageBox.Show(detailMessage,
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int successCount = 0;
                    int failCount = 0;

                    using (var conn = Database.Database.GetConnection())
                    {
                        conn.Open();

                        foreach (int userId in selectedUserIds)
                        {
                            string query = "DELETE FROM users WHERE id = @id";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", userId);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                    successCount++;
                                else
                                    failCount++;
                            }
                        }

                        conn.Close();
                    }

                    string resultMessage = $"Berhasil menghapus {successCount} pengguna";
                    if (failCount > 0)
                        resultMessage += $"\nGagal menghapus {failCount} pengguna";

                    MessageBox.Show(resultMessage,
                        "Hasil Penghapusan", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Gagal menghapus pengguna: " + ex.Message +
                        "\n\nPastikan tidak ada order yang terkait dengan pengguna ini.",
                        "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message,
                        "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Hide();
            OwnerPage register = new OwnerPage();
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
    }
}
