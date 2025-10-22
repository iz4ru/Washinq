using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using WashinqV2.Models;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminAddCustomer : Form
    {
        public AdminAddCustomer()
        {
            InitializeComponent();
        }

        private void AdminAddCustomer_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCustName.Content) ||
                string.IsNullOrWhiteSpace(tbPhone.Content) ||
                string.IsNullOrWhiteSpace(tbAddress.Content))
            {
                MessageBox.Show("Semua field harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string custName = tbCustName.Content.Trim();
            string phone = tbPhone.Content.Trim();
            string address = tbAddress.Content.Trim();

            // Validasi nomor telepon
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0\d+$"))
            {
                MessageBox.Show("Nomor telepon harus berupa angka dan diawali dengan 0.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int customerId = 0;
                using (var conn = Database.Database.GetConnection())
                {
                    string query = "INSERT INTO customers (name, phone, address) VALUES (@name, @phone, @address)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", custName);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@address", address);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Ambil ID customer terakhir yang baru dimasukkan
                        customerId = (int)cmd.LastInsertedId;
                    }
                }

                var result = MessageBox.Show("Customer berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    var addOrder = new AdminAddOrder(customerId);
                    addOrder.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void llbBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
