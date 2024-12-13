using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_CDCWebClient.Models
{
    public class EmailRequest
    {
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class UpdatePasswordRequest
    {
        public string IDKH { get; set; }
        public string MatKhau { get; set; }
    }
    public class DoiMatKhauRequest
    {
        public string IDKH { get; set; }
        public string MatKhauCu { get; set; }
        public string MatKhauMoi { get; set; }
    }
    public class QLyTaiKhoanKHDTO
    {
        public string IDKH { get; set; }
        public string IDTKKH { get; set; }
        public string SDT { get; set; }
        public string MatKhau { get; set; }
        public string TenKhachHang { get; set; }
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
    public class ThoiGianKham
    {
        public DateOnly NgayHen { get; set; }
        public TimeOnly ThoiGianHen { get; set; }
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
    public class LichSuTiem
    {
        public string ID { get; set; } // ID Lịch sử tiêm
        public string IDKH { get; set; } // ID Khách hàng
        public string TenKhachHang { get; set; } // Tên khách hàng
        public string IDVT { get; set; } // ID Vắc-xin
        public string TenVacxin { get; set; } // Tên vắc-xin
        public string XuatXu { get; set; }
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
}
