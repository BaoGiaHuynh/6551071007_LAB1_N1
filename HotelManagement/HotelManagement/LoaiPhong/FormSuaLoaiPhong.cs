
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.LoaiPhong
{
    public partial class FormSuaLoaiPhong : Form
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public string? MaLoaiPhong { get; set; }

        private TextBox txtTenLP = null!;
        private TextBox txtSoGiuong = null!;
        private TextBox txtSoNguoiToiDa = null!;
        private TextBox txtGiaNgay = null!;
        private TextBox txtGiaGio = null!;

        private Button btnCapNhat = null!;
        private Button btnThoat = null!;

        public FormSuaLoaiPhong()
        {
            InitializeComponent();
            TaoGiaoDien();

            Text = "Cập nhật thông tin loại phòng";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            AcceptButton = btnCapNhat;
            CancelButton = btnThoat;

            txtTenLP.MaxLength = 20;
            txtSoGiuong.MaxLength = 2;
            txtSoNguoiToiDa.MaxLength = 2;

            txtSoGiuong.KeyPress += ChiChoNhapSo;
            txtSoNguoiToiDa.KeyPress += ChiChoNhapSo;

            Load += FormSuaLoaiPhong_Load;
        }

        private void TaoGiaoDien()
        {
            ClientSize = new Size(604, 626);
            BackColor = Color.White;

            Label labelTieuDe = new Label
            {
                Text = "Cập nhật thông tin loại phòng",
                Font = new Font(
                    "Microsoft Sans Serif",
                    16F,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 64, 0),
                Location = new Point(100, 49),
                AutoSize = true
            };
            Controls.Add(labelTieuDe);

            Panel panel1 = new Panel
            {
                Location = new Point(48, 121),
                Size = new Size(507, 375)
            };
            Controls.Add(panel1);

            // Ten loai phong
            ThemNhan(panel1, "Tên loại phòng", 39, 45);
            txtTenLP = TaoTextBox(panel1, 208, 45);

            // So giuong
            ThemNhan(panel1, "Số giường", 81, 110);
            txtSoGiuong = TaoTextBox(panel1, 208, 110);

            // So nguoi toi da
            ThemNhan(panel1, "Số người tối đa", 39, 179);
            txtSoNguoiToiDa = TaoTextBox(panel1, 208, 179);

            // Gia ngay
            ThemNhan(panel1, "Giá ngày", 94, 244);
            txtGiaNgay = TaoTextBox(panel1, 208, 244);

            // Gia gio
            ThemNhan(panel1, "Giá giờ", 112, 312);
            txtGiaGio = TaoTextBox(panel1, 208, 308);

            // Nut cap nhat
            btnCapNhat = new Button
            {
                Text = "Cập nhật",
                Location = new Point(136, 519),
                Size = new Size(137, 59),
                BackColor = Color.Moccasin,
                Font = new Font(
                    "Microsoft Sans Serif", 12F)
            };
            Controls.Add(btnCapNhat);

            // Nut thoat
            btnThoat = new Button
            {
                Text = "Thoát",
                Location = new Point(336, 519),
                Size = new Size(137, 59),
                BackColor = SystemColors.GradientActiveCaption,
                Font = new Font(
                    "Microsoft Sans Serif", 12F),
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(btnThoat);

            btnCapNhat.Click += btnCapNhat_Click;
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

        private void FormSuaLoaiPhong_Load(
            object? sender, EventArgs e)
        {
            string? ma = MaLoaiPhong;

            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show(
                    "Chưa xác định được loại phòng cần cập nhật.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                btnCapNhat.Enabled = false;
                return;
            }

            Text = "Cập nhật loại phòng " + ma;

            try
            {
                using var db = new HotelManagementContext();

                var thongTin = db.LoaiPhongs
                    .AsNoTracking()
                    .Where(l => l.MaLph == ma)
                    .Select(l => new
                    {
                        l.TenLph,
                        l.SoGiuong,
                        l.SoNguoiToiDa,
                        l.GiaNgay,
                        l.GiaGio
                    })
                    .FirstOrDefault();

                if (thongTin == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy loại phòng " + ma + ".",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    btnCapNhat.Enabled = false;
                    return;
                }

                txtTenLP.Text = thongTin.TenLph;
                txtSoGiuong.Text =
                    thongTin.SoGiuong.ToString();

                txtSoNguoiToiDa.Text =
                    thongTin.SoNguoiToiDa.ToString();

                txtGiaNgay.Text =
                    thongTin.GiaNgay.ToString("0.##");

                txtGiaGio.Text =
                    thongTin.GiaGio.ToString("0.##");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải thông tin loại phòng: "
                    + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                btnCapNhat.Enabled = false;
            }
        }

        private void btnCapNhat_Click(
            object? sender, EventArgs e)
        {
            string? ma = MaLoaiPhong;

            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show(
                    "Chưa xác định được loại phòng cần cập nhật.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string tenLPH = txtTenLP.Text.Trim();

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

                var loaiPhong = db.LoaiPhongs
                    .FirstOrDefault(l => l.MaLph == ma);

                if (loaiPhong == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy loại phòng cần cập nhật.",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                loaiPhong.TenLph = tenLPH;
                loaiPhong.SoGiuong = soGiuong;
                loaiPhong.SoNguoiToiDa = soNguoiToiDa;
                loaiPhong.GiaNgay = giaNgay;
                loaiPhong.GiaGio = giaGio;

                int rows = db.SaveChanges();

                if (rows > 0)
                {
                    MessageBox.Show(
                        "Cập nhật thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Thông tin không thay đổi nên "
                        + "không có gì để cập nhật.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show(
                    "Lỗi khi cập nhật: "
                    + (ex.InnerException?.Message
                       ?? ex.Message),
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi cập nhật: " + ex.Message,
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
    }
}
