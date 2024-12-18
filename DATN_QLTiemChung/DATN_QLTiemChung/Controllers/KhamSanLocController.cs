using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    public class KhamSanLocController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;

        public KhamSanLocController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> KhamSanLoc()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync("https://65b86c3a46324d531d562e3d.mockapi.io/HangCho");

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Không thể lấy dữ liệu từ API HangCho.");
                    GetSession();
                    TempData["Notification"] = "Không thể lấy dữ liệu từ Hàng Chờ.";
                    TempData["NotificationType"] = "error";
                    TempData["NotificationTitle"] = "Thông báo.";
                    return View("~/Views/Home/KhamSanLoc.cshtml");
                }

                var khachhangapiResponse = await response.Content.ReadAsStringAsync();
                var hangChoList = JsonConvert.DeserializeObject<List<HangCho>>(khachhangapiResponse)
                                   ?.Where(hc => hc.Step == "TiepNhan")
                                   .ToList();

                if (hangChoList == null || !hangChoList.Any())
                {
                    ModelState.AddModelError("", "Không có dữ liệu khách hàng phù hợp.");
                    ViewBag.HangCho = new List<HangCho>();
                }
                else
                {
                    ViewBag.HangCho = hangChoList;
                }

                var response1 = await client.GetAsync("https://localhost:7143/api/KhamSanLoc/GetAllVatTu");

                if (!response1.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Không thể lấy dữ liệu từ API VatTu.");
                    GetSession();
                    TempData["Notification"] = "Không thể lấy dữ liệu từ Vật tư.";
                    TempData["NotificationType"] = "Error";
                    TempData["NotificationTitle"] = "Thông báo."; return View("~/Views/Home/KhamSanLoc.cshtml");
                }

                var apiResponse = await response1.Content.ReadAsStringAsync();
                List<VatTuDTOFull> vatTuDTOFull = JsonConvert.DeserializeObject<List<VatTuDTOFull>>(apiResponse);

                if (vatTuDTOFull == null || !vatTuDTOFull.Any())
                {
                    ModelState.AddModelError("", "Không có dữ liệu vật tư.");
                }
                else
                {
                    ViewBag.VatTus = JsonConvert.SerializeObject(vatTuDTOFull);
                }

                var Response2 = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetNguonCap");
                var apiResponses2 = await Response2.Content.ReadAsStringAsync();
                List<NguonCungCap> NguonCap = JsonConvert.DeserializeObject<List<NguonCungCap>>(apiResponses2);
                ViewBag.NguonCap = NguonCap;

                var Response3 = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetNhacap");
                var apiResponses3 = await Response3.Content.ReadAsStringAsync();
                List<NhaCungCap> NhaCap = JsonConvert.DeserializeObject<List<NhaCungCap>>(apiResponses3);
                ViewBag.NhaCap = NhaCap;


                var Response4 = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/Getxuatxu");
                var apiResponses4 = await Response4.Content.ReadAsStringAsync();
                List<XuatXu> Xuatxu = JsonConvert.DeserializeObject<List<XuatXu>>(apiResponses4);
                ViewBag.Xuatxu = Xuatxu;

                GetSession();
              
                return View("~/Views/Home/KhamSanLoc.cshtml");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                GetSession();
                TempData["Notification"] = "Đã xảy ra lỗi.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                return View("~/Views/Home/KhamSanLoc.cshtml");
            }
        }

        public async Task<IActionResult> ClickHangCho(string IDKH)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Tạo tác vụ đồng thời để gọi API
                var hangChoTask = client.GetAsync("https://65b86c3a46324d531d562e3d.mockapi.io/HangCho");
                var khachHangTask = client.GetAsync($"https://localhost:7143/api/KhamSanLoc/GetKhachHangById/{IDKH}");

                // Chờ cả hai API hoàn tất
                await Task.WhenAll(hangChoTask, khachHangTask);

                // Xử lý kết quả API đầu tiên (HangCho)
                var hangChoResponse = hangChoTask.Result;
                if (hangChoResponse.IsSuccessStatusCode)
                {
                    var hangChoData = await hangChoResponse.Content.ReadAsStringAsync();
                    var hangChoList = JsonConvert.DeserializeObject<List<HangCho>>(hangChoData)
                                       ?.Where(hc => hc.Step == "TiepNhan")
                                       .ToList();

                    ViewBag.HangCho = hangChoList ?? new List<HangCho>();
                }
                else
                {
                    ModelState.AddModelError("", "Không thể lấy dữ liệu danh sách chờ từ API.");
                    ViewBag.HangCho = new List<HangCho>();
                }

                // Xử lý kết quả API thứ hai (KhachHang)
                var khachHangResponse = khachHangTask.Result;
                if (khachHangResponse.IsSuccessStatusCode)
                {
                    var khachHangData = await khachHangResponse.Content.ReadAsStringAsync();
                    var khachHang = JsonConvert.DeserializeObject<KhachHangDTo2>(khachHangData);
                    ViewBag.KhachHang = khachHang;
                }
                else
                {
                    ModelState.AddModelError("", $"Không thể lấy dữ liệu khách hàng từ API với ID: {IDKH}.");
                    ViewBag.KhachHang = null;
                }

                var response1 = await client.GetAsync("https://localhost:7143/api/KhamSanLoc/GetAllVatTu");

                if (!response1.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Không thể lấy dữ liệu từ API VatTu.");
                    GetSession();
                    TempData["Notification"] = "Không thể lấy dữ liệu từ Vật tư.";
                    TempData["NotificationType"] = "error";
                    TempData["NotificationTitle"] = "Thông báo."; return View("~/Views/Home/KhamSanLoc.cshtml");
                }

                var apiResponse = await response1.Content.ReadAsStringAsync();
                List<VatTuDTOFull> vatTuDTOFull = JsonConvert.DeserializeObject<List<VatTuDTOFull>>(apiResponse);

                if (vatTuDTOFull == null || !vatTuDTOFull.Any())
                {
                    ModelState.AddModelError("", "Không có dữ liệu vật tư.");
                }
                else
                {
                    ViewBag.VatTus = JsonConvert.SerializeObject(vatTuDTOFull);
                }
                var response = await client.GetAsync($"https://localhost:7143/api/KhamSanLoc/MuiTiepTheo/{IDKH}");
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.MuiTiepTheo = await response.Content.ReadFromJsonAsync<bool>();
                }

                var Response2 = await client.GetAsync("https://localhost:7143/api/KhamSanLoc/GetNguonCap");
                var apiResponses2 = await Response2.Content.ReadAsStringAsync();
                List<NguonCungCap> NguonCap = JsonConvert.DeserializeObject<List<NguonCungCap>>(apiResponses2);
                ViewBag.NguonCap = NguonCap;

                var Response3 = await client.GetAsync("https://localhost:7143/api/KhamSanLoc/GetNhacap");
                var apiResponses3 = await Response3.Content.ReadAsStringAsync();
                List<NhaCungCap> NhaCap = JsonConvert.DeserializeObject<List<NhaCungCap>>(apiResponses3);
                ViewBag.NhaCap = NhaCap;


                var Response4 = await client.GetAsync("https://localhost:7143/api/KhamSanLoc/Getxuatxu");
                var apiResponses4 = await Response4.Content.ReadAsStringAsync();
                List<XuatXu> Xuatxu = JsonConvert.DeserializeObject<List<XuatXu>>(apiResponses4);
                ViewBag.Xuatxu = Xuatxu;

                GetSession();
                return View("~/Views/Home/KhamSanLoc.cshtml");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
                ViewBag.HangCho = new List<HangCho>();
                ViewBag.KhachHang = null;
                GetSession();
                TempData["Notification"] = "Đã xảy ra lỗi.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo."; return View("~/Views/Home/KhamSanLoc.cshtml");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SubmitForm(
    string? IDVT,
    int? SoLuong,
    string IDKH,
    double ChieuCao,
    double CanNang,
    string IDNV,
    bool TrangThai,
    bool KetQua,
    string GhiChu, string TinhTrangSucKhoe, bool MuiTiepTheo)
        {
            // Kiểm tra trạng thái của mô hình
            if (ModelState.IsValid)
            {
                var ksl = new KhamSangLocDTO
                {
                    IDKH = IDKH,
                    IDNV = IDNV,
                    IDVT = IDVT ?? "",
                    SoLuong = SoLuong ?? 0,
                    CanNang = CanNang,
                    ChieuCao = ChieuCao,
                    TrangThai = TrangThai,
                    KetQua = KetQua,
                    GhiChu = GhiChu,
                    TinhTrangSucKhoe = TinhTrangSucKhoe,
                    MuiTiepTheo = MuiTiepTheo
                };

                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(ksl), Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync("https://localhost:7143/api/KhamSanLoc/AddKhamSangLoc", content);

                    // Kiểm tra mã trạng thái HTTP trả về
                    if (response.IsSuccessStatusCode)
                    {
                        var response1 = await client.GetAsync("https://65b86c3a46324d531d562e3d.mockapi.io/HangCho");
                        if (!response1.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError("", "Không thể lấy dữ liệu từ API HangCho.");
                            GetSession();
                            TempData["Notification"] = "Không thể lấy dữ liệu Hàng chờ.";
                            TempData["NotificationType"] = "error";
                            TempData["NotificationTitle"] = "Thông báo."; return View("~/Views/Home/KhamSanLoc.cshtml");
                        }
                        var khachhangapiResponse = await response1.Content.ReadAsStringAsync();
                        var hangChoList = JsonConvert.DeserializeObject<List<HangCho>>(khachhangapiResponse);

                        var hangCho = hangChoList.FirstOrDefault(Hc => Hc.IDKH == IDKH);
                        if (hangCho == null)
                        {
                            ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng.");
                            GetSession();
                            TempData["Notification"] = "Không thể tìm thấy thông tin khách hàng.";
                            TempData["NotificationType"] = "error";
                            TempData["NotificationTitle"] = "Thông báo.";
                            return View("~/Views/Home/KhamSanLoc.cshtml");
                        }
                        if (TrangThai == false)
                        {
                            // Gửi yêu cầu DELETE để xóa khách hàng
                            var response2 = await client.DeleteAsync($"https://65b86c3a46324d531d562e3d.mockapi.io/HangCho/{hangCho.ID}");
                            if (!response2.IsSuccessStatusCode)
                            {
                                var errorContent = await response2.Content.ReadAsStringAsync();
                                return StatusCode((int)response2.StatusCode, $"Không thể xóa thông tin HangCho: {errorContent}");
                            }
                            else
                            {

                                GetSession();
                                TempData["Notification"] = "Khám sàn lọc thành công.";
                                TempData["NotificationType"] = "success";
                                TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("KhamSanLoc");
                            }
                        }
                        else
                        {
                            if (MuiTiepTheo)
                            {
                                hangCho.Step = "ThanhToan";
                            }
                            else
                            {
                                hangCho.Step = "KhamSanLoc";
                            }
                            // Construct the PUT request with the data to be updated
                            var content1 = new StringContent(JsonConvert.SerializeObject(hangCho), Encoding.UTF8, "application/json");

                            // Make the PUT request
                            var response2 = await client.PutAsync($"https://65b86c3a46324d531d562e3d.mockapi.io/HangCho/{hangCho.ID}", content1);

                            var responseContent = await response2.Content.ReadAsStringAsync();
                            if (response2.IsSuccessStatusCode)
                            {
                                GetSession();
                                TempData["Notification"] = "khám sàn lọc thành công.";
                                TempData["NotificationType"] = "success";
                                TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("KhamSanLoc");
                            }
                            else
                            {
                                ModelState.AddModelError("", $"Không thể cập nhật dữ liệu vào API HangCho. Lỗi: {responseContent}");
                                GetSession();
                                TempData["Notification"] = "Không thể cập nhật dữ liệu.";
                                TempData["NotificationType"] = "error";
                                TempData["NotificationTitle"] = "Thông báo.";
                                return View("~/Views/Home/KhamSanLoc.cshtml");
                            }
                        }


                    }
                    else
                    {
                        // Log mã lỗi HTTP và thông báo chi tiết
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        GetSession(); return StatusCode((int)response.StatusCode, $"Đã có lỗi xảy ra khi thêm khách hàng. Lỗi: {errorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi khi không thể kết nối
                    GetSession(); return StatusCode(500, $"Đã có lỗi khi gửi yêu cầu: {ex.Message}");
                }
            }
            else
            {
                TempData["Notification"] = "Vui Lòng Chọn đầy đủ thông tin.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                GetSession();
                return RedirectToAction("ClickHangCho", new { IDKH = IDKH });

            }
        }




    }
}
