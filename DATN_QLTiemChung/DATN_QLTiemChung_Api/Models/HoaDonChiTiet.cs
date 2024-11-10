using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class HoaDonChiTiet
    {
        [Key]
        public string IDHDCT { get; set; }
        public string IDHD { get; set; }
        public string IDVT { get; set; }
        public int SoLuong { get; set; }
        public float DonGia { get; set; }
        public float ThanhTien { get; set; }
        public string GhiChu { get; set; }
    }

}
