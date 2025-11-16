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
    public partial class AdminEditCashier : Form
    {
        private int cashierId;
        private string originalUsername;
        private string originalEmail;

        public AdminEditCashier(int id)
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 600);
            this.cashierId = id;
        }

        private void AdminEditCashier_Load(object sender, EventArgs e)
        {
            LoadCashierData();
        }

        private void LoadCashierData()
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = @"SELECT name, username, email, phone, address 
                                   FROM users 
                                   WHERE id = @id AND role = 'cashier'";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", cashierId);

                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbCashName.Content = reader["name"].ToString();
                                tbCashUsername.Content = reader["username"].ToString();
                                tbCashEmail.Content = reader["email"].ToString();
                                tbCashPhone.Content = reader["phone"] == DBNull.Value ? "" : reader["phone"].ToString();
                                tbCashAddress.Content = reader["address"] == DBNull.Value ? "" : reader["address"].ToString();

                                originalUsername = reader["username"].ToString();
                                originalEmail = reader["email"].ToString();

                                tbCashPassword.Content = string.Empty;
                                tbCashConfirmPw.Content = string.Empty;
                            }
                            else
                            {
                                MessageBox.Show("Data kasir tidak ditemukan!",
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
            if (string.IsNullOrWhiteSpace(tbCashName.Content))
            {
                MessageBox.Show("Nama Kasir tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbCashName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbCashUsername.Content))
            {
                MessageBox.Show("Username tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbCashUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbCashEmail.Content))
            {
                MessageBox.Show("Email tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbCashEmail.Focus();
                return;
            }

            if (!IsValidEmail(tbCashEmail.Content))
            {
                MessageBox.Show("Format email tidak valid!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbCashEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(tbCashPhone.Content))
            {
                MessageBox.Show("Nomor Telepon tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbCashPhone.Focus();
                return;
            }

            bool updatePassword = false;
            string hashedPassword = null;

            if (!string.IsNullOrWhiteSpace(tbCashPassword.Content))
            {
                if (tbCashPassword.Content.Length < 6)
                {
                    MessageBox.Show("Password minimal 6 karakter!",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbCashPassword.Focus();
                    return;
                }

                if (tbCashPassword.Content != tbCashConfirmPw.Content)
                {
                    MessageBox.Show("Password dan Konfirmasi Password tidak sama!",
                        "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbCashConfirmPw.Focus();
                    return;
                }

                updatePassword = true;
                hashedPassword = HashPassword(tbCashPassword.Content);
            }

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();

                    if (tbCashUsername.Content.Trim() != originalUsername)
                    {
                        string checkUsername = "SELECT COUNT(*) FROM users WHERE username = @username AND id != @id";
                        using (MySqlCommand cmdCheck = new MySqlCommand(checkUsername, conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@username", tbCashUsername.Content.Trim());
                            cmdCheck.Parameters.AddWithValue("@id", cashierId);
                            int userCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                            if (userCount > 0)
                            {
                                MessageBox.Show("Username sudah digunakan, silakan gunakan username lain!",
                                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tbCashUsername.Focus();
                                return;
                            }
                        }
                    }

                    if (tbCashEmail.Content.Trim() != originalEmail)
                    {
                        string checkEmail = "SELECT COUNT(*) FROM users WHERE email = @email AND id != @id";
                        using (MySqlCommand cmdCheck = new MySqlCommand(checkEmail, conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@email", tbCashEmail.Content.Trim());
                            cmdCheck.Parameters.AddWithValue("@id", cashierId);
                            int emailCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                            if (emailCount > 0)
                            {
                                MessageBox.Show("Email sudah digunakan, silakan gunakan email lain!",
                                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tbCashEmail.Focus();
                                return;
                            }
                        }
                    }

                    string query;
                    if (updatePassword)
                    {
                        query = @"UPDATE users SET 
                                name = @name, 
                                username = @username, 
                                email = @email, 
                                address = @address, 
                                phone = @phone, 
                                password = @password 
                                WHERE id = @id AND role = 'cashier'";
                    }
                    else
                    {
                        query = @"UPDATE users SET 
                                name = @name, 
                                username = @username, 
                                email = @email, 
                                address = @address, 
                                phone = @phone 
                                WHERE id = @id AND role = 'cashier'";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", tbCashName.Content.Trim());
                        cmd.Parameters.AddWithValue("@username", tbCashUsername.Content.Trim());
                        cmd.Parameters.AddWithValue("@email", tbCashEmail.Content.Trim());
                        cmd.Parameters.AddWithValue("@address",
                            string.IsNullOrWhiteSpace(tbCashAddress.Content) ? null : tbCashAddress.Content.Trim());
                        cmd.Parameters.AddWithValue("@phone", tbCashPhone.Content.Trim());
                        cmd.Parameters.AddWithValue("@id", cashierId);

                        if (updatePassword)
                        {
                            cmd.Parameters.AddWithValue("@password", hashedPassword);
                        }

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Data kasir berhasil diupdate!",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Gagal mengupdate data kasir!",
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

        private void chkShowPassword1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword1.Checked)
            {
                tbCashPassword.PasswordChar = false;
            }
            else
            {
                tbCashPassword.PasswordChar = true;
            }
        }

        private void chkShowPassword2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword2.Checked)
            {
                tbCashConfirmPw.PasswordChar = false;
            }
            else
            {
                tbCashConfirmPw.PasswordChar = true;
            }
        }
    }
}
