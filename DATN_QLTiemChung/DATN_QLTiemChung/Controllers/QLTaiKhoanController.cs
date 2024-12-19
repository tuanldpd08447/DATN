using Microsoft.AspNetCore.Mvc;
using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Security.AccessControl;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    public class QLTaiKhoanController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;

        public QLTaiKhoanController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> QLTaiKhoan()
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                // Lấy danh sách nhân viên
                var response = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetAllTaiKhoanNV");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    List<QLTaiKhoanNVDTO> qltknhanViens = JsonConvert.DeserializeObject<List<QLTaiKhoanNVDTO>>(apiResponse);
                    ViewBag.QLTaiKhoanNVs = JsonConvert.SerializeObject(qltknhanViens);
                }
                else
                {
                    // Xử lý lỗi nếu API trả về mã trạng thái không thành công
                    ViewBag.ErrorMessage = "Không thể lấy thông tin nhân viên từ API.";
                }

                // Lấy danh sách khách hàng
                var response0 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetAllTaiKhoanKH");
                if (response0.IsSuccessStatusCode)
                {
                    var apiResponse0 = await response0.Content.ReadAsStringAsync();
                    List<QLTaiKhoanKHDTO> qltkkhachHangs = JsonConvert.DeserializeObject<List<QLTaiKhoanKHDTO>>(apiResponse0);
                    ViewBag.QLTaiKhoanKHs = JsonConvert.SerializeObject(qltkkhachHangs);
                }
                else
                {
                    // Xử lý lỗi nếu API trả về mã trạng thái không thành công
                    ViewBag.ErrorMessage = "Không thể lấy thông tin khách hàng từ API.";
                }
                // Lấy danh sách khách hàng
                var response1 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetTaiKhoanNhanVienChuaCoTK");
                if (response1.IsSuccessStatusCode)
                {
                    var apiResponse1 = await response1.Content.ReadAsStringAsync();
                    List<TTNV> NvChuaCoTk = JsonConvert.DeserializeObject<List<TTNV>>(apiResponse1);
                    ViewBag.NvChuaCoTk = JsonConvert.SerializeObject(NvChuaCoTk);
                }
                else
                {
                    // Xử lý lỗi nếu API trả về mã trạng thái không thành công
                    ViewBag.ErrorMessage = "Không thể lấy thông tin nhân viên từ API.";
                }
                var response2 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetTaiKhoanKhachHangChuaCoTK");
                if (response2.IsSuccessStatusCode)
                {
                    var apiResponse2 = await response2.Content.ReadAsStringAsync();
                    List<TTKH> KhChuaCoTk = JsonConvert.DeserializeObject<List<TTKH>>(apiResponse2);
                    ViewBag.KhChuaCoTk = JsonConvert.SerializeObject(KhChuaCoTk);
                }
                else
                {
                    // Xử lý lỗi nếu API trả về mã trạng thái không thành công
                    ViewBag.ErrorMessage = "Không thể lấy thông tin khách hàng từ API.";
                }
                // Gọi hàm GetSession() nếu cần thiết
                GetSession();

                return View("/Views/Home/QLTaiKhoan.cshtml");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra trong quá trình gọi API hoặc xử lý dữ liệu
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải dữ liệu: " + ex.Message;
                return View("/Views/Home/QLTaiKhoan.cshtml");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CapMKNV(string IDNV)
        {
            if (!ModelState.IsValid)
            {
                GetSession(); return BadRequest(ModelState);
            }
            try
            {
                // Tạo HTTP client
                var client = _httpClientFactory.CreateClient();

                // Gửi yêu cầu tới API
                var response = await client.PostAsync($"http://qltiemchungapi.runasp.net/api/QLTaiKhoan/DoiMKNVTuDong/{IDNV}", null);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc phản hồi từ API
                    var apiResponse = await response.Content.ReadAsStringAsync();

                    // Nếu phản hồi có định dạng JSON, kiểm tra và deserialize
                    var result = JsonConvert.DeserializeObject<dynamic>(apiResponse);

                    // Kiểm tra kết quả thành công từ API
                    if (result?.success == true)
                    {
                        TempData["Notification"] = "Cấp mật khẩu thành công.";
                        TempData["NotificationType"] = "success";
                        TempData["NotificationTitle"] = "Thông báo.";
                    }
                    else
                    {
                        TempData["Notification"] = result?.message ?? "Cấp mật khẩu thất bại.";
                        TempData["NotificationType"] = "error";
                        TempData["NotificationTitle"] = "Lỗi.";
                    }

                    GetSession();
                    return RedirectToAction("QLTaiKhoan");
                }
                else
                {
                    TempData["Notification"] = "Không thể cấp mật khẩu. Vui lòng thử lại.";
                    TempData["NotificationType"] = "error";
                    TempData["NotificationTitle"] = "Lỗi.";
                    GetSession();
                    return StatusCode((int)response.StatusCode, "Đã xảy ra lỗi khi gọi API.");
                }
            }
            catch (Exception ex)
            {
                TempData["Notification"] = "Đã xảy ra lỗi khi kết nối với máy chủ.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Lỗi.";
                GetSession();
                return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CapMKKH(string IDKH)
        {
            if (!ModelState.IsValid)
            {
                GetSession();
                return BadRequest(ModelState);
            }
            try
            {
                // Tạo HTTP client
                var client = _httpClientFactory.CreateClient();

                // Gửi yêu cầu tới API
                var response = await client.PostAsync($"http://qltiemchungapi.runasp.net/api/QLTaiKhoan/DoiMKKHTuDong/{IDKH}", null);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc phản hồi từ API
                    var apiResponse = await response.Content.ReadAsStringAsync();

                    // Nếu phản hồi có định dạng JSON, kiểm tra và deserialize
                    var result = JsonConvert.DeserializeObject<dynamic>(apiResponse);

                    // Kiểm tra kết quả thành công từ API
                    if (result?.success == true)
                    {
                        TempData["Notification"] = "Cấp mật khẩu thành công.";
                        TempData["NotificationType"] = "success";
                        TempData["NotificationTitle"] = "Thông báo.";
                    }
                    else
                    {
                        TempData["Notification"] = result?.message ?? "Cấp mật khẩu thất bại.";
                        TempData["NotificationType"] = "error";
                        TempData["NotificationTitle"] = "Lỗi.";
                    }

                    GetSession();
                    return RedirectToAction("QLTaiKhoan");
                }
                else
                {
                    TempData["Notification"] = "Không thể cấp mật khẩu. Vui lòng thử lại.";
                    TempData["NotificationType"] = "error";
                    TempData["NotificationTitle"] = "Lỗi.";
                    GetSession();
                    return StatusCode((int)response.StatusCode, "Đã xảy ra lỗi khi gọi API.");
                }
            }
            catch (Exception ex)
            {
                TempData["Notification"] = "Đã xảy ra lỗi khi kết nối với máy chủ.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Lỗi.";
                GetSession();
                return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaiKhoanNV(string IDTKNV, string IDNV, string email, string password, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
               GetSession(); return BadRequest(ModelState);
            }
            try
            {
                //create HTTP client
                var client = _httpClientFactory.CreateClient();

                // map input parameters to the DTO 
                var createTaiKhoanNVDTO = new CreateTaiKhoanNVDTO
                {
                    IDTKNV = IDTKNV,
                    IDNV = IDNV,
                    Email = email,
                    MatKhau = password
                };
                // Serialize the object to JSON

                var content = new StringContent(JsonConvert.SerializeObject(createTaiKhoanNVDTO), Encoding.UTF8, "application/json");
                // Send the POST request
                var response = await client.PostAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/CreateTaiKhoanNV", content);


                if (response.IsSuccessStatusCode)
                {
                    var apiResonse = await response.Content.ReadAsStringAsync();
                    var addedNhanVien = JsonConvert.DeserializeObject<QLTaiKhoanNVDTO>(apiResonse);

                    TempData["Notification"] = "Thêm tài khoản thành công.";
                    TempData["NotificationType"] = "success";
                    TempData["NotificationTitle"] = "Thông báo.";
                    GetSession(); return RedirectToAction("QLTaiKhoan");

                }
                else
                {
                   GetSession(); return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm tài khoản nhân viên.");

                }

            }
            catch (Exception ex)
            {
               GetSession(); return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMatKhauTKNV(string IDTKNV, string IDNV, string email, string NewPassword)
        {
            if (!ModelState.IsValid)
            {
               GetSession(); return BadRequest(ModelState); // Dữ liệu không hợp lệ
            }
            try
            {
                // Tạo đối tượng HttpClient
                var nv = _httpClientFactory.CreateClient();

                var qlTaiKhoanNVDTO = new QLTaiKhoanNVDTO
                {
                    IDTKNV = IDTKNV,
                    IDNV = IDNV,
                    Email = email,
                    MatKhau = NewPassword,
                };
                // Serialize đối tượng DTO thành JSON
                var content = new StringContent(JsonConvert.SerializeObject(qlTaiKhoanNVDTO), Encoding.UTF8, "application/json");

                // Gửi yêu cầu PUT đến API
                var response = await nv.PutAsync($"http://qltiemchungapi.runasp.net/api/QLTaiKhoan/UpdateMatKhauTKNV/{IDTKNV}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Notification"] = "Cập nhật thành công.";
                    TempData["NotificationType"] = "success";
                    TempData["NotificationTitle"] = "Thông báo.";
                    // Nếu thành công, có thể lấy lại danh sách nhân viên để cập nhật giao diện
                    GetSession(); return RedirectToAction("QLTaiKhoan"); // Chuyển hướng về danh sách nhân viên
                }
                else
                {
                   GetSession(); return StatusCode((int)response.StatusCode, "Không thể cập nhật nhân viên.");
                }
            }
            catch (Exception ex)
            {
               GetSession(); return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }

        public async Task<IActionResult> ClickTK(string? IDNV, string? IDKH)
        {
            var client = _httpClientFactory.CreateClient();

            if(IDNV!= null && IDKH == null)
            {
                var aresponse = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetTKNhanVienById/" + IDNV);

                if (aresponse.IsSuccessStatusCode)
                {
                    var nhanvienapiResponse = await aresponse.Content.ReadAsStringAsync();
                    var qlTaiKhoanNV = JsonConvert.DeserializeObject<QLTaiKhoanNVDTO>(nhanvienapiResponse);
                    ViewBag.QLTaiKhoanNV = qlTaiKhoanNV;
                    ViewBag.ClickNV = true;

                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể tải thông tin nhân viên";

                }
            }
            else if (IDKH != null && IDNV == null)
            {
                var aresponse = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetTKKhachHangById/" + IDKH);

                if (aresponse.IsSuccessStatusCode)
                {
                    var nhanvienapiResponse = await aresponse.Content.ReadAsStringAsync();
                    var QLyTaiKhoanKH = JsonConvert.DeserializeObject<QLTaiKhoanKHDTO>(nhanvienapiResponse);
                    ViewBag.QLyTaiKhoanKH = QLyTaiKhoanKH;
                    ViewBag.ClickKH = true;
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể tải thông tin khách hàng";

                }
            }

            var response = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetAllTaiKhoanNV");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                List<QLTaiKhoanNVDTO> qltknhanViens = JsonConvert.DeserializeObject<List<QLTaiKhoanNVDTO>>(apiResponse);
                ViewBag.QLTaiKhoanNVs = JsonConvert.SerializeObject(qltknhanViens);
            }
            else
            {
                // Xử lý lỗi nếu API trả về mã trạng thái không thành công
                ViewBag.ErrorMessage = "Không thể lấy thông tin nhân viên từ API.";
            }

            // Lấy danh sách khách hàng
            var response0 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetAllTaiKhoanKH");
            if (response0.IsSuccessStatusCode)
            {
                var apiResponse0 = await response0.Content.ReadAsStringAsync();
                List<QLTaiKhoanKHDTO> qltkkhachHangs = JsonConvert.DeserializeObject<List<QLTaiKhoanKHDTO>>(apiResponse0);
                ViewBag.QLTaiKhoanKHs = JsonConvert.SerializeObject(qltkkhachHangs);
            }
            else
            {
                // Xử lý lỗi nếu API trả về mã trạng thái không thành công
                ViewBag.ErrorMessage = "Không thể lấy thông tin khách hàng từ API.";
            }
            // Lấy danh sách khách hàng
            var response1 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetTaiKhoanNhanVienChuaCoTK");
            if (response1.IsSuccessStatusCode)
            {
                var apiResponse1 = await response1.Content.ReadAsStringAsync();
                List<TTNV> NvChuaCoTk = JsonConvert.DeserializeObject<List<TTNV>>(apiResponse1);
                ViewBag.NvChuaCoTk = JsonConvert.SerializeObject(NvChuaCoTk);
            }
            else
            {
                // Xử lý lỗi nếu API trả về mã trạng thái không thành công
                ViewBag.ErrorMessage = "Không thể lấy thông tin nhân viên từ API.";
            }
            var response2 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/GetTaiKhoanKhachHangChuaCoTK");
            if (response2.IsSuccessStatusCode)
            {
                var apiResponse2 = await response2.Content.ReadAsStringAsync();
                List<TTKH> KhChuaCoTk = JsonConvert.DeserializeObject<List<TTKH>>(apiResponse2);
                ViewBag.KhChuaCoTk = JsonConvert.SerializeObject(KhChuaCoTk);
            }
            else
            {
                // Xử lý lỗi nếu API trả về mã trạng thái không thành công
                ViewBag.ErrorMessage = "Không thể lấy thông tin khách hàng từ API.";
            }
            // Nếu thành công, có thể lấy lại danh sách nhân viên để cập nhật giao diện
            // Chuyển hướng về danh sách nhân viên
            GetSession(); return View("/Views/Home/QLTaiKhoan.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaiKhoanKH(string IDTKKH, string IDKH, string sdt, string password, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
               GetSession(); return BadRequest(ModelState);
            }
            try
            {
                //create HTTP client
                var client = _httpClientFactory.CreateClient();

                // map input parameters to the DTO 
                var createTaiKhoanKHDTO = new CreateTaiKhoanKHDTO
                {
                    IDTKKH = IDTKKH,
                    IDKH = IDKH,
                    SDT = sdt,
                    MatKhau = password
                };
                // Serialize the object to JSON

                var content = new StringContent(JsonConvert.SerializeObject(createTaiKhoanKHDTO), Encoding.UTF8, "application/json");
                // Send the POST request
                var response = await client.PostAsync("http://qltiemchungapi.runasp.net/api/QLTaiKhoan/CreateTaiKhoanKH", content);


                if (response.IsSuccessStatusCode)
                {
                    var apiResonse = await response.Content.ReadAsStringAsync();
                    var addedKhachhang = JsonConvert.DeserializeObject<QLTaiKhoanKHDTO>(apiResonse);

                    TempData["Notification"] = "Tạo tài khoản thành công.";
                    TempData["NotificationType"] = "success";
                    TempData["NotificationTitle"] = "Thông báo.";
                    GetSession(); return RedirectToAction("QLTaiKhoan");

                }
                else
                {
                   GetSession(); return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm tài khoản nhân viên.");

                }

            }
            catch (Exception ex)
            {
               GetSession(); return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }
    }
}
