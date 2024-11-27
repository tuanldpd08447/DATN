using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_CDCWebClient.Models
{
    public class NhanVien
    {
        [Key]
        public string IDNV { get; set; }
        public string TenNhanVien { get; set; }
        public string ChucVu { get; set; }

        [Required]
        public string? IDCD { get; set; }


        [ForeignKey("IDCD")]
        public ChucDanh? ChucDanh { get; set; }

        [Required]
        public string? IDPB { get; set; }

        [ForeignKey("IDPB")]
        public PhongBan? PhongBan { get; set; }

        [Required]
        public string IDXP { get; set; }

        // Khóa ngoại cho Ward
        [ForeignKey(nameof(IDXP))]
        public Ward Ward { get; set; }

        public string CCCD { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string GioiTinh { get; set; }
        public string DanToc { get; set; }

        public ICollection<DangKyTiemChung> DangKyTiemChungs { get; set; }
        public ICollection<HoaDon>? HoaDons { get; set; } = new List<HoaDon>();
    }





}
