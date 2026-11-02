using HotelManagement.DangNhap;
using HotelManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagement.SoDoPhong;

public sealed class FormDatPhong : Form
{
    private readonly string _maPhong;
    private readonly TextBox _name = new(), _id = new(), _phone = new(), _nationality = new();
    private readonly ComboBox _gender = new();
    private readonly CheckedListBox _rooms = new();
    private readonly DateTimePicker _checkIn = new(), _checkOut = new();
    private readonly NumericUpDown _people = new();

    public FormDatPhong(string maPhong)
    {
        _maPhong = maPhong; Text = $"Đặt phòng {maPhong}"; StartPosition = FormStartPosition.CenterParent; FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false; ClientSize = new Size(600, 610); BackColor = Color.White; Font = new Font("Segoe UI", 10);
        var table = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18), ColumnCount = 2, RowCount = 10 }; table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _rooms.CheckOnClick = true; AddRow(table, 0, "Chọn phòng", _rooms); AddRow(table, 1, "Họ tên khách", _name); AddRow(table, 2, "CCCD/Passport", _id); AddRow(table, 3, "Số điện thoại", _phone); AddRow(table, 4, "Quốc tịch", _nationality);
        _gender.DropDownStyle = ComboBoxStyle.DropDownList; _gender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" }); _gender.SelectedIndex = 0; AddRow(table, 5, "Giới tính", _gender);
        _checkIn.Format = DateTimePickerFormat.Custom; _checkIn.CustomFormat = "dd/MM/yyyy HH:mm"; _checkIn.ShowUpDown = true; _checkIn.Value = DateTime.Now.AddHours(1); AddRow(table, 6, "Nhận phòng", _checkIn);
        _checkOut.Format = DateTimePickerFormat.Custom; _checkOut.CustomFormat = "dd/MM/yyyy HH:mm"; _checkOut.ShowUpDown = true; _checkOut.Value = DateTime.Now.AddDays(1); AddRow(table, 7, "Trả phòng", _checkOut);
        _people.Minimum = 1; _people.Maximum = 20; _people.Value = 1; AddRow(table, 8, "Số người mỗi phòng", _people);
        AddRow(table, 9, "", new Label { Text = "Chọn một hoặc nhiều phòng trống cùng thời gian.", AutoSize = true, ForeColor = Color.DimGray });
        var save = new Button { Text = "Xác nhận đặt phòng", Width = 190, Height = 38, BackColor = Color.LightGreen }; save.Click += (_, _) => SaveBooking(); var cancel = new Button { Text = "Hủy", Width = 90, Height = 38, DialogResult = DialogResult.Cancel };
        var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 56, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(10) }; actions.Controls.Add(cancel); actions.Controls.Add(save); Controls.Add(table); Controls.Add(actions); CancelButton = cancel;
        _id.Leave += (_, _) => FillKnownCustomer(); _checkIn.ValueChanged += (_, _) => LoadAvailableRooms(); _checkOut.ValueChanged += (_, _) => LoadAvailableRooms(); LoadAvailableRooms();
    }
    private static void AddRow(TableLayoutPanel t, int r, string label, Control control) { t.RowStyles.Add(new RowStyle(SizeType.Absolute, control is CheckedListBox ? 110 : 46)); control.Dock = DockStyle.Fill; if (control is TextBox tb) tb.MaxLength = label == "CCCD/Passport" ? 12 : 100; t.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Padding = new Padding(0, 8, 0, 0) }, 0, r); t.Controls.Add(control, 1, r); }

    private void FillKnownCustomer()
    {
        var cccd = _id.Text.Trim(); if (cccd.Length < 6) return;
        try { using var db = new HotelManagementContext(); var kh = db.KhachHangs.FirstOrDefault(x => x.CccdPassport == cccd && x.DaXoa != true); if (kh == null) return; _name.Text = kh.TenKh; _phone.Text = kh.Sdt; _nationality.Text = kh.QuocTich; _gender.SelectedItem = kh.GioiTinh; }
        catch (Exception ex) { MessageBox.Show("Không tra cứu được khách hàng: " + ex.GetBaseException().Message); }
    }

    private void LoadAvailableRooms()
    {
        try
        {
            var selected = _rooms.CheckedItems.Cast<RoomOption>().Select(x => x.Code).Append(_maPhong).ToHashSet(StringComparer.OrdinalIgnoreCase);
            using var db = new HotelManagementContext();
            var choices = db.Phongs.Include(x => x.MaLphNavigation).Where(x => x.DaXoa != true && x.Ttph == "Bình thường" && !x.Ctdps.Any(c => c.DaXoa != true && c.CheckIn < _checkOut.Value && c.CheckOut > _checkIn.Value)).OrderBy(x => x.MaPh).Select(x => new RoomOption(x.MaPh, x.MaLphNavigation.TenLph, x.MaLphNavigation.SoNguoiToiDa)).ToList();
            _rooms.Items.Clear(); foreach (var room in choices) _rooms.Items.Add(room, selected.Contains(room.Code));
        }
        catch (Exception ex) { MessageBox.Show("Không tải được danh sách phòng trống: " + ex.GetBaseException().Message); }
    }

    private void SaveBooking()
    {
        var name = _name.Text.Trim(); var id = _id.Text.Trim(); var phone = _phone.Text.Trim(); var nationality = _nationality.Text.Trim();
        if (name.Length == 0 || id.Length == 0 || phone.Length == 0 || nationality.Length == 0) { MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng."); return; }
        if (_checkOut.Value <= _checkIn.Value) { MessageBox.Show("Thời gian trả phòng phải sau thời gian nhận phòng."); return; }
        try
        {
            using var db = new HotelManagementContext(); using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
            var roomCodes = _rooms.CheckedItems.Cast<RoomOption>().Select(x => x.Code).ToList();
            if (roomCodes.Count == 0) throw new InvalidOperationException("Hãy chọn ít nhất một phòng.");
            var rooms = db.Phongs.Include(x => x.MaLphNavigation).Where(x => roomCodes.Contains(x.MaPh) && x.DaXoa != true).ToList();
            if (rooms.Count != roomCodes.Count) throw new InvalidOperationException("Một hoặc nhiều phòng không còn tồn tại.");
            foreach (var room in rooms)
            {
                if (room.Ttph != "Bình thường" || db.Ctdps.Any(c => c.MaPh == room.MaPh && c.DaXoa != true && c.CheckIn < _checkOut.Value && c.CheckOut > _checkIn.Value)) throw new InvalidOperationException($"Phòng {room.MaPh} đã có lịch trùng hoặc không ở trạng thái sẵn sàng.");
                if (_people.Value > room.MaLphNavigation.SoNguoiToiDa) throw new InvalidOperationException($"Phòng {room.MaPh} chỉ nhận tối đa {room.MaLphNavigation.SoNguoiToiDa} người.");
            }
            var customer = db.KhachHangs.FirstOrDefault(x => x.CccdPassport == id);
            if (customer == null) { customer = new HotelManagement.Models.KhachHang { MaKh = NextCode(db.KhachHangs.Select(x => x.MaKh), "KH", 5), TenKh = name, CccdPassport = id, Sdt = phone, QuocTich = nationality, GioiTinh = _gender.SelectedItem!.ToString()!, DaXoa = false }; db.KhachHangs.Add(customer); }
            else { customer.TenKh = name; customer.Sdt = phone; customer.QuocTich = nationality; customer.GioiTinh = _gender.SelectedItem!.ToString()!; customer.DaXoa = false; }
            var staffId = FormDangNhap.MaNV_DangNhap; if (string.IsNullOrWhiteSpace(staffId)) throw new InvalidOperationException("Không xác định được nhân viên đang đăng nhập.");
            var bookingId = NextCode(db.PhieuThues.Select(x => x.MaPt), "PT", 5); var detailId = NextCode(db.Ctdps.Select(x => x.MaCtdp), "CTDP", 7);
            db.PhieuThues.Add(new PhieuThue { MaPt = bookingId, NgPt = DateTime.Now, MaKh = customer.MaKh, MaNv = staffId, DaXoa = false });
            var days = Math.Max(1, (int)Math.Ceiling((_checkOut.Value.Date - _checkIn.Value.Date).TotalDays));
            var allDetailCodes = db.Ctdps.Select(x => x.MaCtdp).ToList();
            for (var i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i]; var price = room.MaLphNavigation.GiaNgay; var code = i == 0 ? detailId : NextCode(allDetailCodes, "CTDP", 7); allDetailCodes.Add(code);
                db.Ctdps.Add(new Ctdp { MaCtdp = code, MaPt = bookingId, MaPh = room.MaPh, SoNguoi = (int)_people.Value, CheckIn = _checkIn.Value, CheckOut = _checkOut.Value, TrangThai = "Đã đặt", DonGia = price, ThanhTien = price * days, DaXoa = false, TheoGio = false });
                room.Ttph = "Đã đặt";
            }
            db.SaveChanges(); tx.Commit(); MessageBox.Show($"Đặt thành công {rooms.Count} phòng. Mã phiếu: {bookingId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information); DialogResult = DialogResult.OK; Close();
        }
        catch (Exception ex) { MessageBox.Show("Không thể đặt phòng: " + ex.GetBaseException().Message, "Đặt phòng", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
    private static string NextCode(IEnumerable<string> values, string prefix, int length)
    { var n = values.Where(x => x.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).Select(x => int.TryParse(x[prefix.Length..], out var value) ? value : 0).DefaultIfEmpty(0).Max() + 1; var code = prefix + n.ToString(new string('0', length - prefix.Length)); if (code.Length > length) throw new InvalidOperationException("Đã hết mã định danh có thể tạo tự động."); return code; }
    private sealed record RoomOption(string Code, string Type, int Capacity) { public override string ToString() => $"{Code} - {Type} (tối đa {Capacity} khách)"; }
}
