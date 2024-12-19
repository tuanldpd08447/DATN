using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLTaiKhoanController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly EmailService _emailService;

        public QLTaiKhoanController(DBContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
                    TenNV = t.NhanVien.TenNhanVien,
                    SDT  = t.NhanVien.SoDienThoai,
                    MatKhau = ""
                }).ToListAsync();

            return Ok(taiKhoans);
        }
        [HttpGet("GetTaiKhoanNhanVienChuaCoTK")]
        public async Task<IActionResult> GetTaiKhoanNhanVienChuaCoTK()
        {
            var taiKhoans = await _context.NhanVien
                .Where(nv => !_context.QLyTaiKhoanNV.Any(tk => tk.IDNV == nv.IDNV)) // Tìm nhân viên không có tài khoản
                .Select(nv => new TTNV
                {
                    IDNV = nv.IDNV,
                    TenNV = nv.TenNhanVien,
                    Email = nv.Email,
                    SDT = nv.SoDienThoai,
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
                .Select(kh => new QLTaiKhoanNVDTO
                {
                    IDTKNV = kh.IDTKNV,
                    IDNV = kh.IDNV,
                    TenNV = kh.NhanVien.TenNhanVien,
                    Email = kh.Email,
                    MatKhau = "",
                    SDT = kh.NhanVien.SoDienThoai

                })
                .FirstOrDefaultAsync(kh => kh.IDNV == id);

            if (nhanVien == null)
            {
                var nhanVien2 = await _context.NhanVien.Select(kh => new QLTaiKhoanNVDTO
                {
                    IDTKNV = "",
                    IDNV = kh.IDNV,
                    TenNV = kh.TenNhanVien,
                    Email = kh.Email,
                    MatKhau = "",
                    SDT = kh.SoDienThoai

                })
                .FirstOrDefaultAsync(kh => kh.IDNV == id);
                return Ok(nhanVien2);
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
                   TenKH = t.KhachHang.TenKhachHang,
                   SDT = t.SDT,
                   MatKhau = "",
                   Email = t.KhachHang.Email
               }).ToListAsync();

            return Ok(taiKhoankhs);
        }

        [HttpGet("GetTaiKhoanKhachHangChuaCoTK")]
        public async Task<IActionResult> GetTaiKhoanKhachHangChuaCoTK()
        {
            var taiKhoans = await _context.KhachHang
                .Where(kh => !_context.QLyTaiKhoanKH.Any(tk => tk.IDKH == kh.IDKH)) 
                .Select(kh => new TTKH
                {
                    IDKH = kh.IDKH,
                    TenKH = kh.TenKhachHang,
                    SDT = kh.SoDienThoai,
                    Email = kh.Email,
                }).ToListAsync();

            return Ok(taiKhoans);
        }

        [HttpGet("GetTKKhachHangById/{id}")]
        public async Task<IActionResult> GetTKKhachHangById(string id)
        {
            var khachHang = await _context.QLyTaiKhoanKH
                .Include(kh => kh.KhachHang)
                 .Select(kh => new QLTaiKhoanKHDTO
                 {
                     IDTKKH = kh.IDTKKH,
                     IDKH = kh.IDKH,
                     TenKH = kh.KhachHang.TenKhachHang,
                     Email = kh.KhachHang.Email,
                     MatKhau = "",
                     SDT = kh.KhachHang.SoDienThoai

                 })
                .FirstOrDefaultAsync(kh => kh.IDKH == id);

            if (khachHang == null)
            {
                var khachHang2 = await _context.KhachHang.
                 Select(kh => new QLTaiKhoanKHDTO
                 {
                     IDTKKH = "",
                     IDKH = kh.IDKH,
                     TenKH = kh.TenKhachHang,
                     Email = kh.Email,
                     MatKhau ="",
                     SDT = kh.SoDienThoai

                 })
                
                .FirstOrDefaultAsync(kh => kh.IDKH == id);
                return Ok(khachHang2);
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

        [HttpPost("DoiMKNVTuDong/{idnv}")]
        public async Task<IActionResult> DoiMKNVTuDong(string idnv)
        {
            if (string.IsNullOrEmpty(idnv))
                return Ok(new { success = false, message = "Mã nhân viên không hợp lệ" });

            // Tìm tài khoản theo ID nhân viên trong QLyTaiKhoanNV
            var user = await _context.QLyTaiKhoanNV.FirstOrDefaultAsync(u => u.IDNV == idnv);

            if (user == null)
            {
                // Nếu không tìm thấy tài khoản, kiểm tra trong bảng NhanVien
                var nhanVien = await _context.NhanVien.FirstOrDefaultAsync(nv => nv.IDNV == idnv);
                if (nhanVien == null)
                {
                    return Ok(new { success = false, message = "Không tìm thấy thông tin nhân viên!" });
                }

                // Tạo tài khoản mới trong QLyTaiKhoanNV
                user = new QLyTaiKhoanNV
                {
                    IDTKNV = await GenerateNewIDTKNV(),
                    IDNV = nhanVien.IDNV,
                    Email = nhanVien.Email,
                    MatKhau = "", 
                };
                _context.QLyTaiKhoanNV.Add(user);
            }

            // Sinh mật khẩu mới
            var newPassword = GenerateRandomPassword();
            user.MatKhau = newPassword;

            try
            {
                // Lưu tài khoản
                await _context.SaveChangesAsync();

                // Gửi mật khẩu qua email
                if (!string.IsNullOrEmpty(user.Email))
                {
                    await _emailService.SendEmailAsync(user.Email, "Mật khẩu mới",
                        $"Mật khẩu mới của bạn là: {newPassword}");
                }

                return Ok(new { success = true, message = "Mật khẩu đã được tạo và gửi qua email." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi đổi mật khẩu", error = ex.Message });
            }
        }

        [HttpPost("DoiMKKHTuDong/{idKH}")]
        public async Task<IActionResult> DoiMKKHTuDong(string idKH)
        {
            if (string.IsNullOrEmpty(idKH))
                return Ok(new { success = false, message = "Mã khách hàng không hợp lệ" });

            // Tìm tài khoản theo ID khách hàng trong QLyTaiKhoanKH
            var user = await _context.QLyTaiKhoanKH.Include(kh => kh.KhachHang).FirstOrDefaultAsync(u => u.IDKH == idKH);
            string emai = null; // Khai báo biến emai để lưu email từ KhachHang

            if (user == null)
            {
                // Nếu không tìm thấy tài khoản, kiểm tra trong bảng KhachHang
                var khachHang = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.IDKH == idKH);
                if (khachHang == null)
                {
                    return Ok(new { success = false, message = "Không tìm thấy thông tin khách hàng!" });
                }

                // Tạo tài khoản mới trong QLyTaiKhoanKH
                user = new QLyTaiKhoanKH
                {
                    IDTKKH = await GenerateNewIDTKKH(),
                    IDKH = khachHang.IDKH,
                    SDT = khachHang.SoDienThoai,
                    MatKhau = "",
                };
                emai = khachHang.Email;  // Lưu email của khách hàng từ bảng KhachHang
                _context.QLyTaiKhoanKH.Add(user);
            }

            // Sinh mật khẩu mới
            var newPassword = GenerateRandomPassword();
            user.MatKhau = newPassword;

            try
            {
                // Lưu tài khoản
                await _context.SaveChangesAsync();

                // Gửi mật khẩu qua email
                if (!string.IsNullOrEmpty(user.KhachHang.Email))  // Nếu có email trong tài khoản hiện tại
                {
                    await _emailService.SendEmailAsync(user.KhachHang.Email, "Mật khẩu mới", $"Mật khẩu mới của bạn là: {newPassword}");
                }
                else if (!string.IsNullOrEmpty(emai))  // Nếu không có email trong tài khoản, sử dụng email lưu từ bảng KhachHang
                {
                    await _emailService.SendEmailAsync(emai, "Mật khẩu mới", $"Mật khẩu mới của bạn là: {newPassword}");
                }

                return Ok(new { success = true, message = "Mật khẩu đã được tạo và gửi qua email." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi đổi mật khẩu", error = ex.Message });
            }
        }



        private string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$%^&*()!";
            return new string(Enumerable.Repeat(validChars, 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }


    }
}
