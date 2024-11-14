using DATN_QLTiemChung_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api
{

    public partial class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<ChucDanh> ChucDanh { get; set; }
        public DbSet<ChungTu> ChungTu { get; set; }
        public DbSet<DangKyTiemChung> DangKyTiemChung { get; set; }
        public DbSet<DangKyVaccine> DangKyVaccine { get; set; }
        public DbSet<District> districts { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<HoaDonChiTiet> HoaDonChiTiet { get; set; }
        public DbSet<HoanTra> HoanTra { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<KhamSangLoc> KhamSangLoc { get; set; }
        public DbSet<LoaivatTu> LoaiVatTu { get; set; }
        public DbSet<NguonCungCap> NguonCungCap { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Province> provinces { get; set; }
        public DbSet<PhongBan> PhongBan { get; set; }
        public DbSet<QLyTaiKhoanKH> QLyTaiKhoanKH { get; set; }
        public DbSet<QLyTaiKhoanNV> QLyTaiKhoanNV { get; set; }
        public DbSet<TiemChung> TiemChung { get; set; }
        public DbSet<TheoDoiSauTiem> TheoDoiSauTiem { get; set; }
        public DbSet<VatTuYTe> VatTuYTe { get; set; }
        public DbSet<Ward> wards { get; set; }
        public DbSet<XuatXu> XuatXu { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
       
    }


}
