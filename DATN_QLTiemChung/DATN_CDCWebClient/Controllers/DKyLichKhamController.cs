using DATN_CDCWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DATN_CDCWebClient.Controllers
{
    public class DKyLichKhamController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private string id = null;
        public DKyLichKhamController(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
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
            id = userId;
        }
        public async Task<IActionResult> DKyLichKham()
        {
            GetSession();
            if (id == null) 
            {
                return View("~/Views/Home/Login.cshtml");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                var khTask = client.GetAsync($"https://localhost:7143/api/DKyLichKham/GetThongtinCaNhan/{id}");
                var diaChiTask = khTask.ContinueWith(async task =>
                {
                    var response = await task;
                    if (!response.IsSuccessStatusCode)    return null;
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var kh = JsonConvert.DeserializeObject<KhachHang>(apiResponse);
                    ViewBag.Thongtinkh = kh;

                     return client.GetAsync($"https://localhost:7143/api/Data/GetWardByid/{kh?.IDXP}");
                }).Unwrap();
                var lichKhamTask = client.GetAsync($"https://localhost:7143/api/DKyLichKham/GetAll");

                await Task.WhenAll(khTask, diaChiTask, lichKhamTask);

                if (diaChiTask.Result != null)
                {
                    var diaChiResponse = await diaChiTask.Result;
                    if (diaChiResponse.IsSuccessStatusCode)
                    {
                        var apiResponse = await diaChiResponse.Content.ReadAsStringAsync();
                        var diaChi = JsonConvert.DeserializeObject<DiaChi>(apiResponse);
                        ViewBag.DiaChi = diaChi;
                    }
                }

                if (lichKhamTask.Result.IsSuccessStatusCode)
                {
                    var apiResponse = await lichKhamTask.Result.Content.ReadAsStringAsync();

                    var datLichKham = JsonConvert.DeserializeObject<List<DatLichKham>>(apiResponse);

                  
                    ViewBag.LichKhamList = JsonConvert.SerializeObject(datLichKham) ;
                    Console.WriteLine(ViewBag.LichKhamList);
                }
            }
            catch (Exception ex)
            {

                ViewBag.Error = "Đã xảy ra lỗi khi lấy dữ liệu.";
            }

              GetSession(); TempData["Notification"] = "Đăng ký lịch khám thành công.";
            TempData["NotificationType"] = "success";
            TempData["NotificationTitle"] = "Thông báo."; return View("~/Views/Home/DKyLichKham.cshtml");
        }
        public async Task<IActionResult> DKyLichKhamSumit(string IDKH, DateOnly NgayHen, string ThoiGianHen)
        {
            DatLichKham datLichKham = new DatLichKham
            {
                IDKH = IDKH,
                IDLK = "",  
                ThoiGian = NgayHen.ToDateTime(TimeOnly.Parse(ThoiGianHen))  
            };

            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(datLichKham), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7143/api/DKyLichKham/ThemLichKham", content);

            if (response.IsSuccessStatusCode)
            {
                  GetSession(); TempData["Notification"] = "Tạo lịch khám thành công.";
                TempData["NotificationType"] = "success";
                TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("Home", "Home");
            }
            else
            {
         
                TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi tạo lịch khám. Vui lòng thử lại!";
                
                  GetSession(); TempData["Notification"] = "Tạo lịch khám thất bại.";
                TempData["NotificationType"] = "error";
                TempData["NotificationTitle"] = "Thông báo."; return RedirectToAction("DKyLichKham");
            }
        }


    }
}
