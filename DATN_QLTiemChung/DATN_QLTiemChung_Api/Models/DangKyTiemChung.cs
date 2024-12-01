using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class DangKyTiemChung
    {
        [Key]
        public string IDDK { get; set; }

        [Required]
        public string IDKH { get; set; }
        [ForeignKey(nameof(IDKH))]
        public KhachHang? KhachHang { get; set; }

        [Required]
        public string IDNV { get; set; }
        [ForeignKey(nameof(IDNV))]
        public NhanVien? NhanVien { get; set; }

        [Required]
        public string IDDKVC { get; set; }
        [ForeignKey(nameof(IDDKVC))]
        public DangKyVaccine? DangKyVaccine { get; set; }

        public DateTime ThoiGianDK { get; set; }
        public DateTime ThoiGianTiem { get; set; }
        public string? GhiChu { get; set; }
    }


}
