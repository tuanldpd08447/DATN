using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class HoaDonChiTiet
    {
        [Key]
        public string IDHDCT { get; set; }

        [Required]
        public string IDHD { get; set; }
        [ForeignKey(nameof(IDHD))]
        public HoaDon HoaDon { get; set; }

        [Required]
        public string IDVT { get; set; } 
        [ForeignKey(nameof(IDVT))]
        public VatTuYTe VatTuYTe { get; set; }

        public int SoLuong { get; set; }
        public Double DonGia { get; set; }
        public Double ThanhTien { get; set; }
        public string? GhiChu { get; set; }
    }


}
