namespace WashinqV2.Pages.Views.Admin
{
    partial class AdminAddCashier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminAddCashier));
            this.cuiPanel2 = new CuoreUI.Controls.cuiPanel();
            this.llbBack = new System.Windows.Forms.LinkLabel();
            this.cuiPanel3 = new CuoreUI.Controls.cuiPanel();
            this.tbCashAddress = new CuoreUI.Controls.cuiTextBox();
            this.lbAddress = new System.Windows.Forms.Label();
            this.tbCashPhone = new CuoreUI.Controls.cuiTextBox();
            this.lbPhone = new System.Windows.Forms.Label();
            this.btnTambahkan = new CuoreUI.Controls.cuiButton();
            this.tbCashName = new CuoreUI.Controls.cuiTextBox();
            this.lbCashName = new System.Windows.Forms.Label();
            this.tbCashEmail = new CuoreUI.Controls.cuiTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCashUsername = new CuoreUI.Controls.cuiTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCashConfirmPw = new CuoreUI.Controls.cuiTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCashPassword = new CuoreUI.Controls.cuiTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkShowPassword1 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.chkShowPassword2 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.cuiPanel2.SuspendLayout();
            this.cuiPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cuiPanel2
            // 
            this.cuiPanel2.Controls.Add(this.llbBack);
            this.cuiPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.cuiPanel2.Location = new System.Drawing.Point(0, 0);
            this.cuiPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cuiPanel2.Name = "cuiPanel2";
            this.cuiPanel2.OutlineThickness = 1F;
            this.cuiPanel2.PanelColor = System.Drawing.Color.White;
            this.cuiPanel2.PanelOutlineColor = System.Drawing.Color.Transparent;
            this.cuiPanel2.Rounding = new System.Windows.Forms.Padding(0);
            this.cuiPanel2.Size = new System.Drawing.Size(1233, 71);
            this.cuiPanel2.TabIndex = 19;
            // 
            // llbBack
            // 
            this.llbBack.AutoSize = true;
            this.llbBack.BackColor = System.Drawing.Color.Transparent;
            this.llbBack.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold);
            this.llbBack.LinkColor = System.Drawing.Color.DodgerBlue;
            this.llbBack.Location = new System.Drawing.Point(19, 24);
            this.llbBack.Name = "llbBack";
            this.llbBack.Size = new System.Drawing.Size(88, 26);
            this.llbBack.TabIndex = 16;
            this.llbBack.TabStop = true;
            this.llbBack.Text = "Kembali";
            this.llbBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbBack_LinkClicked);
            // 
            // cuiPanel3
            // 
            this.cuiPanel3.Controls.Add(this.chkShowPassword2);
            this.cuiPanel3.Controls.Add(this.chkShowPassword1);
            this.cuiPanel3.Controls.Add(this.tbCashConfirmPw);
            this.cuiPanel3.Controls.Add(this.label3);
            this.cuiPanel3.Controls.Add(this.tbCashPassword);
            this.cuiPanel3.Controls.Add(this.label4);
            this.cuiPanel3.Controls.Add(this.tbCashEmail);
            this.cuiPanel3.Controls.Add(this.label1);
            this.cuiPanel3.Controls.Add(this.tbCashUsername);
            this.cuiPanel3.Controls.Add(this.label2);
            this.cuiPanel3.Controls.Add(this.tbCashAddress);
            this.cuiPanel3.Controls.Add(this.lbAddress);
            this.cuiPanel3.Controls.Add(this.tbCashPhone);
            this.cuiPanel3.Controls.Add(this.lbPhone);
            this.cuiPanel3.Controls.Add(this.btnTambahkan);
            this.cuiPanel3.Controls.Add(this.tbCashName);
            this.cuiPanel3.Controls.Add(this.lbCashName);
            this.cuiPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cuiPanel3.Location = new System.Drawing.Point(0, 71);
            this.cuiPanel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cuiPanel3.Name = "cuiPanel3";
            this.cuiPanel3.OutlineThickness = 1F;
            this.cuiPanel3.PanelColor = System.Drawing.SystemColors.Control;
            this.cuiPanel3.PanelOutlineColor = System.Drawing.Color.Transparent;
            this.cuiPanel3.Rounding = new System.Windows.Forms.Padding(0);
            this.cuiPanel3.Size = new System.Drawing.Size(1233, 830);
            this.cuiPanel3.TabIndex = 20;
            // 
            // tbCashAddress
            // 
            this.tbCashAddress.BackColor = System.Drawing.Color.Transparent;
            this.tbCashAddress.BackgroundColor = System.Drawing.Color.White;
            this.tbCashAddress.Content = "";
            this.tbCashAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCashAddress.FocusBackgroundColor = System.Drawing.Color.White;
            this.tbCashAddress.FocusImageTint = System.Drawing.Color.White;
            this.tbCashAddress.FocusOutlineColor = System.Drawing.Color.DodgerBlue;
            this.tbCashAddress.Font = new System.Drawing.Font("Figtree", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCashAddress.ForeColor = System.Drawing.Color.Black;
            this.tbCashAddress.Image = null;
            this.tbCashAddress.ImageExpand = new System.Drawing.Point(0, 0);
            this.tbCashAddress.ImageOffset = new System.Drawing.Point(0, 0);
            this.tbCashAddress.Location = new System.Drawing.Point(132, 477);
            this.tbCashAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbCashAddress.Multiline = true;
            this.tbCashAddress.Name = "tbCashAddress";
            this.tbCashAddress.NormalImageTint = System.Drawing.Color.White;
            this.tbCashAddress.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbCashAddress.Padding = new System.Windows.Forms.Padding(25, 7, 25, 7);
            this.tbCashAddress.PasswordChar = false;
            this.tbCashAddress.PlaceholderColor = System.Drawing.SystemColors.WindowText;
            this.tbCashAddress.PlaceholderText = "";
            this.tbCashAddress.Rounding = new System.Windows.Forms.Padding(8);
            this.tbCashAddress.Size = new System.Drawing.Size(450, 202);
            this.tbCashAddress.TabIndex = 23;
            this.tbCashAddress.TextOffset = new System.Drawing.Size(0, 0);
            this.tbCashAddress.UnderlinedStyle = true;
            // 
            // lbAddress
            // 
            this.lbAddress.AutoSize = true;
            this.lbAddress.BackColor = System.Drawing.Color.Transparent;
            this.lbAddress.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAddress.Location = new System.Drawing.Point(127, 446);
            this.lbAddress.Name = "lbAddress";
            this.lbAddress.Size = new System.Drawing.Size(79, 26);
            this.lbAddress.TabIndex = 22;
            this.lbAddress.Text = "Alamat";
            // 
            // tbCashPhone
            // 
            this.tbCashPhone.BackColor = System.Drawing.Color.Transparent;
            this.tbCashPhone.BackgroundColor = System.Drawing.Color.White;
            this.tbCashPhone.Content = "";
            this.tbCashPhone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCashPhone.FocusBackgroundColor = System.Drawing.Color.White;
            this.tbCashPhone.FocusImageTint = System.Drawing.Color.White;
            this.tbCashPhone.FocusOutlineColor = System.Drawing.Color.DodgerBlue;
            this.tbCashPhone.Font = new System.Drawing.Font("Figtree", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCashPhone.ForeColor = System.Drawing.Color.Black;
            this.tbCashPhone.Image = null;
            this.tbCashPhone.ImageExpand = new System.Drawing.Point(0, 0);
            this.tbCashPhone.ImageOffset = new System.Drawing.Point(0, 0);
            this.tbCashPhone.Location = new System.Drawing.Point(132, 214);
            this.tbCashPhone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbCashPhone.Multiline = false;
            this.tbCashPhone.Name = "tbCashPhone";
            this.tbCashPhone.NormalImageTint = System.Drawing.Color.White;
            this.tbCashPhone.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbCashPhone.Padding = new System.Windows.Forms.Padding(25, 15, 25, 0);
            this.tbCashPhone.PasswordChar = false;
            this.tbCashPhone.PlaceholderColor = System.Drawing.SystemColors.WindowText;
            this.tbCashPhone.PlaceholderText = "";
            this.tbCashPhone.Rounding = new System.Windows.Forms.Padding(8);
            this.tbCashPhone.Size = new System.Drawing.Size(450, 55);
            this.tbCashPhone.TabIndex = 21;
            this.tbCashPhone.TextOffset = new System.Drawing.Size(0, 0);
            this.tbCashPhone.UnderlinedStyle = true;
            // 
            // lbPhone
            // 
            this.lbPhone.AutoSize = true;
            this.lbPhone.BackColor = System.Drawing.Color.Transparent;
            this.lbPhone.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPhone.Location = new System.Drawing.Point(127, 182);
            this.lbPhone.Name = "lbPhone";
            this.lbPhone.Size = new System.Drawing.Size(158, 26);
            this.lbPhone.TabIndex = 20;
            this.lbPhone.Text = "Nomor Telepon";
            // 
            // btnTambahkan
            // 
            this.btnTambahkan.BackColor = System.Drawing.Color.Transparent;
            this.btnTambahkan.CheckButton = false;
            this.btnTambahkan.Checked = false;
            this.btnTambahkan.CheckedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.btnTambahkan.CheckedForeColor = System.Drawing.Color.White;
            this.btnTambahkan.CheckedImageTint = System.Drawing.Color.White;
            this.btnTambahkan.CheckedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.btnTambahkan.Content = "Tambahkan";
            this.btnTambahkan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTambahkan.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnTambahkan.Font = new System.Drawing.Font("Figtree", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTambahkan.ForeColor = System.Drawing.Color.White;
            this.btnTambahkan.HoverBackground = System.Drawing.Color.DodgerBlue;
            this.btnTambahkan.HoverForeColor = System.Drawing.Color.White;
            this.btnTambahkan.HoverImageTint = System.Drawing.Color.White;
            this.btnTambahkan.HoverOutline = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnTambahkan.Image = null;
            this.btnTambahkan.ImageAutoCenter = true;
            this.btnTambahkan.ImageExpand = new System.Drawing.Point(0, 0);
            this.btnTambahkan.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnTambahkan.Location = new System.Drawing.Point(496, 725);
            this.btnTambahkan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTambahkan.Name = "btnTambahkan";
            this.btnTambahkan.NormalBackground = System.Drawing.Color.DodgerBlue;
            this.btnTambahkan.NormalForeColor = System.Drawing.Color.White;
            this.btnTambahkan.NormalImageTint = System.Drawing.Color.White;
            this.btnTambahkan.NormalOutline = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnTambahkan.OutlineThickness = 1F;
            this.btnTambahkan.PressedBackground = System.Drawing.Color.Transparent;
            this.btnTambahkan.PressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnTambahkan.PressedImageTint = System.Drawing.Color.White;
            this.btnTambahkan.PressedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnTambahkan.Rounding = new System.Windows.Forms.Padding(8);
            this.btnTambahkan.Size = new System.Drawing.Size(226, 62);
            this.btnTambahkan.TabIndex = 16;
            this.btnTambahkan.TextAlignment = System.Drawing.StringAlignment.Center;
            this.btnTambahkan.TextOffset = new System.Drawing.Point(0, 0);
            this.btnTambahkan.Click += new System.EventHandler(this.btnTambahkan_Click);
            // 
            // tbCashName
            // 
            this.tbCashName.BackColor = System.Drawing.Color.Transparent;
            this.tbCashName.BackgroundColor = System.Drawing.Color.White;
            this.tbCashName.Content = "";
            this.tbCashName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCashName.FocusBackgroundColor = System.Drawing.Color.White;
            this.tbCashName.FocusImageTint = System.Drawing.Color.White;
            this.tbCashName.FocusOutlineColor = System.Drawing.Color.DodgerBlue;
            this.tbCashName.Font = new System.Drawing.Font("Figtree", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCashName.ForeColor = System.Drawing.Color.Black;
            this.tbCashName.Image = null;
            this.tbCashName.ImageExpand = new System.Drawing.Point(0, 0);
            this.tbCashName.ImageOffset = new System.Drawing.Point(0, 0);
            this.tbCashName.Location = new System.Drawing.Point(132, 99);
            this.tbCashName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbCashName.Multiline = false;
            this.tbCashName.Name = "tbCashName";
            this.tbCashName.NormalImageTint = System.Drawing.Color.White;
            this.tbCashName.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbCashName.Padding = new System.Windows.Forms.Padding(25, 15, 25, 0);
            this.tbCashName.PasswordChar = false;
            this.tbCashName.PlaceholderColor = System.Drawing.SystemColors.WindowText;
            this.tbCashName.PlaceholderText = "";
            this.tbCashName.Rounding = new System.Windows.Forms.Padding(8);
            this.tbCashName.Size = new System.Drawing.Size(450, 55);
            this.tbCashName.TabIndex = 9;
            this.tbCashName.TextOffset = new System.Drawing.Size(0, 0);
            this.tbCashName.UnderlinedStyle = true;
            // 
            // lbCashName
            // 
            this.lbCashName.AutoSize = true;
            this.lbCashName.BackColor = System.Drawing.Color.Transparent;
            this.lbCashName.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCashName.Location = new System.Drawing.Point(127, 68);
            this.lbCashName.Name = "lbCashName";
            this.lbCashName.Size = new System.Drawing.Size(119, 26);
            this.lbCashName.TabIndex = 8;
            this.lbCashName.Text = "Nama Kasir";
            // 
            // tbCashEmail
            // 
            this.tbCashEmail.BackColor = System.Drawing.Color.Transparent;
            this.tbCashEmail.BackgroundColor = System.Drawing.Color.White;
            this.tbCashEmail.Content = "";
            this.tbCashEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCashEmail.FocusBackgroundColor = System.Drawing.Color.White;
            this.tbCashEmail.FocusImageTint = System.Drawing.Color.White;
            this.tbCashEmail.FocusOutlineColor = System.Drawing.Color.DodgerBlue;
            this.tbCashEmail.Font = new System.Drawing.Font("Figtree", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCashEmail.ForeColor = System.Drawing.Color.Black;
            this.tbCashEmail.Image = null;
            this.tbCashEmail.ImageExpand = new System.Drawing.Point(0, 0);
            this.tbCashEmail.ImageOffset = new System.Drawing.Point(0, 0);
            this.tbCashEmail.Location = new System.Drawing.Point(630, 214);
            this.tbCashEmail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbCashEmail.Multiline = false;
            this.tbCashEmail.Name = "tbCashEmail";
            this.tbCashEmail.NormalImageTint = System.Drawing.Color.White;
            this.tbCashEmail.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbCashEmail.Padding = new System.Windows.Forms.Padding(25, 15, 25, 0);
            this.tbCashEmail.PasswordChar = false;
            this.tbCashEmail.PlaceholderColor = System.Drawing.SystemColors.WindowText;
            this.tbCashEmail.PlaceholderText = "";
            this.tbCashEmail.Rounding = new System.Windows.Forms.Padding(8);
            this.tbCashEmail.Size = new System.Drawing.Size(450, 55);
            this.tbCashEmail.TabIndex = 27;
            this.tbCashEmail.TextOffset = new System.Drawing.Size(0, 0);
            this.tbCashEmail.UnderlinedStyle = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(625, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 26);
            this.label1.TabIndex = 26;
            this.label1.Text = "Email";
            // 
            // tbCashUsername
            // 
            this.tbCashUsername.BackColor = System.Drawing.Color.Transparent;
            this.tbCashUsername.BackgroundColor = System.Drawing.Color.White;
            this.tbCashUsername.Content = "";
            this.tbCashUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCashUsername.FocusBackgroundColor = System.Drawing.Color.White;
            this.tbCashUsername.FocusImageTint = System.Drawing.Color.White;
            this.tbCashUsername.FocusOutlineColor = System.Drawing.Color.DodgerBlue;
            this.tbCashUsername.Font = new System.Drawing.Font("Figtree", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCashUsername.ForeColor = System.Drawing.Color.Black;
            this.tbCashUsername.Image = null;
            this.tbCashUsername.ImageExpand = new System.Drawing.Point(0, 0);
            this.tbCashUsername.ImageOffset = new System.Drawing.Point(0, 0);
            this.tbCashUsername.Location = new System.Drawing.Point(630, 99);
            this.tbCashUsername.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbCashUsername.Multiline = false;
            this.tbCashUsername.Name = "tbCashUsername";
            this.tbCashUsername.NormalImageTint = System.Drawing.Color.White;
            this.tbCashUsername.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbCashUsername.Padding = new System.Windows.Forms.Padding(25, 15, 25, 0);
            this.tbCashUsername.PasswordChar = false;
            this.tbCashUsername.PlaceholderColor = System.Drawing.SystemColors.WindowText;
            this.tbCashUsername.PlaceholderText = "";
            this.tbCashUsername.Rounding = new System.Windows.Forms.Padding(8);
            this.tbCashUsername.Size = new System.Drawing.Size(450, 55);
            this.tbCashUsername.TabIndex = 25;
            this.tbCashUsername.TextOffset = new System.Drawing.Size(0, 0);
            this.tbCashUsername.UnderlinedStyle = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(625, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 26);
            this.label2.TabIndex = 24;
            this.label2.Text = "Username";
            // 
            // tbCashConfirmPw
            // 
            this.tbCashConfirmPw.BackColor = System.Drawing.Color.Transparent;
            this.tbCashConfirmPw.BackgroundColor = System.Drawing.Color.White;
            this.tbCashConfirmPw.Content = "";
            this.tbCashConfirmPw.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCashConfirmPw.FocusBackgroundColor = System.Drawing.Color.White;
            this.tbCashConfirmPw.FocusImageTint = System.Drawing.Color.White;
            this.tbCashConfirmPw.FocusOutlineColor = System.Drawing.Color.DodgerBlue;
            this.tbCashConfirmPw.Font = new System.Drawing.Font("Figtree", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCashConfirmPw.ForeColor = System.Drawing.Color.Black;
            this.tbCashConfirmPw.Image = null;
            this.tbCashConfirmPw.ImageExpand = new System.Drawing.Point(0, 0);
            this.tbCashConfirmPw.ImageOffset = new System.Drawing.Point(0, 0);
            this.tbCashConfirmPw.Location = new System.Drawing.Point(630, 334);
            this.tbCashConfirmPw.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbCashConfirmPw.Multiline = false;
            this.tbCashConfirmPw.Name = "tbCashConfirmPw";
            this.tbCashConfirmPw.NormalImageTint = System.Drawing.Color.White;
            this.tbCashConfirmPw.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbCashConfirmPw.Padding = new System.Windows.Forms.Padding(25, 15, 25, 0);
            this.tbCashConfirmPw.PasswordChar = false;
            this.tbCashConfirmPw.PlaceholderColor = System.Drawing.SystemColors.WindowText;
            this.tbCashConfirmPw.PlaceholderText = "";
            this.tbCashConfirmPw.Rounding = new System.Windows.Forms.Padding(8);
            this.tbCashConfirmPw.Size = new System.Drawing.Size(450, 55);
            this.tbCashConfirmPw.TabIndex = 31;
            this.tbCashConfirmPw.TextOffset = new System.Drawing.Size(0, 0);
            this.tbCashConfirmPw.UnderlinedStyle = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(625, 302);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 26);
            this.label3.TabIndex = 30;
            this.label3.Text = "Konfirmasi Password";
            // 
            // tbCashPassword
            // 
            this.tbCashPassword.BackColor = System.Drawing.Color.Transparent;
            this.tbCashPassword.BackgroundColor = System.Drawing.Color.White;
            this.tbCashPassword.Content = "";
            this.tbCashPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCashPassword.FocusBackgroundColor = System.Drawing.Color.White;
            this.tbCashPassword.FocusImageTint = System.Drawing.Color.White;
            this.tbCashPassword.FocusOutlineColor = System.Drawing.Color.DodgerBlue;
            this.tbCashPassword.Font = new System.Drawing.Font("Figtree", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCashPassword.ForeColor = System.Drawing.Color.Black;
            this.tbCashPassword.Image = null;
            this.tbCashPassword.ImageExpand = new System.Drawing.Point(0, 0);
            this.tbCashPassword.ImageOffset = new System.Drawing.Point(0, 0);
            this.tbCashPassword.Location = new System.Drawing.Point(132, 334);
            this.tbCashPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbCashPassword.Multiline = false;
            this.tbCashPassword.Name = "tbCashPassword";
            this.tbCashPassword.NormalImageTint = System.Drawing.Color.White;
            this.tbCashPassword.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbCashPassword.Padding = new System.Windows.Forms.Padding(25, 15, 25, 0);
            this.tbCashPassword.PasswordChar = false;
            this.tbCashPassword.PlaceholderColor = System.Drawing.SystemColors.WindowText;
            this.tbCashPassword.PlaceholderText = "";
            this.tbCashPassword.Rounding = new System.Windows.Forms.Padding(8);
            this.tbCashPassword.Size = new System.Drawing.Size(450, 55);
            this.tbCashPassword.TabIndex = 29;
            this.tbCashPassword.TextOffset = new System.Drawing.Size(0, 0);
            this.tbCashPassword.UnderlinedStyle = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(127, 302);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 26);
            this.label4.TabIndex = 28;
            this.label4.Text = "Password";
            // 
            // chkShowPassword1
            // 
            this.chkShowPassword1.AutoSize = true;
            this.chkShowPassword1.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkShowPassword1.CheckedState.BorderRadius = 0;
            this.chkShowPassword1.CheckedState.BorderThickness = 0;
            this.chkShowPassword1.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkShowPassword1.Font = new System.Drawing.Font("Figtree", 9F);
            this.chkShowPassword1.ForeColor = System.Drawing.Color.DimGray;
            this.chkShowPassword1.Location = new System.Drawing.Point(132, 398);
            this.chkShowPassword1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkShowPassword1.Name = "chkShowPassword1";
            this.chkShowPassword1.Size = new System.Drawing.Size(161, 26);
            this.chkShowPassword1.TabIndex = 32;
            this.chkShowPassword1.Text = "Show Password";
            this.chkShowPassword1.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkShowPassword1.UncheckedState.BorderRadius = 0;
            this.chkShowPassword1.UncheckedState.BorderThickness = 0;
            this.chkShowPassword1.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkShowPassword1.CheckedChanged += new System.EventHandler(this.chkShowPassword1_CheckedChanged);
            // 
            // chkShowPassword2
            // 
            this.chkShowPassword2.AutoSize = true;
            this.chkShowPassword2.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkShowPassword2.CheckedState.BorderRadius = 0;
            this.chkShowPassword2.CheckedState.BorderThickness = 0;
            this.chkShowPassword2.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkShowPassword2.Font = new System.Drawing.Font("Figtree", 9F);
            this.chkShowPassword2.ForeColor = System.Drawing.Color.DimGray;
            this.chkShowPassword2.Location = new System.Drawing.Point(630, 398);
            this.chkShowPassword2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkShowPassword2.Name = "chkShowPassword2";
            this.chkShowPassword2.Size = new System.Drawing.Size(161, 26);
            this.chkShowPassword2.TabIndex = 33;
            this.chkShowPassword2.Text = "Show Password";
            this.chkShowPassword2.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkShowPassword2.UncheckedState.BorderRadius = 0;
            this.chkShowPassword2.UncheckedState.BorderThickness = 0;
            this.chkShowPassword2.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkShowPassword2.CheckedChanged += new System.EventHandler(this.chkShowPassword2_CheckedChanged);
            // 
            // AdminAddCashier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 901);
            this.Controls.Add(this.cuiPanel3);
            this.Controls.Add(this.cuiPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdminAddCashier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Washinq | Tambahkan Data Kasir";
            this.cuiPanel2.ResumeLayout(false);
            this.cuiPanel2.PerformLayout();
            this.cuiPanel3.ResumeLayout(false);
            this.cuiPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CuoreUI.Controls.cuiPanel cuiPanel2;
        private System.Windows.Forms.LinkLabel llbBack;
        private CuoreUI.Controls.cuiPanel cuiPanel3;
        private CuoreUI.Controls.cuiTextBox tbCashAddress;
        private System.Windows.Forms.Label lbAddress;
        private CuoreUI.Controls.cuiTextBox tbCashPhone;
        private System.Windows.Forms.Label lbPhone;
        private CuoreUI.Controls.cuiButton btnTambahkan;
        private CuoreUI.Controls.cuiTextBox tbCashName;
        private System.Windows.Forms.Label lbCashName;
        private CuoreUI.Controls.cuiTextBox tbCashEmail;
        private System.Windows.Forms.Label label1;
        private CuoreUI.Controls.cuiTextBox tbCashUsername;
        private System.Windows.Forms.Label label2;
        private CuoreUI.Controls.cuiTextBox tbCashConfirmPw;
        private System.Windows.Forms.Label label3;
        private CuoreUI.Controls.cuiTextBox tbCashPassword;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2CheckBox chkShowPassword2;
        private Guna.UI2.WinForms.Guna2CheckBox chkShowPassword1;
    }
}