using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DATN_QLTiemChung.Controllers
{
    [SessionActionFilter]
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
        public IActionResult HomeQL()
        {
            GetSession();
            return View();
        }

        public IActionResult KhamSanLoc()
        {
              GetSession(); return View();
        }
        public IActionResult PhieuLinh()
        {
              GetSession(); return View();
        }
     
        public IActionResult QLKhachHang()
        {
              GetSession(); return View();
        }
        public IActionResult QLKho()
        {
              GetSession(); return View();
        }
        public IActionResult QLNhanVien()
        {
              GetSession(); return View();
        }
        public IActionResult QLTiemChung()
        {
              GetSession(); return View();
        }
      
        public IActionResult QLXuatNhapKho()
        {
              GetSession(); return View();
        }
        public IActionResult QLHoanTra()
        {
              GetSession(); return View();
        }
        public IActionResult NhapDuoc()
        {
              GetSession(); return View();
        }
        public IActionResult QLTaiKhoan()
        {
              GetSession(); return View();
        }
        public IActionResult TNTiemChungTheoHopDong()
        {
              GetSession(); return View();
        }
        public IActionResult XuatDuoc()
        {
              GetSession(); return View();
        }
      
        public IActionResult Privacy()
        {
              GetSession(); return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
