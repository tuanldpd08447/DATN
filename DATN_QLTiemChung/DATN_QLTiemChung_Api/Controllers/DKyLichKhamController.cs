using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DKyLichKhamController : ControllerBase
    {
        private readonly DBContext _context;
        public DKyLichKhamController(DBContext context)
        {
            _context = context;
        }
        // GET: api/DatLichKham/GetAllDatLichKhams
        [HttpGet("GetAllDatLichKhams/{IDKH}")]
        public async Task<ActionResult<IEnumerable<KhachHangPreOder>>> GetAllDatLichKhams(string IDKH)
        {
            // Truy vấn dữ liệu từ bảng DatLichKham và các bảng liên quan
            var query = _context.DatLichKham
                .Include(d => d.KhachHang)
                    .ThenInclude(kh => kh.Ward)
                        .ThenInclude(w => w.District)
                            .ThenInclude(d => d.Province)
                .Select(kh => new KhachHangPreOder
                {
                    IDKH = kh.KhachHang.IDKH.ToString(),
                    TenKhachHang = kh.KhachHang.TenKhachHang,
                    NgayHen = DateOnly.FromDateTime(kh.ThoiGian),
                    ThoiGianHen = TimeOnly.FromDateTime(kh.ThoiGian),
                    NgaySinh = kh.KhachHang.NgaySinh,
                    GioiTinh = kh.KhachHang.GioiTinh,
                    DiaChi = kh.KhachHang.DiaChi,
                    SoDienThoai = kh.KhachHang.SoDienThoai,
                    Email = kh.KhachHang.Email,
                    CCCD_MDD = kh.KhachHang.CCCD_MDD,
                    DanToc = kh.KhachHang.DanToc,
                    FullAddress = $"{kh.KhachHang.DiaChi}, {kh.KhachHang.Ward.name}, {kh.KhachHang.Ward.District.name}, {kh.KhachHang.Ward.District.Province.name}"
                });

            if (IDKH != null)
            {
                query = query.Where(kh => kh.IDKH == IDKH);
            }


            var datLichKhams = await query.ToListAsync();

            if (datLichKhams == null || datLichKhams.Count == 0)
            {
                return NotFound("Không có lịch khám nào.");
            }

            return Ok(datLichKhams);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<DatLichKham>>> GetAllLichKham()
        {
            
            var datlichkham = await _context.DatLichKham.ToListAsync();
            return Ok(datlichkham);
        }
    }
}
