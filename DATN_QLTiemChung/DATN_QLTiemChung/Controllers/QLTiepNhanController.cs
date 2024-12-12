using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DATN_QLTiemChung.Models;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    [SessionActionFilter]
    public class QLTiepNhanController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public  QLTiepNhanController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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

              GetSession(); return View("~/Views/Home/QLTiepNhan.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> AddHangCho(string IDKH)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                // Kiểm tra trùng lặp trong HangCho
                var response1 = await client.GetAsync($"https://65b86c3a46324d531d562e3d.mockapi.io/HangCho?IDKH={IDKH}&NgayCho={DateOnly.FromDateTime(DateTime.Now)}");
                if (response1.IsSuccessStatusCode)
                {
                    var hangChoList = JsonConvert.DeserializeObject<List<HangCho>>(await response1.Content.ReadAsStringAsync());
                    if (hangChoList.Any())
                    {
                        GetSession();
                        return BadRequest("Khách hàng đã có trong hàng chờ hôm nay.");
                    }
                }

                // Lấy thông tin khách hàng từ API
                var response2 = await client.GetAsync($"https://localhost:7143/api/QLTiepNhan/GetAllKhachHangByIDKH/{IDKH}");
                if (!response2.IsSuccessStatusCode)
                {
                    GetSession();
                    return BadRequest("Không thể lấy thông tin khách hàng.");
                }

                var kh = JsonConvert.DeserializeObject<KhachHangDTo>(await response2.Content.ReadAsStringAsync());

                // Tạo đối tượng HangCho
                var hc = new HangCho
                {
                    ID = null,
                    IDKH = IDKH,
                    HoTen = kh.TenKhachHang,
                    NgaySinh = DateOnly.FromDateTime(kh.NgaySinh),
                    NgayCho = DateOnly.FromDateTime(DateTime.Now),
                    Step = "TiepNhan"
                };

                // Gửi yêu cầu POST thêm vào HangCho
                var content = new StringContent(JsonConvert.SerializeObject(hc), Encoding.UTF8, "application/json");
                var response3 = await client.PostAsync("https://65b86c3a46324d531d562e3d.mockapi.io/HangCho", content);
                if (!response3.IsSuccessStatusCode)
                {
                    GetSession();
                    return StatusCode((int)response3.StatusCode, "Không thể thêm khách hàng vào hàng chờ.");
                }

                GetSession();
                return RedirectToAction("QLTiepNhan");
            }
            catch (Exception ex)
            {
                GetSession();
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }




        [HttpPost]
        public async Task<IActionResult> AddKhachHang(string hoKhauXa, string hoTen, DateTime ngaySinh, string danToc, string gioiTinh, string cmt, string sdt, string email, string diaChiChiTiet)
        {
            if (!ModelState.IsValid)
            {
                  GetSession(); return BadRequest(ModelState);  //   GetSession(); return error if data is invalid
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

                          GetSession(); return View("~/Views/Home/QLTiepNhan.cshtml");
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


    }
}
