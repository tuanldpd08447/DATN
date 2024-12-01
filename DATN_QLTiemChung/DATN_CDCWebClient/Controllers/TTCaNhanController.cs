using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DATN_CDCWebClient.Models;
using System.Text;


namespace DATN_CDCWebClient.Controllers
{
    public class TTCaNhanController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        string id ;
        public TTCaNhanController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
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
            id = userId;
        }
        // GET: Load user information
        [HttpGet]
        public async Task<IActionResult> ThongTinCaNhan(string IDKH)
        {
            GetSession();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7143/"); // API base address

                try
                {
                    var response = await client.GetAsync($"/api/TTCaNhan/GetKhachHang?IDKH={IDKH}");
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var khachHangs = JsonConvert.DeserializeObject<KhachHangDTo2>(apiResponse);

                        if (khachHangs != null)
                        {
                            ViewBag.KhachHangs = khachHangs; // Pass data to the View
                            return View("~/Views/Home/ThongTinCaNhan.cshtml");
                        }
                        else
                        {
                            ViewBag.Error = "Dữ liệu khách hàng không hợp lệ.";
                            return View("~/Views/Home/Home.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Không thể lấy thông tin khách hàng.";
                        return View("~/Views/Home/Home.cshtml");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Đã xảy ra lỗi: {ex.Message}";
                    return View("~/Views/Home/Home.cshtml");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> SuaThongTin(string IDKH, string hoTen, DateTime ngaySinh, string cmnd, string soDienThoai, string hoKhauXa, string diaChi, string email, string gioiTinh, string danToc)
        {
            Console.WriteLine($"IDKH: {IDKH}, HoTen: {hoTen}, NgaySinh: {ngaySinh.ToString("yyyy-MM-dd")}, CMND: {cmnd}, SoDienThoai: {soDienThoai}, HoKhauXa: {hoKhauXa}, DiaChi: {diaChi}, Email: {email}, GioiTinh: {gioiTinh}, DanToc: {danToc}");

            KhachHangUpdateDTO khachHang = new KhachHangUpdateDTO
            {
               IDKH = IDKH,
               IDXP = hoKhauXa ,
               TenKhachHang = hoTen ,
               NgaySinh = ngaySinh,
                GioiTinh =  gioiTinh ,
               DiaChi  = diaChi,
               SoDienThoai = soDienThoai,
                Email = email ,
               CCCD_MDD = cmnd ,
               DanToc = danToc ,
            };

            if (khachHang == null || string.IsNullOrEmpty(khachHang.IDKH) || string.IsNullOrEmpty(khachHang.TenKhachHang))
            {
                return BadRequest("Invalid customer data.");
            }
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(khachHang), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("https://localhost:7143/api/TTCaNhan/UpdateKhachHang", content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {error}");
                GetSession();
                return RedirectToAction("ThongTinCaNhan", new { IDKH = khachHang.IDKH });
            }

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                GetSession();
                return RedirectToAction("Home","Home");
            }
            else
            {

                var error = await response.Content.ReadAsStringAsync();
                GetSession();
                return RedirectToAction("ThongTinCaNhan", new { IDKH = khachHang.IDKH });
            }

        }

    }

}




