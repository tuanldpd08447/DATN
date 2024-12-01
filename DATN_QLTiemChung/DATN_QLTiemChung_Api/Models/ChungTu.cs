using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{
    public class ChungTu
    {
        [Key]
        public string IDXCT { get; set; } 

        [Required]
        public string IDNV { get; set; } 
        [ForeignKey(nameof(IDNV))]
        public NhanVien? NhanVien { get; set; } 

        [Required]
        public string IDOD { get; set; } 
        [ForeignKey(nameof(IDOD))]
        public Order? Order { get; set; } 

        public bool LoaiChungTu { get; set; } 
        public DateTime ThoiGian { get; set; } 
        public double DonGia { get; set; } 
        public double ThanhTien { get; set; }
        public bool TrangThai { get; set; } 
        public string? GhiChu { get; set; } 
        public string? HinhAnh { get; set; } 
    }

}
