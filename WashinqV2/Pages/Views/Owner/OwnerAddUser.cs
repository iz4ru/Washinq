using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Xml.Linq;
using WashinqV2.Helpers;

namespace WashinqV2.Pages.Views.Owner
{
    public partial class OwnerAddUser : Form
    {
        public OwnerAddUser()
        {
            InitializeComponent();
            this.ClientSize = new Size(800, 600);

            tbUserPassword.PasswordChar = true;
            tbUserConfirmPw.PasswordChar = true;
        }

        private void OwnerAddUser_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            cbxRole.Items = new string[] { "admin", "kasir" };
        }

        private void llbBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
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

            // Validasi format email
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

            if (string.IsNullOrWhiteSpace(tbUserPassword.Content))
            {
                MessageBox.Show("Password tidak boleh kosong!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUserPassword.Focus();
                return;
            }

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

            // Validasi role
            if (cbxRole.SelectedIndex == -1)
            {
                MessageBox.Show("Silakan pilih role (Admin atau Kasir)!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxRole.Focus();
                return;
            }

            // Proses insert ke database
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    conn.Open();

                    // Cek username sudah ada atau belum
                    string checkUsername = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkUsername, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@username", tbUserUsername.Content.Trim());
                        int userCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (userCount > 0)
                        {
                            MessageBox.Show("Username sudah digunakan, silakan gunakan username lain!",
                                "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tbUserUsername.Focus();
                            return;
                        }
                    }

                    // Cek email sudah ada atau belum
                    string checkEmail = "SELECT COUNT(*) FROM users WHERE email = @email";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkEmail, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@email", tbUserEmail.Content.Trim());
                        int emailCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (emailCount > 0)
                        {
                            MessageBox.Show("Email sudah digunakan, silakan gunakan email lain!",
                                "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tbUserEmail.Focus();
                            return;
                        }
                    }

                    // Hash password dengan SHA256
                    string hashedPassword = HashPassword(tbUserPassword.Content);

                    // Convert role dari display name ke database value
                    string selectedRole = cbxRole.SelectedItem.ToString();
                    string roleValue = selectedRole == "Admin" ? "admin" : "cashier";

                    // Insert data user dengan role yang dipilih
                    string query = @"INSERT INTO users 
                        (name, username, email, address, phone, role, password) 
                        VALUES 
                        (@name, @username, @email, @address, @phone, @role, @password)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", tbUserName.Content.Trim());
                        cmd.Parameters.AddWithValue("@username", tbUserUsername.Content.Trim());
                        cmd.Parameters.AddWithValue("@email", tbUserEmail.Content.Trim());
                        cmd.Parameters.AddWithValue("@address",
                            string.IsNullOrWhiteSpace(tbUserAddress.Content) ? null : tbUserAddress.Content.Trim());
                        cmd.Parameters.AddWithValue("@phone", tbUserPhone.Content.Trim());
                        cmd.Parameters.AddWithValue("@role", roleValue);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show($"{selectedRole} berhasil ditambahkan!",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LogActivity.Insert("Tambah Data",
                                $"Menambahkan {roleValue.ToLower()} '{tbUserName.Content}' dengan username '{tbUserUsername.Content}'");

                            // Clear form
                            ClearForm();

                            // Close dialog
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menambahkan user!",
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

        // Method untuk hash password dengan SHA256
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

        // Method untuk validasi format email
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

        // Method untuk clear form
        private void ClearForm()
        {
            tbUserName.Content = string.Empty;
            tbUserUsername.Content = string.Empty;
            tbUserEmail.Content = string.Empty;
            tbUserPhone.Content = string.Empty;
            tbUserPassword.Content = string.Empty;
            tbUserConfirmPw.Content = string.Empty;
            tbUserAddress.Content = string.Empty;
            chkShowPassword1.Checked = false;
            chkShowPassword2.Checked = false;
        }


        // Event untuk show/hide password
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
