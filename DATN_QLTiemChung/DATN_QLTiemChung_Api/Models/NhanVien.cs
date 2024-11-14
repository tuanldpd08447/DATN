using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class NhanVien
    {
        [Key]
        public string IDNV { get; set; }
        public string TenNhanVien { get; set; }
        public string ChucVu { get; set; }
        public string IDCD { get; set; }
        public string IDPB { get; set; }
        public string CCCD { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string IDXP { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string GioiTinh { get; set; }
        public string DanToc { get; set; }
    }


}
