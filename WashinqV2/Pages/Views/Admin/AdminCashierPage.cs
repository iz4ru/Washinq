using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminCashierPage : Form
    {
        public AdminCashierPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void AdminCashierPage_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadData();
        }

        private void SetupDataGridView()
        {
            dgvCashier.Columns.Clear();

            DataGridViewTextBoxColumn clId = new DataGridViewTextBoxColumn();
            clId.HeaderText = "ID";
            clId.Name = "id";
            clId.Visible = false;

            DataGridViewTextBoxColumn clNo = new DataGridViewTextBoxColumn();
            clNo.HeaderText = "No";
            clNo.Name = "Nomor";
            clNo.ReadOnly = true;

            DataGridViewTextBoxColumn clName = new DataGridViewTextBoxColumn();
            clName.HeaderText = "Nama Kasir";
            clName.Name = "Nama Kasir";
            clName.ReadOnly = true;

            DataGridViewTextBoxColumn clUsername = new DataGridViewTextBoxColumn();
            clUsername.HeaderText = "Username";
            clUsername.Name = "Username";
            clUsername.ReadOnly = true;

            DataGridViewTextBoxColumn clEmail = new DataGridViewTextBoxColumn();
            clEmail.HeaderText = "Email";
            clEmail.Name = "Email";
            clEmail.ReadOnly = true;

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

            dgvCashier.Columns.Add(clId);
            dgvCashier.Columns.Add(clNo);
            dgvCashier.Columns.Add(clName);
            dgvCashier.Columns.Add(clUsername);
            dgvCashier.Columns.Add(clEmail);
            dgvCashier.Columns.Add(clPhone);
            dgvCashier.Columns.Add(clAddress);
            dgvCashier.Columns.Add(clCreatedAt);
            dgvCashier.Columns.Add(clCheckbox);

            dgvCashier.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCashier.AllowUserToAddRows = false;
        }


        public void LoadData()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = @"SELECT 
                id, name, username, email, phone, address, created_at 
                FROM users 
                WHERE role = 'cashier' 
                ORDER BY id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;
                            dgvCashier.Rows.Clear();

                            while (reader.Read())
                            {
                                dgvCashier.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["name"],
                                    reader["username"],
                                    reader["email"],
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
            AdminAddCashier form = new AdminAddCashier();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool hasSelected = false;
            int selectedCashierId = 0;
            string selectedCashierName = "";

            for (int i = 0; i < dgvCashier.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCashier.Rows[i];
                if (row.Cells["Pilih Aksi"].Value != null &&
                    Convert.ToBoolean(row.Cells["Pilih Aksi"].Value) == true)
                {
                    if (hasSelected)
                    {
                        MessageBox.Show("Silakan pilih hanya satu kasir untuk diedit!",
                            "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    hasSelected = true;
                    selectedCashierId = Convert.ToInt32(row.Cells["id"].Value);
                    selectedCashierName = row.Cells["Nama Kasir"].Value.ToString();
                }
            }

            if (!hasSelected)
            {
                MessageBox.Show("Silakan pilih kasir yang ingin diedit dengan mencentang checkbox!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AdminEditCashier editForm = new AdminEditCashier(selectedCashierId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> selectedCashierIds = new List<int>();
            List<string> selectedCashierDetails = new List<string>();

            foreach (DataGridViewRow row in dgvCashier.Rows)
            {
                if (row.Cells["Pilih Aksi"].Value != null &&
                    Convert.ToBoolean(row.Cells["Pilih Aksi"].Value) == true)
                {
                    int cashierId = Convert.ToInt32(row.Cells["id"].Value);
                    string cashierName = row.Cells["Nama Kasir"].Value.ToString();
                    string username = row.Cells["Username"].Value.ToString();

                    selectedCashierIds.Add(cashierId);
                    selectedCashierDetails.Add($"- {cashierName} ({username})");
                }
            }

            if (selectedCashierIds.Count == 0)
            {
                MessageBox.Show("Silakan pilih kasir yang ingin dihapus dengan mencentang checkbox!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string detailMessage = "Kasir yang akan dihapus:\n\n" +
                string.Join("\n", selectedCashierDetails) +
                $"\n\nTotal: {selectedCashierIds.Count} kasir\n\n" +
                "Apakah Anda yakin ingin menghapus data kasir ini?\n" +
                "Data kasir dan semua order terkait akan dihapus!";

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

                        foreach (int cashierId in selectedCashierIds)
                        {
                            string query = "DELETE FROM users WHERE id = @id AND role = 'cashier'";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", cashierId);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                    successCount++;
                                else
                                    failCount++;
                            }
                        }

                        conn.Close();
                    }

                    string resultMessage = $"Berhasil menghapus {successCount} kasir";
                    if (failCount > 0)
                        resultMessage += $"\nGagal menghapus {failCount} kasir";

                    MessageBox.Show(resultMessage,
                        "Hasil Penghapusan", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Gagal menghapus kasir: " + ex.Message +
                        "\n\nPastikan tidak ada order yang terkait dengan kasir ini.",
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
            AdminPage register = new AdminPage();
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
    }
}
