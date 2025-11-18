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
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Owner
{
    public partial class OwnerEditProfile : Form
    {
        private int userId;
        private string originalUsername;
        private string originalEmail;

        public OwnerEditProfile(int userId)
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 600);
            this.userId = userId;

            tbPassword.PasswordChar = true;
            tbConfirmPw.PasswordChar = true;
        }

        private void OwnerEditProfile_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ✅ Load data user berdasarkan userId dari session
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = @"SELECT name, username, email, phone, address 
                                   FROM users 
                                   WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);

                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbName.Content = reader["name"].ToString();
                                tbUsername.Content = reader["username"].ToString();
                                tbEmail.Content = reader["email"].ToString();
                                tbPhone.Content = reader["phone"] == DBNull.Value ? "" : reader["phone"].ToString();
                                tbAddress.Content = reader["address"] == DBNull.Value ? "" : reader["address"].ToString();

                                originalUsername = reader["username"].ToString();
                                originalEmail = reader["email"].ToString();

                                // Clear password fields
                                tbPassword.Content = string.Empty;
                                tbConfirmPw.Content = string.Empty;
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // ✅ Validasi dan update profile
            if (string.IsNullOrWhiteSpace(tbName.Content))
            {
                MessageBox.Show("Nama tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbUsername.Content))
            {
                MessageBox.Show("Username tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbEmail.Content))
            {
                MessageBox.Show("Email tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEmail.Focus();
                return;
            }

            if (!IsValidEmail(tbEmail.Content))
            {
                MessageBox.Show("Format email tidak valid!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEmail.Focus();
                return;
            }

            // Cek apakah password diisi (optional)
            bool updatePassword = false;
            string hashedPassword = null;

            if (!string.IsNullOrWhiteSpace(tbPassword.Content))
            {
                if (tbPassword.Content.Length < 6)
                {
                    MessageBox.Show("Password minimal 6 karakter!",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbPassword.Focus();
                    return;
                }

                if (tbPassword.Content != tbConfirmPw.Content)
                {
                    MessageBox.Show("Password dan Konfirmasi Password tidak sama!",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbConfirmPw.Focus();
                    return;
                }

                updatePassword = true;
                hashedPassword = HashPassword(tbPassword.Content);
            }

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();

                    // Cek username sudah ada atau belum (kecuali username sendiri)
                    if (tbUsername.Content.Trim() != originalUsername)
                    {
                        string checkUsername = "SELECT COUNT(*) FROM users WHERE username = @username AND id != @id";
                        using (MySqlCommand cmdCheck = new MySqlCommand(checkUsername, conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@username", tbUsername.Content.Trim());
                            cmdCheck.Parameters.AddWithValue("@id", userId);
                            int userCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                            if (userCount > 0)
                            {
                                MessageBox.Show("Username sudah digunakan, silakan gunakan username lain!",
                                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tbUsername.Focus();
                                return;
                            }
                        }
                    }

                    // Cek email sudah ada atau belum (kecuali email sendiri)
                    if (tbEmail.Content.Trim() != originalEmail)
                    {
                        string checkEmail = "SELECT COUNT(*) FROM users WHERE email = @email AND id != @id";
                        using (MySqlCommand cmdCheck = new MySqlCommand(checkEmail, conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@email", tbEmail.Content.Trim());
                            cmdCheck.Parameters.AddWithValue("@id", userId);
                            int emailCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                            if (emailCount > 0)
                            {
                                MessageBox.Show("Email sudah digunakan, silakan gunakan email lain!",
                                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tbEmail.Focus();
                                return;
                            }
                        }
                    }

                    // Update query
                    string query;
                    if (updatePassword)
                    {
                        query = @"UPDATE users SET 
                                name = @name, 
                                username = @username, 
                                email = @email, 
                                phone = @phone,
                                address = @address, 
                                password = @password 
                                WHERE id = @id";
                    }
                    else
                    {
                        query = @"UPDATE users SET 
                                name = @name, 
                                username = @username, 
                                email = @email, 
                                phone = @phone,
                                address = @address 
                                WHERE id = @id";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", tbName.Content.Trim());
                        cmd.Parameters.AddWithValue("@username", tbUsername.Content.Trim());
                        cmd.Parameters.AddWithValue("@email", tbEmail.Content.Trim());
                        cmd.Parameters.AddWithValue("@phone",
                            string.IsNullOrWhiteSpace(tbPhone.Content) ? null : tbPhone.Content.Trim());
                        cmd.Parameters.AddWithValue("@address",
                            string.IsNullOrWhiteSpace(tbAddress.Content) ? null : tbAddress.Content.Trim());
                        cmd.Parameters.AddWithValue("@id", userId);

                        if (updatePassword)
                        {
                            cmd.Parameters.AddWithValue("@password", hashedPassword);
                        }

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            // ✅ Update UserSession jika username berubah
                            UserSession.username = tbUsername.Content.Trim();

                            MessageBox.Show("Profile berhasil diupdate!",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Gagal mengupdate profile!",
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

        private void llbBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void chkShowPassword1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword1.Checked)
            {
                tbPassword.PasswordChar = false;
            }
            else
            {
                tbPassword.PasswordChar = true;
            }
        }

        private void chkShowPassword2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword2.Checked)
            {
                tbConfirmPw.PasswordChar = false;
            }
            else
            {
                tbConfirmPw.PasswordChar = true;
            }
        }
    }
}
