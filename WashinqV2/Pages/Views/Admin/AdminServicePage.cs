using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms.Suite;
using MySql.Data.MySqlClient;
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminServicePage : Form
    {
        public AdminServicePage()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = "SELECT id, name, price_per_kg, description FROM services ORDER BY id DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    { 
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int i = 1;
                            dgvService.Rows.Clear();

                            while (reader.Read())
                            {
                                dgvService.Rows.Add(
                                    reader["id"],
                                    i++,
                                    reader["name"],
                                    "Rp " + Convert.ToInt32(reader["price_per_kg"]).ToString("N0"),
                                    reader["description"]);
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

        private void AdminServicePage_Load(object sender, EventArgs e)
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
            cl2.HeaderText = "Harga per Kilogram";
            cl2.Name = "Harga per Kilogram";
            DataGridViewTextBoxColumn cl3 = new DataGridViewTextBoxColumn();
            cl3.HeaderText = "Deskripsi";
            cl3.Name = "Deskripsi";
            DataGridViewCheckBoxColumn cl4 = new DataGridViewCheckBoxColumn();
            cl4.HeaderText = "Pilih Aksi";
            cl4.Name = "Pilih Aksi";

            dgvService.Columns.Add(clid);
            dgvService.Columns.Add(clnum);
            dgvService.Columns.Add(cl1);
            dgvService.Columns.Add(cl2);
            dgvService.Columns.Add(cl3);
            dgvService.Columns.Add(cl4);

            dgvService.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvService.AllowUserToAddRows = false;

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
            AdminAddService form = new AdminAddService(this);
            form.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Pilih baris yang dicentang pada DataGridView
            var selectedRows = dgvService.Rows
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
            string serviceName = selectedRow.Cells["Nama"].Value.ToString();
            string hargaText = selectedRow.Cells["Harga per Kilogram"].Value.ToString().Replace("Rp", "").Replace(".", "").Trim();
            int price = Convert.ToInt32(hargaText);
            string description = selectedRow.Cells["Deskripsi"].Value.ToString();

            var AdminEditService = new AdminEditService(id, serviceName, price, description, this);

            if (AdminEditService.ShowDialog() == DialogResult.OK)
            {
                selectedRow.Cells["Nama"].Value = AdminEditService.ServiceName;
                selectedRow.Cells["Harga per Kilogram"].Value = AdminEditService.Price;
                selectedRow.Cells["Deskripsi"].Value = AdminEditService.Description;

                try
                {
                    using (var conn = Database.Database.GetConnection())
                    {
                        string query = "UPDATE services SET name = @name, price_per_kg = @price_per_kg, description = @description WHERE id = @id";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            // Benerin syntax AddWithValue
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@name", AdminEditService.ServiceName);
                            cmd.Parameters.AddWithValue("@price_per_kg", AdminEditService.Price);
                            cmd.Parameters.AddWithValue("@description", AdminEditService.Description);

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
            var selectedRows = dgvService.Rows.Cast<DataGridViewRow>().Where(row => Convert.ToBoolean(row.Cells["Pilih Aksi"].Value)).ToList();

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Pilih satu atau lebih baris untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Apakah Anda yakin ingin mengahpus data yang dipilih?", "Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                foreach (var row in selectedRows)
                {
                    string id = row.Cells["id"].Value.ToString();

                    try
                    {
                        using (var conn = Database.Database.GetConnection())
                        {
                            string query = "DELETE FROM services WHERE id =  @id";

                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", id);

                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                        dgvService.Rows.Remove(row);
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

        private void btnOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminOrderPage register = new AdminOrderPage();
            register.ShowDialog();
            this.Close();
        }
    }
}
