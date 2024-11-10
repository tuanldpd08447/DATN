using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class ChucDanh
    {
        [Key]
        public string IDCD { get; set; }
        public string TenChucDanh { get; set; }
        public string? MoTa { get; set; }
    }

}
