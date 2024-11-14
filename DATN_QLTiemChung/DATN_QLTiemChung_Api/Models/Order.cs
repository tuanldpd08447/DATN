using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class Order
    {
        [Key]
        public string IDOD { get; set; } // ID đơn hàng
        public string IDVT { get; set; } // ID vật tư
        public int SoLuong { get; set; } // Số lượng
        public string? GhiChu { get; set; } // Ghi chú
    }

}
