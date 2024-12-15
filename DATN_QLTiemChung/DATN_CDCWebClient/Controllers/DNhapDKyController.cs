using DATN_CDCWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace DATN_CDCWebClient.Controllers
{
    public class DNhapDKyController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public DNhapDKyController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;


        }
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml");
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
        public async Task<IActionResult> SumitQMK( string password, string otp, string email)
        {
            // Kiểm tra nếu các thông tin cần thiết không rỗng
            if ( string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Thông tin không hợp lệ");
                return View();
            }

            // Lấy OTP từ session
            string otpFromSession = HttpContext.Session.GetString("OTP");

            if (string.IsNullOrEmpty(otpFromSession))
            {
                ModelState.AddModelError("", "OTP đã hết hạn hoặc không hợp lệ.");
                return View();
            }

            // Kiểm tra OTP nhập vào
            if (otp != otpFromSession)
            {
                TempData["ErrorMessage"] = "OTP không đúng!";
                ViewBag.SuccessSendMail = true;
                ViewBag.Email = email;
                return View("~/Views/Home/QuenMatKhau.cshtml");
            }

            // Tạo đối tượng yêu cầu (DTO)
            var updatePasswordRequest = new UpdatePasswordRequest
            {
                IDKH = HttpContext.Session.GetString("IDKH"),  // IDKH của người dùng
                MatKhau = password
            };

            // Tạo client HTTP để gọi API
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.PostAsJsonAsync("https://localhost:7143/api/DNhapDky/update-password", updatePasswordRequest);

                if (response.IsSuccessStatusCode)
                {
                    // Nếu thành công, thông báo cho người dùng
                    TempData["Message"] = "Cập nhật mật khẩu thành công!";
                    TempData["Notification"] = "Cập nhật mật khẩu thành công.";
                    TempData["NotificationType"] = "success";
                    TempData["NotificationTitle"] = "Thông báo.";
                    return RedirectToAction("Login");  
                }
                else
                {
                    // Nếu API trả về lỗi, thông báo lỗi
                    TempData["ErrorMessage"] = "Cập nhật mật khẩu thất bại! Vui lòng thử lại sau.";
                    ViewBag.SuccessSendMail = true;
                    ViewBag.Email = email;
                    TempData["Notification"] = "Cập nhật mật khẩu thất bại.";
                    TempData["NotificationType"] = "error";
                    TempData["NotificationTitle"] = "Thông báo.";
                    return View("~/Views/Home/QuenMatKhau.cshtml");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi trong trường hợp không thể kết nối API
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi kết nối với máy chủ. Vui lòng thử lại sau.";
                ViewBag.SuccessSendMail = true;
                ViewBag.Email = email;
                TempData["Notification"] = "Xảy ra lỗi.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                return View("~/Views/Home/QuenMatKhau.cshtml");
            }
        }
        public async Task<bool> SendOtpViaApi(string email, string otp)
        {
            var otpRequest = new EmailRequest
            {
                RecipientEmail = email,
                Subject = "Xác thực đăng nhập",
                Body = $"Mã OTP của bạn là: {otp}. Vui lòng không chia sẻ mã này với bất kỳ ai. Đây là mã OTP duy nhất cho phiên đăng nhập của bạn."
            };
            var client = _httpClientFactory.CreateClient();

            // Chuyển đối tượng OTP thành JSON
            var content = new StringContent(JsonConvert.SerializeObject(otpRequest), Encoding.UTF8, "application/json");

            // Thay thế URL dưới đây bằng URL của API gửi OTP của bạn
            var response = await client.PostAsync("https://localhost:7143/api/DNhapDky/send", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Lỗi gửi OTP: {errorResponse}");
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendOtpEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.ErrorMessage = "Email không hợp lệ.";
                return View();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7143/api/DNhapDky/CheckEmailDaDK/{email}");

            // Kiểm tra phản hồi từ API
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Phản hồi từ API: " + apiResponse);

                // Deserialize chuỗi JSON trả về để lấy IDKH
                string idKH = apiResponse;  // Nếu API trả về một chuỗi IDKH trực tiếp

                if (!string.IsNullOrEmpty(idKH))
                {
                    // Lưu IDKH vào session hoặc xử lý tiếp
                    HttpContext.Session.SetString("IDKH", idKH);

                    // Tiến hành các bước tiếp theo
                    HttpContext.Session.Remove("OTP");

                    // Tạo OTP ngẫu nhiên
                    var otp = new Random().Next(100000, 999999).ToString();

                    // Lưu OTP vào session
                    HttpContext.Session.SetString("OTP", otp);

                    // Gửi OTP qua email
                    bool isOtpSent = await SendOtpViaApi(email, otp);

                    if (isOtpSent)
                    {
                        ViewBag.SuccessMessage = "OTP đã được gửi vào email của bạn.";
                        ViewBag.SuccessSendMail = true;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Có lỗi khi gửi email. Vui lòng thử lại.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Không tìm thấy IDKH từ API.";
                }

                ViewBag.Email = email;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"Lỗi từ API: {errorResponse}";
                ViewBag.Email = email;
            }
            TempData["Notification"] = "Đã gửi mail.";
            TempData["NotificationType"] = "success";
            TempData["NotificationTitle"] = "Thông báo.";
            return View("~/Views/Home/QuenMatKhau.cshtml");
        }

        public async Task<IActionResult> LoginSumit(string sdt, string password)
        {
            LoginKhachHang login = new LoginKhachHang
            {
                Sdt = sdt,
                Password = password
            };
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7143/api/DNhapDky/LoginKH", content);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                LoginResult khachHangs = JsonConvert.DeserializeObject<LoginResult>(apiResponse);

                HttpContext.Session.SetString("Username", khachHangs.Username);
                HttpContext.Session.SetString("Role", khachHangs.Role);
                HttpContext.Session.SetString("ID", khachHangs.ID);

                TempData["Notification"] = "Đặng nhập thành công.";
                TempData["NotificationType"] = "success";
                TempData["NotificationTitle"] = "Thông báo.";
                return RedirectToAction("Home", "Home");
            }
            ViewBag.ErrorMessage = "Thông tin đăng nhập không chính xác.";
            TempData["Notification"] = "Đặng nhập thất bại.";
            TempData["NotificationType"] = "error";
            TempData["NotificationTitle"] = "Thông báo.";
            return View("~/Views/Home/Login.cshtml");

        }
        public async Task<IActionResult> RegisterSumit(
     string hoKhauXa, string hoTen, DateTime ngaySinh, string danToc,
     string gioiTinh, string cmt, string sdt, string email,
     string diaChiChiTiet, string password, string confirmpassword)
        {
            RegisterDto registerDto = new RegisterDto
            {
                TenKhachHang = hoTen,
                IDXP = hoKhauXa,
                NgaySinh = ngaySinh,
                DanToc = danToc,
                GioiTinh = gioiTinh,
                CCCD_MDD = cmt,
                Email = email,
                SoDienThoai = sdt,
                DiaChi = diaChiChiTiet,
                Password = password
            };

            var client = _httpClientFactory.CreateClient();
            var response0 = await client.GetAsync($"https://localhost:7143/api/Data/GetWardByid/{hoKhauXa}");
            var apiResponse = await response0.Content.ReadAsStringAsync();
            DiaChi dc = JsonConvert.DeserializeObject<DiaChi>(apiResponse);

            try
            {
                // Check email, phone, and CCCD/MDD
                var checks = new[]
                {
            CheckAvailability(client, "CheckEmail", email, "errorEmail", "Email đã được sử dụng."),
            CheckAvailability(client, "CheckPhone", sdt, "errorSdt", "Số điện thoại đã được sử dụng."),
            CheckAvailability(client, "CheckCCCD", cmt, "errorCCCD", "CCCD/MDD đã được sử dụng.")
            };

                var checkResults = await Task.WhenAll(checks);
                foreach (var result in checkResults)
                {
                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        TempData[result.ErrorKey] = result.ErrorMessage;
                        ViewBag.register = registerDto;
                        ViewBag.DiaChi = dc;
                        return View("~/Views/Home/Register.cshtml");
                    }
                }

                // Check password confirmation
                if (password != confirmpassword)
                {
                    TempData["errorConfirmpassword"] = "Mật khẩu xác nhận không khớp.";
                    ViewBag.register = registerDto;
                    TempData["Notification"] = "Mật khẩu không khớp.";
                    TempData["NotificationType"] = "error";
                    TempData["NotificationTitle"] = "Thông báo.";
                    return View("~/Views/Home/Register.cshtml");
                }

                // Send registration request
                var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7143/api/DNhapDky/khachHangRegister", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }

                // Show API error
                var errorResponse = await response.Content.ReadAsStringAsync();
                ViewBag.register = registerDto;
                ViewBag.DiaChi = dc;
                TempData["errorAPI"] = $"Đăng ký thất bại: {errorResponse}";
                TempData["Notification"] = "Đặng ký thất bại.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                return View("~/Views/Home/Register.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.register = registerDto;
                ViewBag.DiaChi = dc;
                TempData["errorAPI"] = $"Lỗi hệ thống: {ex.Message}";
                TempData["Notification"] = "Lỗi hệ thống.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                return View("~/Views/Home/Register.cshtml");
            }
        }

        // Helper method to check availability
        private async Task<(string ErrorMessage, string ErrorKey)> CheckAvailability(HttpClient client, string endpoint, string value, string errorKey, string errorMessage)
        {
            var response = await client.GetAsync($"https://localhost:7143/api/DNhapDky/{endpoint}/{value}");
            if (!response.IsSuccessStatusCode)
            {
                return (errorMessage, errorKey);
            }
            return (null, null);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Notification"] = "Đặng xuất thành công.";
            TempData["NotificationType"] = "success";
            TempData["NotificationTitle"] = "Thông báo.";
            return RedirectToAction("Home", "Home"); 
        }
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml"); ;
        }
    }
}
