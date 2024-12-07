using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung.Models
{
    public class KHDTO
    {
        public string IDKH { get; set; }
        public string IDXP { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string CCCD_MDD { get; set; }
        public string DanToc { get; set; }

    }

    public class HoaDonDTO
    {
        public string IDHD { get; set; }
        public string IDKH { get; set; }
        public string IDNV { get; set; }
        public DateTime ThoiGian { get; set; }
        public string NoiDung { get; set; }

        public Double TongTien { get; set; }
        public bool ThanhToan { get; set; }
        public bool TrangThai { get; set; }
        public string GhiChu { get; set; }


        public KhachHang? KhachHang { get; set; }
        public NhanVien? NhanVien { get; set; }
        public List<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();
    }
    public class KhachHangDTo
    {
        public string IDKH { get; set; }

        public string TenKhachHang { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string CCCD_MDD { get; set; }
        public string DanToc { get; set; }
        public string FullAddress { get; set; }
    }
    public class KhachHangCreateDTO
    {
        [Required]
        public string IDXP { get; set; }

        [Required]
        public string TenKhachHang { get; set; }

        [Required]
        public DateTime NgaySinh { get; set; }

        [Required]
        public string GioiTinh { get; set; }

        [Required]
        public string DiaChi { get; set; }

        [Required]
        public string SoDienThoai { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CCCD_MDD { get; set; }

        public string DanToc { get; set; }
    }
    public class KhachHanginput
    {
        
        public string hoTen { get; set; }
        public DateTime ngaySinh { get; set; }
        public string danToc { get; set; }
        public string gioiTinh { get; set; }
        public string cmt { get; set; }
        public string sdt { get; set; }
        public string email { get; set; }
        public string hoKhauXa { get; set; }
        public string diaChiChiTiet { get; set; }


    }
    public class KhachHangPreOder
    {
        public string IDKH { get; set; }

        public string TenKhachHang { get; set; }
        public TimeOnly ThoiGianHen { get; set; }
        public DateOnly NgayHen { get; set; }

        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string CCCD_MDD { get; set; }
        public string DanToc { get; set; }
        public string FullAddress { get; set; }
    }
    public class HangCho
    {
        public string ID { get; set; }
        public string IDKH { get; set; }
        public string HoTen { get; set; }
        public DateOnly NgaySinh { get; set; }
        public DateOnly NgayCho { get; set; }
        public string Step { get; set; }

    }
    public class LoginNhanVien
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginKhachHang
    {
        public string Sdt { get; set; }
        public string Password { get; set; }
    }
    public class LoginResult
    {
        public string ID { get; set; }
        public string Role { get; set; }

        public string Username { get; set; }
    }
    public class VatTuDTO
    {
        public string IDVT { get; set; }
        public string TenVatTu { get; set; }
        public double? DonGia { get; set; }
        public int SoLuong { get; set; }

        public DateTime HanSuDung { get; set; }
        public string? GhiChu { get; set; }
        public string? IDTL { get; set; }
        public string? IDNGC { get; set; }
        public string? IDNHC { get; set; }
        public string? IDXX { get; set; }
    }
    public class VaccineCreateDTO
    {

        [Required]
        public string IDTL { get; set; }
        [Required]
        public string TenVatTu { get; set; }
        public string? IDNGC { get; set; }
        public string IDXX { get; set; }
        public string? IDNHC { get; set; }
        [Required]
        public double DonGia { get; set; }
        [Required]
        public DateTime HanSuDung { get; set; }

        public string? GhiChu { get; set; }
        [Required]
        public int SoLuong { get; set; }
    }
    public class FindvattuDTO
    {
        public string? IDVT { get; set; }
        public string? TenVatTu { get; set; }
        public string? IDTL { get; set; }
        public string? IDNGC { get; set; }
        public string? IDNHC { get; set; }
        public DateTime? HanSuDung { get; set; }

    }
    public class HoaDonCreateDTO
    {
        public string IDHD { get; set; }
        public string IDHDCT { get; set; }
        public string IDKH { get; set; }
        public string IDNV { get; set; }
        public string IDVT { get; set; }
        public DateTime ThoiGian { get; set; }
        public string NoiDung { get; set; }
        public int SoLuong { get; set; }
        public Double DonGia { get; set; }
        public Double ThanhTien { get; set; }
        public Double TongTien { get; set; }

        public string GhiChu { get; set; }




    }

    public class ChungTuDetail
    {
        public string IDXCT { get; set; }
        public string IDNV { get; set; }
        public string IDVT { get; set; }
        public string TenVatTu { get; set; }
        public string? IDTL { get; set; }
        public string? IDNHC { get; set; }
        public string? IDNGC { get; set; }
        public string? IDXX { get; set; }
        public DateTime HanSuDung { get; set; }
        public string? GhiChu { get; set; }
        public double? DonGia { get; set; }
        public int SoLuongTonKho { get; set; }
        public int SoLuongXuatNhap { get; set; }
        public bool LoaiChungTu { get; set; }
        public DateTime ThoiGianXuatNhap { get; set; }
        public double? DonGiaXuatNhap { get; set; }
        public double ThanhTien { get; set; }
        public bool TrangThai { get; set; }
        public string? HinhAnh { get; set; }
    }
    public class crteateChungTu
    {
        public string IDXCT { get; set; }
        public string IDNV { get; set; }
        public string IDVT { get; set; }
        public bool LoaiChungTu { get; set; }
        public DateTime ThoiGian { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien { get; set; }
        public bool TrangThai { get; set; }
        public string? GhiChu { get; set; }
        public string? HinhAnh { get; set; }
        public int SoLuongXuatNhap { get; set; }
    }
    public class KQKhamSangLocDTO
    {
        public string IDKH { get; set; }
        public string TenKhachHang { get; set; }
        public string GioiTinh { get; set; }
        public Double ChieuCao { get; set; }
        public Double CanNang { get; set; }
        public string TinhTrangSucKhoe { get; set; }
        public bool KetQua { get; set; }
        public bool TrangThai { get; set; }
        public KhachHang? KhachHang { get; set; }
    }

    public class DSKhamSangLocDTO
    {
        public string IDKH { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime NgaySinh { get; set; }
        public DateTime ThoiGian { get; set; }
        public KhachHang? KhachHang { get; set; }
        public DangKyTiemChung? DangKyTiemChung { get; set; }
    }

    public class CDVaccineDTO
    {
        public string IDDK { get; set; }
        public string IDKH { get; set; }
        public KhachHang? KhachHang { get; set; }
        public string IDDKVC { get; set; }
        public string TenVaccine { get; set; }
        public int SoLuong { get; set; }
        public DangKyVaccine? DangKyVaccine { get; set; }
        public string IDNV { get; set; }
        public string TenNhanVien { get; set; }
        public NhanVien? NhanVien { get; set; }
    }

    public class createTiemChung
    {
        public string IDKH { get; set; }

        public string IDNV { get; set; }
        public string IDDK { get; set; }

        public DateTime ThoiGian { get; set; }
        public bool TrangThai { get; set; }
        public string? GhiChu { get; set; }
    }

    public class createTheoDoi
    {
        public string IDST { get; set; }
        public string IDTC { get; set; }
        public string IDNV { get; set; }
        public string IDKH { get; set; }
        public TimeSpan ThoiGian { get; set; }
        public bool TrangThai { get; set; }
        public string? GhiChu { get; set; }

    }
    public class NhanVienDTO
    {
        public string IDNV { get; set; }
        public string IDXP { get; set; }

        public string TenNhanVien { get; set; }
        public string ChucVu { get; set; }
        public string TenChucDanh { get; set; }
        public string? TenPhongBan { get; set; }

        public string DiaChi { get; set; }
        public string CCCD { get; set; }
        public string DanToc { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string FullAddress { get; set; }

    }
    public class NhanVienCreateDTO()
    {
        public string IDXP { get; set; }
        public string IDNV { get; set; }
        public string TenNhanVien { get; set; }
        public string ChucVu { get; set; }
        public string TenChucDanh { get; set; }
        public string? TenPhongBan { get; set; }
        public string DiaChi { get; set; }
        public string CCCD { get; set; }
        public string DanToc { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
    }

    public class DiaChi
    {
        public string IDXP { get; set; }
        public string NameXP { get; set; }
        public string IDQH { get; set; }
        public string NameQH { get; set; }
        public string IDTTP { get; set; }
        public string NameTTP { get; set; }
    }
    public class KhachHangUpdateDTO
    {
        public string IDKH { get; set; }
        public string IDXP { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string CCCD_MDD { get; set; }

        public string DanToc { get; set; }
    }
    public class KhachHangDTo2
    {
        public string IDKH { get; set; }

        public string TenKhachHang { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string CCCD_MDD { get; set; }
        public string DanToc { get; set; }
        public string FullAddress { get; set; }
        public string IDXP { get; set; }
        public string NameXP { get; set; }
        public string IDQH { get; set; }
        public string NameQH { get; set; }
        public string IDTTP { get; set; }
        public string NameTTP { get; set; }
    }
	public class FindKhachHang
	{
		public string? IDKH { get; set; }

		public string? TenKhachHang { get; set; }
		public string? SoDienThoai { get; set; }
		public string? CCCD_MDD { get; set; }
	}
}
