using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class NguonCungCap
    {
        [Key]
        public string IDNGC { get; set; }
        public string TenNguonCungCap { get; set; }
        public string? GhiChu { get; set; }
    }

}
