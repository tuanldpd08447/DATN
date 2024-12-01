using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung.Models
{
    public class ChungTu
    {
        [Key]
        public string IDXCT { get; set; } // ID chứng từ

        [Required]
        public string IDNV { get; set; } // ID nhân viên
        [ForeignKey(nameof(IDNV))]
        public NhanVien? NhanVien { get; set; } // Quan hệ với bảng NhanVien

        [Required]
        public string IDOD { get; set; } // ID đơn hàng
        [ForeignKey(nameof(IDOD))]
        public Order? Order { get; set; } // Quan hệ với bảng Order (Đơn hàng)

        public bool LoaiChungTu { get; set; } // Loại chứng từ
        public DateTime ThoiGian { get; set; } // Thời gian
        public double DonGia { get; set; } // Đơn giá
        public double ThanhTien { get; set; } // Thành tiền
        public bool TrangThai { get; set; } // Trạng thái
        public string GhiChu { get; set; } // Ghi chú
    }

}
