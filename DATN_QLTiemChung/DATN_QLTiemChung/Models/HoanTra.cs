using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung.Models
{
    public class HoanTra
    {
        [Key]
        public string IDHT { get; set; } // ID hoàn trả
        public string IDNV { get; set; } // ID nhân viên
        public string IDKH { get; set; } // ID khách hàng
        public string HoaDonCu { get; set; } // Hóa đơn cũ
        public string HoaDonMoi { get; set; } // Hóa đơn mới
        public string LyDo { get; set; } // Lý do hoàn trả
        public DateTime ThoiGian { get; set; } // Thời gian
        public Double DonGia { get; set; } // Đơn giá
        public Double ThanhTien { get; set; } // Thành tiền
        public string TrangThai { get; set; } // Trạng thái
    }
}
