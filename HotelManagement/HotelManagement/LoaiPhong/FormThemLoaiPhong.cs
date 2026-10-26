
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.LoaiPhong
{
    public partial class FormThemLoaiPhong : Form
    {
        private TextBox txtMaLoaiPhong = null!;
        private TextBox txtTenLP = null!;
        private TextBox txtSoGiuong = null!;
        private TextBox txtSoNguoiToiDa = null!;
        private TextBox txtGiaNgay = null!;
        private TextBox txtGiaGio = null!;

        private Button btnThemMoi = null!;
        private Button btnThoat = null!;

        public FormThemLoaiPhong()
        {
            InitializeComponent();
            TaoGiaoDien();

            Text = "Thêm mới loại phòng";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            AcceptButton = btnThemMoi;
            CancelButton = btnThoat;

            txtMaLoaiPhong.MaxLength = 5;
            txtTenLP.MaxLength = 20;
            txtSoGiuong.MaxLength = 2;
            txtSoNguoiToiDa.MaxLength = 2;

            txtSoGiuong.KeyPress += ChiChoNhapSo;
            txtSoNguoiToiDa.KeyPress += ChiChoNhapSo;
        }

        private void TaoGiaoDien()
        {
            ClientSize = new Size(548, 639);
            BackColor = Color.White;

            Label labelTieuDe = new Label
            {
                Text = "Thêm mới thông tin loại phòng",
                Font = new Font(
                    "Microsoft Sans Serif",
                    16F,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 64, 0),
                Location = new Point(55, 31),
                AutoSize = true
            };
            Controls.Add(labelTieuDe);

            Panel panel1 = new Panel
            {
                Location = new Point(21, 101),
                Size = new Size(507, 439)
            };
            Controls.Add(panel1);

            // Ma loai phong
            ThemNhan(panel1, "Mã loại phòng", 29, 29);
            txtMaLoaiPhong = TaoTextBox(panel1, 198, 29);

            // Ten loai phong
            ThemNhan(panel1, "Tên loại phòng", 29, 95);
            txtTenLP = TaoTextBox(panel1, 198, 95);

            // So giuong
            ThemNhan(panel1, "Số giường", 71, 160);
            txtSoGiuong = TaoTextBox(panel1, 198, 160);

            // So nguoi toi da
            ThemNhan(panel1, "Số người tối đa", 29, 229);
            txtSoNguoiToiDa = TaoTextBox(panel1, 198, 229);

            // Gia ngay
            ThemNhan(panel1, "Giá ngày", 84, 294);
            txtGiaNgay = TaoTextBox(panel1, 198, 294);

            // Gia gio
            ThemNhan(panel1, "Giá giờ", 102, 362);
            txtGiaGio = TaoTextBox(panel1, 198, 358);

            // Nut them moi
            btnThemMoi = new Button
            {
                Text = "Thêm mới",
                Location = new Point(97, 548),
                Size = new Size(137, 59),
                BackColor = Color.Moccasin,
                Font = new Font(
                    "Microsoft Sans Serif", 12F)
            };
            Controls.Add(btnThemMoi);

            // Nut thoat
            btnThoat = new Button
            {
                Text = "Thoát",
                Location = new Point(297, 548),
                Size = new Size(137, 59),
                BackColor = SystemColors.GradientActiveCaption,
                Font = new Font(
                    "Microsoft Sans Serif", 12F),
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(btnThoat);

            btnThemMoi.Click += btnThemMoi_Click;
            btnThoat.Click += btnThoat_Click;
        }

        private void ThemNhan(
            Control cha, string noiDung, int x, int y)
        {
            Label label = new Label
            {
                Text = noiDung,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font(
                    "Microsoft Sans Serif", 13F)
            };

            cha.Controls.Add(label);
        }

        private TextBox TaoTextBox(
            Control cha, int x, int y)
        {
            TextBox txt = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(275, 30),
                Font = new Font(
                    "Microsoft Sans Serif", 12F)
            };

            cha.Controls.Add(txt);
            return txt;
        }

        private void btnThemMoi_Click(
            object? sender, EventArgs e)
        {
            string maLPH =
                txtMaLoaiPhong.Text.Trim().ToUpper();

            string tenLPH =
                txtTenLP.Text.Trim();

            if (maLPH.Length == 0)
            {
                MessageBox.Show(
                    "Vui lòng nhập mã loại phòng.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtMaLoaiPhong.Focus();
                return;
            }

            if (tenLPH.Length == 0)
            {
                MessageBox.Show(
                    "Vui lòng nhập tên loại phòng.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTenLP.Focus();
                return;
            }

            if (!int.TryParse(
                txtSoGiuong.Text.Trim(),
                out int soGiuong) || soGiuong <= 0)
            {
                MessageBox.Show(
                    "Số giường không hợp lệ!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSoGiuong.Focus();
                return;
            }

            if (!int.TryParse(
                txtSoNguoiToiDa.Text.Trim(),
                out int soNguoiToiDa) || soNguoiToiDa <= 0)
            {
                MessageBox.Show(
                    "Số người tối đa không hợp lệ!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSoNguoiToiDa.Focus();
                return;
            }

            if (!decimal.TryParse(
                txtGiaNgay.Text.Trim(),
                out decimal giaNgay) || giaNgay <= 0)
            {
                MessageBox.Show(
                    "Giá ngày không hợp lệ!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtGiaNgay.Focus();
                return;
            }

            if (!decimal.TryParse(
                txtGiaGio.Text.Trim(),
                out decimal giaGio) || giaGio <= 0)
            {
                MessageBox.Show(
                    "Giá giờ không hợp lệ!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtGiaGio.Focus();
                return;
            }

            try
            {
                using var db = new HotelManagementContext();

                if (db.LoaiPhongs.Any(
                    l => l.MaLph == maLPH))
                {
                    MessageBox.Show(
                        "Mã loại phòng đã tồn tại. "
                        + "Vui lòng nhập mã khác.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtMaLoaiPhong.Focus();
                    return;
                }

                db.LoaiPhongs.Add(
                    new HotelManagement.Models.LoaiPhong
                    {
                        MaLph = maLPH,
                        TenLph = tenLPH,
                        SoGiuong = soGiuong,
                        SoNguoiToiDa = soNguoiToiDa,
                        GiaNgay = giaNgay,
                        GiaGio = giaGio
                    });

                int rows = db.SaveChanges();

                if (rows > 0)
                {
                    MessageBox.Show(
                        "Thêm loại phòng thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Không thể thêm loại phòng!",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show(
                    "Lỗi: "
                    + (ex.InnerException?.Message
                       ?? ex.Message),
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(
            object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ChiChoNhapSo(
            object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Bo sung ham con thieu de sua loi trong Designer
        private void FormThemLoaiPhong_Load(
            object? sender, EventArgs e)
        {
            // Khong can xu ly gi khi Form duoc mo
        }
    }
}
