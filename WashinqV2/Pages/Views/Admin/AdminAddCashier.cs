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
    public partial class AdminAddCashier : Form
    {
        public AdminAddCashier()
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 600);
            tbCashPassword.PasswordChar = true;
            tbCashConfirmPw.PasswordChar = true;
        }

        private void llbBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnTambahkan_Click(object sender, EventArgs e)
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

            if (string.IsNullOrWhiteSpace(tbCashPassword.Content))
            {
                MessageBox.Show("Password tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbCashPassword.Focus();
                return;
            }

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

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();

                    string checkUsername = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkUsername, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@username", tbCashUsername.Content.Trim());
                        int userCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (userCount > 0)
                        {
                            MessageBox.Show("Username sudah digunakan, silakan gunakan username lain!",
                                "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tbCashUsername.Focus();
                            return;
                        }
                    }

                    string checkEmail = "SELECT COUNT(*) FROM users WHERE email = @email";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkEmail, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@email", tbCashEmail.Content.Trim());
                        int emailCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (emailCount > 0)
                        {
                            MessageBox.Show("Email sudah digunakan, silakan gunakan email lain!",
                                "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tbCashEmail.Focus();
                            return;
                        }
                    }

                    string hashedPassword = HashPassword(tbCashPassword.Content);

                    string query = @"INSERT INTO users 
                        (name, username, email, address, phone, role, password) 
                        VALUES 
                        (@name, @username, @email, @address, @phone, 'cashier', @password)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", tbCashName.Content.Trim());
                        cmd.Parameters.AddWithValue("@username", tbCashUsername.Content.Trim());
                        cmd.Parameters.AddWithValue("@email", tbCashEmail.Content.Trim());
                        cmd.Parameters.AddWithValue("@address",
                            string.IsNullOrWhiteSpace(tbCashAddress.Content) ? null : tbCashAddress.Content.Trim());
                        cmd.Parameters.AddWithValue("@phone", tbCashPhone.Content.Trim());
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Kasir berhasil ditambahkan!",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ClearForm();

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menambahkan kasir!",
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

        private void ClearForm()
        {
            tbCashName.Content = string.Empty;
            tbCashUsername.Content = string.Empty;
            tbCashEmail.Content = string.Empty;
            tbCashPhone.Content = string.Empty;
            tbCashPassword.Content = string.Empty;
            tbCashConfirmPw.Content = string.Empty;
            tbCashAddress.Content = string.Empty;
            chkShowPassword1.Checked = false;
            chkShowPassword2.Checked = false;
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
