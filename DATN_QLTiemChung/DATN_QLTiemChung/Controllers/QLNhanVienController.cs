using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.AccessControl;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    [SessionActionFilter]
    [Route("QLNhanVien/[action]")]
    public class QLNhanVienController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        public QLNhanVienController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> QLNhanVien()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7143/api/QLNhanVien/GetAllNhanVien");


            var apiResponse = await response.Content.ReadAsStringAsync();
            List<NhanVienDTO> nhanViens = JsonConvert.DeserializeObject<List<NhanVienDTO>>(apiResponse);
            ViewBag.NhanViens = JsonConvert.SerializeObject(nhanViens);


            var response1 = await client.GetAsync("https://localhost:7143/api/QLNhanVien/GetPhongBan");
            var apiResponse1 = await response1.Content.ReadAsStringAsync();
            List<PhongBan> phongBan = JsonConvert.DeserializeObject<List<PhongBan>>(apiResponse1);
            ViewBag.PhongBans = phongBan;

            var response2 = await client.GetAsync("https://localhost:7143/api/QLNhanVien/GetChucDanh");
            var apiResponse2 = await response2.Content.ReadAsStringAsync();
            List<ChucDanh> chucDanh = JsonConvert.DeserializeObject<List<ChucDanh>>(apiResponse2);
            ViewBag.ChucDanhs = chucDanh;




              GetSession(); return View("/Views/Home/QLNhanVien.cshtml");
        }




        public async Task<IActionResult> AddNhanVien(string IDNV, string hoKhauXa, string hoTen, string chucVu, string IDCD, string IDPB, DateTime ngaySinh, string danToc, string gioiTinh, string cmt, string email, string sdt, string diaChiChiTiet)
        {
            if (!ModelState.IsValid)
            {
                  GetSession(); return BadRequest(ModelState);
            }
            try
            {
                //create HTTP client
                var client = _httpClientFactory.CreateClient();

                // map input parameters to the DTO 
                var nhanVienCreateDTO = new NhanVienCreateDTO
                {
                    IDNV = IDNV,
                    IDXP = hoKhauXa,
                    TenNhanVien = hoTen,
                    ChucVu = chucVu,
                    TenChucDanh = IDCD,
                    TenPhongBan = IDPB,
                    NgaySinh = ngaySinh,
                    DanToc = danToc,
                    GioiTinh = gioiTinh,
                    CCCD = cmt,
                    Email = email,
                    SoDienThoai = sdt,
                    DiaChi = diaChiChiTiet

                };
                // Serialize the object to JSON

                var content = new StringContent(JsonConvert.SerializeObject(nhanVienCreateDTO), Encoding.UTF8, "application/json");
                // Send the POST request
                var response = await client.PostAsync("https://localhost:7143/api/QLNhanVien/AddNhanVien", content);


                if (response.IsSuccessStatusCode)
                {
                    var apiResonse = await response.Content.ReadAsStringAsync();
                    var addedNhanVien = JsonConvert.DeserializeObject<NhanVienDTO>(apiResonse);

                    // get all nhân viên after adding
                    var allResponse = await client.GetAsync("https://localhost:7143/api/QLNhanVien/GetAllNhanVien");
                    if (allResponse.IsSuccessStatusCode)
                    {
                        var allApiResponse = await allResponse.Content.ReadAsStringAsync();
                        List<NhanVienDTO> nhanViens = JsonConvert.DeserializeObject<List<NhanVienDTO>>(allApiResponse);

                          GetSession(); return RedirectToAction("QLNhanVien");
                    }
                    else
                    {
                          GetSession(); return StatusCode((int)allResponse.StatusCode, "Không thể lấy danh sách nhân viên.");
                    }
                }
                else
                {
                      GetSession(); return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm nhân viên.");

                }

            }
            catch (Exception ex)
            {
                  GetSession(); return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateNhanVien(string IDNV, string hoKhauXa, string hoTen, string chucVu, string IDCD, string IDPB, DateTime ngaySinh, string danToc, string gioiTinh, string cmt, string email, string sdt, string diaChiChiTiet)
        {
            if (!ModelState.IsValid)
            {
                  GetSession(); return BadRequest(ModelState);
            }

            try
            {
                var nv = _httpClientFactory.CreateClient();
                var nhanVienUpdateDTO = new NhanVienCreateDTO
                {
                    IDXP = hoKhauXa,
                    IDNV = IDNV,
                    TenNhanVien = hoTen,
                    ChucVu = chucVu,
                    TenChucDanh = IDCD,
                    TenPhongBan = IDPB,
                    NgaySinh = ngaySinh,
                    DanToc = danToc,
                    GioiTinh = gioiTinh,
                    CCCD = cmt,
                    Email = email,
                    SoDienThoai = sdt,
                    DiaChi = diaChiChiTiet,
                };

                var content = new StringContent(JsonConvert.SerializeObject(nhanVienUpdateDTO), Encoding.UTF8, "application/json");

                var response = await nv.PutAsync($"https://localhost:7143/api/QLNhanVien/UpdateNhanVien", content);

                if (response.IsSuccessStatusCode)
                {
                      GetSession(); return RedirectToAction("QLNhanVien");
                }
                else
                {
                      GetSession(); return StatusCode((int)response.StatusCode, "Không thể cập nhật nhân viên.");
                }
            }
            catch (Exception ex)
            {
                  GetSession(); return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }








        public async Task<IActionResult> ClickNhanVien(string IDNV)
        {
            var client = _httpClientFactory.CreateClient();


            var response = await client.GetAsync("https://localhost:7143/api/QLNhanVien/GetAllBYIDNV/" + IDNV);

            if (response.IsSuccessStatusCode)
            {
                var nhanvienapiResponse = await response.Content.ReadAsStringAsync();
               var nhanVien = JsonConvert.DeserializeObject<NhanVienDTO>(nhanvienapiResponse);
                ViewBag.NhanVien = nhanVien;

                var response0 = await client.GetAsync("https://localhost:7143/api/QLNhanVien/GetAllNhanVien");


                var apiResponse0 = await response0.Content.ReadAsStringAsync();
                List<NhanVienDTO> nhanViens = JsonConvert.DeserializeObject<List<NhanVienDTO>>(apiResponse0);
                ViewBag.NhanViens = JsonConvert.SerializeObject(nhanViens);


                var response1 = await client.GetAsync("https://localhost:7143/api/QLNhanVien/GetPhongBan");
                var apiResponse1 = await response1.Content.ReadAsStringAsync();
                List<PhongBan> phongBan = JsonConvert.DeserializeObject<List<PhongBan>>(apiResponse1);
                ViewBag.PhongBans = phongBan;

                var response2 = await client.GetAsync("https://localhost:7143/api/QLNhanVien/GetChucDanh");
                var apiResponse2 = await response2.Content.ReadAsStringAsync();
                List<ChucDanh> chucDanh = JsonConvert.DeserializeObject<List<ChucDanh>>(apiResponse2);
                ViewBag.ChucDanhs = chucDanh;

                var response3 = await client.GetAsync($"https://localhost:7143/api/QLNhanVien/GetWardByid/{nhanVien.IDXP}");
                var apiResponse3 = await response3.Content.ReadAsStringAsync();
                DiaChi dc = JsonConvert.DeserializeObject<DiaChi>(apiResponse3);
                ViewBag.DiaChi = dc;

                  GetSession(); return View("/Views/Home/QLNhanVien.cshtml");
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải thông tin nhân viên";
                  GetSession(); return NotFound();
            }
         

            // Nếu thành công, có thể lấy lại danh sách nhân viên để cập nhật giao diện
             // Chuyển hướng về danh sách nhân viên

        }

        
        /*
        [HttpDelete("DeleteNhanVien")]
        public async Task<IActionResult> DeleteNhanVien()
        {

        }
        */

    }
}
