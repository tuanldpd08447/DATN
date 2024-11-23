using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    public class QLHoaDonController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        
        public QLHoaDonController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
           
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;


        }
        public async Task<IActionResult> QLHoaDon()
        {

            var hd = _httpClientFactory.CreateClient();
            var foodResponse = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAll/" + "HD002");
            if (foodResponse.IsSuccessStatusCode)
            {
                var apiResponse = await foodResponse.Content.ReadAsStringAsync();
                HoaDonDTO hoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);
                var Response = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAllVatTu");
                var Responses = await Response.Content.ReadAsStringAsync();
                List<VatTuYTe> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuYTe>>(Responses);
                ViewBag.vatTuYTe = vatTuYTe;
                
                var response = await hd.GetAsync("https://localhost:7143/api/QLTiepNhan/GetAllKhachHang");

                if (response.IsSuccessStatusCode)
                {
                    var khachhangapiResponse = await response.Content.ReadAsStringAsync();
                    List<KhachHang> khachHangs = JsonConvert.DeserializeObject<List<KhachHang>>(khachhangapiResponse);
                    ViewBag.KhachHangs = khachHangs;
                }
                return View("~/Views/Home/QLHoaDon.cshtml", hoaDon);

            }
           

            return View("~/Views/Home/HomeQl.cshtml");
        }
       

        [HttpPost]
        public async Task<IActionResult> AddHoaDon(string MaKH, string MaNV, string MaHD, string NoiDung, Double TongTien, DateTime ThoiGian,
            bool TrangThai, string GhiChu,  List<HoaDonChiTiet> hoaDonChiTiets)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return error if data is invalid
            }

            try
            {
                // Create HTTP client
                var hd = _httpClientFactory.CreateClient();

                // Map input parameters to the DTO
                var hoadonCreateDTO = new HoaDonDTO
                {
                    IDKH = MaKH,
                    IDHD = MaHD,
                    IDNV = MaNV,
                    NoiDung = NoiDung,
                    ThoiGian = ThoiGian,
                    TongTien = TongTien,
                    TrangThai = TrangThai,
                    GhiChu = GhiChu,
                    HoaDonChiTiets = hoaDonChiTiets
                };

                // Serialize the object to JSON
                var content = new StringContent(JsonConvert.SerializeObject(hoadonCreateDTO), Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await hd.PostAsync("https://localhost:7143/api/DataQLHoaDon/AddHoaDon", content);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var addedHoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);

                    // Get all customers after adding
                    var allResponse = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAll{IDHD}");
                    if (allResponse.IsSuccessStatusCode)
                    {
                        var allApiResponse = await allResponse.Content.ReadAsStringAsync();
                        List<HoaDonDTO> hoaDons = JsonConvert.DeserializeObject<List<HoaDonDTO>>(allApiResponse);

                        return View("~/Views/Home/QLHoaDon.cshtml");
                    }
                    else
                    {
                        return StatusCode((int)allResponse.StatusCode, "Không thể lấy danh sách khách hàng.");
                    }
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm khách hàng.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }

        
    }
}
