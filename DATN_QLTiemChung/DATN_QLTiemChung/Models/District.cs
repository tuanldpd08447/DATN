﻿using System.ComponentModel.DataAnnotations;

namespace DATN_QLTiemChung.Models
{
    public class District
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string full_name { get; set; }
        public string full_name_en { get; set; }
        public string code_name { get; set; }
        public string province_code { get; set; }
        public int? administrative_unit_id { get; set; }
        public Province Province { get; set; }
        public ICollection<Ward> Wards { get; set; }
    }

}