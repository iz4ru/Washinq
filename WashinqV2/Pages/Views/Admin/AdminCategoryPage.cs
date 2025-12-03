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
using WashinqV2.Helpers;
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminCategoryPage : Form
    {
        public AdminCategoryPage()
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
                    string query = "SELECT id, name, unit_type FROM categories ORDER BY id DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;
                            dgvCategory.Rows.Clear();

                            while (reader.Read())
                            {
                                dgvCategory.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["name"],
                                    reader["unit_type"]);
                            }
                        }
                        conn.Close();
                    }
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

        private void AdminCategoryPage_Load(object sender, EventArgs e)
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
            cl1.HeaderText = "Nama";
            cl1.Name = "Nama";
            DataGridViewTextBoxColumn cl2 = new DataGridViewTextBoxColumn();
            cl2.HeaderText = "Jenis Unit";
            cl2.Name = "Jenis Unit";
            DataGridViewCheckBoxColumn cl3 = new DataGridViewCheckBoxColumn();
            cl3.HeaderText = "Pilih Aksi";
            cl3.Name = "Pilih Aksi";

            dgvCategory.Columns.Add(clid);
            dgvCategory.Columns.Add(clnum);
            dgvCategory.Columns.Add(cl1);
            dgvCategory.Columns.Add(cl2);
            dgvCategory.Columns.Add(cl3);

            dgvCategory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategory.AllowUserToAddRows = false;

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
            AdminAddCategory form = new AdminAddCategory(this);
            form.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Pilih baris yang dicentang pada DataGridView
            var selectedRows = dgvCategory.Rows
                .Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells["Pilih Aksi"].Value))
                .ToList();

            // Validasi jumlah baris yang dipilih
            if (selectedRows.Count > 1 || selectedRows.Count == 0)
            {
                MessageBox.Show("Pilih salah satu baris untuk diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = selectedRows[0]; // Ambil baris yang dipilih

            // Ambil data dari data grid view
            int id = Convert.ToInt32(selectedRow.Cells["id"].Value);
            string categoryName = selectedRow.Cells["Nama"].Value.ToString();
            string unitType = selectedRow.Cells["Jenis Unit"].Value.ToString();

            var AdminEditCategory = new AdminEditCategory(id, categoryName, unitType, this);

            if (AdminEditCategory.ShowDialog() == DialogResult.OK)
            {
                selectedRow.Cells["Nama"].Value = AdminEditCategory.CategoryName;
                selectedRow.Cells["Harga per Kilogram"].Value = AdminEditCategory.UnitType;

                try
                {
                    using (var conn = Database.Database.GetConnection())
                    {
                        string query = "UPDATE categories SET name = @name, unit_type = @unit_type WHERE id = @id";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            // Benerin syntax AddWithValue
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@name", AdminEditCategory.CategoryName);
                            cmd.Parameters.AddWithValue("@unit_type", AdminEditCategory.UnitType);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Data berhasil diperbarui!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedRows = dgvCategory.Rows.Cast<DataGridViewRow>().Where(row => Convert.ToBoolean(row.Cells["Pilih Aksi"].Value)).ToList();

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Pilih satu atau lebih baris untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Apakah Anda yakin ingin menghapus data yang dipilih?", "Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                foreach (var row in selectedRows)
                {
                    string id = row.Cells["id"].Value.ToString();
                    string categoryName = row.Cells["Nama"].Value?.ToString() ?? "";
                    int categoryId = int.TryParse(id, out int parsedId) ? parsedId : 0;

                    try
                    {
                        using (var conn = Database.Database.GetConnection())
                        {
                            string query = "DELETE FROM categories WHERE id =  @id";

                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", id);

                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                        dgvCategory.Rows.Remove(row);

                        MessageBox.Show("Kategori berhasil dihapus!", "Sukses");

                        LogActivity.Insert("Hapus Data",
                            $"Menghapus kategori '{categoryName}' (ID: {categoryId})");
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
            }
            LoadData();
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
            AdminEditProfile form = new AdminEditProfile(UserSession.id);
            form.ShowDialog();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminPage register = new AdminPage();
            register.ShowDialog();
            this.Close();
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

        private void btnLog_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminLogPage register = new AdminLogPage();
            register.ShowDialog();
            this.Close();
        }
    }
}
