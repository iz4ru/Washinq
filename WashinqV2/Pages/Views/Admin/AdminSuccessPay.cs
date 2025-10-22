using MySql.Data.MySqlClient;
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
    public partial class AdminSuccessPay : Form
    {
        private decimal changeAmount;

        public event EventHandler PaymentCompleted;
        public AdminSuccessPay(decimal changeAmount)
        {
            InitializeComponent();
            this.changeAmount = changeAmount;
        }

        private void AdminSuccessPay_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lbChange.Text = changeAmount.ToString("C");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var orderPage = Application.OpenForms.OfType<AdminOrderPage>().FirstOrDefault();
            if (orderPage != null)
            {
                orderPage.LoadData(); 
            }

            this.Close();
        }
    }
}