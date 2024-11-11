using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DATN_QLTiemChung.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult HomeQL()
        {
            return View();
        }
        public IActionResult KhamSanLoc()
        {
            return View();
        }
        public IActionResult PhieuLinh()
        {
            return View();
        }
        public IActionResult QLHoaDon()
        {
            return View();
        }
        public IActionResult QLKhachHang()
        {
            return View();
        }
        public IActionResult QLKho()
        {
            return View();
        }
        public IActionResult QLNhanVien()
        {
            return View();
        }
        public IActionResult QLTiemChung()
        {
            return View();
        }
        public IActionResult QLTiepNhan()
        {
            return View();
        }
        public IActionResult QLXuatNhapKho()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
