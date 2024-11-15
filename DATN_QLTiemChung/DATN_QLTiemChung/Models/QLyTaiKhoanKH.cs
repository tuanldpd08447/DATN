using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung.Models
{
    public class QLyTaiKhoanKH
    {
        [Key]
        public string IDTKKH { get; set; } // ID tài khoản khách hàng

        [Required]
        public string IDKH { get; set; } // ID khách hàng
        [ForeignKey(nameof(IDKH))]
        public KhachHang KhachHang { get; set; }

        public string SDT { get; set; }
        public string MatKhau { get; set; }
    }

}
