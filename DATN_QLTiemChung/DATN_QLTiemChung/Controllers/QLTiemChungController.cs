using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using DATN_QLTiemChung.Models;
using Newtonsoft.Json;

namespace DATN_QLTiemChung.Controllers
{

    public class QLTiemChungController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public QLTiemChungController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }
            public async Task<IActionResult> QLTiemChung()
            {
                var client = _httpClientFactory.CreateClient();
                try
                {

                    // Lấy danh sách khách hàng khám sàng lọc
                    var response = await client.GetAsync("https://localhost:7143/api/QLTiemChung/DSKhamSangLoc");
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        List<DSKhamSangLocDTO> KhachHangKSL = JsonConvert.DeserializeObject<List<DSKhamSangLocDTO>>(apiResponse);
                    // Dùng Newtonsoft.Json
                    ViewBag.DSKhamSangLocDTO = KhachHangKSL;
                }
                    else
                    {
                        // Xử lý khi API không trả về thành công
                        ViewBag.ErrorMessage = "Không thể tải danh sách khách hàng khám sàng lọc";
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi kết nối hoặc lỗi khác
                    ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
                }


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
                    Console.WriteLine(kQKhamSangLocDTO.CanNang);
                    Console.WriteLine(kQKhamSangLocDTO.ChieuCao);
                    ViewBag.KQKhamSangLocDTO  = kQKhamSangLocDTO;
                    Console.WriteLine(apiResponse);
                }
                else
                {
                    // Xử lý khi API không trả về thành công
                    ViewBag.ErrorMessage = "Không thể tải thông tin khách hàng";
                }
                if (screeningResult == null)
                {
                    return NotFound();
                }

            // Lấy danh sách khách hàng đặt lịch
            var response1 = await client.GetAsync("https://localhost:7143/api/QLTiemChung/DSKhamSangLoc");
           
                var apiResponse1 = await response1.Content.ReadAsStringAsync();
                List<DSKhamSangLocDTO> KhachHangKSL = JsonConvert.DeserializeObject<List<DSKhamSangLocDTO>>(apiResponse1);
                // Dùng Newtonsoft.Json
                ViewBag.DSKhamSangLocDTO = KhachHangKSL;


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
                Console.WriteLine(apiResponse2);
            }
            if (response2 == null)
            {
                return NotFound();
            }
            return View("~/Views/Home/QLTiemChung.cshtml"); 
        }
    }
}
