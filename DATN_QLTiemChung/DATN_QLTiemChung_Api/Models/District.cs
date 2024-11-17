using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Models
{

    public class District
    {
        [Key]
        public string code { get; set; } // Mã quận/huyện

        public string name { get; set; } // Tên quận/huyện

        public string name_en { get; set; } // Tên quận/huyện (tiếng Anh)

        public string full_name { get; set; } // Tên đầy đủ quận/huyện

        public string full_name_en { get; set; } // Tên đầy đủ quận/huyện (tiếng Anh)

        public string code_name { get; set; } // Mã tên quận/huyện

        public string province_code { get; set; } // Mã tỉnh (Khóa ngoại)

        [ForeignKey(nameof(province_code))]
        public Province Province { get; set; } // Quan hệ với bảng Province

        public int? administrative_unit_id { get; set; } // ID đơn vị hành chính

        public ICollection<Ward> Wards { get; set; } // Danh sách phường/xã
    }



}
