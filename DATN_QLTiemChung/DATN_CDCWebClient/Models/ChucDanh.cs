using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_CDCWebClient.Models
{
    public class ChucDanh
    {
        [Key]
        public string IDCD { get; set; }
        public string TenChucDanh { get; set; }
        public string? MoTa { get; set; }
    }


}
