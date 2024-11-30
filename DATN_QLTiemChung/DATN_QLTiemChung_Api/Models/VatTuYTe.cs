using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class VatTuYTe
    {
        [Key]
        public string IDVT { get; set; } // ID Vật Tư Y Tế

        public string? IDTL { get; set; }
        [ForeignKey(nameof(IDTL))]
        public LoaivatTu? LoaivatTu { get; set; }

        public string? IDNHC { get; set; }
        [ForeignKey(nameof(IDNHC))]
        public NhaCungCap? NhaCungCap { get; set; }

        public string? IDNGC { get; set; }
        [ForeignKey(nameof(IDNGC))]
        public NguonCungCap? NguonCungCap { get; set; }

        public string? IDXX { get; set; }
        [ForeignKey(nameof(IDXX))]
        public XuatXu? XuatXu { get; set; }

        public string TenVatTu { get; set; }
        public DateTime HanSuDung { get; set; }
        public string? GhiChu { get; set; }
        public double? DonGia { get; set; }
        public int SoLuong { get; set; }
    }

}
