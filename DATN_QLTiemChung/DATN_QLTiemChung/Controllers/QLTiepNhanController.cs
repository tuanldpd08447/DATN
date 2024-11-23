using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DATN_QLTiemChung.Models;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    public class QLTiepNhanController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public  QLTiepNhanController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;


        }
        public async Task<IActionResult> QLTiepNhan()
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                // Lấy danh sách khách hàng
                var response = await client.GetAsync("https://localhost:7143/api/QLTiepNhan/GetAllKhachHang");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    List<KhachHangDTo> khachHangs = JsonConvert.DeserializeObject<List<KhachHangDTo>>(apiResponse);
                    ViewBag.KhachHangs = khachHangs;
                }
                else
                {
                    // Xử lý khi API không trả về thành công
                    ViewBag.ErrorMessage = "Không thể tải danh sách khách hàng.";
                }

                // Lấy danh sách khách hàng đặt lịch
                var response1 = await client.GetAsync("https://localhost:7143/api/QLTiepNhan/GetAllDatLichKhams");
                if (response1.IsSuccessStatusCode)
                {
                    var apiResponse1 = await response1.Content.ReadAsStringAsync();
                    List<KhachHangPreOder> khachHangPreoder = JsonConvert.DeserializeObject<List<KhachHangPreOder>>(apiResponse1);
                    ViewBag.KhachHangPreorder = khachHangPreoder;
                }
                else
                {
                    // Xử lý khi API không trả về thành công
                    ViewBag.ErrorMessage = "Không thể tải danh sách khách hàng đặt lịch.";
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi kết nối hoặc lỗi khác
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }

            return View("~/Views/Home/QLTiepNhan.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> AddKhachHang(string hoKhauXa, string hoTen, DateTime ngaySinh, string danToc, string gioiTinh, string cmt, string sdt, string email, string diaChiChiTiet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return error if data is invalid
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
                var response = await client.PostAsync("https://localhost:7143/api/QLTiepNhan/AddKhachHang", content);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var addedKhachHang = JsonConvert.DeserializeObject<KhachHangDTo>(apiResponse);

                    // Get all customers after adding
                    var allResponse = await client.GetAsync("https://localhost:7143/api/QLTiepNhan/GetAllKhachHang");
                    if (allResponse.IsSuccessStatusCode)
                    {
                        var allApiResponse = await allResponse.Content.ReadAsStringAsync();
                        List<KhachHangDTo> khachHangs = JsonConvert.DeserializeObject<List<KhachHangDTo>>(allApiResponse);

                        return View("~/Views/Home/QLTiepNhan.cshtml");
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
