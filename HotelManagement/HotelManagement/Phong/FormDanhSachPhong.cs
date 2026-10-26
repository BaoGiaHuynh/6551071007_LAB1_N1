
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Phong
{
    public partial class FormDanhSachPhong : Form
    {
        private TextBox txtTimKiem = null!;
        private Button btnXuatExcel = null!;
        private Button btnThemMoiPhong = null!;
        private DataGridView dtgDanhSachPhong = null!;

        public FormDanhSachPhong()
        {
            InitializeComponent();
            TaoGiaoDien();
            Load += FormDanhSachPhong_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Quản lý thông tin phòng";
            BackColor = Color.White;
            Size = new Size(1050, 700);

            // Thanh công cụ
            Panel panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Honeydew
            };

            Controls.Add(panelTop);

            Label lblTimKiem = new Label
            {
                Text = "Tìm kiếm:",
                Location = new Point(15, 23),
                AutoSize = true,
                Font = new Font("Arial", 11)
            };

            panelTop.Controls.Add(lblTimKiem);

            txtTimKiem = new TextBox
            {
                Location = new Point(110, 17),
                Size = new Size(200, 30),
                PlaceholderText = "Nhập mã phòng"
            };

            panelTop.Controls.Add(txtTimKiem);

            btnXuatExcel = new Button
            {
                Text = "Xuất file Excel",
                Location = new Point(360, 12),
                Size = new Size(170, 38),
                BackColor = Color.DarkTurquoise,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            panelTop.Controls.Add(btnXuatExcel);

            btnThemMoiPhong = new Button
            {
                Text = "Thêm mới phòng",
                Location = new Point(550, 12),
                Size = new Size(190, 38),
                BackColor = Color.MediumSpringGreen,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            panelTop.Controls.Add(btnThemMoiPhong);

            // Bảng danh sách phòng
            dtgDanhSachPhong = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 40 }
            };

            Controls.Add(dtgDanhSachPhong);

            // Giữ thanh công cụ phía trên
            panelTop.BringToFront();

            // Sự kiện
            txtTimKiem.TextChanged +=
                txtTimKiem_TextChanged;

            btnXuatExcel.Click +=
                btnXuatExcel_Click;

            btnThemMoiPhong.Click +=
                btnThemMoiPhong_Click;

            dtgDanhSachPhong.CellClick +=
                dtgDanhSachPhong_CellClick;
        }

        private void FormDanhSachPhong_Load(
            object? sender, EventArgs e)
        {
            CauHinhLuoi();
            LoadDanhSachPhong();
        }

        private void CauHinhLuoi()
        {
            if (dtgDanhSachPhong.Columns.Count > 0)
                return;

            dtgDanhSachPhong.Columns.Add(
                "MaPH", "Mã phòng");

            dtgDanhSachPhong.Columns.Add(
                "TTPH", "Tình trạng phòng");

            dtgDanhSachPhong.Columns.Add(
                "TTDD", "Dọn dẹp");

            dtgDanhSachPhong.Columns.Add(
                "TenLPH", "Loại phòng");

            DataGridViewButtonColumn colSua =
                new DataGridViewButtonColumn
                {
                    Name = "colSua",
                    HeaderText = "Sửa",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true,
                    Width = 70,
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.None
                };

            DataGridViewButtonColumn colXoa =
                new DataGridViewButtonColumn
                {
                    Name = "colXoa",
                    HeaderText = "Xóa",
                    Text = "Xóa",
                    UseColumnTextForButtonValue = true,
                    Width = 70,
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.None
                };

            dtgDanhSachPhong.Columns.Add(colSua);
            dtgDanhSachPhong.Columns.Add(colXoa);
        }

        private void LoadDanhSachPhong()
        {
            try
            {
                using var db = new HotelManagementContext();

                var dsPhong = db.Phongs
                    .AsNoTracking()
                    .Where(p => p.DaXoa != true)
                    .OrderBy(p => p.MaPh)
                    .Select(p => new
                    {
                        p.MaPh,
                        p.Ttph,
                        p.Ttdd,
                        TenLph = p.MaLphNavigation.TenLph
                    })
                    .ToList();

                dtgDanhSachPhong.Rows.Clear();

                foreach (var p in dsPhong)
                {
                    dtgDanhSachPhong.Rows.Add(
                        p.MaPh,
                        p.Ttph,
                        p.Ttdd,
                        p.TenLph
                    );
                }

                LocTheoTuKhoa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách phòng: " + ex.Message);
            }
        }

        private void txtTimKiem_TextChanged(
            object? sender, EventArgs e)
        {
            LocTheoTuKhoa();
        }

        private void LocTheoTuKhoa()
        {
            string keyword = txtTimKiem.Text.Trim();

            dtgDanhSachPhong.CurrentCell = null;

            foreach (DataGridViewRow row
                in dtgDanhSachPhong.Rows)
            {
                string maPhong =
                    row.Cells["MaPH"].Value?.ToString() ?? "";

                row.Visible = keyword.Length == 0
                    || maPhong.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase);
            }
        }


        private void dtgDanhSachPhong_CellClick(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string? maPhong = dtgDanhSachPhong
                .Rows[e.RowIndex]
                .Cells["MaPH"]
                .Value?.ToString();

            if (string.IsNullOrEmpty(maPhong))
                return;

            string tenCot = dtgDanhSachPhong
                .Columns[e.ColumnIndex].Name;

            if (tenCot == "colSua")
            {
                using (var formSua = new FormSuaPhong(maPhong))
                {
                    if (formSua.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadDanhSachPhong();
                    }
                }
            }
            else if (tenCot == "colXoa")
            {
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa phòng {maPhong} không?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    XoaPhong(maPhong);
                    LoadDanhSachPhong();
                }
            }
        }


        private void XoaPhong(string maPhong)
        {
            try
            {
                using var db = new HotelManagementContext();

                var phong = db.Phongs.FirstOrDefault(
                    p => p.MaPh == maPhong
                    && p.DaXoa != true);

                if (phong == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy phòng để xóa.");
                    return;
                }

                if (phong.Ttph == "Đang thuê"
                    || phong.Ttph == "Đã đặt")
                {
                    MessageBox.Show(
                        $"Phòng {maPhong} đang ở trạng thái "
                        + $"\"{phong.Ttph}\" nên không thể xóa.",
                        "Không thể xóa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                phong.DaXoa = true;
                db.SaveChanges();

                MessageBox.Show(
                    "Đã xóa phòng thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi xóa phòng: " + ex.Message);
            }
        }

        private void btnThemMoiPhong_Click(
            object? sender, EventArgs e)
        {
            using (var form = new FormThemPhong())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadDanhSachPhong();
                }
            }
        }

        private void btnXuatExcel_Click(
            object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng xuất Excel sẽ được bổ sung sau khi cài ClosedXML.");
        }

        private void FormDanhSachPhong_Load_1(object sender, EventArgs e)
        {

        }
    }
}
