using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    [SessionActionFilter]
    public class QLHoaDonController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;

        public QLHoaDonController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;


        }
        public void GetSession()
        {
            string userId = HttpContext.Session.GetString("ID");
            string Username = HttpContext.Session.GetString("Username");
            string userRole = HttpContext.Session.GetString("Role");

            TempData["ID"] = userId;
            TempData["Username"] = Username;
            TempData["Role"] = userRole;
        }
        public async Task<IActionResult> ClickKhachHang(string IDKH)
        {
            var hd = _httpClientFactory.CreateClient();
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
            var foodResponse = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAllBYIDKh/" + IDKH);

            var apiResponse = await foodResponse.Content.ReadAsStringAsync();
            HoaDonDTO hoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);
            if (hoaDon == null)
            {
                var khachHangResponse = await hd.GetAsync($"https://localhost:7143/api/DataQLHoaDon/GetAllBYIDKh/{IDKH}");
                if (khachHangResponse.IsSuccessStatusCode)
                {
                    var khachHangApiResponse = await khachHangResponse.Content.ReadAsStringAsync();
                    KhachHang khachHang = JsonConvert.DeserializeObject<KhachHang>(khachHangApiResponse);
                    ViewBag.Khachhang = khachHang;
                }
            }
            else
            {
                ViewBag.HoaDons = hoaDon;
            }



              GetSession(); return View("~/Views/Home/QLHoaDon.cshtml");
        }
        public async Task<IActionResult> QLHoaDon()
        {

            var hd = _httpClientFactory.CreateClient();




            var apiResponse = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAllVatTu");
            var Responses = await apiResponse.Content.ReadAsStringAsync();

            List<VatTuYTe> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuYTe>>(Responses);

            ViewBag.vatTuYTe = vatTuYTe;

            var response = await hd.GetAsync("https://localhost:7143/api/QLTiepNhan/GetAllKhachHang");

            if (response.IsSuccessStatusCode)
            {
                var khachhangapiResponse = await response.Content.ReadAsStringAsync();
                List<KhachHang> khachHangs = JsonConvert.DeserializeObject<List<KhachHang>>(khachhangapiResponse);
                ViewBag.KhachHangs = khachHangs;
            }


              GetSession(); return View("~/Views/Home/QLHoaDon.cshtml");




        }


        [HttpPost]
        public async Task<IActionResult> AddHoaDon(string MaKH, string MaNV, string MaHD, string NoiDung, Double TongTien, DateTime ThoiGian,
            bool TrangThai, string GhiChu)
        { 
            if (!ModelState.IsValid)
            {
                  GetSession(); return BadRequest(ModelState);  // Return error if data is invalid
            }

            try
            {
                // Create HTTP client
                var hd = _httpClientFactory.CreateClient();
                var response1 = await hd.GetAsync("https://localhost:7143/api/QLTiepNhan/GetAllKhachHang");

                if (response1.IsSuccessStatusCode)
                {
                    var khachhangapiResponse = await response1.Content.ReadAsStringAsync();
                    List<KhachHang> khachHangs = JsonConvert.DeserializeObject<List<KhachHang>>(khachhangapiResponse);
                   KhachHang khachHang = khachHangs.FirstOrDefault(kh=>kh.IDKH ==MaKH);
                    if (khachHang!= null)
                    {
                          GetSession(); return BadRequest("Không tìm thấy khách hàng"); 
                    }
                }

               
                // Map input parameters to the DTO
                var hoadonCreateDTO = new HoaDonCreateDTO
                {
                    IDKH = MaKH,
                    IDHD = MaHD,
                    IDNV = MaNV,
                    NoiDung = NoiDung,
                    ThoiGian = ThoiGian,
                    TongTien = TongTien,
                    TrangThai = TrangThai,
                    GhiChu = GhiChu,
   
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
                    var allResponse = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAll");
                    if (allResponse.IsSuccessStatusCode)
                    {
                        var allApiResponse = await allResponse.Content.ReadAsStringAsync();
                        List<HoaDonDTO> hoaDons = JsonConvert.DeserializeObject<List<HoaDonDTO>>(allApiResponse);

                          GetSession(); return RedirectToAction("QLHoaDon");
                    }
                    else
                    {
                          GetSession(); return StatusCode((int)allResponse.StatusCode, "Không thể lấy danh sách khách hàng.");
                    }
                }
                else
                {
                      GetSession(); return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm khách hàng.");
                }
            }
            catch (Exception ex)
            {
                  GetSession(); return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateHoaDon(string MaHD, string MaKH, string MaNV, string NoiDung, double TongTien, DateTime ThoiGian,
                     bool TrangThai, string GhiChu, List<HoaDonChiTiet> hoaDonChiTiets)
        {
            if (!ModelState.IsValid)
            {
                  GetSession(); return BadRequest(ModelState); // Dữ liệu không hợp lệ
            }

            try
            {
                // Tạo đối tượng HttpClient
                var hd = _httpClientFactory.CreateClient();

                // Map dữ liệu từ input vào DTO
                var hoadonUpdateDTO = new HoaDonDTO
                {
                    IDHD = MaHD,
                    IDKH = MaKH,
                    IDNV = MaNV,
                    NoiDung = NoiDung,
                    ThoiGian = ThoiGian,
                    TongTien = TongTien,
                    TrangThai = TrangThai,
                    GhiChu = GhiChu,
                    HoaDonChiTiets = hoaDonChiTiets
                };

                // Serialize đối tượng DTO thành JSON
                var content = new StringContent(JsonConvert.SerializeObject(hoadonUpdateDTO), Encoding.UTF8, "application/json");

                // Gửi yêu cầu PUT đến API
                var response = await hd.PutAsync($"https://localhost:7143/api/DataQLHoaDon/UpdateHoaDon/{MaHD}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Nếu thành công, có thể lấy lại danh sách hóa đơn để cập nhật giao diện
                      GetSession(); return RedirectToAction("QLHoaDon"); // Chuyển hướng về danh sách hóa đơn
                }
                else
                {
                      GetSession(); return StatusCode((int)response.StatusCode, "Không thể cập nhật hóa đơn.");
                }
            }
            catch (Exception ex)
            {
                  GetSession(); return StatusCode(500, $"Có lỗi xảy ra khi kết nối với máy chủ: {ex.Message}");
            }
        }
    }


}

