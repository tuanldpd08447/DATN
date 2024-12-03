using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung.Models
{
    public class TiemChung
    {
        [Key]
        public string IDTC { get; set; } // ID tiêm chủng

        [Required]
        public string IDKH { get; set; } // ID khách hàng
        [ForeignKey(nameof(IDKH))]
        public KhachHang KhachHang { get; set; }

        [Required]
        public string IDNV { get; set; } // ID nhân viên
        [ForeignKey(nameof(IDNV))]
        public NhanVien NhanVien { get; set; }

        [Required]
        public string IDDK { get; set; } // ID đăng ký
        [ForeignKey(nameof(IDDK))]
        public DangKyTiemChung DangKyTiemChung { get; set; }

        public DateTime ThoiGian { get; set; }
        public bool TrangThai { get; set; }
        public string? GhiChu { get; set; }
    }

}
