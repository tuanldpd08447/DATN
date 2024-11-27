using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_CDCWebClient.Models
{
    public class Ward
    {
        [Key]
        public string code { get; set; } // Mã phường/xã

        public string name { get; set; } // Tên phường/xã

        public string name_en { get; set; } // Tên phường/xã (tiếng Anh)

        public string full_name { get; set; } // Tên đầy đủ phường/xã

        public string full_name_en { get; set; } // Tên đầy đủ phường/xã (tiếng Anh)

        public string code_name { get; set; } // Mã tên phường/xã

        public string district_code { get; set; } // Mã quận/huyện (Khóa ngoại)

        [ForeignKey(nameof(district_code))]
        public District District { get; set; } // Quan hệ với bảng District

        public int? administrative_unit_id { get; set; } // ID đơn vị hành chính
    }
}
