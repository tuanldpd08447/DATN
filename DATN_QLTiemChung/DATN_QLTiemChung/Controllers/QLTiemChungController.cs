using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using DATN_QLTiemChung.Models;
using Newtonsoft.Json;
using System.Text;
using System;

namespace DATN_QLTiemChung.Controllers
{
    [SessionActionFilter]
    public class QLTiemChungController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public QLTiemChungController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
        public async Task<List<HangCho>> GetHangCho()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://65b86c3a46324d531d562e3d.mockapi.io/HangCho");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<HangCho>(); // Trả về danh sách rỗng nếu không thành công
                }

                var khachhangapiResponse = await response.Content.ReadAsStringAsync();
                var hangChoList = JsonConvert.DeserializeObject<List<HangCho>>(khachhangapiResponse)
                                   ?.Where(hc => hc.Step == "ThanhToan")
                                   .ToList();

                return hangChoList ?? new List<HangCho>(); // Trả về danh sách rỗng nếu dữ liệu null
            }
        }
        public async Task<IActionResult> QLTiemChung()
            {
               

            ViewBag.HangCho = await GetHangCho();
            GetSession();
        
            return View("~/Views/Home/QLTiemChung.cshtml");
            }

        // Phương thức lấy thông tin kết quả khám sàng lọc của khách hàng từ cơ sở dữ liệu
        public async Task<IActionResult> KQKhamSangLoc(string IDKH)
        {
            var client = _httpClientFactory.CreateClient();

                var screeningResult = await client.GetAsync($"https://localhost:7143/api/QLTiemChung/KQKhamSangLoc/{IDKH}");

                if (screeningResult.IsSuccessStatusCode)
                {
                    var apiResponse = await screeningResult.Content.ReadAsStringAsync();
                    var kqKhamSangLoc = JsonConvert.DeserializeObject<KQKhamSangLocDTO>(apiResponse);
                    KQKhamSangLocDTO kQKhamSangLocDTO =new KQKhamSangLocDTO 
                    {
                        IDKH = kqKhamSangLoc.IDKH,
                        KetQua= kqKhamSangLoc.KetQua,
                        TenKhachHang = kqKhamSangLoc.TenKhachHang,
                        TinhTrangSucKhoe = kqKhamSangLoc.TinhTrangSucKhoe,
                        TrangThai = kqKhamSangLoc.TrangThai,
                        CanNang = kqKhamSangLoc.CanNang,
                        ChieuCao = kqKhamSangLoc.ChieuCao,
                        GioiTinh =  kqKhamSangLoc.GioiTinh,
                        KhachHang = kqKhamSangLoc.KhachHang
                    };
                   
                    ViewBag.KQKhamSangLocDTO  = kQKhamSangLocDTO;
                }
                else
                {
                    // Xử lý khi API không trả về thành công
                    ViewBag.ErrorMessage = "Không thể tải thông tin khách hàng";
                }
                if (screeningResult == null)
                {
                      GetSession(); return NotFound();
                }

            // Lấy danh sách khách hàng đặt lịch
            var response1 = await client.GetAsync("https://localhost:7143/api/QLTiemChung/DSKhamSangLoc");
           
                var apiResponse1 = await response1.Content.ReadAsStringAsync();
                List<DSKhamSangLocDTO> KhachHangKSL = JsonConvert.DeserializeObject<List<DSKhamSangLocDTO>>(apiResponse1);
                // Dùng Newtonsoft.Json
                ViewBag.DSKhamSangLocDTO = KhachHangKSL;

            string idDK = "";
            //Lấy chỉ vaccine được chỉ định
            var response2 = await client.GetAsync($"https://localhost:7143/api/QLTiemChung/CDVaccine/{IDKH}");
            if (response2.IsSuccessStatusCode)
            {
                var apiResponse2 = await response2.Content.ReadAsStringAsync();
                var cdVaccine = JsonConvert.DeserializeObject<CDVaccineDTO>(apiResponse2);
                CDVaccineDTO cdVaccineDTO = new CDVaccineDTO
                {
                    IDKH = cdVaccine.IDKH,
                    IDDK = cdVaccine.IDDK,
                    IDDKVC = cdVaccine.IDDKVC,
                    IDNV = cdVaccine.IDNV,
                    TenVaccine = cdVaccine.TenVaccine,
                    SoLuong = cdVaccine.SoLuong,
                    TenNhanVien = cdVaccine.TenNhanVien,
                    DangKyVaccine = cdVaccine.DangKyVaccine,
                    KhachHang = cdVaccine.KhachHang,
                    NhanVien = cdVaccine.NhanVien,
                    
                };
                ViewBag.CDVaccineDTO = cdVaccineDTO;
                idDK = cdVaccineDTO.IDDK;
                Console.WriteLine(apiResponse2);
            }
            if (response2 == null)
            {
                GetSession(); return NotFound();
            }
            var response3 = await client.GetAsync($"https://localhost:7143/api/QLTiemChung/TheoDoiSauTiemByIDDK/{idDK}");
            if (response3.IsSuccessStatusCode)
            {
                var apiResponse3 = await response3.Content.ReadAsStringAsync();
                var TheoDoiSauTiem = JsonConvert.DeserializeObject<TheoDoiSauTiem>(apiResponse3);
                ViewBag.TheoDoiSauTiem = TheoDoiSauTiem;
            }
            ViewBag.HangCho = await GetHangCho();
            GetSession();

            return View("~/Views/Home/QLTiemChung.cshtml"); 
        }
        public async Task<IActionResult> CreateTiemChung(string IDNV, string IDDK , string IDKH, DateTime thoiGian)
        {
            if (!ModelState.IsValid)
            {
                GetSession(); return BadRequest(ModelState);
            }
            try
            {
                //create HTTP client
                var client = _httpClientFactory.CreateClient();

                var createTiemChung = new createTiemChung 
                { 
                    IDDK = IDDK,
                    IDNV = IDNV,
                    IDKH = IDKH,
                    ThoiGian = thoiGian,
                    TrangThai = false,
                    GhiChu = ""

                };

                var content = new StringContent(JsonConvert.SerializeObject(createTiemChung), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7143/api/QLTiemChung/CreateTiemChung", content);


                if (response.IsSuccessStatusCode)
                {

                    GetSession();
                    TempData["Notification"] = "Tạo tiêm chủng thành công.";
                    TempData["NotificationType"] = "success";
                    TempData["NotificationTitle"] = "Thông báo.";
                    return RedirectToAction("QLTiemChung");
                }
                else
                {
                    GetSession(); return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra .");

                }

            }
            catch (Exception ex)
            {
                GetSession(); return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }
        public async Task<IActionResult> UpdateTheoDoiSauTiem(string id, string idNV, string idKH, string idTC, DateTime thoiGian, bool trangThai, string? ghiChu)
        {
            if (!ModelState.IsValid)
            {
                GetSession();
                return BadRequest(ModelState);
            }

            try
            {
                using var client = _httpClientFactory.CreateClient();

                // Tạo đối tượng DTO để gửi dữ liệu
                var updateTheoDoi = new createTheoDoi
                {
                    IDST = id,
                    IDNV = idNV,
                    IDKH = idKH,
                    IDTC = idTC,
                    ThoiGian = thoiGian.TimeOfDay,
                    TrangThai = trangThai,
                    GhiChu = ghiChu
                };

                // Gửi yêu cầu PUT đến API
                var content = new StringContent(JsonConvert.SerializeObject(updateTheoDoi), Encoding.UTF8, "application/json");
                var response = await client.PutAsync("https://localhost:7143/api/QLTiemChung/UpdateTheoDoiSauTiem", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Lỗi khi cập nhật thông tin: {errorContent}");
                }

                // Lấy danh sách HangCho từ API
                var response1 = await client.GetAsync("https://65b86c3a46324d531d562e3d.mockapi.io/HangCho");
                if (!response1.IsSuccessStatusCode)
                {
                    var errorResponse = await response1.Content.ReadAsStringAsync();
                    return StatusCode((int)response1.StatusCode, $"Không thể lấy dữ liệu từ API HangCho: {errorResponse}");
                }

                var khachhangapiResponse = await response1.Content.ReadAsStringAsync();
                var hangChoList = JsonConvert.DeserializeObject<List<HangCho>>(khachhangapiResponse);

                // Tìm khách hàng trong danh sách HangCho
                var hangCho = hangChoList.FirstOrDefault(hc => hc.IDKH == idKH);
                if (hangCho == null)
                {
                    return BadRequest("Không tìm thấy thông tin khách hàng trong API HangCho.");
                }

                // Gửi yêu cầu DELETE để xóa khách hàng
                var response2 = await client.DeleteAsync($"https://65b86c3a46324d531d562e3d.mockapi.io/HangCho/{hangCho.ID}");
                if (!response2.IsSuccessStatusCode)
                {
                    var errorContent = await response2.Content.ReadAsStringAsync();
                    return StatusCode((int)response2.StatusCode, $"Không thể xóa thông tin HangCho: {errorContent}");
                }

                TempData["Message"] = "Cập nhật và xóa dữ liệu thành công.";
                GetSession();
                TempData["Notification"] = "Cập nhật trạng thái hóa đơn thành công.";
                TempData["NotificationType"] = "success";
                TempData["NotificationTitle"] = "Thông báo.";
                return RedirectToAction("QLTiemChung");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi khi xử lý yêu cầu: {ex.Message}");
            }
        }

    }
}
