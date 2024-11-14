using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class QLyTaiKhoanKH
    {
        [Key]
        public string IDTKKH { get; set; } // ID tài khoản khách hàng
        public string IDKH { get; set; } // ID khách hàng
        public string SDT { get; set; } // Số điện thoại
        public string MatKhau { get; set; } // Mật khẩu
    }
}
