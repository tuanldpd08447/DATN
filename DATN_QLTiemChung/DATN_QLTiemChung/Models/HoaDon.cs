using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung.Models
{
    public class HoaDon
    {
        [Key]
        public string IDHD { get; set; }

        [Required]
        public string IDKH { get; set; }
        [ForeignKey(nameof(IDKH))]
        public KhachHang KhachHang { get; set; }

        [Required]
        public string IDNV { get; set; }
        [ForeignKey(nameof(IDNV))]
        public NhanVien NhanVien { get; set; }

        public DateTime ThoiGian { get; set; }
        public string NoiDung { get; set; }
        public Double TongTien { get; set; }
        public bool TrangThai { get; set; }
        public string? GhiChu { get; set; }

        public ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; } = new List<HoaDonChiTiet>();
    }





}
