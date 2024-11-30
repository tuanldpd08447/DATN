using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class DatLichKham
    {
        [Key]
        public string IDLK { get; set; } 
        public string IDKH { get; set; } 
        public DateTime ThoiGian { get; set; }


        public KhachHang? KhachHang { get; set; }
    }
}
