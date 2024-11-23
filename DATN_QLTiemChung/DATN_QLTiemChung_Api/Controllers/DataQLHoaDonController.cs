using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DATN_QLTiemChung_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataQLHoaDonController : ControllerBase
    {
        private readonly DBContext _context;

        public DataQLHoaDonController(DBContext context)
        {
            _context = context;
        }

        [HttpGet("GetDiaChi")]
        public async Task<IActionResult> GetDiaChi(string idxp)
        {
            var ward = await _context.wards
               .Include(w => w.District)
                   .ThenInclude(d => d.Province).Select(diachi => new DiaChiDTO
                   {
                       IDXP = diachi.code,
                       TenXaPhuong = diachi.full_name,
                       IDQH = diachi.District.code,
                       TenQuanHuyen = diachi.District.full_name,
                       IDTTP = diachi.District.Province.code,
                       TenTinhThanhPho = diachi.District.Province.full_name
                   }).FirstOrDefaultAsync(w => w.IDXP == idxp)
           ;

            return Ok(ward);
        }
        [HttpGet("GetAll/{IDHD}")]
        public async Task<ActionResult<HoaDonDTO>> GetAll(string IDHD)
        {
            var hoaDon = await _context.HoaDon
                .Include(hd => hd.KhachHang)
                .Include(hd => hd.NhanVien)
                .Include(hd => hd.HoaDonChiTiets)
                .FirstOrDefaultAsync(hd => hd.IDHD == IDHD);

            if (hoaDon == null)
            {
                return NotFound(new { message = "Hóa đơn không tồn tại" });
            }

            // Tạo DTO từ hóa đơn
            var dto = new HoaDonDTO
            {
                IDHD = hoaDon.IDHD,
                IDKH = hoaDon.KhachHang?.IDKH,
                IDNV = hoaDon.NhanVien?.IDNV,
                ThoiGian = hoaDon.ThoiGian,
                GhiChu = hoaDon.GhiChu,
                NoiDung = hoaDon.NoiDung,
                TongTien = hoaDon.TongTien,
                TrangThai = hoaDon.TrangThai,

                // Ánh xạ các thuộc tính cần thiết cho Khách hàng
                KhachHang = hoaDon.KhachHang != null ? new KhachHang
                {
                    IDKH = hoaDon.KhachHang.IDKH,
                    TenKhachHang = hoaDon.KhachHang.TenKhachHang,
                    SoDienThoai = hoaDon.KhachHang.SoDienThoai,
                    NgaySinh = hoaDon.KhachHang.NgaySinh,
                    IDXP = hoaDon.KhachHang.IDXP,
                    DiaChi = hoaDon.KhachHang.DiaChi
                } : null,

                // Ánh xạ các thuộc tính cần thiết cho Nhân viên
                NhanVien = hoaDon.NhanVien != null ? new NhanVien
                {
                    IDNV = hoaDon.NhanVien.IDNV,

                } : null,

                // Ánh xạ danh sách chi tiết hóa đơn
                HoaDonChiTiets = hoaDon.HoaDonChiTiets?.Select(hdct => new HoaDonChiTiet
                {
                    IDHDCT = hdct.IDHDCT,
                    IDVT = hdct.IDVT,
                    SoLuong = hdct.SoLuong,
                    DonGia = hdct.DonGia,
                    ThanhTien = hdct.ThanhTien,
                    GhiChu = hdct.GhiChu
                }).ToList() ?? new List<HoaDonChiTiet>()
            };

            return Ok(dto);
        }

    }
}