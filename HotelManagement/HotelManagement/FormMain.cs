
using HotelManagement.DangNhap;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace HotelManagement
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            KhoiPhucMenu();
            HienThiThongTinNguoiDung();
            GanAnh();

            this.Load += (s, e) => HienThiTrangChu();
        }

        private void btnLoaiPhong_Click(object sender, EventArgs e)
        {
            LoadFormCon(
                new HotelManagement.LoaiPhong.FormDanhSachLoaiPhong());
        }

        private void KhoiPhucMenu()
        {
            Button[] buttons =
            {
        btnTrangChu,
        btnSoDoPhong,
        btnQuanLyPhong,
        button4,
        button9,
        button10,
        button11,
        button12,
        button6,
        button7,
        button5,
        button13
    };

            string[] texts =
            {
        "Trang chủ",
        "Sơ đồ phòng",
        "Phòng",
        "Loại phòng",
        "Danh sách đặt phòng",
        "Danh sách hóa đơn",
        "Danh sách dịch vụ",
        "Danh sách tiện nghi",
        "Danh sách khách hàng",
        "Danh sách nhân viên",
        "Danh sách tài khoản",
        "Thống kê"
    };

            panel4.SuspendLayout();

            panel4.AutoScroll = true;

            for (int i = 0; i < buttons.Length; i++)
            {
                Button btn = buttons[i];

                btn.Dock = DockStyle.None;
                btn.Location = new Point(2, i * 58);
                btn.Size = new Size(298, 50);

                btn.Text = texts[i];

                btn.BackColor = Color.DarkCyan;
                btn.ForeColor = Color.White;
                btn.Font = new Font(
                    "Microsoft Sans Serif", 12F);

                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.ImageAlign = ContentAlignment.MiddleLeft;

                btn.UseVisualStyleBackColor = false;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = Color.White;
                btn.FlatAppearance.BorderSize = 1;
            }

            panel4.ResumeLayout();

            // Khoi phuc su kien Click
            btnTrangChu.Click += btnTrangChu_Click;
            btnSoDoPhong.Click += btnSoDoPhong_Click;
            btnQuanLyPhong.Click += btnQuanLyPhong_Click;

            button4.Click += button4_Click;
            button9.Click += button9_Click;
            button10.Click += button10_Click;
            button11.Click += button11_Click;
            button12.Click += button12_Click;
            button6.Click += button6_Click;
            button7.Click += button7_Click;
            button5.Click += button5_Click;
            button13.Click += button13_Click;

            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
        }


        private void linkLabel1_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show(
                "Bạn có chắc muốn đăng xuất?",
                "Đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            this.Hide();

            using (var formLogin = new FormDangNhap())
            {
                if (formLogin.ShowDialog() == DialogResult.OK)
                {
                    HienThiThongTinNguoiDung();
                    HienThiTrangChu();
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            HienThiDangPhatTrien("Thống kê");
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            HienThiTrangChu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HienThiDangPhatTrien("Danh sách tài khoản");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadFormCon(
                new HotelManagement.LoaiPhong.FormDanhSachLoaiPhong());
        }
        private void button7_Click(object sender, EventArgs e)
        {
            HienThiDangPhatTrien("Danh sách nhân viên");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            HienThiDangPhatTrien("Danh sách khách hàng");
        }

     private void btnQuanLyPhong_Click(object sender, EventArgs e)
{
    LoadFormCon(
        new HotelManagement.Phong.FormDanhSachPhong());
}

        private void button12_Click(object sender, EventArgs e)
        {
            HienThiDangPhatTrien("Danh sách tiện nghi");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            HienThiDangPhatTrien("Danh sách dịch vụ");
        }

        private void btnSoDoPhong_Click(object sender, EventArgs e)
        {
            LoadFormCon(new FormSoDoPhong());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            HienThiDangPhatTrien("Danh sách đặt phòng");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            HienThiDangPhatTrien("Danh sách hóa đơn");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            HienThiTrangChu();
        }

        private void HienThiThongTinNguoiDung()
        {
            lblTenNguoiDung.Text = FormDangNhap.TenNV_DangNhap;

            lblTenNguoiDung.Left =
                (panel2.Width - lblTenNguoiDung.PreferredWidth) / 2;
        }

        private void XoaNoiDungPanelMain()
        {
            foreach (Control c in panelMain.Controls.Cast<Control>().ToList())
            {
                panelMain.Controls.Remove(c);
                c.Dispose();
            }
        }

        private void LoadUserControl(UserControl uc)
        {
            XoaNoiDungPanelMain();

            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Add(uc);
            uc.BringToFront();
        }

        private void LoadFormCon(Form formCon)
        {
            XoaNoiDungPanelMain();

            formCon.TopLevel = false;
            formCon.FormBorderStyle = FormBorderStyle.None;
            formCon.Dock = DockStyle.Fill;

            panelMain.Controls.Add(formCon);
            formCon.Show();
        }

        private void HienThiTrangChu()
        {
            LoadUserControl(new FormTrangChu());
        }



        private void HienThiDangPhatTrien(string tenChucNang)
        {
            XoaNoiDungPanelMain();

            panelMain.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Text = tenChucNang + Environment.NewLine
                       + "(Chức năng đang được xây dựng)",
                Font = new Font(
                    "Microsoft Sans Serif",
                    16F,
                    FontStyle.Italic),
                ForeColor = Color.DarkCyan,
                TextAlign = ContentAlignment.MiddleCenter
            });
        }

        private void GanAnh()
        {

            // Hien thi anh dai dien nhan vien
            pictureBox2.Image = Properties.Resources.admin;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            btnTrangChu.Image = Properties.Resources.MenuTrangChu;
            btnSoDoPhong.Image = Properties.Resources.MenuSoDoPhong;
            btnQuanLyPhong.Image = Properties.Resources.MenuPhong;

            button4.Image = Properties.Resources.MenuLoaiPhong;
            button9.Image = Properties.Resources.MenuDatPhong;
            button10.Image = Properties.Resources.MenuDanhSachHoaDon;
            button11.Image = Properties.Resources.DichVu;
            button12.Image = Properties.Resources.MenuTienNghi;
            button6.Image = Properties.Resources.KhachHang;
            button7.Image = Properties.Resources.MenuQuanLyNhanVien;
            button5.Image = Properties.Resources.MenuQuanLyTaiKhoan;
            button13.Image = Properties.Resources.MenuThongKe;

            Button[] buttons =
            {
        btnTrangChu,
        btnSoDoPhong,
        btnQuanLyPhong,
        button4,
        button9,
        button10,
        button11,
        button12,
        button6,
        button7,
        button5,
        button13
    };

            foreach (Button btn in buttons)
            {
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
