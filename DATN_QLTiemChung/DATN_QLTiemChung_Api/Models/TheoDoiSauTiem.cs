using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class TheoDoiSauTiem
    {
        [Key]
        public string IDST { get; set; } // ID theo dõi sau tiêm
        public string IDNV { get; set; } // ID nhân viên
        public string IDKH { get; set; } // ID khách hàng
        public TimeSpan ThoiGian { get; set; } // Thời gian
        public bool TrangThai { get; set; } // Trạng thái
        public string? GhiChu { get; set; } // Ghi chú
    }
}
