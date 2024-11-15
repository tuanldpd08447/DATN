using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung.Models
{
    public class NhaCungCap
    {
        [Key]
        public string IDNHC { get; set; }
        public string TenNhaCungCap { get; set; }
        public string? GhiChu { get; set; }
        public ICollection<VatTuYTe> VatTuYTes { get; set; }
    }

}
