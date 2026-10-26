
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Phong
{
    public partial class FormSuaPhong : Form
    {
        private readonly string maPhong;

        private ComboBox cbTrangThaiPhong = null!;
        private ComboBox cbTinhTrangDonDep = null!;
        private ComboBox cbLoaiPhong = null!;
        private RichTextBox txtGhiChu = null!;
        private Button btnCapNhat = null!;
        private Button btnThoat = null!;
        private Label labelTieuDe = null!;

        public FormSuaPhong(string maPhong)
        {
            InitializeComponent();
            this.maPhong = maPhong;

            TaoGiaoDien();

            Text = $"Cập nhật phòng {maPhong}";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            AcceptButton = btnCapNhat;
            CancelButton = btnThoat;

            Load += FormSuaPhong_Load;
        }

        private void TaoGiaoDien()
        {
            ClientSize = new Size(626, 615);
            BackColor = Color.White;

            labelTieuDe = new Label
            {
                Text = $"Cập nhật thông tin phòng {maPhong}",
                Location = new Point(75, 30),
                Size = new Size(480, 45),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 64, 0)
            };
            Controls.Add(labelTieuDe);

            Panel panel1 = new Panel
            {
                Location = new Point(65, 100),
                Size = new Size(507, 375)
            };
            Controls.Add(panel1);

            ThemNhan(panel1, "Trạng thái phòng", 15, 36);

            cbTrangThaiPhong = new ComboBox
            {
                Location = new Point(208, 36),
                Size = new Size(275, 33),
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cbTrangThaiPhong.Items.AddRange(new object[]
            {
                "Bình thường",
                "Đã đặt",
                "Đang thuê",
                "Đang sửa chữa"
            });
            panel1.Controls.Add(cbTrangThaiPhong);

            ThemNhan(panel1, "Loại phòng", 83, 101);

            cbLoaiPhong = new ComboBox
            {
                Location = new Point(208, 101),
                Size = new Size(275, 33),
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panel1.Controls.Add(cbLoaiPhong);

            ThemNhan(panel1, "Tình trạng dọn dẹp", 10, 170);

            cbTinhTrangDonDep = new ComboBox
            {
                Location = new Point(208, 168),
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

            ThemNhan(panel1, "Ghi chú", 115, 230);

            txtGhiChu = new RichTextBox
            {
                Location = new Point(208, 230),
                Size = new Size(275, 119),
                Font = new Font("Arial", 11),
                MaxLength = 100
            };
            panel1.Controls.Add(txtGhiChu);

            btnCapNhat = new Button
            {
                Text = "Cập nhật",
                Location = new Point(153, 498),
                Size = new Size(137, 59),
                BackColor = Color.Moccasin,
                Font = new Font("Arial", 12)
            };
            Controls.Add(btnCapNhat);

            btnThoat = new Button
            {
                Text = "Thoát",
                Location = new Point(353, 498),
                Size = new Size(137, 59),
                BackColor = SystemColors.GradientActiveCaption,
                Font = new Font("Arial", 12),
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
                Font = new Font("Arial", 12)
            };
            cha.Controls.Add(label);
        }

        private void FormSuaPhong_Load(object? sender, EventArgs e)
        {
            LoadLoaiPhong();
            LoadThongTinPhong();
        }

        private void LoadLoaiPhong()
        {
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
                    "Lỗi tải danh sách loại phòng: " + ex.Message);
                btnCapNhat.Enabled = false;
            }
        }

        private void LoadThongTinPhong()
        {
            try
            {
                using var db = new HotelManagementContext();

                var thongTin = db.Phongs
                    .AsNoTracking()
                    .Where(p => p.MaPh == maPhong && p.DaXoa != true)
                    .Select(p => new
                    {
                        p.Ttph,
                        p.Ttdd,
                        p.MaLph,
                        p.GhiChu
                    })
                    .FirstOrDefault();

                if (thongTin == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy phòng " + maPhong);
                    btnCapNhat.Enabled = false;
                    return;
                }

                cbTrangThaiPhong.SelectedIndex =
                    cbTrangThaiPhong.FindStringExact(thongTin.Ttph);

                cbTinhTrangDonDep.SelectedIndex =
                    cbTinhTrangDonDep.FindStringExact(thongTin.Ttdd);

                cbLoaiPhong.SelectedValue = thongTin.MaLph;
                txtGhiChu.Text = thongTin.GhiChu ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải thông tin phòng: " + ex.Message);
                btnCapNhat.Enabled = false;
            }
        }

        private void btnCapNhat_Click(object? sender, EventArgs e)
        {
            string trangThai = cbTrangThaiPhong.Text;
            string donDep = cbTinhTrangDonDep.Text;
            string? maLPH = cbLoaiPhong.SelectedValue?.ToString();
            string ghiChu = txtGhiChu.Text.Trim();

            if (string.IsNullOrEmpty(maLPH))
            {
                MessageBox.Show("Vui lòng chọn loại phòng.");
                return;
            }

            if (trangThai.Length == 0 || donDep.Length == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn trạng thái phòng và tình trạng dọn dẹp.");
                return;
            }

            try
            {
                using var db = new HotelManagementContext();

                var phong = db.Phongs.FirstOrDefault(
                    p => p.MaPh == maPhong && p.DaXoa != true);

                if (phong == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy phòng cần cập nhật.");
                    return;
                }

                phong.Ttph = trangThai;
                phong.Ttdd = donDep;
                phong.MaLph = maLPH;
                phong.GhiChu =
                    ghiChu.Length == 0 ? null : ghiChu;

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
                        "Thông tin không thay đổi nên không có gì để cập nhật.");
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show(
                    "Lỗi khi cập nhật phòng: "
                    + (ex.InnerException?.Message ?? ex.Message));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi cập nhật phòng: " + ex.Message);
            }
        }

        private void btnThoat_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
