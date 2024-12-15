using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Components.Web;
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
        public async Task<List<HangCho>> GetHangCho()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://65b86c3a46324d531d562e3d.mockapi.io/HangCho");

                if (!response.IsSuccessStatusCode)
                {
                   GetSession();  return new List<HangCho>(); // Trả về danh sách rỗng nếu không thành công
                }

                var khachhangapiResponse = await response.Content.ReadAsStringAsync();
                var hangChoList = JsonConvert.DeserializeObject<List<HangCho>>(khachhangapiResponse)
                                   ?.Where(hc => hc.Step == "KhamSanLoc")
                                   .ToList();

               GetSession(); 
                
                return hangChoList ?? new List<HangCho>(); // Trả về danh sách rỗng nếu dữ liệu null
            }
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


            if (!foodResponse.IsSuccessStatusCode)
            {

                var khachHangResponse = await hd.GetAsync($"https://localhost:7143/api/DataQLHoaDon/GetKHBYID/{IDKH}");
                if (khachHangResponse.IsSuccessStatusCode)
                {
                    var khachHangApiResponse = await khachHangResponse.Content.ReadAsStringAsync();
                    KHDTO khachHang = JsonConvert.DeserializeObject<KHDTO>(khachHangApiResponse);
                    ViewBag.Khachhang = khachHang;
                    Console.WriteLine(khachHang.IDXP);

                    var response0 = await hd.GetAsync($"https://localhost:7143/api/Data/GetWardByid/{khachHang.IDXP}");
                    var apiResponse0 = await response0.Content.ReadAsStringAsync();
                    DiaChi dc = JsonConvert.DeserializeObject<DiaChi>(apiResponse0);
                    ViewBag.DiaChi = dc;
                }

            }
            else
            {
                var apiResponse = await foodResponse.Content.ReadAsStringAsync();
                HoaDonDTO hoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);
                ViewBag.HoaDons = hoaDon;
                var response0 = await hd.GetAsync($"https://localhost:7143/api/Data/GetWardByid/{hoaDon.KhachHang.IDXP}");
                var apiResponse0 = await response0.Content.ReadAsStringAsync();
                DiaChi dc = JsonConvert.DeserializeObject<DiaChi>(apiResponse0);
                ViewBag.DiaChi = dc;
            }

            ViewBag.HangCho = await GetHangCho();

           GetSession();
            TempData["Notification"] = "Lấy hàng chờ thành công.";
            TempData["NotificationType"] = "success";
            TempData["NotificationTitle"] = "Thông báo.";
            return View("~/Views/Home/QLHoaDon.cshtml");
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

            ViewBag.HangCho = await GetHangCho();

           GetSession(); 

           return View("~/Views/Home/QLHoaDon.cshtml");


        }


        [HttpPost]
        public async Task<IActionResult> AddHoaDon(string MaKH, string MaNV, string MaVT, DateTime ThoiGian,
             string NoiDung, int SoLuong, double DonGia, double ThanhTien, bool ThanhToan, string GhiChu, bool TrangThai)
        {
            if (!ModelState.IsValid)
            {
               GetSession();  return BadRequest(ModelState);  
            }
            try
            {
    
                var client = _httpClientFactory.CreateClient();

                var hoadonCreateDTO = new HoaDonCreateDTO
                {
                    IDHD = "",
                    IDHDCT = "",
                    IDKH = MaKH,
                    IDNV = MaNV,
                    IDVT = MaVT,
                    NoiDung = NoiDung,
                    ThoiGian = ThoiGian,
                    SoLuong = SoLuong,
                    DonGia = DonGia,
                    ThanhTien = ThanhTien,
                    TongTien = DonGia * SoLuong,
                    GhiChu = GhiChu,
                    ThanhToan= ThanhToan,
                    TrangThai = TrangThai
                    
                };

                var content = new StringContent(JsonConvert.SerializeObject(hoadonCreateDTO), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7143/api/DataQLHoaDon/AddHoaDon", content);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var addedHoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);
                   GetSession(); 
                    
                    return RedirectToAction("QLHoaDon");

                }
                else
                {
                   GetSession();  return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm khách hàng.");
                }
            }
            catch (Exception ex)
            {
               GetSession();  return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }
        public async Task<IActionResult> CancelHoaDon(string TrangThai, string MaHD)
        {
            if (string.IsNullOrEmpty(TrangThai) || string.IsNullOrEmpty(MaHD))
            {
               GetSession();  return BadRequest("TrangThai và MaHD không được để trống.");
            }

            try
            {
                bool tt = TrangThai.Equals("true", StringComparison.OrdinalIgnoreCase);

                var client = _httpClientFactory.CreateClient();

                var content = new StringContent(JsonConvert.SerializeObject(tt), Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"https://localhost:7143/api/DataQLHoaDon/CancelHoaDon/{MaHD}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Cập nhật trạng thái hóa đơn thành công.";
                   GetSession();
                    TempData["Notification"] = "Cập nhật trạng thái hóa đơn thành công.";
                    TempData["NotificationType"] = "success";
                    TempData["NotificationTitle"] = "Thông báo.";
                    return RedirectToAction("QLHoaDon");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                   GetSession();  return StatusCode((int)response.StatusCode, $"Không thể cập nhật trạng thái hóa đơn: {errorContent}");
                }
            }
            catch (Exception ex)
            {
               GetSession();  return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }
        public async Task<IActionResult> UpdateHoaDon(bool ThanhToan, string MaHD, string MaKH)
        {
            if ( string.IsNullOrEmpty(MaHD) || string.IsNullOrEmpty(MaKH))
            {
               GetSession();  return BadRequest("ThanhToan, MaHD và MaKH không được để trống.");
            }
            try
            {
                using var client = _httpClientFactory.CreateClient();

                var content = new StringContent(JsonConvert.SerializeObject(ThanhToan), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:7143/api/DataQLHoaDon/UpdateHoaDon/{MaHD}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                   GetSession();  return StatusCode((int)response.StatusCode, $"Không thể cập nhật trạng thái hóa đơn: {errorContent}");
                }

                if(ThanhToan)
                {
                    var response1 = await client.GetAsync("https://65b86c3a46324d531d562e3d.mockapi.io/HangCho");
                    if (!response1.IsSuccessStatusCode)
                    {
                        var errorResponse = await response1.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"Không thể lấy dữ liệu từ API HangCho: {errorResponse}");
                        GetSession(); TempData["Notification"] = "Không thể lấy dữ liệu từ Hàng chờ.";
                        TempData["NotificationType"] = "error";
                        TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("QLHoaDon");
                    }

                    var khachhangapiResponse = await response1.Content.ReadAsStringAsync();
                    var hangChoList = JsonConvert.DeserializeObject<List<HangCho>>(khachhangapiResponse);

                    var hangCho = hangChoList.FirstOrDefault(hc => hc.IDKH == MaKH);
                    if (hangCho == null)
                    {
                        ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng trong API HangCho.");
                        GetSession(); return BadRequest("Không tìm thấy thông tin khách hàng trong API HangCho.");
                    }

                    hangCho.Step = "ThanhToan";
                    var content1 = new StringContent(JsonConvert.SerializeObject(hangCho), Encoding.UTF8, "application/json");
                    var response2 = await client.PutAsync($"https://65b86c3a46324d531d562e3d.mockapi.io/HangCho/{hangCho.ID}", content1);

                    if (!response2.IsSuccessStatusCode)
                    {
                        var errorContent = await response2.Content.ReadAsStringAsync();
                        GetSession(); return StatusCode((int)response2.StatusCode, $"Không thể cập nhật thông tin HangCho: {errorContent}");
                    }
                }

                TempData["Message"] = "Thanh Toán hóa đơn thành công.";
               GetSession(); TempData["Notification"] = "Thanh toán hóa đơn thành công.";
                TempData["NotificationType"] = "success";
                TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("QLHoaDon");
            }
            catch (Exception ex)
            {
               GetSession();  return StatusCode(500, $"Đã xảy ra lỗi khi kết nối với API hoặc xử lý dữ liệu: {ex.Message}");
            }
        }



    }


}

