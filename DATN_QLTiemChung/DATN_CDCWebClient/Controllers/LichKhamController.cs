using DATN_CDCWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN_CDCWebClient.Controllers
{
    public class LichKhamController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public LichKhamController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> LichKham(string IDKH)
        {
            var client = _httpClientFactory.CreateClient();

            var screeningResult = await client.GetAsync($"http://qltiemchungapi.runasp.net/api/DKyLichKham/LichKham/{IDKH}");

            if (screeningResult.IsSuccessStatusCode)
            {
                var apiResponse = await screeningResult.Content.ReadAsStringAsync();
                var LichKham = JsonConvert.DeserializeObject<List<LichKham>>(apiResponse);
                ViewBag.LichKham = LichKham;
            }
            GetSession();
            return View("~/Views/Home/LichKham.cshtml");
        }
        
    }
}
