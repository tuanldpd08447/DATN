using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class XuatXu
    {
        [Key]
        public string IDXX { get; set; } // ID xuất xứ
        public string QuocGia { get; set; } // Quốc gia
        public string? GhiChu { get; set; } // Ghi chú
    }
}
