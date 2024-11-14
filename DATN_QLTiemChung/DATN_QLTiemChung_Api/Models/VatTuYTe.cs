using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class VatTuYTe
    {
        [Key]
        public string IDVT { get; set; } // ID vật tư y tế
        public string? IDTL { get; set; } // ID loại vật tư
        public string? IDNHC { get; set; } // ID nhóm hàng chờ
        public string? IDNGC { get; set; } // ID nhóm chi tiết
        public string? IDXX { get; set; } // ID xuất xứ
        public string? TenVatTu { get; set; } // Tên vật tư
        public DateTime? HanSuDung { get; set; } // Hạn sử dụng
        public string? GhiChu { get; set; } // Ghi chú
        public double? DonGia { get; set; } // Đơn giá
    }
}
