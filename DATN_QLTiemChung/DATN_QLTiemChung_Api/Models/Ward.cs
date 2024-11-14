using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung_Api.Models
{
    public class Ward
{
        [Key]
        public string code { get; set; } // Mã phường/xã
    public string name { get; set; } // Tên phường/xã
    public string name_en { get; set; } // Tên phường/xã (tiếng Anh)
    public string full_name { get; set; } // Tên đầy đủ phường/xã
    public string full_name_en { get; set; } // Tên đầy đủ phường/xã (tiếng Anh)
    public string code_name { get; set; } // Mã tên
    public string district_code { get; set; } // Mã quận/huyện
    public int? administrative_unit_id { get; set; } // ID đơn vị hành chính
}
}
