using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Models;

// File riêng (partial) để khi scaffold lại DB, EF Core Power Tools ghi đè
// HotelManagementContext.cs
// thì cấu hình kết nối này không bị mất.
public partial class HotelManagementContext
{
    // Đổi Server cho đúng máy của bạn:
    // "." | "localhost" | ".\\SQLEXPRESS" | "(localdb)\\MSSQLLocalDB"
    private const string ConnectionString =
     "Server=localhost\\SQLEXPRESS;Database=HotelManagement;Trusted_Connection=True;TrustServerCertificate=True;";

    // Constructor không tham số để dùng: new HotelManagementContext()
    public HotelManagementContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(ConnectionString);
    }
}