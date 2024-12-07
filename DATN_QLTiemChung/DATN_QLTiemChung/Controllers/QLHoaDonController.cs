using DATN_QLTiemChung.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
        public async Task<IActionResult> ClickKhachHang(string IDKH)
        {
            var hd = _httpClientFactory.CreateClient();
            var Response = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAllVatTu");
            var Responses = await Response.Content.ReadAsStringAsync();
            List<VatTuYTe> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuYTe>>(Responses);
            ViewBag.vatTuYTe = vatTuYTe;

            var response = await hd.GetAsync("https://localhost:7143/api/QLTiepNhan/GetAllKhachHang");

            if (response.IsSuccessStatusCode)
            {
                var khachhangapiResponse = await response.Content.ReadAsStringAsync();
                List<KhachHang> khachHangs = JsonConvert.DeserializeObject<List<KhachHang>>(khachhangapiResponse);
                ViewBag.KhachHangs = khachHangs;

            }


            var foodResponse = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAllBYIDKh/" + IDKH);


            if (!foodResponse.IsSuccessStatusCode)
            {

                var khachHangResponse = await hd.GetAsync($"https://localhost:7143/api/DataQLHoaDon/GetKHBYID/{IDKH}");
                if (khachHangResponse.IsSuccessStatusCode)
                {
                    var khachHangApiResponse = await khachHangResponse.Content.ReadAsStringAsync();
                    KHDTO khachHang = JsonConvert.DeserializeObject<KHDTO>(khachHangApiResponse);
                    ViewBag.Khachhang = khachHang;
                    Console.WriteLine(khachHang.IDXP);

                    var response0 = await hd.GetAsync($"https://localhost:7143/api/Data/GetWardByid/{khachHang.IDXP}");
                    var apiResponse0 = await response0.Content.ReadAsStringAsync();
                    DiaChi dc = JsonConvert.DeserializeObject<DiaChi>(apiResponse0);
                    ViewBag.DiaChi = dc;
                }

            }
            else
            {
                var apiResponse = await foodResponse.Content.ReadAsStringAsync();
                HoaDonDTO hoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);
                ViewBag.HoaDons = hoaDon;
                var response0 = await hd.GetAsync($"https://localhost:7143/api/Data/GetWardByid/{hoaDon.KhachHang.IDXP}");
                var apiResponse0 = await response0.Content.ReadAsStringAsync();
                DiaChi dc = JsonConvert.DeserializeObject<DiaChi>(apiResponse0);
                ViewBag.DiaChi = dc;
            }


            return View("~/Views/Home/QLHoaDon.cshtml");
        }
        public async Task<IActionResult> QLHoaDon()
        {

            var hd = _httpClientFactory.CreateClient();




            var apiResponse = await hd.GetAsync("https://localhost:7143/api/DataQLHoaDon/GetAllVatTu");
            var Responses = await apiResponse.Content.ReadAsStringAsync();

            List<VatTuYTe> vatTuYTe = JsonConvert.DeserializeObject<List<VatTuYTe>>(Responses);

            ViewBag.vatTuYTe = vatTuYTe;

            var response = await hd.GetAsync("https://localhost:7143/api/QLTiepNhan/GetAllKhachHang");

            if (response.IsSuccessStatusCode)
            {
                var khachhangapiResponse = await response.Content.ReadAsStringAsync();
                List<KhachHang> khachHangs = JsonConvert.DeserializeObject<List<KhachHang>>(khachhangapiResponse);
                ViewBag.KhachHangs = khachHangs;
            }


            return View("~/Views/Home/QLHoaDon.cshtml");




        }


        [HttpPost]
        public async Task<IActionResult> AddHoaDon(string MaKH, string MaNV, string MaVT, DateTime ThoiGian,
             string NoiDung, int SoLuong, double DonGia, double ThanhTien, bool ThanhToan, string GhiChu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return error if data is invalid
            }
            try
            {
                // Create HTTP client
                var client = _httpClientFactory.CreateClient();

                // Map input parameters to the DTO
                var hoadonCreateDTO = new HoaDonCreateDTO
                {
                    IDHD = "",
                    IDHDCT = "",
                    IDKH = MaKH,
                    IDNV = MaNV,
                    IDVT = MaVT,
                    NoiDung = NoiDung,
                    ThoiGian = ThoiGian,
                    SoLuong = SoLuong,
                    DonGia = DonGia,
                    ThanhTien = ThanhTien,
                    TongTien = DonGia * SoLuong,
                    GhiChu = GhiChu,
                };

                // Serialize the object to JSON
                var content = new StringContent(JsonConvert.SerializeObject(hoadonCreateDTO), Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await client.PostAsync("https://localhost:7143/api/DataQLHoaDon/AddHoaDon", content);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var addedHoaDon = JsonConvert.DeserializeObject<HoaDonDTO>(apiResponse);
                    return RedirectToAction("QLHoaDon");

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

    }


}

