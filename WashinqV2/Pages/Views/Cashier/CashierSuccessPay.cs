using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WashinqV2.Pages.Views.Admin;

namespace WashinqV2.Pages.Views.Cashier
{
    public partial class CashierSuccessPay : Form
    {
        private decimal changeAmount;

        public event EventHandler PaymentCompleted;
        public CashierSuccessPay(decimal changeAmount)
        {
            InitializeComponent();
            this.changeAmount = changeAmount;
            this.ClientSize = new Size(480, 600);
        }

        private void CashierSuccessPay_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lbChange.Text = changeAmount.ToString("C");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var orderPage = Application.OpenForms.OfType<CashierOrderPage>().FirstOrDefault();
            if (orderPage != null)
            {
                orderPage.LoadData();
            }

            this.Close();
        }
    }
}
