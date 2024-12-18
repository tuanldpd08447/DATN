using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    [SessionActionFilter]
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
        public async Task<IActionResult> LoginSubmit(string email, string password)
        {
            LoginNhanVien login = new LoginNhanVien
            {
                Email = email,
                Password = password
            };

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7143/api/DNhapDky/LoginNV", content);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                LoginResult nhanVien = JsonConvert.DeserializeObject<LoginResult>(apiResponse);

                HttpContext.Session.SetString("Username", nhanVien.Username);
                HttpContext.Session.SetString("Role", nhanVien.Role);
                HttpContext.Session.SetString("ID", nhanVien.ID);

                TempData["Notification"] = "Đăng nhập thành công.";
                TempData["NotificationType"] = "success";
                TempData["NotificationTitle"] = "Thông báo.";

                return RedirectToAction("HomeQL", "Home");
            }

            // Đăng nhập thất bại
            TempData["Notification"] = "Đăng nhập thất bại. Thông tin đăng nhập không chính xác.";
            TempData["NotificationType"] = "error";
            TempData["NotificationTitle"] = "Thông báo.";
            ViewBag.ErrorMessage = "Thông tin đăng nhập không chính xác.";
            ViewBag.Email = email;
            return View("~/Views/Home/Login.cshtml");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            GetSession();
            TempData["Notification"] = "Đăng xuất thành công.";
            TempData["NotificationType"] = "success";
            TempData["NotificationTitle"] = "Thông báo.";
            return View("~/Views/Home/HomeQL.cshtml"); 
        }
    }
}
