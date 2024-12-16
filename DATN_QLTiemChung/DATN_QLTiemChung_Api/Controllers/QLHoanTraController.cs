using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLHoanTraController : ControllerBase
    {
        private readonly DBContext _context;

        public QLHoanTraController(DBContext context)
        {
            _context = context;
        }
        [HttpGet("GetSoMuiDaTiem/{IDKH}")]
        public async Task<IActionResult> GetSoMuiDaTiem( string IDKH)
        {
    

            var theoDoiSauTiemList = await _context.TheoDoiSauTiem
           .Include(st => st.TiemChung)
           .ThenInclude(tc => tc.DangKyTiemChung)
           .ThenInclude(dktc => dktc.DangKyVaccine)
           .Include(kh => kh.KhachHang)
           .Where(st => st.TrangThai == false &&  st.IDKH == IDKH)
           .ToListAsync();
            var khachHangHT = theoDoiSauTiemList
              .Where(st => st.TiemChung.DangKyTiemChung.DangKyVaccine.SoLuong > int.Parse(st.TiemChung.DangKyTiemChung.GhiChu))
              .OrderByDescending(st => st.TiemChung.DangKyTiemChung.ThoiGianTiem)
              .Select(st => st.TiemChung.DangKyTiemChung.GhiChu)
              .FirstOrDefault();

            if (khachHangHT == null)
            {
                return NotFound("Không tìm thấy thông tin tiêm chủng cho IDVT hoặc IDKH này.");
            }

            return Ok(khachHangHT);
        }

        [HttpGet("GetAllHoanTra")]
        public async Task<IActionResult> GetAllHoanTra()
        {
            // Lấy tất cả các thông tin hoàn trả
            var hoanTraList = await _context.HoanTra
                .ToListAsync();

            if (hoanTraList == null || !hoanTraList.Any())
            {
                return NotFound("Không tìm thấy thông tin hoàn trả.");
            }

            // Lấy thông tin hóa đơn cũ và hóa đơn mới cho mỗi hoàn trả
            var hoanTraDTOs = new List<HoanTraDTO>();

            foreach (var hoanTra in hoanTraList)
            {
                var hoaDonCu = await _context.HoaDon
                    .Include(hd => hd.HoaDonChiTiets)
                    .ThenInclude(hdct => hdct.VatTuYTe)
                    .Include(hd => hd.KhachHang)
                    .Where(hd => hd.IDHD == hoanTra.HoaDonCu)
                    .FirstOrDefaultAsync();

                var hoaDonMoi = await _context.HoaDon
                    .Include(hd => hd.HoaDonChiTiets)
                    .ThenInclude(hdct => hdct.VatTuYTe)
                     .Include(hd => hd.KhachHang)
                    .Where(hd => hd.IDHD == hoanTra.HoaDonMoi)
                    .FirstOrDefaultAsync();

                if (hoaDonCu == null || hoaDonMoi == null)
                    continue;


                var hoaDonCuChiTiets = hoaDonCu.HoaDonChiTiets?.ToList() ?? new List<HoaDonChiTiet>();
                var hoaDonMoiChiTiets = hoaDonMoi.HoaDonChiTiets?.ToList() ?? new List<HoaDonChiTiet>();

                var hoanTraDTO = new HoanTraDTO
                {
                    IDHT = hoanTra.IDHT,
                    IDKH = hoanTra.IDKH,
                    LyDo = hoanTra.LyDo,
                    ThoiGian = hoanTra.ThoiGian,
                    DonGia = hoanTra.DonGia,
                    ThanhTien = hoanTra.ThanhTien,
                    HoaDonCu = new HoaDonDTO2
                    {
                        IDHD = hoaDonCu.IDHD,
                        IDNV = hoaDonCu.IDNV,
                        KhachHang = hoaDonCu.KhachHang?.TenKhachHang ?? "Không có tên khách hàng",
                        ThoiGian = hoaDonCu.ThoiGian,
                        TongTien = hoaDonCu.TongTien,
                        NoiDung = hoaDonCu.NoiDung,
                        HoaDonChiTiets = hoaDonCuChiTiets.Select(hdct => new HoaDonChiTietDTO
                        {
                            IDVT = hdct.IDVT,
                            TenVT = hdct.VatTuYTe?.TenVatTu ?? "Không có tên vật tư",
                            DonGia = hdct.DonGia,
                            SoLuong = hdct.SoLuong,
                            ThanhTien = hdct.ThanhTien
                        }).ToList()
                    },
                    HoaDonMoi = new HoaDonDTO2
                    {
                        IDHD = hoaDonMoi.IDHD,
                        IDNV = hoaDonMoi.IDNV,
                        KhachHang = hoaDonMoi.KhachHang?.TenKhachHang ?? "Không có tên khách hàng",
                        ThoiGian = hoaDonMoi.ThoiGian,
                        TongTien = hoaDonMoi.TongTien,
                        NoiDung = hoaDonMoi.NoiDung,
                        HoaDonChiTiets = hoaDonMoiChiTiets.Select(hdct => new HoaDonChiTietDTO
                        {
                            IDVT = hdct.IDVT,
                            TenVT = hdct.VatTuYTe?.TenVatTu ?? "Không có tên vật tư",
                            DonGia = hdct.DonGia,
                            SoLuong = hdct.SoLuong,
                            ThanhTien = hdct.ThanhTien
                        }).ToList()
                    }
                };

                hoanTraDTOs.Add(hoanTraDTO);
            }

            // Trả về danh sách các hóa đơn hoàn trả
            return Ok(hoanTraDTOs);
        }



        [HttpGet("DSHoanTra")]
        public async Task<ActionResult> DSHoanTra()
        {
            // Lấy danh sách IDKH từ bảng HoanTra
            var hoanTraIDs = await _context.HoanTra
                .Select(ht => ht.IDKH)
                .ToListAsync();


            var theoDoiSauTiemList = await _context.TheoDoiSauTiem
                .Include(st => st.TiemChung)
                .ThenInclude(tc => tc.DangKyTiemChung)
                .ThenInclude(dktc => dktc.DangKyVaccine)
                .Include(kh => kh.KhachHang)
                .Where(st => st.TrangThai == false && !hoanTraIDs.Contains(st.IDKH))
                .ToListAsync();


            var khachHangHT = theoDoiSauTiemList
                .Where(st => st.TiemChung.DangKyTiemChung.DangKyVaccine.SoLuong > int.Parse(st.TiemChung.DangKyTiemChung.GhiChu))
                .Select(kh => new DSHoanTraDTO
                {
                    IDKH = kh.IDKH.ToString(),
                    GhiChu = kh.GhiChu,
                    ThoiGian = DateOnly.FromDateTime(kh.TiemChung.ThoiGian),
                    KhachHang = kh.KhachHang != null ? new KhachHang
                    {
                        IDKH = kh.KhachHang.IDKH,
                        TenKhachHang = kh.KhachHang.TenKhachHang,
                    } : null,
                })
                .ToList();
            return Ok(khachHangHT);
        }


        [HttpGet("HoaDonHT/{IDKH}")]
        public async Task<ActionResult> HoaDonHT(string IDKH)
        {
            var hoaDon = await _context.HoaDon.Include(h => h.KhachHang)
                .Include(hd => hd.NhanVien)
                .Include(hd => hd.HoaDonChiTiets)
               .Where(st => st.IDKH == IDKH)
              .OrderByDescending(st => st.ThoiGian)
              .FirstOrDefaultAsync();

            if (hoaDon == null)
            {
                return NotFound();
            }

            // Chuyển đổi từ entity sang ViewModel
            HoaDonHT hoaDonHT = new HoaDonHT
            {
                IDHD = hoaDon.IDHD,
                IDKH = hoaDon.IDKH,
                ThoiGian = hoaDon.ThoiGian,
                TongTien = hoaDon.TongTien,
                NoiDung = hoaDon.NoiDung,
                TrangThai = hoaDon.TrangThai,
                ThanhToan = hoaDon.ThanhToan,
                GhiChu = hoaDon.GhiChu,
                KhachHang = hoaDon.KhachHang != null ? new KhachHang
                {
                    IDKH = hoaDon.KhachHang.IDKH,
                    TenKhachHang = hoaDon.KhachHang.TenKhachHang,
                } : null,
                HoaDonChiTiet = hoaDon.HoaDonChiTiets != null ? hoaDon.HoaDonChiTiets.Select(hdct => new HoaDonChiTiet
                {
                    IDVT = hdct.IDVT,
                    DonGia = hdct.DonGia,
                    SoLuong = hdct.SoLuong,
                }).ToList() ?? new List<HoaDonChiTiet>() // Lặp qua các chi tiết hóa đơn
            : null,
                NhanVien = hoaDon.NhanVien != null ? new NhanVien
                {
                    IDNV = hoaDon.NhanVien.IDNV,
                    TenNhanVien = hoaDon.NhanVien.TenNhanVien,
                } : null,
            };

            return Ok(hoaDonHT);
        }

        private async Task<string> GenerateNewIDHT()
        {
            // Lấy ID lớn nhất từ database
            var idHT = await _context.HoanTra
                .OrderByDescending(o => o.IDHT)
                .FirstOrDefaultAsync();

            string newIDHT;
            if (idHT == null || string.IsNullOrEmpty(idHT.IDHT))
            {
                newIDHT = "HT001";
            }
            else
            {
                string idHTNumber = idHT.IDHT.Substring(2);
                int number = int.Parse(idHTNumber) + 1;

                newIDHT = $"HT{number:D3}";
            }

            return newIDHT;
        }
        private async Task<string> GenerateNewIDHD()
        {
            // Lấy ID lớn nhất từ database
            var idHD = await _context.HoaDon
                .OrderByDescending(o => o.IDHD)
                .FirstOrDefaultAsync();

            string newIDHD;
            if (idHD == null || string.IsNullOrEmpty(idHD.IDHD))
            {
                newIDHD = "HD001";
            }
            else
            {
                string idHDNumber = idHD.IDHD.Substring(2);
                int number = int.Parse(idHDNumber) + 1;

                newIDHD = $"HD{number:D3}";
            }

            return newIDHD;
        }
        private async Task<string> GenerateNewIDHDCT()
        {
            // Lấy khách hàng có IDHD lớn nhất hiện tại
            var lastHDKH = await _context.HoaDonChiTiet
                .OrderByDescending(HD => HD.IDHDCT)
                .FirstOrDefaultAsync();

            string newIDHDCT;

            if (lastHDKH != null)
            {
                // Lấy phần số cuối cùng từ IDHD (giả sử dạng HD001, HD002,...)
                string lastNumber = lastHDKH.IDHD.Substring(2); // Lấy phần sau "HD"
                int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
                newIDHDCT = "HDCT" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
            }
            else
            {
                // Nếu chưa có HDách hàng nào, bắt đầu từ HD001
                newIDHDCT = "HDCT001";
            }

            return newIDHDCT;
        }

        [HttpPost("AddNewHoaDon")]
        public async Task<IActionResult> AddNewHoaDon([FromBody] HoaDonHT HoanTraDto)
        {
            // Kiểm tra dữ liệu đầu vào
            if (HoanTraDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu hóa đơn không hợp lệ." });
            }

            try
            {
                // Kiểm tra hóa đơn có tồn tại không
                var hoaDon = await _context.HoaDonChiTiet
                    .Include(hd => hd.HoaDon)
                    .Where(st => st.IDHD == HoanTraDto.IDHD && st.IDVT == HoanTraDto.IDVT)
                    .FirstOrDefaultAsync();

                if (hoaDon == null)
                {
                    return NotFound(new { Message = "Không tìm thấy hóa đơn chi tiết tương ứng." });
                }

                // Tạo hóa đơn mới
                string newIDHD = await GenerateNewIDHD();
                var newHoaDon = new HoaDon
                {
                    IDHD = newIDHD,
                    IDKH = HoanTraDto.IDKH,
                    IDNV = HoanTraDto.IDNV,
                    ThoiGian = HoanTraDto.ThoiGian,
                    GhiChu = "Tiền mặt",
                    NoiDung = HoanTraDto.NoiDung,
                    TongTien = HoanTraDto.DonGia * HoanTraDto.SoLuong,
                    TrangThai = HoanTraDto.TrangThai,
                    ThanhToan = HoanTraDto.ThanhToan,
                };

                // Tạo hóa đơn chi tiết mới
                string newIDHDCT = await GenerateNewIDHDCT();
                var newHoaDonChiTiet = new HoaDonChiTiet
                {
                    IDHDCT = newIDHDCT,
                    IDHD = newIDHD,
                    IDVT = HoanTraDto.IDVT,
                    SoLuong = HoanTraDto.SoLuong,
                    DonGia = HoanTraDto.DonGia,
                    ThanhTien = HoanTraDto.DonGia * HoanTraDto.SoLuong,
                    GhiChu = null,
                };

                // Tạo hóa đơn hoàn trả mới
                string newIDHT = await GenerateNewIDHT();
                var newHoanTra = new HoanTra
                {
                    IDHT = newIDHT,
                    IDNV = HoanTraDto.IDNV,
                    IDKH = HoanTraDto.IDKH,
                    HoaDonCu = HoanTraDto.IDHD,
                    HoaDonMoi = newIDHD,
                    LyDo = HoanTraDto.GhiChu,
                    ThoiGian = HoanTraDto.ThoiGian,
                    DonGia = HoanTraDto.DonGia,
                    ThanhTien = (hoaDon.SoLuong - HoanTraDto.SoLuong >= 0) ? (hoaDon.SoLuong - HoanTraDto.SoLuong) * HoanTraDto.DonGia : 0,
                    TrangThai = HoanTraDto.TrangThai,
                };

                // Cập nhật trạng thái hóa đơn cũ
                var hoadoncu = await _context.HoaDon.FirstOrDefaultAsync(hd => hd.IDHD == HoanTraDto.HoaDonCu);
                if (hoadoncu != null)
                {
                    hoadoncu.TrangThai = true;
                    _context.HoaDon.Update(hoadoncu);
                }

                // Bắt đầu giao dịch
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    await _context.HoaDon.AddAsync(newHoaDon);
                    await _context.HoaDonChiTiet.AddAsync(newHoaDonChiTiet);
                    await _context.HoanTra.AddAsync(newHoanTra);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                return Ok(new { HoaDon = newHoaDon, HoaDonChiTiet = newHoaDonChiTiet, HoanTra = newHoanTra });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Error = ex.Message });
            }
        }
    }

}
