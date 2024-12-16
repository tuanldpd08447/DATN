using Microsoft.AspNetCore.Mvc;
using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.AccessControl;
using System.Text;

namespace DATN_QLTiemChung.Controllers
{
    public class QLTaiKhoanController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;

        public QLTaiKhoanController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;


        }

        public async Task<IActionResult> QLTaiKhoan()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7143/api/QLTaiKhoan/GetAllTaiKhoanNV");


            var apiResponse = await response.Content.ReadAsStringAsync();
            List<QLyTaiKhoanNV> qltknhanViens = JsonConvert.DeserializeObject<List<QLyTaiKhoanNV>>(apiResponse);
            ViewBag.QLTaiKhoanNVs = JsonConvert.SerializeObject(qltknhanViens);



            var response0 = await client.GetAsync("https://localhost:7143/api/QLTaiKhoan/GetAllTaiKhoanKH");


            var apiResponse1 = await response0.Content.ReadAsStringAsync();
            List<QLyTaiKhoanKH> qltkkhachHangs = JsonConvert.DeserializeObject<List<QLyTaiKhoanKH>>(apiResponse1);

            ViewBag.QLTaiKhoanKHs = JsonConvert.SerializeObject(qltkkhachHangs);

            //GẤP ĐÔI PHẦN TRÊN ĐỂ QLKH

            return View("/Views/Home/QLTaiKhoan.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaiKhoanNV(string IDTKNV, string IDNV, string email, string password, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //create HTTP client
                var client = _httpClientFactory.CreateClient();

                // map input parameters to the DTO 
                var createTaiKhoanNVDTO = new CreateTaiKhoanNVDTO
                {
                    IDTKNV = IDTKNV,
                    IDNV = IDNV,
                    Email = email,
                    MatKhau = password
                };
                // Serialize the object to JSON

                var content = new StringContent(JsonConvert.SerializeObject(createTaiKhoanNVDTO), Encoding.UTF8, "application/json");
                // Send the POST request
                var response = await client.PostAsync("https://localhost:7143/api/QLTaiKhoan/CreateTaiKhoanNV", content);


                if (response.IsSuccessStatusCode)
                {
                    var apiResonse = await response.Content.ReadAsStringAsync();
                    var addedNhanVien = JsonConvert.DeserializeObject<QLTaiKhoanNVDTO>(apiResonse);


                    return RedirectToAction("QLTaiKhoan");

                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm tài khoản nhân viên.");

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMatKhauTKNV(string IDTKNV, string IDNV, string email, string NewPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Dữ liệu không hợp lệ
            }
            try
            {
                // Tạo đối tượng HttpClient
                var nv = _httpClientFactory.CreateClient();

                var qlTaiKhoanNVDTO = new QLTaiKhoanNVDTO
                {
                    IDTKNV = IDTKNV,
                    IDNV = IDNV,
                    Email = email,
                    MatKhau = NewPassword,
                };
                // Serialize đối tượng DTO thành JSON
                var content = new StringContent(JsonConvert.SerializeObject(qlTaiKhoanNVDTO), Encoding.UTF8, "application/json");

                // Gửi yêu cầu PUT đến API
                var response = await nv.PutAsync($"https://localhost:7143/api/QLTaiKhoan/UpdateMatKhauTKNV/{IDTKNV}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Nếu thành công, có thể lấy lại danh sách nhân viên để cập nhật giao diện
                    return RedirectToAction("QLTaiKhoan"); // Chuyển hướng về danh sách nhân viên
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

        public async Task<IActionResult> ClickTK(string IDTKNV)
        {
            var client = _httpClientFactory.CreateClient();


            var response = await client.GetAsync("https://localhost:7143/api/QLTaiKhoan/GetTKNhanVienById/" + IDTKNV);

            if (response.IsSuccessStatusCode)
            {
                var nhanvienapiResponse = await response.Content.ReadAsStringAsync();
                var qlTaiKhoanNV = JsonConvert.DeserializeObject<QLTaiKhoanNVDTO>(nhanvienapiResponse);
                ViewBag.QLTaiKhoanNV = qlTaiKhoanNV;

                var response1 = await client.GetAsync("https://localhost:7143/api/QLTaiKhoan/GetAllTaiKhoanNV");


                var apiResponse = await response1.Content.ReadAsStringAsync();
                List<QLyTaiKhoanNV> qltknhanViens = JsonConvert.DeserializeObject<List<QLyTaiKhoanNV>>(apiResponse);
                ViewBag.QLTaiKhoanNVs = JsonConvert.SerializeObject(qltknhanViens);



                var response0 = await client.GetAsync("https://localhost:7143/api/QLTaiKhoan/GetAllTaiKhoanKH");


                var apiResponse1 = await response0.Content.ReadAsStringAsync();
                List<QLyTaiKhoanKH> qltkkhachHangs = JsonConvert.DeserializeObject<List<QLyTaiKhoanKH>>(apiResponse1);

                ViewBag.QLTaiKhoanKHs = JsonConvert.SerializeObject(qltkkhachHangs);




                return View("/Views/Home/QLTaiKhoan.cshtml");
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải thông tin nhân viên";
                return View("/Views/Home/QLTaiKhoan.cshtml");
            }


            // Nếu thành công, có thể lấy lại danh sách nhân viên để cập nhật giao diện
            // Chuyển hướng về danh sách nhân viên

        }

        [HttpPost]
        public async Task<IActionResult> CreateTaiKhoanKH(string IDTKKH, string IDKH, string sdt, string password, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //create HTTP client
                var client = _httpClientFactory.CreateClient();

                // map input parameters to the DTO 
                var createTaiKhoanKHDTO = new CreateTaiKhoanKHDTO
                {
                    IDTKKH = IDTKKH,
                    IDKH = IDKH,
                    SDT = sdt,
                    MatKhau = password
                };
                // Serialize the object to JSON

                var content = new StringContent(JsonConvert.SerializeObject(createTaiKhoanKHDTO), Encoding.UTF8, "application/json");
                // Send the POST request
                var response = await client.PostAsync("https://localhost:7143/api/QLTaiKhoan/CreateTaiKhoanKH", content);


                if (response.IsSuccessStatusCode)
                {
                    var apiResonse = await response.Content.ReadAsStringAsync();
                    var addedKhachhang = JsonConvert.DeserializeObject<QLTaiKhoanKHDTO>(apiResonse);


                    return RedirectToAction("QLTaiKhoan");

                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Đã có lỗi xảy ra khi thêm tài khoản nhân viên.");

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi khi kết nối với máy chủ: {ex.Message}");
            }
        }
    }
}
