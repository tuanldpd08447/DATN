using DATN_CDCWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DATN_CDCWebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult Home()
        {
            GetSession();
            return View();
        }
    
        public IActionResult ThongTinCaNhan()
        {
            GetSession();
            return View();
        }
        public IActionResult QuenMatKhau()
        {
            GetSession();
            return View();
        }
        public IActionResult Privacy()
        {
            GetSession();
            return View();
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
