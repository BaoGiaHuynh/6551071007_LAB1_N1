
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.LoaiPhong
{
    public partial class FormDanhSachLoaiPhong : Form
    {
        private DataGridView dtgDanhSachLoaiPhong = null!;
        private TextBox txtTimKiem = null!;
        private Button btnXuatExcel = null!;
        private Button btnThemMoi = null!;

        public FormDanhSachLoaiPhong()
        {
            InitializeComponent();
            TaoGiaoDien();

            Load += FormDanhSachLoaiPhong_Load;
        }

        private void TaoGiaoDien()
        {
            Text = "Quản lý thông tin loại phòng";
            BackColor = Color.White;
            Size = new Size(1050, 700);

            Panel panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Honeydew
            };

            Label lblTimKiem = new Label
            {
                Text = "Tìm kiếm:",
                Location = new Point(20, 23),
                AutoSize = true,
                Font = new Font("Arial", 11)
            };
            panelTop.Controls.Add(lblTimKiem);

            txtTimKiem = new TextBox
            {
                Location = new Point(115, 17),
                Size = new Size(220, 32),
                Font = new Font("Arial", 11),
                PlaceholderText = "Mã hoặc tên loại phòng"
            };
            panelTop.Controls.Add(txtTimKiem);

            btnXuatExcel = new Button
            {
                Text = "Xuất file Excel",
                Location = new Point(365, 12),
                Size = new Size(170, 38),
                BackColor = Color.DarkTurquoise,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            panelTop.Controls.Add(btnXuatExcel);

            btnThemMoi = new Button
            {
                Text = "Thêm mới",
                Location = new Point(555, 12),
                Size = new Size(170, 38),
                BackColor = Color.LightPink,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            panelTop.Controls.Add(btnThemMoi);

            dtgDanhSachLoaiPhong = new DataGridView
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

            // Thứ tự thêm control để Dock không che nhau
            Controls.Add(dtgDanhSachLoaiPhong);
            Controls.Add(panelTop);

            txtTimKiem.TextChanged +=
                txtTimKiem_TextChanged;

            btnXuatExcel.Click +=
                btnXuatExcel_Click;

            btnThemMoi.Click +=
                btnThemMoi_Click;

            dtgDanhSachLoaiPhong.CellClick +=
                dtgDanhSachLoaiPhong_CellClick;
        }

        private void FormDanhSachLoaiPhong_Load(
            object? sender, EventArgs e)
        {
            CauHinhLuoi();
            LoadLoaiPhong();
        }

        private void CauHinhLuoi()
        {
            if (dtgDanhSachLoaiPhong.Columns.Count > 0)
                return;

            dtgDanhSachLoaiPhong.Columns.Add(
                "MaLPH", "Mã loại");

            dtgDanhSachLoaiPhong.Columns.Add(
                "TenLPH", "Tên loại");

            dtgDanhSachLoaiPhong.Columns.Add(
                "SoGiuong", "Số giường");

            dtgDanhSachLoaiPhong.Columns.Add(
                "SoNguoiToiDa", "Số người tối đa");

            dtgDanhSachLoaiPhong.Columns.Add(
                "GiaNgay", "Giá ngày");

            dtgDanhSachLoaiPhong.Columns.Add(
                "GiaGio", "Giá giờ");

            dtgDanhSachLoaiPhong.Columns["GiaNgay"]!
                .DefaultCellStyle.Format = "N0";

            dtgDanhSachLoaiPhong.Columns["GiaGio"]!
                .DefaultCellStyle.Format = "N0";

            dtgDanhSachLoaiPhong.Columns["GiaNgay"]!
                .DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

            dtgDanhSachLoaiPhong.Columns["GiaGio"]!
                .DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

            DataGridViewButtonColumn colTienNghi =
                new DataGridViewButtonColumn
                {
                    Name = "colTienNghi",
                    HeaderText = "Tiện nghi",
                    Text = "Chi tiết",
                    UseColumnTextForButtonValue = true,
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.None,
                    Width = 90
                };

            DataGridViewButtonColumn colSua =
                new DataGridViewButtonColumn
                {
                    Name = "colSua",
                    HeaderText = "Cập nhật",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true,
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.None,
                    Width = 90
                };

            dtgDanhSachLoaiPhong.Columns.Add(colTienNghi);
            dtgDanhSachLoaiPhong.Columns.Add(colSua);
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
                        l.TenLph,
                        l.SoGiuong,
                        l.SoNguoiToiDa,
                        l.GiaNgay,
                        l.GiaGio
                    })
                    .ToList();

                dtgDanhSachLoaiPhong.Rows.Clear();

                foreach (var l in dsLoaiPhong)
                {
                    dtgDanhSachLoaiPhong.Rows.Add(
                        l.MaLph,
                        l.TenLph,
                        l.SoGiuong,
                        l.SoNguoiToiDa,
                        l.GiaNgay,
                        l.GiaGio
                    );
                }

                LocTheoTuKhoa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi tải loại phòng: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

            dtgDanhSachLoaiPhong.CurrentCell = null;

            foreach (DataGridViewRow row
                in dtgDanhSachLoaiPhong.Rows)
            {
                string ma =
                    row.Cells["MaLPH"].Value?.ToString() ?? "";

                string ten =
                    row.Cells["TenLPH"].Value?.ToString() ?? "";

                row.Visible = keyword.Length == 0
                    || ma.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase)
                    || ten.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase);
            }
        }

        private void dtgDanhSachLoaiPhong_CellClick(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            DataGridViewRow dong =
                dtgDanhSachLoaiPhong.Rows[e.RowIndex];

            string maLPH =
                dong.Cells["MaLPH"].Value?.ToString() ?? "";

            string tenLPH =
                dong.Cells["TenLPH"].Value?.ToString() ?? "";

            if (maLPH.Length == 0)
                return;

            string tenCot = dtgDanhSachLoaiPhong
                .Columns[e.ColumnIndex].Name;


            if (tenCot == "colSua")
            {
                using (var formSua = new FormSuaLoaiPhong())
                {
                    formSua.MaLoaiPhong = maLPH;

                    if (formSua.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadLoaiPhong();
                    }
                }
            }

            else if (tenCot == "colTienNghi")
            {
                MessageBox.Show(
                    $"Chức năng chi tiết tiện nghi của "
                    + $"loại phòng {tenLPH} ({maLPH}) "
                    + "đang được xây dựng.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnThemMoi_Click(
       object? sender, EventArgs e)
        {
            using (var form = new FormThemLoaiPhong())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadLoaiPhong();
                }
            }
        }

        private void btnXuatExcel_Click(
            object? sender, EventArgs e)
        {
            var cacDong = dtgDanhSachLoaiPhong.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.Visible && !r.IsNewRow)
                .ToList();

            if (cacDong.Count == 0)
            {
                MessageBox.Show(
                    "Không có dữ liệu để xuất!");
                return;
            }

            using SaveFileDialog hopLuu = new SaveFileDialog
            {
                Title = "Xuất danh sách loại phòng",
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName =
                    $"DanhSachLoaiPhong_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };

            if (hopLuu.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using var wb = new XLWorkbook();

                var ws = wb.Worksheets.Add(
                    "Danh sách loại phòng");

                string[] tenCot =
                {
                    "MaLPH",
                    "TenLPH",
                    "SoGiuong",
                    "SoNguoiToiDa",
                    "GiaNgay",
                    "GiaGio"
                };

                for (int c = 0; c < tenCot.Length; c++)
                {
                    ws.Cell(1, c + 1).Value =
                        dtgDanhSachLoaiPhong
                        .Columns[tenCot[c]].HeaderText;
                }

                ws.Row(1).Style.Font.Bold = true;

                for (int i = 0; i < cacDong.Count; i++)
                {
                    for (int c = 0; c < tenCot.Length; c++)
                    {
                        var o = ws.Cell(i + 2, c + 1);

                        object? giaTri =
                            cacDong[i].Cells[tenCot[c]].Value;

                        switch (giaTri)
                        {
                            case decimal d:
                                o.Value = d;
                                o.Style.NumberFormat.Format = "#,##0";
                                break;

                            case int n:
                                o.Value = n;
                                break;

                            default:
                                o.Value = giaTri?.ToString() ?? "";
                                break;
                        }
                    }
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(hopLuu.FileName);

                DialogResult traLoi = MessageBox.Show(
                    "Xuất dữ liệu thành công!\n"
                    + "Bạn có muốn mở file ngay không?",
                    "Thông báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (traLoi == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo(
                            hopLuu.FileName)
                        {
                            UseShellExecute = true
                        });
                }
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Không ghi được file. Hãy đóng file "
                    + "trong Excel rồi thử lại.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi xuất Excel: " + ex.Message);
            }
        }
    }
}
