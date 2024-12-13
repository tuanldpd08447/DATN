using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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


            var Response4 = await hd.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/Getxuatxu");
            var apiResponses4 = await Response4.Content.ReadAsStringAsync();
            List<XuatXu> Xuatxu = JsonConvert.DeserializeObject<List<XuatXu>>(apiResponses4);
            ViewBag.Xuatxu = Xuatxu;

            return View("~/Views/Home/QLKho.cshtml");
        }
        public async Task<IActionResult> QLKhoVaccine()
        {
            var client = _httpClientFactory.CreateClient();

            var Response = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetAllVatTu");
            var apiResponses = await Response.Content.ReadAsStringAsync();
            List<VatTuDTO> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuDTO>>(apiResponses);
            ViewBag.vatTuYTe = vatTuYTe;

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


            var Response4 = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/Getxuatxu");
            var apiResponses4 = await Response4.Content.ReadAsStringAsync();
            List<XuatXu> Xuatxu = JsonConvert.DeserializeObject<List<XuatXu>>(apiResponses4);
            ViewBag.Xuatxu = Xuatxu;


            return View("~/Views/Home/QLKho.cshtml");
        }

        public async Task<IActionResult> Findvattu(string? IDVT, string? IDTL, string? TenVatTu,
            string? IDNGC, string? IDNHC, DateTime? HanSuDung)
        {
            if (IDVT == null) { IDVT = null; }
            if (IDTL == null) { IDTL = null; }
            if (TenVatTu == null) { TenVatTu = null; }
            if (IDNGC == null) { IDNGC = null; }
            if (IDNHC == null) { IDNHC = null; }
            if (HanSuDung == null) { HanSuDung = null; }
            var find = new FindvattuDTO
            {
                IDVT = IDVT,
                IDTL = IDTL,
                TenVatTu = TenVatTu,
                IDNGC = IDNGC,
                IDNHC = IDNHC,
                HanSuDung = HanSuDung,
            };

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(find), Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await client.PostAsync("https://localhost:7143/api/DataQLKhoVaccine/Findvattu", content);
            if (response.IsSuccessStatusCode)
            {
                var apiResponses = await response.Content.ReadAsStringAsync();
                List<VatTuDTO> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuDTO>>(apiResponses);
                ViewBag.vatTuYTe = vatTuYTe;
            }
            var vattuResponse = await client.GetAsync($"https://localhost:7143/api/DataQLKhoVaccine/GetAllBYIDVT/{IDVT}");

            var vattuApiResponse = await vattuResponse.Content.ReadAsStringAsync();
            VatTuDTO vattuyte = JsonConvert.DeserializeObject<VatTuDTO>(vattuApiResponse);
            ViewBag.vattu = vattuyte;

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


            var Response4 = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/Getxuatxu");
            var apiResponses4 = await Response4.Content.ReadAsStringAsync();
            List<XuatXu> Xuatxu = JsonConvert.DeserializeObject<List<XuatXu>>(apiResponses4);
            ViewBag.Xuatxu = Xuatxu;

            return View("~/Views/Home/QLKho.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> AddVaccine(string MaTL, string TenVatTu, string? MaNGC, string? MaNHC, string MaXX,
            double DonGia, string? GhiChu, DateTime HanSuDung, int SoLuong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            try
            {
                // Create HTTP client
                var client = _httpClientFactory.CreateClient();
                // Map input parameters to the DTO
                var vaccinecreatedto = new VaccineCreateDTO
                {
                    IDTL = MaTL,
                    TenVatTu = TenVatTu,
                    IDNGC = MaNGC,
                    IDNHC = MaNHC,
                    SoLuong = SoLuong,
                    DonGia = DonGia,
                    GhiChu = GhiChu,
                    IDXX = MaXX,
                    HanSuDung = HanSuDung,

                };

                // Serialize the object to JSON
                var content = new StringContent(JsonConvert.SerializeObject(vaccinecreatedto), Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await client.PostAsync("https://localhost:7143/api/DataQLKhoVaccine/AddVaccine", content);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var addevaccine = JsonConvert.DeserializeObject<VatTuYTe>(apiResponse);

                    // Get all customers after adding
                    var allResponse = await client.GetAsync("https://localhost:7143/api/DataQLKhoVaccine/GetAllVatTu");
                    if (allResponse.IsSuccessStatusCode)
                    {
                        var allApiResponse = await allResponse.Content.ReadAsStringAsync();
                        List<VatTuYTe> vaccines = JsonConvert.DeserializeObject<List<VatTuYTe>>(allApiResponse);
                        TempData["Notification"] = "Lấy danh sách thành công.";
                        TempData["NotificationType"] = "success";
                        TempData["NotificationTitle"] = "Thông báo.";
                        return RedirectToAction("QLKhoVaccine");
                    }
                    else
                    {
                        return StatusCode((int)allResponse.StatusCode, "Không thể lấy danh sách khách hàng.");
                    }
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm khách hàng.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }




        public async Task<IActionResult> UpdateVaccine(string MaVT, string MaTL, string TenVatTu, string? MaNGC, string? MaNHC, string MaXX,
            double DonGia, string? GhiChu, DateTime HanSuDung, int SoLuong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return error if data is invalid
            }
            try
            {
                var vt = _httpClientFactory.CreateClient();
                var vaccineUpdateDTO = new VaccineCreateDTO
                {
                    IDTL = MaTL,
                    TenVatTu = TenVatTu,
                    IDNGC = MaNGC,
                    IDNHC = MaNHC,
                    IDXX = MaXX,
                    DonGia = DonGia,
                    GhiChu = GhiChu,
                    HanSuDung = HanSuDung,
                    SoLuong = SoLuong,

                };
                var content = new StringContent(JsonConvert.SerializeObject(vaccineUpdateDTO), Encoding.UTF8, "application/json");

                var response = await vt.PutAsync($"https://localhost:7143/api/DataQLKhoVaccine/UpdateVaccine/{MaVT}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("QLKhoVaccine");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Không thể cập nhật nhân viên.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }

        }

    }
}
