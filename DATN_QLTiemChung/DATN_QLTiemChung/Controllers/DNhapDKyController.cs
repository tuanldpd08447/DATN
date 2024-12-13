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
        public async Task<IActionResult> LoginSumit(string email ,string password)
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
                LoginResult NhanVien = JsonConvert.DeserializeObject<LoginResult>(apiResponse);

                HttpContext.Session.SetString("Username", NhanVien.Username);
                HttpContext.Session.SetString("Role", NhanVien.Role);
                HttpContext.Session.SetString("ID", NhanVien.ID);

                GetSession();
                TempData["Notification"] = "Đăng nhập thành công.";
                TempData["NotificationType"] = "success";
                TempData["NotificationTitle"] = "Thông báo.";
                return RedirectToAction("HomeQL", "Home");
            }
                GetSession();
                ViewBag.ErrorMessage = "Thông tin đăng nhập không chính xác.";
                TempData["Notification"] = "Đăng nhập thất bại. Thông tin đăng nhập không chính xác.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo.";
                return RedirectToAction("Login");

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
