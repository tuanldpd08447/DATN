using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class DangKyTiemChung
    {
        [Key]
        public string IDDK { get; set; }
        public string IDKH { get; set; }
        public string IDDKVC { get; set; }
        public string IDNV { get; set; }
        public DateTime ThoiGianDK { get; set; }
        public DateTime ThoiGianTiem { get; set; }
        public string? GhiChu { get; set; }
    }

}
