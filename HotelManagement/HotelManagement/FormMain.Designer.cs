
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagement
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panel1;
        private PictureBox pictureBox1;
        private Label labelTenKhachSan;
        private Panel panel2;
        private PictureBox pictureBox2;
        private LinkLabel linkLabel1;
        private Label lblTenNguoiDung;
        private Panel panelMain;
        private Panel panel4;

        private Button button13;
        private Button btnTrangChu;
        private Button button5;
        private Button button4;
        private Button button7;
        private Button button6;
        private Button btnQuanLyPhong;
        private Button btnSoDoPhong;
        private Button button9;
        private Button button12;
        private Button button11;
        private Button button10;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panel1 = new Panel();
            labelTenKhachSan = new Label();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            lblTenNguoiDung = new Label();
            pictureBox2 = new PictureBox();
            linkLabel1 = new LinkLabel();
            panelMain = new Panel();
            panel4 = new Panel();
            button13 = new Button();
            btnTrangChu = new Button();
            button5 = new Button();
            button4 = new Button();
            button7 = new Button();
            button6 = new Button();
            btnQuanLyPhong = new Button();
            button12 = new Button();
            button11 = new Button();
            btnSoDoPhong = new Button();
            button9 = new Button();
            button10 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.SeaGreen;
            panel1.Controls.Add(labelTenKhachSan);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(1, 4);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1834, 92);
            panel1.TabIndex = 0;
            // 
            // labelTenKhachSan
            // 
            labelTenKhachSan.AutoSize = true;
            labelTenKhachSan.BackColor = Color.Transparent;
            labelTenKhachSan.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            labelTenKhachSan.ForeColor = Color.LightSalmon;
            labelTenKhachSan.Location = new Point(66, 19);
            labelTenKhachSan.Name = "labelTenKhachSan";
            labelTenKhachSan.Size = new Size(285, 41);
            labelTenKhachSan.TabIndex = 3;
            labelTenKhachSan.Text = "Hotel Management";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(3, 4);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(48, 78);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.MediumSeaGreen;
            panel2.Controls.Add(lblTenNguoiDung);
            panel2.Controls.Add(pictureBox2);
            panel2.Controls.Add(linkLabel1);
            panel2.Location = new Point(1, 94);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(304, 198);
            panel2.TabIndex = 1;
            // 
            // lblTenNguoiDung
            // 
            lblTenNguoiDung.AutoSize = true;
            lblTenNguoiDung.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold);
            lblTenNguoiDung.Location = new Point(28, 159);
            lblTenNguoiDung.Name = "lblTenNguoiDung";
            lblTenNguoiDung.Size = new Size(76, 26);
            lblTenNguoiDung.TabIndex = 2;
            lblTenNguoiDung.Text = "label1";
            lblTenNguoiDung.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Location = new Point(101, 48);
            pictureBox2.Margin = new Padding(3, 4, 3, 4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(87, 108);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Microsoft Sans Serif", 13F);
            linkLabel1.LinkColor = Color.MidnightBlue;
            linkLabel1.Location = new Point(4, 11);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(111, 26);
            linkLabel1.TabIndex = 0;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Đăng xuất";
            // 
            // panelMain
            // 
            panelMain.AutoScroll = true;
            panelMain.BackColor = Color.Honeydew;
            panelMain.Location = new Point(312, 105);
            panelMain.Margin = new Padding(3, 4, 3, 4);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1647, 1549);
            panelMain.TabIndex = 2;
          //  panelMain.Paint += this.panelMain_Paint;
            // 
            // panel4
            // 
            panel4.BackColor = Color.DarkCyan;
            panel4.Controls.Add(button13);
            panel4.Controls.Add(btnTrangChu);
            panel4.Controls.Add(button5);
            panel4.Controls.Add(button4);
            panel4.Controls.Add(button7);
            panel4.Controls.Add(button6);
            panel4.Controls.Add(btnQuanLyPhong);
            panel4.Controls.Add(button12);
            panel4.Controls.Add(button11);
            panel4.Controls.Add(btnSoDoPhong);
            panel4.Controls.Add(button9);
            panel4.Controls.Add(button10);
            panel4.Location = new Point(1, 289);
            panel4.Margin = new Padding(3, 4, 3, 4);
            panel4.Name = "panel4";
            panel4.Size = new Size(304, 1161);
            panel4.TabIndex = 4;
            // 
            // button13
            // 
            button13.Location = new Point(0, 0);
            button13.Name = "button13";
            button13.Size = new Size(75, 23);
            button13.TabIndex = 0;
            // 
            // btnTrangChu
            // 
            btnTrangChu.Location = new Point(0, 0);
            btnTrangChu.Name = "btnTrangChu";
            btnTrangChu.Size = new Size(75, 23);
            btnTrangChu.TabIndex = 1;
            // 
            // button5
            // 
            button5.Location = new Point(0, 0);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 2;
            // 
            // button4
            // 
            button4.Location = new Point(0, 0);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 3;
            // 
            // button7
            // 
            button7.Location = new Point(0, 0);
            button7.Name = "button7";
            button7.Size = new Size(75, 23);
            button7.TabIndex = 4;
            // 
            // button6
            // 
            button6.Location = new Point(0, 0);
            button6.Name = "button6";
            button6.Size = new Size(75, 23);
            button6.TabIndex = 5;
            // 
            // btnQuanLyPhong
            // 
            btnQuanLyPhong.Location = new Point(0, 0);
            btnQuanLyPhong.Name = "btnQuanLyPhong";
            btnQuanLyPhong.Size = new Size(75, 23);
            btnQuanLyPhong.TabIndex = 6;
            // 
            // button12
            // 
            button12.Location = new Point(0, 0);
            button12.Name = "button12";
            button12.Size = new Size(75, 23);
            button12.TabIndex = 7;
            // 
            // button11
            // 
            button11.Location = new Point(0, 0);
            button11.Name = "button11";
            button11.Size = new Size(75, 23);
            button11.TabIndex = 8;
            // 
            // btnSoDoPhong
            // 
            btnSoDoPhong.Location = new Point(0, 0);
            btnSoDoPhong.Name = "btnSoDoPhong";
            btnSoDoPhong.Size = new Size(75, 23);
            btnSoDoPhong.TabIndex = 9;
            // 
            // button9
            // 
            button9.Location = new Point(0, 0);
            button9.Name = "button9";
            button9.Size = new Size(75, 23);
            button9.TabIndex = 10;
            // 
            // button10
            // 
            button10.Location = new Point(0, 0);
            button10.Name = "button10";
            button10.Size = new Size(75, 23);
            button10.TabIndex = 11;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1745, 1055);
            Controls.Add(panel4);
            Controls.Add(panelMain);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hotel Management";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void CaiDatButton(
            Button btn, string name, string text,
            int x, int y, int width, int height)
        {
            btn.BackColor = Color.DarkCyan;
            btn.Font = new Font(
                "Microsoft Sans Serif", 13F, FontStyle.Regular);
            btn.ForeColor = SystemColors.ButtonHighlight;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.Location = new Point(x, y);
            btn.Margin = new Padding(3, 4, 3, 4);
            btn.Name = name;
            btn.Size = new Size(width, height);
            btn.Text = text;
            btn.UseVisualStyleBackColor = false;
        }

        #endregion
    }
}
