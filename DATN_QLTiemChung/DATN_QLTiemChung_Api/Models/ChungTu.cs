using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class ChungTu
    {
        [Key]
        public string IDXCT { get; set; }
        public string IDNV { get; set; }
        public string IDOD { get; set; }
        public bool LoaiChungTu { get; set; }
        public DateTime ThoiGian { get; set; }
        public float DonGia { get; set; }
        public float ThanhTien { get; set; }
        public bool TrangThai { get; set; }
        public string GhiChu { get; set; }
    }

}
