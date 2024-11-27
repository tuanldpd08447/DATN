using System.ComponentModel.DataAnnotations;

namespace DATN_CDCWebClient.Models
{
    public class NguonCungCap
    {
        [Key]
        public string IDNGC { get; set; }
        public string TenNguonCungCap { get; set; }
        public string? GhiChu { get; set; }

        public ICollection<VatTuYTe> VatTuYTes { get; set; }
    }

}
