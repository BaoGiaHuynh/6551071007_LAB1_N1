
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagement
{
    partial class FormTrangChu
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictureBox1;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();

            ((System.ComponentModel.ISupportInitialize)
                pictureBox1).BeginInit();

            SuspendLayout();

            // pictureBox1
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(843, 889);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;

            pictureBox1.Click += pictureBox1_Click;

            // FormTrangChu
            Controls.Add(pictureBox1);
            Name = "FormTrangChu";
            Size = new Size(843, 889);

            ((System.ComponentModel.ISupportInitialize)
                pictureBox1).EndInit();

            ResumeLayout(false);
        }
    }
}
