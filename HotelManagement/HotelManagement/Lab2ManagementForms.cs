using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.DangNhap;
using ClosedXML.Excel;

namespace HotelManagement.Lab2
{

/// <summary>Reusable CRUD screen for the scalar master-data tables used in Lab 2.</summary>
public class EntityManagementForm<T> : Form where T : class, new()
{
    private readonly string _title;
    private readonly string _keyName;
    private readonly string _keyPrefix;
    private readonly string[] _visibleProperties;
    private readonly DataGridView _grid = new();
    private readonly TextBox _search = new();

    public EntityManagementForm(string title, string keyName, string keyPrefix, params string[] visibleProperties)
    {
        _title = title; _keyName = keyName; _keyPrefix = keyPrefix; _visibleProperties = visibleProperties;
        Text = title; BackColor = Color.White; Font = new Font("Segoe UI", 10); MinimumSize = new Size(800, 520); Size = new Size(1080, 680);
        var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 58, Padding = new Padding(12), BackColor = Color.Honeydew, WrapContents = false };
        _search.Width = 240; _search.PlaceholderText = "Tìm kiếm..."; _search.TextChanged += (_, _) => LoadRows();
        var add = Button("Thêm mới", Color.LightPink); add.Click += (_, _) => EditRow(null);
        var edit = Button("Cập nhật", Color.LightBlue); edit.Click += (_, _) => { if (_grid.CurrentRow?.Tag is string key) EditRow(key); };
        var delete = Button("Xóa", Color.MistyRose); delete.Click += (_, _) => DeleteRow();
        delete.Visible = FormDangNhap.CapDoQuyen_DangNhap >= 3;
        var refresh = Button("Làm mới", Color.LightCyan); refresh.Click += (_, _) => LoadRows();
        var export = Button("Xuất Excel", Color.DarkTurquoise); export.Width = 130; export.Click += (_, _) => ExportRows();
        bar.Controls.AddRange(new Control[] { _search, add, edit, delete, refresh, export });
        if (typeof(T) == typeof(HotelManagement.Models.TienNghi)) { var details = Button("Gán theo loại phòng", Color.PaleTurquoise); details.Width = 180; details.Click += (_, _) => { using var f = new HotelManagement.ChiTietTienNghi.FormDanhSachChiTietTienNghi(); f.ShowDialog(this); }; bar.Controls.Add(details); }
        _grid.Dock = DockStyle.Fill; _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.AllowUserToDeleteRows = false;
        _grid.MultiSelect = false; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.CellDoubleClick += (_, _) => { if (_grid.CurrentRow?.Tag is string key) EditRow(key); };
        Controls.Add(_grid); Controls.Add(bar); Load += (_, _) => LoadRows();
    }

    private static Button Button(string text, Color color) => new() { Text = text, BackColor = color, Width = 100, Height = 34, FlatStyle = FlatStyle.Flat, Margin = new Padding(8, 0, 0, 0) };
    private static PropertyInfo[] Fields => typeof(T).GetProperties().Where(p => p.CanRead && p.CanWrite && (p.PropertyType.IsValueType || p.PropertyType == typeof(string)) && p.Name != "DaXoa").ToArray();

    private void LoadRows()
    {
        try
        {
            using var db = new HotelManagementContext();
            var rows = db.Set<T>().AsNoTracking().ToList().Where(x => typeof(T).GetProperty("DaXoa")?.GetValue(x) is not true).ToList();
            var term = _search.Text.Trim();
            if (term.Length > 0) rows = rows.Where(x => Fields.Any(p => (p.GetValue(x)?.ToString() ?? "").Contains(term, StringComparison.CurrentCultureIgnoreCase))).ToList();
            _grid.Columns.Clear(); _grid.Rows.Clear();
            foreach (var name in _visibleProperties) { var p = typeof(T).GetProperty(name); if (p != null) _grid.Columns.Add(name, Header(name)); }
            foreach (var x in rows) { var i = _grid.Rows.Add(_visibleProperties.Select(n => typeof(T).GetProperty(n)?.GetValue(x)?.ToString() ?? "").ToArray()); _grid.Rows[i].Tag = typeof(T).GetProperty(_keyName)?.GetValue(x)?.ToString(); }
        }
        catch (Exception ex) { MessageBox.Show($"Không tải được dữ liệu {_title}: {ex.GetBaseException().Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private static string Header(string n) => n switch { "MaDv" => "Mã dịch vụ", "TenDv" => "Tên dịch vụ", "DonGia" => "Đơn giá", "SlconLai" => "Số lượng còn lại", "LoaiDv" => "Loại dịch vụ", "MaTn" => "Mã tiện nghi", "TenTn" => "Tên tiện nghi", "MaKh" => "Mã khách hàng", "TenKh" => "Tên khách hàng", "Sdt" => "Số điện thoại", "CccdPassport" => "CCCD/Passport", "QuocTich" => "Quốc tịch", "GioiTinh" => "Giới tính", "MaNv" => "Mã nhân viên", "TenNv" => "Tên nhân viên", "ChucVu" => "Chức vụ", "Luong" => "Lương", "Cccd" => "CCCD", "NgaySinh" => "Ngày sinh", "DiaChi" => "Địa chỉ", "Email" => "Email", _ => n };

    private void EditRow(string? key)
    {
        try
        {
            using var db = new HotelManagementContext();
            var entity = key == null ? new T() : db.Set<T>().Find(key);
            if (entity == null) { MessageBox.Show("Không tìm thấy dữ liệu cần cập nhật."); return; }
            var creating = key == null;
            if (creating) { var kp = typeof(T).GetProperty(_keyName)!; kp.SetValue(entity, NextCode(db)); }
            using var editor = new EntityEditorForm(_title, entity, Fields, _keyName, creating);
            if (editor.ShowDialog(this) != DialogResult.OK) return;
            if (!creating)
            {
                var current = db.Set<T>().Find(key)!;
                foreach (var p in Fields.Where(p => p.Name != _keyName)) p.SetValue(current, p.GetValue(entity));
            }
            else
            {
                db.Set<T>().Add(entity);
            }
            db.SaveChanges(); LoadRows();
        }
        catch (Exception ex) { MessageBox.Show($"Không lưu được dữ liệu: {ex.GetBaseException().Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private string NextCode(HotelManagementContext db)
    {
        var prop = typeof(T).GetProperty(_keyName)!;
        var max = db.Set<T>().AsNoTracking().ToList().Select(x => prop.GetValue(x)?.ToString() ?? "").Where(x => x.StartsWith(_keyPrefix, StringComparison.OrdinalIgnoreCase)).Select(x => int.TryParse(x[_keyPrefix.Length..], out var n) ? n : 0).DefaultIfEmpty(0).Max();
        var width = prop.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? 5;
        return _keyPrefix + (max + 1).ToString(new string('0', Math.Max(1, width - _keyPrefix.Length)), CultureInfo.InvariantCulture);
    }

    private void DeleteRow()
    {
        if (_grid.CurrentRow?.Tag is not string key) { MessageBox.Show("Vui lòng chọn một dòng."); return; }
        if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi này?", _title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        try
        {
            using var db = new HotelManagementContext(); var item = db.Set<T>().Find(key);
            if (item == null) return;
            var deleted = typeof(T).GetProperty("DaXoa");
            if (deleted?.CanWrite == true) deleted.SetValue(item, true); else db.Set<T>().Remove(item);
            db.SaveChanges(); LoadRows();
        }
        catch (Exception ex) { MessageBox.Show($"Không xóa được dữ liệu đang được sử dụng: {ex.GetBaseException().Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void ExportRows()
    {
        if (_grid.Rows.Count == 0) { MessageBox.Show("Không có dữ liệu để xuất.", _title, MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        using var dialog = new SaveFileDialog { Title = "Xuất danh sách ra Excel", Filter = "Excel Workbook (*.xlsx)|*.xlsx", FileName = $"{_title}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var workbook = new XLWorkbook(); var sheet = workbook.Worksheets.Add(_title.Length > 31 ? _title[..31] : _title);
            for (var c = 0; c < _grid.Columns.Count; c++) { var cell = sheet.Cell(1, c + 1); cell.Value = _grid.Columns[c].HeaderText; cell.Style.Font.Bold = true; cell.Style.Fill.BackgroundColor = XLColor.LightBlue; }
            for (var r = 0; r < _grid.Rows.Count; r++) for (var c = 0; c < _grid.Columns.Count; c++) sheet.Cell(r + 2, c + 1).Value = _grid.Rows[r].Cells[c].Value?.ToString() ?? string.Empty;
            sheet.Columns().AdjustToContents(); workbook.SaveAs(dialog.FileName);
            MessageBox.Show("Đã xuất danh sách ra Excel.", _title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show("Không thể xuất Excel: " + ex.GetBaseException().Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}

}
namespace HotelManagement.Lab2
{
internal sealed class EntityEditorForm : Form
{
    private readonly object _entity; private readonly PropertyInfo[] _properties; private readonly Dictionary<PropertyInfo, Control> _inputs = new();
    public EntityEditorForm(string title, object entity, PropertyInfo[] properties, string keyName, bool creating)
    {
        _entity = entity; _properties = properties; Text = creating ? "Thêm " + title : "Cập nhật " + title; StartPosition = FormStartPosition.CenterParent; FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false; AutoScroll = true; ClientSize = new Size(540, Math.Min(760, 120 + properties.Length * 54));
        var layout = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(16), Width = 520 }; layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170)); layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        foreach (var p in properties)
        {
            var label = new Label { Text = Header(p.Name), AutoSize = true, Anchor = AnchorStyles.Left, Padding = new Padding(0, 8, 0, 0) }; Control input;
            if (p.PropertyType == typeof(bool) || p.PropertyType == typeof(bool?)) { input = new CheckBox { Text = "Có", Checked = p.GetValue(entity) is true, AutoSize = true }; }
            else { var tb = new TextBox { Dock = DockStyle.Fill, Text = Convert.ToString(p.GetValue(entity), CultureInfo.CurrentCulture) ?? "" }; if (p.Name == keyName) tb.ReadOnly = true; input = tb; }
            _inputs[p] = input; layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44)); layout.Controls.Add(label); layout.Controls.Add(input);
        }
        var save = new Button { Text = "Lưu", DialogResult = DialogResult.None, Width = 100, Height = 36, BackColor = Color.LightGreen }; save.Click += (_, _) => Save();
        var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 100, Height = 36 };
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 52, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8) }; buttons.Controls.Add(cancel); buttons.Controls.Add(save);
        Controls.Add(buttons); Controls.Add(layout); AcceptButton = save; CancelButton = cancel;
    }
    private void Save()
    {
        try
        {
            foreach (var p in _properties)
            {
                var c = _inputs[p];
                if (c is CheckBox check) { p.SetValue(_entity, check.Checked); continue; }
                var text = ((TextBox)c).Text.Trim(); if (string.IsNullOrWhiteSpace(text) && p.PropertyType != typeof(string)) throw new FormatException($"{Header(p.Name)} không được để trống.");
                var target = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                object? value = target == typeof(string) ? text : target == typeof(DateOnly) ? DateOnly.Parse(text, CultureInfo.CurrentCulture) : Convert.ChangeType(text, target, CultureInfo.CurrentCulture);
                p.SetValue(_entity, value);
            }
            DialogResult = DialogResult.OK; Close();
        }
        catch (Exception ex) { MessageBox.Show("Vui lòng kiểm tra lại thông tin: " + ex.Message, "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
    private static string Header(string n) => n switch { "MaDv" => "Mã dịch vụ", "TenDv" => "Tên dịch vụ", "DonGia" => "Đơn giá", "SlconLai" => "Số lượng còn lại", "LoaiDv" => "Loại dịch vụ", "MaTn" => "Mã tiện nghi", "TenTn" => "Tên tiện nghi", "MaKh" => "Mã khách hàng", "TenKh" => "Tên khách hàng", "Sdt" => "Số điện thoại", "CccdPassport" => "CCCD/Passport", "QuocTich" => "Quốc tịch", "GioiTinh" => "Giới tính", "MaNv" => "Mã nhân viên", "TenNv" => "Tên nhân viên", "ChucVu" => "Chức vụ", "Luong" => "Lương", "Cccd" => "CCCD", "NgaySinh" => "Ngày sinh (yyyy-MM-dd)", "DiaChi" => "Địa chỉ", "Email" => "Email", _ => n };
}
}

namespace HotelManagement.DichVu { public sealed class FormDanhSachDichVu : HotelManagement.Lab2.EntityManagementForm<HotelManagement.Models.DichVu> { public FormDanhSachDichVu() : base("Quản lý dịch vụ", "MaDv", "DV", "MaDv", "TenDv", "DonGia", "SlconLai", "LoaiDv") { } } }
namespace HotelManagement.TienNghi { public sealed class FormDanhSachTienNghi : HotelManagement.Lab2.EntityManagementForm<HotelManagement.Models.TienNghi> { public FormDanhSachTienNghi() : base("Quản lý tiện nghi", "MaTn", "TN", "MaTn", "TenTn") { } } }
namespace HotelManagement.KhachHang { public sealed class FormDanhSachKhachHang : HotelManagement.Lab2.EntityManagementForm<HotelManagement.Models.KhachHang> { public FormDanhSachKhachHang() : base("Quản lý khách hàng", "MaKh", "KH", "MaKh", "TenKh", "Sdt", "CccdPassport", "QuocTich", "GioiTinh") { } } }
namespace HotelManagement.NhanVien { public sealed class FormDanhSachNhanVien : HotelManagement.Lab2.EntityManagementForm<HotelManagement.Models.NhanVien> { public FormDanhSachNhanVien() : base("Quản lý nhân viên", "MaNv", "NV", "MaNv", "TenNv", "ChucVu", "Luong", "Sdt", "Email") { } } }
