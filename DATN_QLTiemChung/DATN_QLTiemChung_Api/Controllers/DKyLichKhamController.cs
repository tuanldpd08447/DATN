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

        private string GetNextLichKhamID()
        {
            var lastLichKham = _context.DatLichKham
                .OrderByDescending(lk => lk.IDLK)
                .FirstOrDefault();

            if (lastLichKham == null)
            {
                return "LK001";
            }
            else
            {
                var lastIDNumber = int.Parse(lastLichKham.IDLK.Substring(2)); 
                var nextIDNumber = lastIDNumber + 1;
                return $"LK{nextIDNumber:D3}"; 
            }
        }

        [HttpPost("ThemLichKham")]
        public async Task<IActionResult> ThemLichKham([FromBody] DatLichKham lichKham)
        {
            if (lichKham == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            lichKham.IDLK = GetNextLichKhamID();

            var existingLichKham = _context.DatLichKham
                .FirstOrDefault(lk => lk.ThoiGian == lichKham.ThoiGian);

            if (existingLichKham != null)
            {
                return Conflict("Thời gian này đã có lịch khám.");
            }

            _context.DatLichKham.Add(lichKham);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ThemLichKham), new { id = lichKham.IDLK }, lichKham);
        }



        [HttpGet("GetThongtinCaNhan/{id}")]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetThongtinCaNhan(string id)
        {

            var khachHang = await _context.KhachHang.FirstOrDefaultAsync(kh=>kh.IDKH ==id);

            if (khachHang == null )
            {
                return NotFound("Không tìm thấy thông tin.");
            }

            return Ok(khachHang);
        }

        [HttpGet("LichKham/{id}")]
        public async Task<ActionResult<IEnumerable<LichKham>>> LichKham(string id)
        {
            // Truy vấn dữ liệu lịch khám và bao gồm thông tin khách hàng
            var lichKham = await _context.DatLichKham
                                          .Where(lk => lk.IDKH == id)  // Lọc theo ID khách hàng
                                          .Include(lk => lk.KhachHang) // Bao gồm thông tin khách hàng
                                          .Select(lk => new LichKham
                                          {
                                              ID = lk.IDLK,                        // ID lịch khám
                                              IDKH = lk.IDKH,                    // ID khách hàng
                                              TenKhachHang = lk.KhachHang.TenKhachHang,   // Tên khách hàng
                                              NgayKham = DateOnly.FromDateTime(lk.ThoiGian),  // Chuyển DateTime sang DateOnly
                                              GioKham = TimeOnly.FromDateTime(lk.ThoiGian)     // Chuyển DateTime sang TimeOnly
                                          })
                                          .Where(lk=> lk.IDKH == id).ToListAsync();  // Chuyển sang danh sách

            if (lichKham == null || !lichKham.Any())  // Kiểm tra nếu không có dữ liệu
            {
                return NotFound("Không tìm thấy thông tin.");
            }

            return Ok(lichKham); // Trả về dữ liệu
        }


    }
}
