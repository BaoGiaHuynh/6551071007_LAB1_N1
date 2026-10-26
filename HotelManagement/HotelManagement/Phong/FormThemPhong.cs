
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Phong
{
    public partial class FormThemPhong : Form
    {
        private TextBox txtMaPhong = null!;
        private ComboBox cbTrangThaiPhong = null!;
        private ComboBox cbLoaiPhong = null!;
        private ComboBox cbTinhTrangDonDep = null!;
        private RichTextBox txtGhiChu = null!;
        private Button btnThemMoi = null!;
        private Button btnThoat = null!;

        public FormThemPhong()
        {
            InitializeComponent();
            TaoGiaoDien();

            Text = "Thêm mới phòng";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            AcceptButton = btnThemMoi;
            CancelButton = btnThoat;

            Load += FormThemPhong_Load;
        }

        private void TaoGiaoDien()
        {
            ClientSize = new Size(595, 588);
            BackColor = Color.White;

            Panel panel1 = new Panel
            {
                Location = new Point(44, 38),
                Size = new Size(507, 458)
            };
            Controls.Add(panel1);

            ThemNhan(panel1, "Mã số phòng", 30, 65);

            txtMaPhong = new TextBox
            {
                Location = new Point(203, 65),
                Size = new Size(275, 33),
                Font = new Font("Arial", 12)
            };
            panel1.Controls.Add(txtMaPhong);

            ThemNhan(panel1, "Trạng thái phòng", 15, 125);

            cbTrangThaiPhong = new ComboBox
            {
                Location = new Point(203, 125),
                Size = new Size(275, 33),
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cbTrangThaiPhong.Items.AddRange(new object[]
            {
                "Bình thường",
                "Đang sửa chữa"
            });

            panel1.Controls.Add(cbTrangThaiPhong);

            ThemNhan(panel1, "Loại phòng", 75, 190);

            cbLoaiPhong = new ComboBox
            {
                Location = new Point(203, 190),
                Size = new Size(275, 33),
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panel1.Controls.Add(cbLoaiPhong);

            ThemNhan(panel1, "Tình trạng dọn dẹp", 5, 256);

            cbTinhTrangDonDep = new ComboBox
            {
                Location = new Point(203, 256),
                Size = new Size(275, 33),
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cbTinhTrangDonDep.Items.AddRange(new object[]
            {
                "Đã dọn dẹp",
                "Chưa dọn dẹp"
            });

            panel1.Controls.Add(cbTinhTrangDonDep);

            ThemNhan(panel1, "Ghi chú", 110, 319);

            txtGhiChu = new RichTextBox
            {
                Location = new Point(203, 319),
                Size = new Size(275, 119),
                Font = new Font("Arial", 11)
            };
            panel1.Controls.Add(txtGhiChu);

            btnThemMoi = new Button
            {
                Text = "Thêm mới",
                Location = new Point(132, 502),
                Size = new Size(137, 59),
                BackColor = Color.Moccasin,
                Font = new Font("Arial", 12)
            };
            Controls.Add(btnThemMoi);

            btnThoat = new Button
            {
                Text = "Thoát",
                Location = new Point(332, 502),
                Size = new Size(137, 59),
                BackColor = SystemColors.GradientActiveCaption,
                Font = new Font("Arial", 12),
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
                Font = new Font("Arial", 12)
            };

            cha.Controls.Add(label);
        }

        private void FormThemPhong_Load(
            object? sender, EventArgs e)
        {
            txtMaPhong.MaxLength = 5;
            txtGhiChu.MaxLength = 100;

            try
            {
                using var db = new HotelManagementContext();

                var dsLoaiPhong = db.LoaiPhongs
                    .AsNoTracking()
                    .OrderBy(l => l.MaLph)
                    .Select(l => new
                    {
                        l.MaLph,
                        l.TenLph
                    })
                    .ToList();

                cbLoaiPhong.DisplayMember = "TenLph";
                cbLoaiPhong.ValueMember = "MaLph";
                cbLoaiPhong.DataSource = dsLoaiPhong;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách loại phòng: "
                    + ex.Message);
            }

            cbTrangThaiPhong.SelectedIndex = 0;
            cbTinhTrangDonDep.SelectedIndex = 0;

            txtMaPhong.Focus();
        }

        private void btnThemMoi_Click(
            object? sender, EventArgs e)
        {
            string maPH =
                txtMaPhong.Text.Trim().ToUpperInvariant();

            string trangThai = cbTrangThaiPhong.Text;
            string donDep = cbTinhTrangDonDep.Text;
            string? maLPH =
                cbLoaiPhong.SelectedValue?.ToString();
            string ghiChu = txtGhiChu.Text.Trim();

            if (maPH.Length == 0)
            {
                MessageBox.Show(
                    "Vui lòng nhập mã phòng.");
                txtMaPhong.Focus();
                return;
            }

            if (string.IsNullOrEmpty(maLPH))
            {
                MessageBox.Show(
                    "Vui lòng chọn loại phòng.");
                return;
            }

            if (trangThai.Length == 0 || donDep.Length == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn trạng thái phòng "
                    + "và tình trạng dọn dẹp.");
                return;
            }

            try
            {
                using var db = new HotelManagementContext();

                var tonTai = db.Phongs
                    .FirstOrDefault(p => p.MaPh == maPH);

                if (tonTai != null)
                {
                    if (tonTai.DaXoa != true)
                    {
                        MessageBox.Show(
                            "Mã phòng đã tồn tại. "
                            + "Vui lòng nhập mã khác.");
                        txtMaPhong.Focus();
                        return;
                    }

                    DialogResult traLoi = MessageBox.Show(
                        $"Mã phòng {maPH} từng bị xóa.\n"
                        + "Bạn có muốn khôi phục phòng này?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (traLoi != DialogResult.Yes)
                        return;

                    tonTai.Ttph = trangThai;
                    tonTai.Ttdd = donDep;
                    tonTai.MaLph = maLPH;
                    tonTai.GhiChu =
                        ghiChu.Length == 0 ? null : ghiChu;
                    tonTai.DaXoa = false;
                }
                else
                {
                    db.Phongs.Add(new HotelManagement.Models.Phong
                    {
                        MaPh = maPH,
                        Ttph = trangThai,
                        Ttdd = donDep,
                        MaLph = maLPH,
                        GhiChu =
                            ghiChu.Length == 0 ? null : ghiChu,
                        DaXoa = false
                    });
                }

                int result = db.SaveChanges();

                if (result > 0)
                {
                    MessageBox.Show(
                        "Thêm phòng thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Không thể thêm phòng.");
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show(
                    "Lỗi khi thêm phòng: "
                    + (ex.InnerException?.Message ?? ex.Message));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi thêm phòng: " + ex.Message);
            }
        }

        private void btnThoat_Click(
            object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
