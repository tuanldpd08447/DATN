using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class UpdateHoaDonRequest
    {
        public bool ThanhToan { get; set; }
        public string GhiChu { get; set; }
    }
    public class EmailRequest
    {
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class DoiMatKhauRequest
    {
        public string IDKH { get; set; }
        public string MatKhauCu { get; set; }
        public string MatKhauMoi { get; set; }
    }
    public class UpdatePasswordRequest
    {
        public string IDKH { get; set; }
        public string MatKhau { get; set; }
    }
    public class QLyTaiKhoanKHDTO
    {
        public string IDKH { get; set; }
        public string IDTKKH { get; set; }
        public string SDT { get; set; }
        public string MatKhau { get; set; }
        public string TenKhachHang { get; set; }
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
    public class DiaChiDTO
    {
        public string IDXP { get; set; }
        public string TenXaPhuong { get; set; }
        public string IDQH { get; set; }
        public string TenQuanHuyen { get; set; }
        public string IDTP { get; set; }
        public string TenTinhThanhPho { get; set; }
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
    public class ThoiGianKham
    {
        DateOnly Ngayhen { get; set; }
        TimeOnly ThoiGianHen {  get; set; }
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
    public class RegisterDto
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

        public string Password { get; set; }

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
    public class VatTuDTOFull
    {
        public string IDVT { get; set; }
        public string TenVatTu { get; set; }
        public double? DonGia { get; set; }
        public int SoLuong { get; set; }

        public DateTime HanSuDung { get; set; }
        public string? GhiChu { get; set; }
        public string? IDTL { get; set; }
        public string? TenLoai { get; set; }
        public string? IDNGC { get; set; }
        public string? TenNguon { get; set; }
        public string? IDNHC { get; set; }
        public string? TenNhaCungCap { get; set; }
        public string? IDXX { get; set; }
        public string? NoiXuatXu { get; set; }
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

        public bool TrangThai { get; set; }
        public bool ThanhToan { get; set; }
        public string GhiChu { get; set; }




    }

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
        public string GhiChu { get; set; }
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
        [Required]

        public string IDXP { get; set; }


        public string? IDNV { set; get; }

        [Required]
        public string TenNhanVien { get; set; }
        [Required]
        public string ChucVu { get; set; }
        [Required]
        public string TenChucDanh { get; set; }
        [Required]
        public string? TenPhongBan { get; set; }

        [Required]
        public string DiaChi { get; set; }
        [Required]
        public string CCCD { get; set; }
        public string DanToc { get; set; }
        public DateTime NgaySinh { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string GioiTinh { get; set; }
        [Required]
        [Phone]
        public string SoDienThoai { get; set; }


    }
    public class FindKhachHang
    {
		public string? IDKH { get; set; }
		public string? TenKhachHang { get; set; }
		public string? SoDienThoai { get; set; }
		public string? CCCD_MDD { get; set; }
	}
    public class KhamSangLocDTO
    {
        public string IDVT { get; set; }
        public int SoLuong { get; set; }
        public string IDKH { get; set; }
        public Double ChieuCao { get; set; }
        public Double CanNang { get; set; }
        public string IDNV { get; set; }
        public string TinhTrangSucKhoe { get; set; }
        public bool TrangThai { get; set; }
        public bool KetQua { get; set; }
        public string? GhiChu { get; set; }
        public bool MuiTiepTheo { get; set; }
    }
    public class LichSuTiem
    {
        public string ID { get; set; } // ID Lịch sử tiêm
        public string IDKH { get; set; } // ID Khách hàng
        public string TenKhachHang { get; set; } // Tên khách hàng
        public string IDVT { get; set; } // ID Vắc-xin
        public string TenVacxin { get; set; } // Tên vắc-xin
        public string XuatXu {  get; set; }
        public DateTime NgayTiem { get; set; } // Ngày tiêm
        public Double? DonGia { get; set; }
        public Double? ThanhTien { get; set; }
        public bool TrangThaiTiem { get; set; }
        public bool TrangThaiSauTiem { get; set; }
        public string LieuTiem { get; set; } // Liều tiêm (mũi 1, mũi 2, booster,...)
        public string IDNV { get; set; } // ID Nhân viên
        public string TenNhanVien { get; set; } // Tên nhân viên thực hiện
        public string? GhiChu { get; set; } // Ghi chú
    }
    public class LichKham
    {
        public string ID { get; set; } // ID lịch khám
        public string IDKH { get; set; } // ID khách hàng
        public string TenKhachHang { get; set; } // Tên khách hàng
        public DateOnly NgayKham { get; set; } // Ngày khám
        public TimeOnly GioKham { get; set; } // Giờ khám
    }

    public class DSHoanTraDTO
    {
        public string IDKH { get; set; }
        public string TenKhachHang { get; set; }
        public DateOnly ThoiGian { get; set; }
        public string GhiChu { get; set; }
        public KhachHang? KhachHang { get; set; }
    }

    public class HoaDonHT
    {
        public string IDHT { get; set; }
        public string HoaDonMoi { get; set; }
        public string HoaDonCu { get; set; }
        public string GhiChu { get; set; } // Lý do hoàn trả
        public Double DonGia { get; set; } // Đơn giá
        public bool TrangThai { get; set; } // Trạng thái
        public string IDHD { get; set; }
        public string IDKH { get; set; }
        public string IDHDCT { get; set; }
        public string TenKhachHang { get; set; }
        public bool ThanhToan { get; set; }
        public DateTime ThoiGian { get; set; }
        public string IDVT { get; set; }
        public string IDNV { get; set; }
        public string NoiDung { get; set; }
        public int SoLuong { get; set; }
        public Double TongTien { get; set; }

        public HoaDon? HoaDon { get; set; }
        public KhachHang? KhachHang { get; set; }
        public NhanVien? NhanVien { get; set; }
        public ICollection<HoaDonChiTiet> HoaDonChiTiet { get; set; } = new List<HoaDonChiTiet>();
    }
    public class HoaDonChiTietDTO
    {
        public string IDVT { get; set; }
        public string TenVT { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien { get; set; }
    }

    public class HoaDonDTO2
    {
        public string IDHD { get; set; }
        public string IDNV { get; set; }
        public string KhachHang { get; set; }
        public DateTime ThoiGian { get; set; }
        public double TongTien { get; set; }
        public string NoiDung { get; set; }
        public List<HoaDonChiTietDTO> HoaDonChiTiets { get; set; }
    }

    public class HoanTraDTO
    {
        public string IDHT { get; set; }
        public string IDKH { get; set; }
        public string LyDo { get; set; }
        public DateTime ThoiGian { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien { get; set; }

        // Sửa thành HoaDonDTO2
        public HoaDonDTO2 HoaDonCu { get; set; }
        public HoaDonDTO2 HoaDonMoi { get; set; }
    }
    public class QLTaiKhoanNVDTO
    {
        [Required]
        public string IDTKNV { get; set; }
        [Required]
        public string IDNV { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string MatKhau { get; set; }

    }
    public class CreateTaiKhoanNVDTO
    {
        public string IDTKNV { get; set; }
        public string IDNV { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
    }
    public class UpdateMatKhauNVDTO
    {
        public string NewPassword { get; set; }
    }
    public class QLTaiKhoanKHDTO
    {
        [Required]
        public string IDTKKH { get; set; }
        [Required]
        public string IDKH { get; set; }
        [Required]
        public string SDT { get; set; }
        [Required]
        public string MatKhau { get; set; }
    }
    public class CreateTaiKhoanKHDTO
    {

        public string IDTKKH { get; set; }

        public string IDKH { get; set; }

        public string SDT { get; set; }

        public string MatKhau { get; set; }
    }

}
