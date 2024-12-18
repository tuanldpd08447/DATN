using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DATN_QLTiemChung.Models;
using System.Text;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DATN_QLTiemChung.Controllers
{
    public class QLXuatNhapKhoController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public QLXuatNhapKhoController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
        [HttpPost]
        public async Task<IActionResult> HuyPhieu(string IDCT)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsync($"https://localhost:7143/api/QLXuatNhapKho/CapNhatTrangThai/{IDCT}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["NotificationTitle"] = "Thông báo.";
                TempData["Notification"] = "Đã hủy phiếu thành công.";
                TempData["NotificationType"] = "success";
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Notification"] = $"Không thể hủy phiếu. Chi tiết: {errorMessage}";
                TempData["NotificationType"] = "error";
            }

              GetSession(); return RedirectToAction("QLXuatNhapKho");
        }


        public async Task<IActionResult> Clickvaccine(string IDCT)
        {
            var client = _httpClientFactory.CreateClient();


            var chungTuResponse = await client.GetAsync($"https://localhost:7143/api/QLXuatNhapKho/GetChungTuById/{IDCT}");

            var chungTuApiResponse = await chungTuResponse.Content.ReadAsStringAsync();
            ChungTuDetail chungTu = JsonConvert.DeserializeObject<ChungTuDetail>(chungTuApiResponse);
            ViewBag.chungTu = chungTu;


            var Response = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetAllVatTu");
            var apiResponses = await Response.Content.ReadAsStringAsync();
            List<VatTuDTO> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuDTO>>(apiResponses);
            ViewBag.vatTuYTe = vatTuYTe;

            var Response1 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetLoaiVatTu");
            var apiResponses1 = await Response1.Content.ReadAsStringAsync();
            List<LoaivatTu> loaivattu = JsonConvert.DeserializeObject<List<LoaivatTu>>(apiResponses1);
            ViewBag.loaivattu = loaivattu;


            var Response2 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetNguonCap");
            var apiResponses2 = await Response2.Content.ReadAsStringAsync();
            List<NguonCungCap> NguonCap = JsonConvert.DeserializeObject<List<NguonCungCap>>(apiResponses2);
            ViewBag.NguonCap = NguonCap;

            var Response3 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetNhacap");
            var apiResponses3 = await Response3.Content.ReadAsStringAsync();
            List<NhaCungCap> NhaCap = JsonConvert.DeserializeObject<List<NhaCungCap>>(apiResponses3);
            ViewBag.NhaCap = NhaCap;


            var Response4 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetAllChungTu");
            var apiResponses4 = await Response4.Content.ReadAsStringAsync();
            List<ChungTuDetail> chungTuList = JsonConvert.DeserializeObject<List<ChungTuDetail>>(apiResponses4);
            ViewBag.ListChungTu = chungTuList;

            var Response5 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetXuatXu");
            var apiResponses5 = await Response5.Content.ReadAsStringAsync();
            List<XuatXu> xuatXus = JsonConvert.DeserializeObject<List<XuatXu>>(apiResponses5);
            ViewBag.ListXuatSu = xuatXus;

            var Response6 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GenerateNewId");
            var apiResponses6 = await Response6.Content.ReadAsStringAsync();
            string IDctnew = apiResponses6;
            ViewBag.IDCTNew = IDctnew;

              GetSession(); return View("~/Views/Home/QLXuatNhapKho.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> AddChungTu(string IDCT, string Loaiphieu, int soLuong, DateTime NgayXuatNhap, string IDVT,
     string DonGia, string ThanhTien, string? GhiChu, IFormFile ChungtuFile)
        {
            Console.WriteLine("Bắt đầu xử lý yêu cầu AddChungTu...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model không hợp lệ. Trả về QLXuatNhapKho.");
                string erro = "";
                // Lấy chi tiết lỗi từ ModelState
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        erro = $"Field: {key}, Error: {error.ErrorMessage}";
                    }
                }

                // Thêm thông báo vào TempData
                TempData["Notification"] = erro;
                TempData["NotificationTitle"] = "Thông báo.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Cannot Send Mail";

                  GetSession(); return RedirectToAction("QLXuatNhapKho");
            }


            try
            {
                var client = _httpClientFactory.CreateClient();

                // Kiểm tra và xử lý tên file
                string fileName = string.Empty;
                if (ChungtuFile != null && ChungtuFile.Length > 0)
                {
                    if (ChungtuFile.ContentType != "application/pdf")
                    {
                        Console.WriteLine($"Loại file không hợp lệ: {ChungtuFile.ContentType}");
                        ModelState.AddModelError("ChungtuFile", "Chỉ chấp nhận file PDF.");
                          GetSession(); 
                        TempData["Notification"] = "Thêm thất bại.";
                        TempData["NotificationType"] = "error";
                        TempData["NotificationTitle"] = "Thông báo."; 
                        return RedirectToAction("QLXuatNhapKho");
                    }

                    fileName = ChungtuFile.FileName;
                    Console.WriteLine($"File PDF nhận được: {fileName}");

                    string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdf");

                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                        Console.WriteLine("Tạo thư mục lưu trữ PDF.");
                    }

                    fileName = EnsureUniqueFileName(uploadDirectory, fileName);
                    Console.WriteLine($"Tên file sau kiểm tra trùng lặp: {fileName}");

                    // Lưu file lên server
                    string filePath = Path.Combine(uploadDirectory, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ChungtuFile.CopyToAsync(stream);
                    }
                    Console.WriteLine($"File được lưu tại: {filePath}");
                }

                bool isLoaiPhieuNhap = Loaiphieu == "true";
                Console.WriteLine($"Loại phiếu: {(isLoaiPhieuNhap ? "Phiếu Nhập" : "Phiếu Xuất")}");

                // Tạo đối tượng chứng từ
                var createChungTu = new crteateChungTu
                {
                    IDXCT = IDCT,
                    IDNV = "NV001", // Giả định ID nhân viên
                    IDVT = IDVT,
                    DonGia = double.TryParse(DonGia, out var donGia) ? donGia : 0,
                    ThanhTien = double.TryParse(ThanhTien, out var thanhTien) ? thanhTien : 0,
                    ThoiGian = NgayXuatNhap,
                    LoaiChungTu = isLoaiPhieuNhap,
                    HinhAnh = fileName, // Lưu tên file duy nhất
                    GhiChu = GhiChu,
                    SoLuongXuatNhap = soLuong,
                    TrangThai = true
                };

                Console.WriteLine("Tạo đối tượng chứng từ: ");
                Console.WriteLine($"Số phiếu: {createChungTu.IDXCT}, Số lượng: {createChungTu.SoLuongXuatNhap}, Đơn giá: {createChungTu.DonGia}");

                var content = new StringContent(JsonConvert.SerializeObject(createChungTu), Encoding.UTF8, "application/json");

                // Gửi request POST tới API
                Console.WriteLine("Gửi request POST tới API...");
                var response = await client.PostAsync("https://localhost:7143/api/QLXuatNhapKho/CreateChungTu", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Request thành công! Redirect đến QLXuatNhapKho.");
                      GetSession();
                    TempData["Notification"] = "Thêm thành công.";
                    TempData["NotificationType"] = "success";
                    TempData["NotificationTitle"] = "Thông báo.";
                    return RedirectToAction("QLXuatNhapKho");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API lỗi: {errorMessage}");
                    ModelState.AddModelError("", $"API lỗi: {errorMessage}");
                      GetSession();
                    TempData["Notification"] = "Thêm thất bại.";
                    TempData["NotificationType"] = "error";
                    TempData["NotificationTitle"] = "Thông báo.";
                    return RedirectToAction("QLXuatNhapKho");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi khi kết nối với máy chủ: {ex.Message}");
                ModelState.AddModelError("", $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
                  GetSession();
                TempData["Notification"] = "Xảy ra lỗi.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                return RedirectToAction("QLXuatNhapKho");
            }
        }


        public async Task<IActionResult> QLXuatNhapKho()
        {
            var client = _httpClientFactory.CreateClient();

            var Response = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetAllVatTu");
            var apiResponses = await Response.Content.ReadAsStringAsync();
            List<VatTuDTO> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuDTO>>(apiResponses);
            ViewBag.vatTuYTe = vatTuYTe;

            var Response1 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetLoaiVatTu");
            var apiResponses1 = await Response1.Content.ReadAsStringAsync();
            List<LoaivatTu> loaivattu = JsonConvert.DeserializeObject<List<LoaivatTu>>(apiResponses1);
            ViewBag.loaivattu = loaivattu;


            var Response2 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetNguonCap");
            var apiResponses2 = await Response2.Content.ReadAsStringAsync();
            List<NguonCungCap> NguonCap = JsonConvert.DeserializeObject<List<NguonCungCap>>(apiResponses2);
            ViewBag.NguonCap = NguonCap;

            var Response3 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetNhacap");
            var apiResponses3 = await Response3.Content.ReadAsStringAsync();
            List<NhaCungCap> NhaCap = JsonConvert.DeserializeObject<List<NhaCungCap>>(apiResponses3);
            ViewBag.NhaCap = NhaCap;


            var Response4 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetAllChungTu");
            var apiResponses4 = await Response4.Content.ReadAsStringAsync();
            List<ChungTuDetail> chungTuList = JsonConvert.DeserializeObject<List<ChungTuDetail>>(apiResponses4);
            ViewBag.ListChungTu = chungTuList;

            var Response5 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetXuatXu");
            var apiResponses5 = await Response5.Content.ReadAsStringAsync();
            List<XuatXu> xuatXus = JsonConvert.DeserializeObject<List<XuatXu>>(apiResponses5);
            ViewBag.ListXuatSu = xuatXus;

            var Response6 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GenerateNewId");
            var apiResponses6 = await Response6.Content.ReadAsStringAsync();
            string IDctnew = apiResponses6;
            ViewBag.IDCTNew = IDctnew;

              GetSession(); return View("~/Views/Home/QLXuatNhapKho.cshtml");
        }

        private string EnsureUniqueFileName(string directory, string fileName)
        {
            string filePath = Path.Combine(directory, fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string fileExtension = Path.GetExtension(fileName);
            int counter = 1;

            while (System.IO.File.Exists(filePath))
            {
                fileName = $"{fileNameWithoutExtension}({counter}){fileExtension}";
                filePath = Path.Combine(directory, fileName);
                counter++;
            }

              GetSession(); return fileName;
        }
    }

}
