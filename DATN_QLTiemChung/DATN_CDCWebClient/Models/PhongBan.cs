using System.ComponentModel.DataAnnotations;

namespace DATN_CDCWebClient.Models
{
    public class PhongBan
    {
        [Key]
        public string IDPB { get; set; } // ID phòng ban
        public string TenPhongBan { get; set; } // Tên phòng ban
        public string? GhiChu { get; set; } // Ghi chú

    }
}
