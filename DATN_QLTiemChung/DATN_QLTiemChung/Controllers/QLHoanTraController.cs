using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using DATN_QLTiemChung.Models;

namespace DATN_QLTiemChung.Controllers
{
    public class QLHoanTraController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public QLHoanTraController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> QLHoanTra()
        {
            var client = _httpClientFactory.CreateClient();
            try
            {

                // Lấy danh sách khách hàng hoàn trả
                var response = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLHoanTra/DSHoanTra");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    List<DSHoanTraDTO> KhachHangHT = JsonConvert.DeserializeObject<List<DSHoanTraDTO>>(apiResponse);
                    // Dùng Newtonsoft.Json
                    ViewBag.DSHoanTraDTO = KhachHangHT;
                    // Lấy danh sách khách hàng hoàn trả
                    var response1 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLHoanTra/GetAllHoanTra");
                    if (response1.IsSuccessStatusCode)
                    {
                        var apiResponse1 = await response1.Content.ReadAsStringAsync();
                        List<HoanTraDTO> HoanTraDTO = JsonConvert.DeserializeObject<List<HoanTraDTO>>(apiResponse1);
                        // Dùng Newtonsoft.Json
                        ViewBag.HoanTraDTO = HoanTraDTO;

                    }
                }
                else
                {
                    // Xử lý khi API không trả về thành công
                    ViewBag.ErrorMessage = "Không thể tải danh sách khách hàng hoàn trả";
                }

            }
            catch (Exception ex)
            {
                // Xử lý lỗi kết nối hoặc lỗi khác
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }


            GetSession(); return View("~/Views/Home/QLHoanTra.cshtml");
        }

        public async Task<IActionResult> HoaDonHT(string IDKH)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLHoanTra/DSHoanTra");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                List<DSHoanTraDTO> KhachHangHT = JsonConvert.DeserializeObject<List<DSHoanTraDTO>>(apiResponse);
                // Dùng Newtonsoft.Json
                ViewBag.DSHoanTraDTO = KhachHangHT;
            }

            var HoaDon = await client.GetAsync($"http://qltiemchungapi.runasp.net/api/QLHoanTra/HoaDonHT/{IDKH}");

            if (HoaDon.IsSuccessStatusCode)
            {
                var apiResponse = await HoaDon.Content.ReadAsStringAsync();
                var hoaDon = JsonConvert.DeserializeObject<HoaDonHT>(apiResponse);
                HoaDonHT hoaDonHT = new HoaDonHT
                {
                    IDHD = hoaDon.IDHD,
                    IDKH = hoaDon.IDKH,
                    IDVT = hoaDon.IDVT,
                    IDNV = hoaDon.IDNV,
                    NoiDung = hoaDon.NoiDung,
                    ThoiGian = hoaDon.ThoiGian,
                    IDHDCT = hoaDon.IDHDCT,
                    TenKhachHang = hoaDon.TenKhachHang,
                    DonGia = hoaDon.DonGia,
                    SoLuong = hoaDon.SoLuong,
                    TongTien = hoaDon.TongTien,
                    ThanhToan = hoaDon.ThanhToan,
                    TrangThai = hoaDon.TrangThai,
                    KhachHang = hoaDon.KhachHang,
                    NhanVien = hoaDon.NhanVien,
                    HoaDonChiTiet = hoaDon.HoaDonChiTiet,
                };
                ViewBag.HoaDonHT = hoaDonHT;
                var response2 = await client.GetAsync($"http://qltiemchungapi.runasp.net/api/QLHoanTra/GetSoMuiDaTiem/{IDKH}");
                if (response2.IsSuccessStatusCode)
                {
                    var apiResponse2 = await response2.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(apiResponse2))
                    {
                        string MuiDaTiem = JsonConvert.DeserializeObject<string>(apiResponse2);
                        if (int.TryParse(MuiDaTiem, out int soMui))
                        {
                            ViewBag.MuiDaTiem = soMui;
                        }
                        else
                        {
                            ViewBag.MuiDaTiem = 0;
                        }
                    }
                }
               
            }
            else
            {
                // Xử lý khi API không trả về thành công
                ViewBag.ErrorMessage = "Không thể tải thông tin khách hàng";
            }
            if (HoaDon == null)
            {
                GetSession(); return NotFound();
            }

            GetSession(); return View("~/Views/Home/QLHoanTra.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewHoaDon(string IDHD, string MaKH, string MaNV, string TenKH, string TenNV, string MaVT, DateTime ThoiGian,
    string NoiDung, int SoLuongHT,int SoLuongtt, double DonGia, bool ThanhToan, string GhiChu, bool TrangThai)
        {
            if (!ModelState.IsValid)
            {
                GetSession();
                return BadRequest(new { Message = "Dữ liệu không hợp lệ.", Errors = ModelState.Values });
            }

            try
            {
                int soluong = SoLuongtt - SoLuongHT;
                // Tạo DTO từ tham số đầu vào
                var hoaDonHT = new HoaDonHT
                {
                    IDHT = "",
                    HoaDonMoi = "",
                    IDHDCT = "",
                    IDHD = IDHD,
                    TenKhachHang = TenKH,
                    IDKH = MaKH,
                    IDNV = MaNV,
                    IDVT = MaVT,
                    NoiDung = NoiDung,
                    ThoiGian = ThoiGian,
                    SoLuong = soluong,
                    DonGia = DonGia,
                    TongTien = DonGia * SoLuongHT,
                    GhiChu = GhiChu,
                    ThanhToan = ThanhToan,
                    TrangThai = TrangThai,
                    HoaDonCu = IDHD,
                };

                // Serialize DTO thành JSON
                var content = new StringContent(JsonConvert.SerializeObject(hoaDonHT), Encoding.UTF8, "application/json");

                // Tạo HTTP client và gửi POST request
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync("http://qltiemchungapi.runasp.net/api/QLHoanTra/AddNewHoaDon", content);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var addedHoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);

                    TempData["Notification"] = "Hoàn trả thành công.";
                    TempData["NotificationType"] = "success";
                    TempData["NotificationTitle"] = "Thông báo.";
                    GetSession();
                    return RedirectToAction("QLHoanTra");
                }
                else
                {
                    // Trích xuất thông tin lỗi từ phản hồi API (nếu có)
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    GetSession();
                    return StatusCode((int)response.StatusCode, new { Message = "Lỗi từ API.", Details = errorDetails });
                }
            }
            catch (Exception ex)
            {
                
                GetSession();
                return StatusCode(500, new { Message = "Có lỗi khi kết nối với máy chủ.", Error = ex.Message });
            }
        }

    }
}

