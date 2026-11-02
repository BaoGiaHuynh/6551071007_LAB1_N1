using HotelManagement.Models;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.DangNhap;

namespace HotelManagement.ChiTietTienNghi;

public sealed class FormDanhSachChiTietTienNghi : Form
{
    private readonly DataGridView _grid = new();
    public FormDanhSachChiTietTienNghi()
    {
        Text = "Tiện nghi theo loại phòng"; BackColor = Color.White; Font = new Font("Segoe UI", 10); Size = new Size(1000, 650);
        var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 56, BackColor = Color.Honeydew, Padding = new Padding(12) };
        foreach (var (text, action, color) in new (string, Action, Color)[] { ("Thêm mới", () => Edit(null), Color.LightPink), ("Cập nhật", () => Edit(Selected()), Color.LightBlue), ("Xóa", Delete, Color.MistyRose), ("Làm mới", LoadRows, Color.LightCyan) })
        { var b = new Button { Text = text, Width = 120, Height = 34, BackColor = color, FlatStyle = FlatStyle.Flat, Visible = text != "Xóa" || FormDangNhap.CapDoQuyen_DangNhap >= 3 }; b.Click += (_, _) => action(); bar.Controls.Add(b); }
        _grid.Dock = DockStyle.Fill; _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.MultiSelect = false; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        Controls.Add(_grid); Controls.Add(bar); Load += (_, _) => LoadRows();
    }
    private (string RoomType, string Amenity)? Selected() => _grid.CurrentRow?.Tag is ValueTuple<string,string> x ? x : null;
    private void LoadRows()
    {
        try { using var db = new HotelManagementContext(); var rows = db.Cttns.Where(x => x.DaXoa != true).Select(x => new { x.MaLph, LoaiPhong = x.MaLphNavigation.TenLph, x.MaTn, TienNghi = x.MaTnNavigation.TenTn, x.Sl }).ToList(); _grid.Columns.Clear(); _grid.Rows.Clear(); foreach (var c in new[] { ("Loại phòng", 150), ("Tiện nghi", 150), ("Số lượng", 100) }) _grid.Columns.Add(c.Item1, c.Item1); foreach (var r in rows) { var i = _grid.Rows.Add(r.MaLph + " - " + r.LoaiPhong, r.MaTn + " - " + r.TienNghi, r.Sl is null or < 0 ? "Không giới hạn" : r.Sl); _grid.Rows[i].Tag = (r.MaLph, r.MaTn); } }
        catch (Exception ex) { MessageBox.Show("Không tải được danh sách tiện nghi: " + ex.GetBaseException().Message); }
    }
    private void Edit((string RoomType, string Amenity)? key)
    {
        try
        {
            using var db = new HotelManagementContext(); var types = db.LoaiPhongs.ToList(); var amenities = db.TienNghis.Where(x => x.DaXoa != true).ToList();
            if (!types.Any() || !amenities.Any()) { MessageBox.Show("Hãy tạo loại phòng và tiện nghi trước."); return; }
            var type = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, DataSource = types, DisplayMember = "TenLph", ValueMember = "MaLph", Width = 260 };
            var amenity = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, DataSource = amenities, DisplayMember = "TenTn", ValueMember = "MaTn", Width = 260 };
            var quantity = new NumericUpDown { Minimum = -1, Maximum = 10000, Value = 1, Width = 260 };
            var unlimited = new CheckBox { Text = "Không giới hạn", AutoSize = true };
            var old = key is null ? null : db.Cttns.Find(key.Value.RoomType, key.Value.Amenity);
            if (old != null) { type.SelectedValue = old.MaLph; amenity.SelectedValue = old.MaTn; if (old.Sl is null or < 0) unlimited.Checked = true; else quantity.Value = old.Sl.Value; }
            unlimited.CheckedChanged += (_, _) => quantity.Enabled = !unlimited.Checked;
            using var form = new Form { Text = old == null ? "Thêm tiện nghi cho loại phòng" : "Cập nhật số lượng tiện nghi", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, ClientSize = new Size(430, 250), MaximizeBox = false, MinimizeBox = false };
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), ColumnCount = 2 }; layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130)); layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.Controls.Add(new Label { Text = "Loại phòng", AutoSize = true },0,0); layout.Controls.Add(type,1,0); layout.Controls.Add(new Label { Text = "Tiện nghi", AutoSize = true },0,1); layout.Controls.Add(amenity,1,1); layout.Controls.Add(new Label { Text = "Số lượng", AutoSize = true },0,2); layout.Controls.Add(quantity,1,2); layout.Controls.Add(unlimited,1,3);
            var save = new Button { Text = "Lưu", DialogResult = DialogResult.OK, Width = 90 }; var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 90 }; var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 45, FlowDirection = FlowDirection.RightToLeft }; actions.Controls.Add(cancel); actions.Controls.Add(save); form.Controls.Add(layout); form.Controls.Add(actions);
            if (form.ShowDialog(this) != DialogResult.OK) return;
            if (old == null) db.Cttns.Add(new Cttn { MaLph = (string)type.SelectedValue!, MaTn = (string)amenity.SelectedValue!, Sl = unlimited.Checked ? -1 : (int)quantity.Value, DaXoa = false }); else { old.Sl = unlimited.Checked ? -1 : (int)quantity.Value; old.DaXoa = false; }
            db.SaveChanges(); LoadRows();
        }
        catch (Exception ex) { MessageBox.Show("Không lưu được thông tin: " + ex.GetBaseException().Message); }
    }
    private void Delete() { var key = Selected(); if (key is null) { MessageBox.Show("Vui lòng chọn một dòng."); return; } if (MessageBox.Show("Xóa tiện nghi khỏi loại phòng này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return; try { using var db = new HotelManagementContext(); var x = db.Cttns.Find(key.Value.RoomType, key.Value.Amenity); if (x != null) { x.DaXoa = true; db.SaveChanges(); } LoadRows(); } catch (Exception ex) { MessageBox.Show(ex.GetBaseException().Message); } }
}
