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

            return response.IsSuccessStatusCode;
        }

        [HttpPost]
        public async Task<IActionResult> SendOtpEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.ErrorMessage = "Email không hợp lệ.";
                return View();
            }
            HttpContext.Session.Remove("OTP");

            // Tạo OTP ngẫu nhiên
            var otp = new Random().Next(100000, 999999).ToString();  

      
            HttpContext.Session.SetString("OTP", otp);

            bool isOtpSent = await SendOtpViaApi(email, otp);

            if (isOtpSent)
            {
                ViewBag.SuccessMessage = "OTP đã được gửi vào email của bạn.";
            }
            else
            {
                ViewBag.ErrorMessage = "Có lỗi khi gửi email. Vui lòng thử lại.";
            }

            return View();
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

                return RedirectToAction("Home", "Home");
            }

            ViewBag.ErrorMessage = "Thông tin đăng nhập không chính xác.";
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
                return View("~/Views/Home/Register.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.register = registerDto;
                ViewBag.DiaChi = dc;
                TempData["errorAPI"] = $"Lỗi hệ thống: {ex.Message}";
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
            return RedirectToAction("Home", "Home"); 
        }
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml"); ;
        }
    }
}
