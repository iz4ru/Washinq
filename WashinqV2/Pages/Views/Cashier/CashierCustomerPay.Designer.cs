namespace WashinqV2.Pages.Views.Cashier
{
    partial class CashierCustomerPay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashierCustomerPay));
            this.cuiPanel2 = new CuoreUI.Controls.cuiPanel();
            this.cuiPanel3 = new CuoreUI.Controls.cuiPanel();
            this.lbTotalPrice = new System.Windows.Forms.Label();
            this.tbPay = new CuoreUI.Controls.cuiTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPay = new CuoreUI.Controls.cuiButton();
            this.lbSubtotal = new System.Windows.Forms.Label();
            this.cuiPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cuiPanel2
            // 
            this.cuiPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.cuiPanel2.Location = new System.Drawing.Point(0, 0);
            this.cuiPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cuiPanel2.Name = "cuiPanel2";
            this.cuiPanel2.OutlineThickness = 1F;
            this.cuiPanel2.PanelColor = System.Drawing.Color.White;
            this.cuiPanel2.PanelOutlineColor = System.Drawing.Color.Transparent;
            this.cuiPanel2.Rounding = new System.Windows.Forms.Padding(0);
            this.cuiPanel2.Size = new System.Drawing.Size(731, 71);
            this.cuiPanel2.TabIndex = 20;
            // 
            // cuiPanel3
            // 
            this.cuiPanel3.Controls.Add(this.lbTotalPrice);
            this.cuiPanel3.Controls.Add(this.tbPay);
            this.cuiPanel3.Controls.Add(this.label1);
            this.cuiPanel3.Controls.Add(this.btnPay);
            this.cuiPanel3.Controls.Add(this.lbSubtotal);
            this.cuiPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cuiPanel3.Location = new System.Drawing.Point(0, 71);
            this.cuiPanel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cuiPanel3.Name = "cuiPanel3";
            this.cuiPanel3.OutlineThickness = 1F;
            this.cuiPanel3.PanelColor = System.Drawing.SystemColors.Control;
            this.cuiPanel3.PanelOutlineColor = System.Drawing.Color.Transparent;
            this.cuiPanel3.Rounding = new System.Windows.Forms.Padding(0);
            this.cuiPanel3.Size = new System.Drawing.Size(731, 830);
            this.cuiPanel3.TabIndex = 21;
            // 
            // lbTotalPrice
            // 
            this.lbTotalPrice.AutoSize = true;
            this.lbTotalPrice.BackColor = System.Drawing.Color.Transparent;
            this.lbTotalPrice.Font = new System.Drawing.Font("Figtree", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalPrice.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lbTotalPrice.Location = new System.Drawing.Point(115, 110);
            this.lbTotalPrice.Name = "lbTotalPrice";
            this.lbTotalPrice.Size = new System.Drawing.Size(0, 86);
            this.lbTotalPrice.TabIndex = 19;
            // 
            // tbPay
            // 
            this.tbPay.BackColor = System.Drawing.Color.Transparent;
            this.tbPay.BackgroundColor = System.Drawing.Color.White;
            this.tbPay.Content = "";
            this.tbPay.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbPay.FocusBackgroundColor = System.Drawing.Color.White;
            this.tbPay.FocusImageTint = System.Drawing.Color.White;
            this.tbPay.FocusOutlineColor = System.Drawing.Color.DodgerBlue;
            this.tbPay.Font = new System.Drawing.Font("Figtree", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPay.ForeColor = System.Drawing.Color.Black;
            this.tbPay.Image = null;
            this.tbPay.ImageExpand = new System.Drawing.Point(0, 0);
            this.tbPay.ImageOffset = new System.Drawing.Point(0, 0);
            this.tbPay.Location = new System.Drawing.Point(128, 274);
            this.tbPay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbPay.Multiline = false;
            this.tbPay.Name = "tbPay";
            this.tbPay.NormalImageTint = System.Drawing.Color.White;
            this.tbPay.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbPay.Padding = new System.Windows.Forms.Padding(68, 36, 68, 0);
            this.tbPay.PasswordChar = false;
            this.tbPay.PlaceholderColor = System.Drawing.SystemColors.WindowText;
            this.tbPay.PlaceholderText = "";
            this.tbPay.Rounding = new System.Windows.Forms.Padding(8);
            this.tbPay.Size = new System.Drawing.Size(450, 141);
            this.tbPay.TabIndex = 18;
            this.tbPay.TextOffset = new System.Drawing.Size(0, 0);
            this.tbPay.UnderlinedStyle = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(124, 242);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 26);
            this.label1.TabIndex = 17;
            this.label1.Text = "Total Dibayar (Rp tidak memakai \".\")";
            // 
            // btnPay
            // 
            this.btnPay.BackColor = System.Drawing.Color.Transparent;
            this.btnPay.CheckButton = false;
            this.btnPay.Checked = false;
            this.btnPay.CheckedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.btnPay.CheckedForeColor = System.Drawing.Color.White;
            this.btnPay.CheckedImageTint = System.Drawing.Color.White;
            this.btnPay.CheckedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.btnPay.Content = "Bayar Sekarang";
            this.btnPay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPay.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnPay.Font = new System.Drawing.Font("Figtree", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPay.ForeColor = System.Drawing.Color.White;
            this.btnPay.HoverBackground = System.Drawing.Color.DodgerBlue;
            this.btnPay.HoverForeColor = System.Drawing.Color.White;
            this.btnPay.HoverImageTint = System.Drawing.Color.White;
            this.btnPay.HoverOutline = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPay.Image = null;
            this.btnPay.ImageAutoCenter = true;
            this.btnPay.ImageExpand = new System.Drawing.Point(0, 0);
            this.btnPay.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnPay.Location = new System.Drawing.Point(248, 710);
            this.btnPay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPay.Name = "btnPay";
            this.btnPay.NormalBackground = System.Drawing.Color.DodgerBlue;
            this.btnPay.NormalForeColor = System.Drawing.Color.White;
            this.btnPay.NormalImageTint = System.Drawing.Color.White;
            this.btnPay.NormalOutline = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPay.OutlineThickness = 1F;
            this.btnPay.PressedBackground = System.Drawing.Color.Transparent;
            this.btnPay.PressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnPay.PressedImageTint = System.Drawing.Color.White;
            this.btnPay.PressedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPay.Rounding = new System.Windows.Forms.Padding(8);
            this.btnPay.Size = new System.Drawing.Size(210, 62);
            this.btnPay.TabIndex = 16;
            this.btnPay.TextAlignment = System.Drawing.StringAlignment.Center;
            this.btnPay.TextOffset = new System.Drawing.Point(0, 0);
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // lbSubtotal
            // 
            this.lbSubtotal.AutoSize = true;
            this.lbSubtotal.BackColor = System.Drawing.Color.Transparent;
            this.lbSubtotal.Font = new System.Drawing.Font("Figtree Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSubtotal.Location = new System.Drawing.Point(124, 71);
            this.lbSubtotal.Name = "lbSubtotal";
            this.lbSubtotal.Size = new System.Drawing.Size(270, 26);
            this.lbSubtotal.TabIndex = 8;
            this.lbSubtotal.Text = "Jumlah yang Harus Dibayar";
            // 
            // CashierCustomerPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 901);
            this.Controls.Add(this.cuiPanel3);
            this.Controls.Add(this.cuiPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CashierCustomerPay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Washinq | Bayar";
            this.Load += new System.EventHandler(this.CashierCustomerPay_Load);
            this.cuiPanel3.ResumeLayout(false);
            this.cuiPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CuoreUI.Controls.cuiPanel cuiPanel2;
        private CuoreUI.Controls.cuiPanel cuiPanel3;
        private System.Windows.Forms.Label lbTotalPrice;
        private CuoreUI.Controls.cuiTextBox tbPay;
        private System.Windows.Forms.Label label1;
        private CuoreUI.Controls.cuiButton btnPay;
        private System.Windows.Forms.Label lbSubtotal;
    }
}