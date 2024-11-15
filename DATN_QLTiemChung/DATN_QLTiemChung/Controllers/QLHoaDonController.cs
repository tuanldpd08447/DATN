using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_QLTiemChung.Controllers
{
    public class QLHoaDonController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public QLHoaDonController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;


        }
        public async Task<IActionResult> QLHoaDon()
        {

            var client = _httpClientFactory.CreateClient();
            var foodResponse = await client.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAll/" + "HD001");
            if (foodResponse.IsSuccessStatusCode)
            {
                var apiResponse = await foodResponse.Content.ReadAsStringAsync();
                HoaDonDTO hoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);

                return View("~/Views/Home/QLHoaDon.cshtml", hoaDon);
            }
            return View();
        }
    }
}
