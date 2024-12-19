using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DATN_QLTiemChung.Models;

namespace DATN_QLTiemChung.Controllers
{
    public class ThongTinCaNhanController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public ThongTinCaNhanController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> ThongTinCaNhan(string IDNV)
        {
            var client = _httpClientFactory.CreateClient();


            var response = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLNhanVien/GetAllBYIDNV/" + IDNV);

            if (response.IsSuccessStatusCode)
            {
                var nhanvienapiResponse = await response.Content.ReadAsStringAsync();
                var nhanVien = JsonConvert.DeserializeObject<NhanVienDTO>(nhanvienapiResponse);
                ViewBag.NhanVien = nhanVien;

                var response1 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLNhanVien/GetPhongBan");
                var apiResponse1 = await response1.Content.ReadAsStringAsync();
                List<PhongBan> phongBan = JsonConvert.DeserializeObject<List<PhongBan>>(apiResponse1);
                ViewBag.PhongBans = phongBan;

                var response2 = await client.GetAsync("http://qltiemchungapi.runasp.net/api/QLNhanVien/GetChucDanh");
                var apiResponse2 = await response2.Content.ReadAsStringAsync();
                List<ChucDanh> chucDanh = JsonConvert.DeserializeObject<List<ChucDanh>>(apiResponse2);
                ViewBag.ChucDanhs = chucDanh;

                var response3 = await client.GetAsync($"http://qltiemchungapi.runasp.net/api/QLNhanVien/GetWardByid/{nhanVien.IDXP}");
                var apiResponse3 = await response3.Content.ReadAsStringAsync();
                DiaChi dc = JsonConvert.DeserializeObject<DiaChi>(apiResponse3);
                ViewBag.DiaChi = dc;

                GetSession(); return View("/Views/Home/ThongTinCaNhan.cshtml");
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải thông tin nhân viên";
                GetSession(); return NotFound();
            }


          

        }
    }
}
