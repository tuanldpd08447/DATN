using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class QLyTaiKhoanNV
    {
        [Key]
        public string IDTKNV { get; set; } // ID tài khoản nhân viên

        [Required]
        public string IDNV { get; set; } // ID nhân viên
        [ForeignKey(nameof(IDNV))]
        public NhanVien? NhanVien { get; set; }

        public string Email { get; set; }
        public string MatKhau { get; set; }
    }

}
