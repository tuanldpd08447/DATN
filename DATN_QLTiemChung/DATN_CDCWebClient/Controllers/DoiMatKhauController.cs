using DATN_CDCWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DATN_CDCWebClient.Controllers
{
    public class DoiMatKhauController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DoiMatKhauController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task<QLyTaiKhoanKHDTO> GetUserInfoAsync(string IDKH)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync($"https://localhost:7143/api/DoiMK/GetTKKhachHang?IDKH={IDKH}");
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<QLyTaiKhoanKHDTO>(apiResponse);
                }

                TempData["Error"] = "Không thể lấy thông tin tài khoản.";
                return null;
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi kết nối với API: {ex.Message}";
                return null;
            }
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

        [HttpGet]
        public async Task<IActionResult> DoiMatKhau(string IDKH)
        {
            if (string.IsNullOrEmpty(IDKH))
            {
                TempData["Error"] = "IDKH không hợp lệ.";
                GetSession();
                TempData["Notification"] = "IDKH không hợp lệ";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                return View("~/Views/Home/DoiMatKhau.cshtml");
            }

            var userInfo = await GetUserInfoAsync(IDKH);
            if (userInfo != null)
            {
                ViewBag.KhachHang = userInfo;
            }
            else
            {
                TempData["Error"] = "Không thể lấy thông tin tài khoản khách hàng.";
            }

            GetSession();
            TempData["Notification"] = "Cập nhật trạng thái hóa đơn thành công.";
            TempData["NotificationType"] = "success";
            TempData["NotificationTitle"] = "Thông báo."; 
            return View("~/Views/Home/DoiMatKhau.cshtml");
        }



        [HttpPost]
        public async Task<IActionResult> DoiMatKhau(string IDKH, string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword != ConfirmPassword)
            {
                TempData["Error"] = "Mật khẩu Nhập lại không đúng.";
                GetSession();
                TempData["Notification"] = "Mật khẩu nhập lại không đúng.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("DoiMatKhau", new { IDKH = IDKH });
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7143/api/DoiMK/GetTKKhachHang?IDKH={IDKH}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Không thể lấy thông tin tài khoản khách hàng. Vui lòng thử lại!";
                GetSession();
                TempData["Notification"] = "Không thể lấy thông tin khách hàng.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                return RedirectToAction("DoiMatKhau", new { IDKH = IDKH });
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            var tk = JsonConvert.DeserializeObject<QLyTaiKhoanKHDTO>(apiResponse);

            if (tk?.MatKhau != CurrentPassword)
            {
                TempData["Error"] = "Mật khẩu cũ không đúng!";
                GetSession();
                TempData["Notification"] = "Mật khẩu cũ không đúng.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("DoiMatKhau", new { IDKH = IDKH });
            }

            DoiMatKhauRequest doiMatKhauRequest = new DoiMatKhauRequest
            {
                IDKH = IDKH,
                MatKhauCu = CurrentPassword,
                MatKhauMoi = NewPassword,
            };

            var jsonContent = JsonConvert.SerializeObject(doiMatKhauRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response1 = await client.PostAsync("https://localhost:7143/api/DoiMK/DoiMatKhau", content);

            if (response1.IsSuccessStatusCode)
            {
                TempData["Message"] = "Mật khẩu đã được thay đổi thành công!";
            }
            else
            {
                TempData["Error"] = "Lỗi khi thay đổi mật khẩu! Vui lòng thử lại.";
            }

            GetSession();
            TempData["Notification"] = "Đổi mật khẩu thành công.";
            TempData["NotificationType"] = "success";
            TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("DoiMatKhau", new { IDKH = doiMatKhauRequest.IDKH });
        }

    }
}

