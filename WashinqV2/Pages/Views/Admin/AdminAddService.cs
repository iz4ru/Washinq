using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminAddService : Form
    {
        private AdminServicePage parentForm;

        // Simpan categoryId yang dipilih
        private int selectedCategoryId;

        // Mapping: Nama kategori -> ID & unit_type
        private Dictionary<string, (int Id, string UnitType)> categoryMap = new Dictionary<string, (int, string)>();

        public AdminAddService(AdminServicePage parent, int categoryId)
        {
            InitializeComponent();
            parentForm = parent;
            this.selectedCategoryId = categoryId;
        }

        private void AdminAddService_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Harga hanya angka
            tbPrice.KeyPress += TbPrice_KeyPress;

            LoadCategory();

            cbxCategory.SelectedIndexChanged += cbxCategory_SelectedIndexChanged;

            SetInitialCategoryFromId();
        }

        private void LoadCategory()
        {
            categoryMap.Clear();
            cbxCategory.Items = new string[0];

            using (var conn = Database.Database.GetConnection())
            {
                string query = @"SELECT id, name, unit_type 
                                 FROM categories 
                                 ORDER BY name ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    var list = new List<string>();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string name = reader["name"].ToString();
                            string unitType = reader["unit_type"].ToString();

                            list.Add(name);
                            categoryMap[name] = (id, unitType);
                        }
                    }

                    cbxCategory.Items = list.ToArray();
                }
            }

            // Event ketika kategori diganti
            cbxCategory.SelectedIndexChanged += cbxCategory_SelectedIndexChanged;
        }

        private void SetInitialCategoryFromId()
        {
            if (selectedCategoryId == 0)
            {
                lbUnitType.Text = "-";
                return;
            }

            using (var conn = Database.Database.GetConnection())
            {
                string query = @"SELECT name, unit_type 
                         FROM categories 
                         WHERE id = @id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", selectedCategoryId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader["name"].ToString();
                            string unitType = reader["unit_type"].ToString();

                            lbUnitType.Text = unitType;

                            // ✅ Karena Items adalah string[], set pakai SelectedItem
                            cbxCategory.SelectedItem = name;
                        }
                    }
                }
            }
        }


        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCategory.SelectedIndex < 0) return;

            string selectedName = cbxCategory.SelectedItem.ToString();

            if (categoryMap.TryGetValue(selectedName, out var info))
            {
                selectedCategoryId = info.Id;
                lbUnitType.Text = info.UnitType; // contoh: kg, pcs, pasang, meter
            }
        }

        private void TbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Hanya izinkan angka dan tombol kontrol seperti Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Kolom harga hanya boleh angka!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbService.Content) ||
                string.IsNullOrWhiteSpace(tbPrice.Content) ||
                string.IsNullOrWhiteSpace(tbDescription.Content))
            {
                MessageBox.Show("Semua field harus diisi!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbxCategory.SelectedIndex < 0 || selectedCategoryId == 0)
            {
                MessageBox.Show("Silakan pilih kategori layanan!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(tbPrice.Content.Trim(), out int price))
            {
                MessageBox.Show("Harga harus berupa angka!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbPrice.Focus();
                return;
            }

            string serviceName = tbService.Content.Trim();
            string description = tbDescription.Content.Trim();

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    // Sesuaikan nama kolom dengan tabel services Anda
                    string query = @"INSERT INTO services 
                                    (category_id, name, price, description) 
                                    VALUES 
                                    (@category_id, @name, @price, @description)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@category_id", selectedCategoryId);
                        cmd.Parameters.AddWithValue("@name", serviceName);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@description",
                            string.IsNullOrWhiteSpace(description) ? null : description);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Layanan berhasil ditambahkan!",
                        "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    parentForm.LoadData(); // refresh table di AdminServicePage
                    this.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Koneksi ke database gagal: " + ex.Message,
                    "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message,
                    "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void llbBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
