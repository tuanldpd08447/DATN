using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLTaiKhoanController : ControllerBase
    {
        private readonly DBContext _context;

        public QLTaiKhoanController(DBContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllTaiKhoanNV")]
        public async Task<IActionResult> GetAllTaiKhoanNV()
        {
            var taiKhoans = await _context.QLyTaiKhoanNV
                .Include(t => t.NhanVien)
                .Select(t => new QLTaiKhoanNVDTO
                {
                    IDTKNV = t.IDTKNV.ToString(),
                    IDNV = t.NhanVien.IDNV,
                    Email = t.Email,
                    MatKhau = t.MatKhau
                }).ToListAsync();

            return Ok(taiKhoans);
        }
        [HttpPost("CreateTaiKhoanNV")]
        public async Task<IActionResult> CreateTaiKhoanNV([FromBody] CreateTaiKhoanNVDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.NhanVien.FindAsync(dto.IDNV) == null)
            {
                return NotFound(new { message = "Nhân viên không tồn tại" });
            }

            // Kiểm tra nhân viên đã có tài khoản hay chưa
            var existingAccount = await _context.QLyTaiKhoanNV.FirstOrDefaultAsync(x => x.IDNV == dto.IDNV);
            if (existingAccount != null)
            {
                return Conflict(new { message = "Nhân viên đã có tài khoản. Không thể tạo thêm." });
            }

            // Tạo IDTKNV mới
            string newIDTKNV = await GenerateNewIDTKNV();

            var taiKhoan = new QLyTaiKhoanNV
            {
                IDTKNV = newIDTKNV, // Tạo ID tự động
                IDNV = dto.IDNV,
                Email = dto.Email,
                MatKhau = dto.MatKhau
            };

            // Thêm Nhan Vien vào Database
            await _context.QLyTaiKhoanNV.AddAsync(taiKhoan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTKNhanVienById), new { id = taiKhoan.IDTKNV }, taiKhoan);
        }

        private async Task<string> GenerateNewIDTKNV()
        {
            // Lấy Nhan Vien có IDTKNV lớn nhất hiện tại
            var lastNhanVien = await _context.QLyTaiKhoanNV
                .OrderByDescending(kh => kh.IDTKNV)
                .FirstOrDefaultAsync();

            string newIDTKNV;

            if (lastNhanVien != null)
            {
                // Lấy phần số cuối cùng từ IDNV (giả sử dạng TKNV001, TKNV002,...)
                string lastNumber = lastNhanVien.IDTKNV.Substring(4); // Lấy phần sau "TKNV"
                int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
                newIDTKNV = "TKNV" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
            }
            else
            {
                // Nếu chưa có tai khoan nhan vien nào, bắt đầu từ IDTKNV001
                newIDTKNV = "TKNV001";
            }

            return newIDTKNV;
        }

        // GET: api/TKNhanVien/{id}
        [HttpGet("GetTKNhanVienById/{id}")]
        public async Task<IActionResult> GetTKNhanVienById(string id)
        {
            var nhanVien = await _context.QLyTaiKhoanNV
                .Include(kh => kh.NhanVien)
                .FirstOrDefaultAsync(kh => kh.IDTKNV == id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return Ok(nhanVien);
        }

        // 3. Sửa mật khẩu
        [HttpPut("UpdateMatKhauTKNV/{id}")]
        public async Task<IActionResult> UpdateMatKhauTKNV(string id, [FromBody] UpdateMatKhauNVDTO dto)
        {

            // Kiểm tra dữ liệu hợp lệ
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taiKhoan = await _context.QLyTaiKhoanNV.FindAsync(id);

            if (taiKhoan == null)
            {
                return NotFound(new { message = "Tài khoản không tồn tại" });
            }

            // Cập nhật các trường thông tin của nhân viên từ DTO

            taiKhoan.MatKhau = dto.NewPassword;


            // Lưu các thay đổi vào cơ sở dữ liệu
            try
            {
                _context.QLyTaiKhoanNV.Update(taiKhoan);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi cập nhật nhân viên: {ex.Message}");
            }
        }

        [HttpGet("GetAllTaiKhoanKH")]
        public async Task<IActionResult> GetAllTaiKhoanKH()
        {
            var taiKhoankhs = await _context.QLyTaiKhoanKH
               .Include(t => t.KhachHang)
               .Select(t => new QLTaiKhoanKHDTO
               {
                   IDTKKH = t.IDTKKH.ToString(),
                   IDKH = t.KhachHang.IDKH,
                   SDT = t.SDT,
                   MatKhau = t.MatKhau
               }).ToListAsync();

            return Ok(taiKhoankhs);
        }

        [HttpGet("GetTKKhachHangById/{id}")]
        public async Task<IActionResult> GetTKKhachHangById(string id)
        {
            var khachHang = await _context.QLyTaiKhoanKH
                .Include(kh => kh.KhachHang)
                .FirstOrDefaultAsync(kh => kh.IDTKKH == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return Ok(khachHang);
        }

        [HttpPost("CreateTaiKhoanKH")]
        public async Task<IActionResult> CreateTaiKhoanKH([FromBody] CreateTaiKhoanKHDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.KhachHang.FindAsync(dto.IDKH) == null)
            {
                return NotFound(new { message = "Khách hàng không tồn tại" });
            }

            // Kiểm tra nhân viên đã có tài khoản hay chưa
            var existingAccount = await _context.QLyTaiKhoanKH.FirstOrDefaultAsync(x => x.IDKH == dto.IDKH);
            if (existingAccount != null)
            {
                return Conflict(new { message = "Khách hàng đã có tài khoản. Không thể tạo thêm." });
            }

            // Tạo IDTKNV mới
            string newIDTKKH = await GenerateNewIDTKKH();

            var taiKhoankh = new QLyTaiKhoanKH
            {
                IDTKKH = newIDTKKH, // Tạo ID tự động
                IDKH = dto.IDKH,
                SDT = dto.SDT,
                MatKhau = dto.MatKhau
            };

            // Thêm Nhan Vien vào Database
            await _context.QLyTaiKhoanKH.AddAsync(taiKhoankh);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTKKhachHangById), new { id = taiKhoankh.IDTKKH }, taiKhoankh);
        }

        private async Task<string> GenerateNewIDTKKH()
        {
            // Lấy Nhan Vien có IDTKNV lớn nhất hiện tại
            var lastKhachhang = await _context.QLyTaiKhoanKH
                .OrderByDescending(kh => kh.IDTKKH)
                .FirstOrDefaultAsync();

            string newIDTKKH;

            if (lastKhachhang != null)
            {
                // Lấy phần số cuối cùng từ IDNV (giả sử dạng TKNV001, TKNV002,...)
                string lastNumber = lastKhachhang.IDTKKH.Substring(4); // Lấy phần sau "TKNV"
                int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
                newIDTKKH = "TKKH" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
            }
            else
            {
                // Nếu chưa có tai khoan nhan vien nào, bắt đầu từ IDTKNV001
                newIDTKKH = "TKKH001";
            }

            return newIDTKKH;
        }

    }
}
