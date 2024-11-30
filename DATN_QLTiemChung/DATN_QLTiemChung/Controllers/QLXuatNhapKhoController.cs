using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DATN_QLTiemChung.Models;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    public class QLXuatNhapKhoController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public QLXuatNhapKhoController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task<IActionResult> QLXuatNhapKho()
        {
            var client = _httpClientFactory.CreateClient();
            var Response1 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetLoaiVatTu");
            var apiResponses1 = await Response1.Content.ReadAsStringAsync();
            List<LoaivatTu> loaivattu = JsonConvert.DeserializeObject<List<LoaivatTu>>(apiResponses1);
            ViewBag.loaivattu = loaivattu;


            var Response2 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetNguonCap");
            var apiResponses2 = await Response2.Content.ReadAsStringAsync();
            List<NguonCungCap> NguonCap = JsonConvert.DeserializeObject<List<NguonCungCap>>(apiResponses2);
            ViewBag.NguonCap = NguonCap;

            var Response3 = await client.GetAsync("https://localhost:7143/api/QLXuatNhapKho/GetNhacap");
            var apiResponses3 = await Response3.Content.ReadAsStringAsync();
            List<NhaCungCap> NhaCap = JsonConvert.DeserializeObject<List<NhaCungCap>>(apiResponses3);
            ViewBag.NhaCap = NhaCap;

            return View("~/Views/Home/QLXuatNhapKho.cshtml");
        }
    }
}
