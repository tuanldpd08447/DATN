using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class TiemChung
    {
        [Key]
        public string IDTC { get; set; } // ID tiêm chủng
        public string IDKH { get; set; } // ID khách hàng
        public string IDNV { get; set; } // ID nhân viên
        public string IDDK { get; set; } // ID đăng ký
        public TimeSpan ThoiGian { get; set; } // Thời gian
        public bool TrangThai { get; set; } // Trạng thái
        public string? GhiChu { get; set; } // Ghi chú
    }
}
