using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class KhamSangLoc
    {
        [Key]
        public string IDKS { get; set; }
        public string IDKH { get; set; }
        [ForeignKey(nameof(IDKH))]
        public KhachHang? KhachHang { get; set; }
        public Double ChieuCao { get; set; }
        public Double CanNang { get; set; }
        public DateTime ThoiGian { get; set; }
        public string IDNV { get; set; }
        [ForeignKey(nameof(IDNV))]
        public NhanVien? NhanVien { get; set; }
        public string TinhTrangSucKhoe { get; set; }
        public bool TrangThai { get; set; }
        public bool KetQua { get; set; }
        public string? GhiChu { get; set; }
    }

}
