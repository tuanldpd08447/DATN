using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DATN_QLTiemChung.Controllers
{
    public class QLKhoVaccineController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;

        public QLKhoVaccineController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;


        }
        public async Task<IActionResult> Clickvaccine(string IDVT)
        {
            var hd = _httpClientFactory.CreateClient();

          
            var vattuResponse = await hd.GetAsync($"https://localhost:7143/api/DataQLKhoVaccine/GetAllBYIDVT/{IDVT}");
           
                var vattuApiResponse = await vattuResponse.Content.ReadAsStringAsync();
                VatTuDTO vattuyte = JsonConvert.DeserializeObject<VatTuDTO>(vattuApiResponse);
                ViewBag.vattu = vattuyte;
            
         

            var Response = await hd.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetAllVatTu");
            var apiResponses = await Response.Content.ReadAsStringAsync();
            List<VatTuDTO> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuDTO>>(apiResponses);
            ViewBag.vatTuYTe = vatTuYTe;

            var Response1 = await hd.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetLoaiVatTu");
            var apiResponses1 = await Response1.Content.ReadAsStringAsync();
            List<LoaivatTu> loaivattu = JsonConvert.DeserializeObject<List<LoaivatTu>>(apiResponses1);
            ViewBag.loaivattu = loaivattu;


            var Response2 = await hd.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetNguonCap");
            var apiResponses2 = await Response2.Content.ReadAsStringAsync();
            List<NguonCungCap> NguonCap = JsonConvert.DeserializeObject<List<NguonCungCap>>(apiResponses2);
            ViewBag.NguonCap = NguonCap;

            var Response3 = await hd.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetNhacap");
            var apiResponses3 = await Response3.Content.ReadAsStringAsync();
            List<NhaCungCap> NhaCap = JsonConvert.DeserializeObject<List<NhaCungCap>>(apiResponses3);
            ViewBag.NhaCap = NhaCap;

            return View("~/Views/Home/QLKho.cshtml");
        }
        public async Task<IActionResult> QLKhoVaccine()
        {
            var client = _httpClientFactory.CreateClient();

            var Response = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetAllVatTu");
            var apiResponses = await Response.Content.ReadAsStringAsync();
            List<VatTuDTO> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuDTO>>(apiResponses);
            ViewBag.vatTuYTe= vatTuYTe;

            var Response1 = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetLoaiVatTu");
            var apiResponses1 = await Response1.Content.ReadAsStringAsync();
            List<LoaivatTu> loaivattu = JsonConvert.DeserializeObject<List<LoaivatTu>>(apiResponses1);
            ViewBag.loaivattu = loaivattu;


            var Response2 = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetNguonCap");
            var apiResponses2 = await Response2.Content.ReadAsStringAsync();
            List<NguonCungCap> NguonCap = JsonConvert.DeserializeObject<List<NguonCungCap>>(apiResponses2);
            ViewBag.NguonCap = NguonCap;

            var Response3 = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetNhacap");
            var apiResponses3 = await Response3.Content.ReadAsStringAsync();
            List<NhaCungCap> NhaCap = JsonConvert.DeserializeObject<List<NhaCungCap>>(apiResponses3);
            ViewBag.NhaCap = NhaCap;

            return View("~/Views/Home/QLKho.cshtml");
        }
    }
    }
