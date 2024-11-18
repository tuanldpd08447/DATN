using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class HoaDonDTO
    {
        public string IDHD { get; set; }
        public string IDKH { get; set; }
        public string IDNV { get; set; }
        public DateTime ThoiGian { get; set; }
        public string NoiDung { get; set; }

        public Double TongTien { get; set; }

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
}
