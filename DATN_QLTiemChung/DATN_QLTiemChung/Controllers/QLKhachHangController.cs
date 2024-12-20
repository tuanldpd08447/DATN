﻿using Microsoft.AspNetCore.Mvc;
using DATN_QLTiemChung.Models;
using Newtonsoft.Json;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    public class QLKhachHangController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;

        public QLKhachHangController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpClientFactory = httpClientFactory;
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
        public async Task<IActionResult> QLKhachHang()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLKhachHang/GetAllKhachHang");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                List<KhachHangDTo> khachHangs = JsonConvert.DeserializeObject<List<KhachHangDTo>>(apiResponse);
                ViewBag.KhachHangs = khachHangs;
            }
           GetSession(); return View("~/Views/Home/QLKhachHang.cshtml");
        }

		public async Task<IActionResult> FindKhachHang(string? FindCode, string? FindName ,string? FindCCCD , string? FindSDT)
		{
			if (FindCode == null) { FindCode = null; }
			if (FindName == null) { FindName = null; }
			if (FindCCCD == null) { FindCCCD = null; }
			if (FindSDT == null) { FindSDT = null; }

			var find = new FindKhachHang
            {
                IDKH = FindCode,
                TenKhachHang = FindName,
                CCCD_MDD = FindCCCD,
                SoDienThoai = FindSDT
			};
			var client = _httpClientFactory.CreateClient();
			var content = new StringContent(JsonConvert.SerializeObject(find), Encoding.UTF8, "application/json");

			// Send the POST request
			var response = await client.PostAsync("http://qltiemchungapi.runasp.net/api/QLKhachHang/GetAllKhachHangByFind", content);
			if (response.IsSuccessStatusCode)
			{
				var apiResponse = await response.Content.ReadAsStringAsync();
				List<KhachHangDTo> khachHangs = JsonConvert.DeserializeObject<List<KhachHangDTo>>(apiResponse);
				ViewBag.KhachHangs = khachHangs;
			}
			
			return View("~/Views/Home/QLKhachHang.cshtml");
		}



		[HttpPost]
        public async Task<IActionResult> AddKhachHang(string hoKhauXa, string hoTen, DateTime ngaySinh, string danToc, string gioiTinh, string cmt, string sdt, string email, string diaChiChiTiet)
        {
            if (!ModelState.IsValid)
            {
               GetSession(); return BadRequest(ModelState);  //GetSession(); return error if data is invalid
            }

            try
            {
                // Create HTTP client
                var client = _httpClientFactory.CreateClient();

                // Map input parameters to the DTO
                var khachHangCreateDTO = new KhachHangCreateDTO
                {
                    IDXP = hoKhauXa,
                    TenKhachHang = hoTen,
                    NgaySinh = ngaySinh,
                    DanToc = danToc,
                    GioiTinh = gioiTinh,
                    CCCD_MDD = cmt,
                    SoDienThoai = sdt,
                    Email = email,
                    DiaChi = diaChiChiTiet
                };

                // Serialize the object to JSON
                var content = new StringContent(JsonConvert.SerializeObject(khachHangCreateDTO), Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await client.PostAsync("http://qltiemchungapi.runasp.net/api/QLKhachHang/AddKhachHang", content);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var addedKhachHang = JsonConvert.DeserializeObject<KhachHangDTo>(apiResponse);

                    // Get all customers after adding
                    var allResponse = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLKhachHang/GetAllKhachHang");
                    if (allResponse.IsSuccessStatusCode)
                    {
                        var allApiResponse = await allResponse.Content.ReadAsStringAsync();
                        List<KhachHangDTo> khachHangs = JsonConvert.DeserializeObject<List<KhachHangDTo>>(allApiResponse);

                        TempData["Notification"] = "Thêm khách hàng thành công.";
                        TempData["NotificationType"] = "success";
                        TempData["NotificationTitle"] = "Thông báo.";
                        GetSession(); return RedirectToAction("QLKhachHang"); ;
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



        public async Task<IActionResult> ClickKhachHang(string IDKH)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLKhachHang/GetAllKhachHang");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                List<KhachHangDTo> khachHangs = JsonConvert.DeserializeObject<List<KhachHangDTo>>(apiResponse);
                ViewBag.KhachHangs = khachHangs;
            }

            var response1 = await client.GetAsync($"http://qltiemchungapi.runasp.net/api/QLKhachHang/GetKhachHangById/{IDKH}");

            if (response1.IsSuccessStatusCode)
            {
                var apiResponse1 = await response1.Content.ReadAsStringAsync();
                KhachHangDTo2 khachHang = JsonConvert.DeserializeObject<KhachHangDTo2>(apiResponse1);
                ViewBag.KhachHang = khachHang;
            }

            ViewBag.edit = true;
           GetSession(); return View("~/Views/Home/QLKhachHang.cshtml");
        }


        public async Task<IActionResult> UpdateKhachHang(string IDKH, string hoKhauXa, string hoTen, DateTime ngaySinh, string danToc, string gioiTinh, string cmt, string sdt, string email, string diaChiChiTiet)
        {

            KhachHangUpdateDTO khachHang = new KhachHangUpdateDTO
            {
                IDKH = IDKH,
                IDXP = hoKhauXa,
                TenKhachHang = hoTen,
                NgaySinh = ngaySinh,
                GioiTinh = gioiTinh,
                DiaChi = diaChiChiTiet,
                SoDienThoai = sdt,
                Email = email,
                CCCD_MDD = cmt,
                DanToc = danToc,
            };

            if (khachHang == null || string.IsNullOrEmpty(khachHang.IDKH) || string.IsNullOrEmpty(khachHang.TenKhachHang))
            {
               GetSession(); return BadRequest("Invalid customer data.");
            }
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(khachHang), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("http://qltiemchungapi.runasp.net/api/QLKhachHang/UpdateKhachHang", content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {error}");

               GetSession(); return RedirectToAction("ClickKhachHang", new { IDKH = khachHang.IDKH });
            }

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                TempData["Notification"] = "Cập nhật thông tin khách hàng thành công.";
                TempData["NotificationType"] = "success";
                TempData["NotificationTitle"] = "Thông báo.";
                GetSession(); return RedirectToAction("QLKhachHang");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
               GetSession(); return RedirectToAction("ClickKhachHang", new { IDKH = khachHang.IDKH });
            }

        }

    }




}
