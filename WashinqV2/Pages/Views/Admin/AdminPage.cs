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

namespace WashinqV2.Pages.Views
{
    public partial class AdminPage : Form
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void AdminPage_Load(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Konfirmasi logout
            var result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide(); // Menutup form dashboard
                LoginPage LoginPage = new LoginPage();
                LoginPage.ShowDialog(); // Menampilkan form login
                this.Close();
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {

        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminOrderPage register = new AdminOrderPage();
            register.ShowDialog();
            this.Close();
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminServicePage register = new AdminServicePage();
            register.ShowDialog();
            this.Close();
        }
    }
}
