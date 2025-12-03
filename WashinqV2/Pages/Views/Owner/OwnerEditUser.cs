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
using WashinqV2.Helpers;

namespace WashinqV2.Pages.Views.Owner
{
    public partial class OwnerEditUser : Form
    {
        private int userId;
        private string originalUsername;
        private string originalEmail;
        private string originalName;

        public OwnerEditUser(int id)
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 600);
            this.userId = id;
            tbUserPassword.PasswordChar = true;
            tbUserConfirmPw.PasswordChar = true;
        }

        private void OwnerEditUser_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Setup ComboBox role
            cbxRole.Items = new string[] { "admin", "kasir" };

            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    // ✅ PERBAIKAN: Hapus '=' setelah 'IN'
                    string query = @"SELECT name, username, email, role, phone, address 
                                   FROM users 
                                   WHERE id = @id AND role IN ('cashier', 'admin')";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);

                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbUserName.Content = reader["name"].ToString();
                                tbUserUsername.Content = reader["username"].ToString();
                                tbUserEmail.Content = reader["email"].ToString();
                                originalName = reader["name"].ToString();

                                // ✅ PERBAIKAN: Load role ke ComboBox
                                string role = reader["role"].ToString();
                                if (role == "admin")
                                    cbxRole.SelectedItem = "admin";
                                else if (role == "cashier")
                                    cbxRole.SelectedItem = "kasir";

                                tbUserPhone.Content = reader["phone"] == DBNull.Value ? "" : reader["phone"].ToString();
                                tbUserAddress.Content = reader["address"] == DBNull.Value ? "" : reader["address"].ToString();

                                originalUsername = reader["username"].ToString();
                                originalEmail = reader["email"].ToString();

                                // Clear password fields
                                tbUserPassword.Content = string.Empty;
                                tbUserConfirmPw.Content = string.Empty;
                            }
                            else
                            {
                                MessageBox.Show("Data user tidak ditemukan!",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Kesalahan database: " + ex.Message,
                    "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message,
                    "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void llbBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // ✅ IMPLEMENTASI LENGKAP

            // Validasi input
            if (string.IsNullOrWhiteSpace(tbUserName.Content))
            {
                MessageBox.Show("Nama tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUserName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbUserUsername.Content))
            {
                MessageBox.Show("Username tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUserUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbUserEmail.Content))
            {
                MessageBox.Show("Email tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUserEmail.Focus();
                return;
            }

            if (!IsValidEmail(tbUserEmail.Content))
            {
                MessageBox.Show("Format email tidak valid!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUserEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbUserPhone.Content))
            {
                MessageBox.Show("Nomor Telepon tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUserPhone.Focus();
                return;
            }

            // Validasi role
            if (cbxRole.SelectedIndex == -1)
            {
                MessageBox.Show("Silakan pilih role (Admin atau Kasir)!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxRole.Focus();
                return;
            }

            // Cek apakah password diisi (optional untuk edit)
            bool updatePassword = false;
            string hashedPassword = null;

            if (!string.IsNullOrWhiteSpace(tbUserPassword.Content))
            {
                if (tbUserPassword.Content.Length < 6)
                {
                    MessageBox.Show("Password minimal 6 karakter!",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbUserPassword.Focus();
                    return;
                }

                if (tbUserPassword.Content != tbUserConfirmPw.Content)
                {
                    MessageBox.Show("Password dan Konfirmasi Password tidak sama!",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbUserConfirmPw.Focus();
                    return;
                }

                updatePassword = true;
                hashedPassword = HashPassword(tbUserPassword.Content);
            }

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();

                    // Cek username sudah ada atau belum (kecuali username sendiri)
                    if (tbUserUsername.Content.Trim() != originalUsername)
                    {
                        string checkUsername = "SELECT COUNT(*) FROM users WHERE username = @username AND id != @id";
                        using (MySqlCommand cmdCheck = new MySqlCommand(checkUsername, conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@username", tbUserUsername.Content.Trim());
                            cmdCheck.Parameters.AddWithValue("@id", userId);
                            int userCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                            if (userCount > 0)
                            {
                                MessageBox.Show("Username sudah digunakan, silakan gunakan username lain!",
                                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tbUserUsername.Focus();
                                return;
                            }
                        }
                    }

                    // Cek email sudah ada atau belum (kecuali email sendiri)
                    if (tbUserEmail.Content.Trim() != originalEmail)
                    {
                        string checkEmail = "SELECT COUNT(*) FROM users WHERE email = @email AND id != @id";
                        using (MySqlCommand cmdCheck = new MySqlCommand(checkEmail, conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@email", tbUserEmail.Content.Trim());
                            cmdCheck.Parameters.AddWithValue("@id", userId);
                            int emailCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                            if (emailCount > 0)
                            {
                                MessageBox.Show("Email sudah digunakan, silakan gunakan email lain!",
                                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tbUserEmail.Focus();
                                return;
                            }
                        }
                    }

                    // Convert role dari display name ke database value
                    string selectedRole = cbxRole.SelectedItem.ToString();
                    string roleValue = selectedRole == "admin" ? "admin" : "cashier";

                    // Update query
                    string query;
                    if (updatePassword)
                    {
                        query = @"UPDATE users SET 
                                name = @name, 
                                username = @username, 
                                email = @email, 
                                role = @role,
                                address = @address, 
                                phone = @phone, 
                                password = @password 
                                WHERE id = @id AND role IN ('cashier', 'admin')";
                    }
                    else
                    {
                        query = @"UPDATE users SET 
                                name = @name, 
                                username = @username, 
                                email = @email, 
                                role = @role,
                                address = @address, 
                                phone = @phone 
                                WHERE id = @id AND role IN ('cashier', 'admin')";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", tbUserName.Content.Trim());
                        cmd.Parameters.AddWithValue("@username", tbUserUsername.Content.Trim());
                        cmd.Parameters.AddWithValue("@email", tbUserEmail.Content.Trim());
                        cmd.Parameters.AddWithValue("@role", roleValue);
                        cmd.Parameters.AddWithValue("@address",
                            string.IsNullOrWhiteSpace(tbUserAddress.Content) ? null : tbUserAddress.Content.Trim());
                        cmd.Parameters.AddWithValue("@phone", tbUserPhone.Content.Trim());
                        cmd.Parameters.AddWithValue("@id", userId);

                        if (updatePassword)
                        {
                            cmd.Parameters.AddWithValue("@password", hashedPassword);
                        }

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Data user berhasil diupdate!",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.DialogResult = DialogResult.OK;
                            string passwordChanged = string.IsNullOrWhiteSpace(tbUserPassword.Content)
                ? ""
                : " (password diubah)";

                            LogActivity.Insert("Edit Data",
                                $"Mengubah data {selectedRole.ToLower()} '{tbUserName.Content}' (ID: {userId}){passwordChanged}");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Gagal mengupdate data user!",
                                "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Kesalahan database: " + ex.Message,
                    "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message,
                    "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ TAMBAHAN: Helper Methods
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // ✅ TAMBAHAN: Event handlers untuk show/hide password
        private void chkShowPassword1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword1.Checked)
            {
                tbUserPassword.PasswordChar = false;
            }
            else
            {
                tbUserPassword.PasswordChar = true;
            }
        }

        private void chkShowPassword2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword2.Checked)
            {
                tbUserConfirmPw.PasswordChar = false;
            }
            else
            {
                tbUserConfirmPw.PasswordChar = true;
            }
        }
    }
}
