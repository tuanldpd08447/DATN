using DATN_CDCWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace DATN_CDCWebClient.Controllers
{
    public class LichSuTiemController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LichSuTiemController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
        public async Task<IActionResult> LichSuTiem(string IDKH)
        {
            var client = _httpClientFactory.CreateClient();

            var screeningResult = await client.GetAsync($"https://localhost:7143/api/QLTiemChung/LsTiem/{IDKH}");

            if (screeningResult.IsSuccessStatusCode)
            {
                var apiResponse = await screeningResult.Content.ReadAsStringAsync();
                var lstiem = JsonConvert.DeserializeObject<List<LichSuTiem>>(apiResponse);
                ViewBag.LichSuTiem = lstiem;
            }
            GetSession();
            return View("~/Views/Home/LichSuTiem.cshtml");
        }

    }
}
