using System.ComponentModel.DataAnnotations;

namespace DATN_CDCWebClient.Models
{
    public class Province
    {
        [Key]
        public string code { get; set; } // Mã tỉnh

        public string name { get; set; } // Tên tỉnh

        public string name_en { get; set; } // Tên tỉnh (tiếng Anh)

        public string full_name { get; set; } // Tên đầy đủ tỉnh

        public string full_name_en { get; set; } // Tên đầy đủ tỉnh (tiếng Anh)

        public string code_name { get; set; } // Mã tên tỉnh

        public int? administrative_unit_id { get; set; } // ID đơn vị hành chính

        public ICollection<District> Districts { get; set; } // Danh sách quận/huyện
    }

}
