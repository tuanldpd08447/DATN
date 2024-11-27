using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_CDCWebClient.Models
{
    public class KhachHang
    {
        [Key]
        public string IDKH { get; set; }

        [Required]
        public string IDXP { get; set; }
        [ForeignKey(nameof(IDXP))]
        public Ward Ward { get; set; }

        public string TenKhachHang { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string CCCD_MDD { get; set; }
        public string DanToc { get; set; }

       

        public ICollection<DangKyTiemChung> DangKyTiemChungs { get; set; }
        public ICollection<HoaDon>? HoaDons { get; set; } = new List<HoaDon>();
        public ICollection<DatLichKham>? DatLichKhams { get; set; }
    }




}
