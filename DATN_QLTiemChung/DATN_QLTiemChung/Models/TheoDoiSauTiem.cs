using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung.Models
{


    public class TheoDoiSauTiem
    {
        [Key]
        public string IDST { get; set; } // ID theo dõi sau tiêm
        [Required]
        public string IDTC { get; set; } // ID nhân viên
        [ForeignKey(nameof(IDTC))]
        public TiemChung? TiemChung { get; set; }

        [Required]
        public string IDNV { get; set; } // ID nhân viên
        [ForeignKey(nameof(IDNV))]
        public NhanVien? NhanVien { get; set; }

        [Required]
        public string IDKH { get; set; } // ID khách hàng
        [ForeignKey(nameof(IDKH))]
        public KhachHang? KhachHang { get; set; }

        public TimeSpan ThoiGian { get; set; }
        public bool TrangThai { get; set; }
        public string? GhiChu { get; set; }
    }
}
