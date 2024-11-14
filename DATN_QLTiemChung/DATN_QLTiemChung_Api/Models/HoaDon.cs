using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class HoaDon
    {
        [Key]
        public string IDHD { get; set; }
        public string IDKH { get; set; }
        public string IDNV { get; set; }
        public DateTime ThoiGian { get; set; }
        public string NoiDung { get; set; }
        public float TongTien { get; set; }
        public bool TrangThai { get; set; }
        public string GhiChu { get; set; }
    }

}
