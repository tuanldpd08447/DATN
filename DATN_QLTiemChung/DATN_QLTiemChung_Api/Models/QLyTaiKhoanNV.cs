using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class QLyTaiKhoanNV
    {
        [Key]
        public string IDTKNV { get; set; } // ID tài khoản nhân viên
        public string IDNV { get; set; } // ID nhân viên
        public string Email { get; set; } // Email
        public string MatKhau { get; set; } // Mật khẩu
    }
}
