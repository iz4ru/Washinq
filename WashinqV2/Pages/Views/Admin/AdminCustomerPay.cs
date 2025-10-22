using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WashinqV2.Pages.Views.Admin
{
    public partial class AdminCustomerPay : Form
    {
        private int customerId;
        private decimal totalPrice;
        public AdminCustomerPay(int customerId, decimal totalPrice)
        {
            InitializeComponent();
            this.customerId = customerId;
            this.totalPrice = totalPrice;
        }

        private void AdminCustomerPay_Load(object sender, EventArgs e)
        {
            // Lock window style
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.StartPosition = FormStartPosition.CenterScreen;

            // Disable close button
            this.ControlBox = false;

            // Tampilkan harga total ke label
            lbTotalPrice.Text = totalPrice.ToString("C");
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(tbPay.Content?.ToString(), out decimal paymentAmount) || paymentAmount <= 0)
            {
                MessageBox.Show("Masukkan nominal pembayaran yang valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (paymentAmount < totalPrice)
            {
                MessageBox.Show("Nominal kurang untuk melakukan pembayaran!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paymentMethod = "cash";

            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string updateQuery = @"
                UPDATE orders
                SET paid = @paid,
                    payment = @payment
                WHERE customer_id = @customerId 
                  AND (paid IS NULL OR paid = 0)
            ";

                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(updateQuery, conn))
                    {
                        // ✅ Gunakan paymentAmount (decimal) bukan string paid
                        cmd.Parameters.AddWithValue("@paid", paymentAmount);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd.Parameters.AddWithValue("@customerId", customerId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            decimal change = paymentAmount - totalPrice;
                            AdminSuccessPay successForm = new AdminSuccessPay(change);
                            successForm.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Tidak ada order yang perlu dibayar atau sudah dibayar.",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memproses pembayaran: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
