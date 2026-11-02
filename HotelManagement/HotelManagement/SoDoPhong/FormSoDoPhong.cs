
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement
{
    public partial class FormSoDoPhong : Form
    {
        private DataGridView dataGridViewPhong = null!;
        private DateTimePicker dtNgayChon = null!;
        private DateTimePicker dtNgayTra = null!;
        private TextBox txtSearchMaPhong = null!;
        private Button btnDatPhong = null!;

        private RadioButton[] radioTrangThai = null!;
        private RadioButton[] radioLoaiPhong = null!;
        private RadioButton[] radioDonDep = null!;

        private ToolTip toolTip1 = new ToolTip();
        private bool _dangKhoiTao = true;

        public FormSoDoPhong()
        {
            InitializeComponent();
            TaoGiaoDien();

            Load += FormSoDoPhong_Load;
        }

        private void FormSoDoPhong_Load(object? sender, EventArgs e)
        {
            _dangKhoiTao = true;

            radioTrangThai[4].Checked = true;
            radioLoaiPhong[4].Checked = true;

            dtNgayChon.ShowCheckBox = true;
            dtNgayChon.Checked = false;

            dataGridViewPhong.ReadOnly = true;
            dataGridViewPhong.AllowUserToAddRows = false;
            dataGridViewPhong.AllowUserToDeleteRows = false;
            dataGridViewPhong.MultiSelect = false;
            dataGridViewPhong.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            toolTip1.SetToolTip(
                txtSearchMaPhong,
                "Ví dụ: P101, P201...");

            _dangKhoiTao = false;

            TaiLaiDanhSach();
        }

        private void TaoGiaoDien()
        {
            Text = "Sơ đồ phòng";
            BackColor = Color.Honeydew;
            Size = new Size(1276, 851);

            Panel panelLoc = new Panel
            {
                Dock = DockStyle.Left,
                Width = 267,
                BackColor = Color.GreenYellow,
                AutoScroll = true
            };

            Panel panelTimKiem = new Panel
            {
                Dock = DockStyle.Top,
                Height = 66,
                BackColor = Color.Honeydew
            };

            dataGridViewPhong = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            Controls.Add(dataGridViewPhong);
            Controls.Add(panelTimKiem);
            Controls.Add(panelLoc);

            panelLoc.BringToFront();
            panelTimKiem.BringToFront();

            GroupBox groupTrangThai = TaoGroupBox(
                "Trạng thái phòng", 10, 10, 245, 230);

            radioTrangThai = ThemRadioButton(
                groupTrangThai,
                new string[]
                {
                    "Phòng trống",
                    "Phòng đã đặt",
                    "Phòng đang thuê",
                    "Phòng đang sửa chữa",
                    "Tất cả phòng"
                });

            panelLoc.Controls.Add(groupTrangThai);

            GroupBox groupLoaiPhong = TaoGroupBox(
                "Loại phòng", 10, 250, 245, 230);

            radioLoaiPhong = ThemRadioButton(
                groupLoaiPhong,
                new string[]
                {
                    "Phòng thường đơn",
                    "Phòng thường đôi",
                    "Phòng VIP đơn",
                    "Phòng VIP đôi",
                    "Tất cả phòng"
                });

            panelLoc.Controls.Add(groupLoaiPhong);

            GroupBox groupDonDep = TaoGroupBox(
                "Tình trạng dọn dẹp", 10, 490, 245, 150);

            radioDonDep = ThemRadioButton(
                groupDonDep,
                new string[]
                {
                    "Đã dọn dẹp",
                    "Chưa dọn dẹp",
                    "Tất cả"
                });

            panelLoc.Controls.Add(groupDonDep);

            dtNgayChon = new DateTimePicker
            {
                Location = new Point(20, 20),
                Width = 200,
                ShowCheckBox = true
            };

            panelTimKiem.Controls.Add(dtNgayChon);

            dtNgayTra = new DateTimePicker
            {
                Location = new Point(240, 20),
                Width = 200
            };

            panelTimKiem.Controls.Add(dtNgayTra);

            txtSearchMaPhong = new TextBox
            {
                Location = new Point(470, 20),
                Width = 150,
                PlaceholderText = "Nhập số phòng"
            };

            panelTimKiem.Controls.Add(txtSearchMaPhong);

            btnDatPhong = new Button
            {
                Text = "Đặt phòng",
                Location = new Point(650, 15),
                Size = new Size(120, 35),
                ForeColor = Color.Blue,
                Font = new Font(
                    "Arial", 11, FontStyle.Bold)
            };

            panelTimKiem.Controls.Add(btnDatPhong);

            foreach (RadioButton radio in radioTrangThai)
                radio.CheckedChanged += LocTheoRadio;

            foreach (RadioButton radio in radioLoaiPhong)
                radio.CheckedChanged += LocTheoRadio;

            foreach (RadioButton radio in radioDonDep)
                radio.CheckedChanged += LocTheoRadio;

            txtSearchMaPhong.TextChanged +=
                txtSearchMaPhong_TextChanged;

            dtNgayChon.ValueChanged +=
                dtNgayChon_ValueChanged;

            btnDatPhong.Click += btnDatPhong_Click;
        }

        private GroupBox TaoGroupBox(
            string text, int x, int y, int w, int h)
        {
            return new GroupBox
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, h),
                Font = new Font(
                    "Arial", 10, FontStyle.Bold)
            };
        }

        private RadioButton[] ThemRadioButton(
            GroupBox group, string[] danhSach)
        {
            RadioButton[] radios =
                new RadioButton[danhSach.Length];

            for (int i = 0; i < danhSach.Length; i++)
            {
                RadioButton radio = new RadioButton
                {
                    Text = danhSach[i],
                    Location = new Point(
                        12, 30 + i * 35),
                    AutoSize = true,
                    Font = new Font("Arial", 10)
                };

                radios[i] = radio;
                group.Controls.Add(radio);
            }

            return radios;
        }

        private void LocTheoRadio(
            object? sender, EventArgs e)
        {
            if (_dangKhoiTao) return;

            if (sender is RadioButton radio &&
                !radio.Checked)
                return;

            TaiLaiDanhSach();
        }

        private void txtSearchMaPhong_TextChanged(
            object? sender, EventArgs e)
        {
            if (_dangKhoiTao) return;

            TaiLaiDanhSach();
        }

        private void dtNgayChon_ValueChanged(
            object? sender, EventArgs e)
        {
            if (_dangKhoiTao) return;

            TaiLaiDanhSach();
        }

        private string? TrangThaiPhongDangChon()
        {
            if (radioTrangThai[0].Checked)
                return "Binh thuong";

            if (radioTrangThai[1].Checked)
                return "Da dat";

            if (radioTrangThai[2].Checked)
                return "Dang thue";

            if (radioTrangThai[3].Checked)
                return "Dang sua chua";

            return null;
        }

        private string? MaLoaiPhongDangChon()
        {
            if (radioLoaiPhong[0].Checked)
                return "NOR01";

            if (radioLoaiPhong[1].Checked)
                return "NOR02";

            if (radioLoaiPhong[2].Checked)
                return "VIP01";

            if (radioLoaiPhong[3].Checked)
                return "VIP02";

            return null;
        }

        private string? TinhTrangDonDepDangChon()
        {
            if (radioDonDep[0].Checked)
                return "Da don dep";

            if (radioDonDep[1].Checked)
                return "Chua don dep";

            return null;
        }

        private string? MaPhongDangChon()
        {
            return dataGridViewPhong.CurrentRow?
                .Cells["MaPH"].Value?.ToString();
        }

        private void btnDatPhong_Click(
            object? sender, EventArgs e)
        {
            string? maPhong = MaPhongDangChon();
            if (maPhong == null)
            {
                MessageBox.Show("Vui lòng chọn một phòng trước khi đặt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var form = new HotelManagement.SoDoPhong.FormDatPhong(maPhong);
            if (form.ShowDialog(this) == DialogResult.OK) TaiLaiDanhSach();
        }

        private void TaiLaiDanhSach()
        {
            try
            {
                string? ttph =
                    TrangThaiPhongDangChon();

                string? malph =
                    MaLoaiPhongDangChon();

                string? ttdd =
                    TinhTrangDonDepDangChon();

                string maPhong =
                    txtSearchMaPhong.Text.Trim();

                using var db =
                    new HotelManagementContext();

                var query = db.Phongs
                    .AsNoTracking()
                    .Where(p => p.DaXoa != true);

                if (ttph != null)
                    query = query.Where(
                        p => p.Ttph == ttph);

                if (malph != null)
                    query = query.Where(
                        p => p.MaLph == malph);

                if (ttdd != null)
                    query = query.Where(
                        p => p.Ttdd == ttdd);

                if (maPhong.Length > 0)
                    query = query.Where(
                        p => p.MaPh.Contains(maPhong));

                if (dtNgayChon.Checked)
                {
                    DateTime ngayChon =
                        dtNgayChon.Value.Date;

                    DateTime ngayTiep =
                        ngayChon.AddDays(1);

                    query = query.Where(p =>
                        p.Ttph == "Binh thuong"
                        && !p.Ctdps.Any(c =>
                            c.DaXoa != true
                            && c.CheckIn < ngayTiep
                            && c.CheckOut > ngayChon));
                }

                var dsPhong = query
                    .OrderBy(p => p.MaPh)
                    .Select(p => new
                    {
                        p.MaPh,
                        TenLph =
                            p.MaLphNavigation.TenLph,
                        p.Ttph,
                        p.Ttdd,
                        p.GhiChu
                    })
                    .ToList();

                dataGridViewPhong.DataSource =
                    dsPhong.Select(p => new
                    {
                        MaPH = p.MaPh,

                        Tang = p.MaPh.Length > 1
                            ? p.MaPh.Substring(1, 1)
                            : "",

                        TenLPH = p.TenLph,
                        TTPH = p.Ttph,
                        TTDD = p.Ttdd,
                        GhiChu = p.GhiChu
                    }).ToList();

                DatTieuDeCot(
                    "MaPH", "Mã phòng");

                DatTieuDeCot(
                    "Tang", "Tầng");

                DatTieuDeCot(
                    "TenLPH", "Loại phòng");

                DatTieuDeCot(
                    "TTPH", "Trạng thái phòng");

                DatTieuDeCot(
                    "TTDD", "Tình trạng dọn dẹp");

                DatTieuDeCot(
                    "GhiChu", "Ghi chú");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách phòng:\n"
                    + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DatTieuDeCot(
            string tenCot, string tieuDe)
        {
            if (dataGridViewPhong.Columns
                .Contains(tenCot))
            {
                dataGridViewPhong
                    .Columns[tenCot]
                    .HeaderText = tieuDe;
            }
        }
    }
}
