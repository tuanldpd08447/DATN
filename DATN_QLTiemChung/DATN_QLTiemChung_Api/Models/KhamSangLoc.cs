using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class KhamSangLoc
    {
        [Key]
        public string IDKS { get; set; }
        public string IDKH { get; set; }
        public float ChieuCao { get; set; }
        public float CanNang { get; set; }
        public DateTime ThoiGian { get; set; }
        public string IDNV { get; set; }
        public string TinhTrangSucKhoe { get; set; }
        public bool TrangThai { get; set; }
        public bool KetQua { get; set; }
        public string? GhiChu { get; set; }
    }

}
