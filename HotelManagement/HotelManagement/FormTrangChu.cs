
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class FormTrangChu : UserControl
    {
        public FormTrangChu()
        {
            InitializeComponent();
            HienThiAnhKhachSan();
        }

        private void HienThiAnhKhachSan()
        {
            // Lay anh tu Resources cua project
            pictureBox1.Image = Properties.Resources.pictureIcon;

            // Hien thi anh khong bi meo
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            // Anh chiem toan bo UserControl
            pictureBox1.Dock = DockStyle.Fill;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}
