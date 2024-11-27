using System.ComponentModel.DataAnnotations;

namespace DATN_CDCWebClient.Models
{
    public class DatLichKham
    {
        [Key]
        public string IDLK { get; set; } 
        public string IDKH { get; set; } 
        public DateTime ThoiGian { get; set; }


        public KhachHang KhachHang { get; set; }
    }
}
