using System.ComponentModel.DataAnnotations;

namespace DATN_CDCWebClient.Models
{
    public class LoaivatTu
    {
        [Key]
        public string IDTL { get; set; }
        public string TenLoaiVatTu { get; set; }
        public string? MoTa { get; set; }

        public ICollection<VatTuYTe> VatTuYTes { get; set; }
    }

}
