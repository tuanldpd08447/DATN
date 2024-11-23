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
        [HttpGet("GetAllBYIDKh/{IDKH}")]
        public async Task<ActionResult<HoaDonDTO>> GetAllBYIDKh(string IDKH)
        {
            var hoaDon = await _context.HoaDon
                .Include(hd => hd.KhachHang)
                .Include(hd => hd.NhanVien)
                .Include(hd => hd.HoaDonChiTiets)
                .FirstOrDefaultAsync(hd => hd.IDKH == IDKH);


            if (hoaDon == null)
            {
                return NotFound(new { message = "Hóa đơn không tồn tại" });
            }


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
                       IDTP = diachi.District.Province.code,
                       TenTinhThanhPho = diachi.District.Province.full_name
                   }).FirstOrDefaultAsync(w => w.IDXP == idxp)
           ;

            return Ok(ward);
        }
        [HttpGet("GetAllVatTu")]
        public async Task<IActionResult> GetAllVatTu()
        {
            var VatTuYTe = await _context.VatTuYTe.ToListAsync();

            return Ok(VatTuYTe);
        }
        [HttpPost("AddHoaDon")]
        public async Task<IActionResult> AddHoaDon([FromBody] HoaDonDTO HoaDonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var hd = await _context.HoaDon.FindAsync(HoaDonDto.IDHD);
            if (hd == null)
            {
                string newIDHD = await GenerateNewIDHD();

                var newHoaDon = new HoaDon
                {

                    IDHD = newIDHD,
                    IDKH = HoaDonDto.IDKH,
                    IDNV = HoaDonDto.IDNV,
                    ThoiGian = HoaDonDto.ThoiGian,
                    GhiChu = HoaDonDto.GhiChu,
                    NoiDung = HoaDonDto.NoiDung,
                    TongTien = HoaDonDto.TongTien,
                    TrangThai = HoaDonDto.TrangThai,

                };

                // Thêm vào Database
                await _context.HoaDon.AddAsync(newHoaDon);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAll), new { id = newHoaDon.IDHD }, newHoaDon);
            }



            return NotFound(new { Message = "Hóa đơn không tồn tại." });
        }
        private async Task<string> GenerateNewIDHD()
        {
            // Lấy khách hàng có IDHD lớn nhất hiện tại
            var lastHDKH = await _context.HoaDon
                .OrderByDescending(HD => HD.IDHD)
                .FirstOrDefaultAsync();

            string newIDHD;

            if (lastHDKH != null)
            {
                // Lấy phần số cuối cùng từ IDHD (giả sử dạng HD001, HD002,...)
                string lastNumber = lastHDKH.IDHD.Substring(2); // Lấy phần sau "HD"
                int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
                newIDHD = "HD" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
            }
            else
            {
                // Nếu chưa có HDách hàng nào, bắt đầu từ HD001
                newIDHD = "HD001";
            }

            return newIDHD;
        }
    }
}