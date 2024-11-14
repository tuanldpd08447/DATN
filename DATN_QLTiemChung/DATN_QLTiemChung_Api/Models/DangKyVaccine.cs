using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class DangKyVaccine
    {
        [Key]
        public string IDDKVC { get; set; }
        public string IDVT { get; set; }
        [ForeignKey(nameof(IDVT))]
        public VatTuYTe VatTuYTe { get; set; }
        public string TenVaccine { get; set; }
        public int SoLuong { get; set; }
        public Double? DonGia { get; set; }
        public Double? ThanhTien { get; set; }
        public string? GhiChu { get; set; }

        public ICollection<DangKyTiemChung> DangKyTiemChungs { get; set; }
    }

}
