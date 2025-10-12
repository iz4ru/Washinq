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

namespace WashinqV2
{
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();

            tbPassword.PasswordChar = true; // Pastikan password disembunyikan di awal
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            string username = tbUsername.Content?.Trim();
            string password = tbPassword.Content?.Trim();

            string hashedPassword = ComputeSha256Hash(password);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username dan Password tidak boleh kosongy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var conn = Database.Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM users WHERE username = @username AND password = @password";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserSession.id = Convert.ToInt16(reader["id"]);
                            UserSession.username = reader["username"].ToString();
                            UserSession.role = reader["role"].ToString();

                            MessageBox.Show("Login berhasil!", "Success", MessageBoxButtons.OK);

                            if (UserSession.role == "admin")
                            {
                                this.Hide();
                                using (var adminPage = new Pages.Views.AdminPage())
                                {
                                    adminPage.ShowDialog();
                                }
                                this.Close();
                                return;
                            }
                            else if (UserSession.role == "owner")
                            {
                                this.Hide();
                                using (var ownerPage = new Pages.Views.OwnerPage())
                                {
                                    ownerPage.ShowDialog();
                                }
                                this.Close();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Invalid Role", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Username atau password invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
            {
                tbPassword.PasswordChar = false; // Menampilkan password
            }
            else
            {
                tbPassword.PasswordChar = true; // Menyembunyikan password
            }
        }
    }
}
