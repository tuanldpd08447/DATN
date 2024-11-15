using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung.Models
{
    public class Order
    {
        [Key]
        public string IDOD { get; set; } // ID đơn hàng

        [Required]
        public string IDVT { get; set; } // ID vật tư
        [ForeignKey(nameof(IDVT))]
        public VatTuYTe VatTuYTe { get; set; }

        public int SoLuong { get; set; } // Số lượng
        public string? GhiChu { get; set; } // Ghi chú
    }


}
