using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DNhapDkyController : ControllerBase
    {

        private readonly DBContext _context;

        public DNhapDkyController(DBContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllKhachHang")]
        public async Task<IActionResult> GetAllKhachHang()
        {
            var khachHangs = await _context.KhachHang
           .Include(kh => kh.Ward)
               .ThenInclude(w => w.District)
                   .ThenInclude(d => d.Province)
           .Select(kh => new KhachHangDTo
           {
               IDKH = kh.IDKH.ToString(),
               TenKhachHang = kh.TenKhachHang,
               NgaySinh = kh.NgaySinh,
               GioiTinh = kh.GioiTinh,
               DiaChi = kh.DiaChi,
               SoDienThoai = kh.SoDienThoai,
               Email = kh.Email,
               CCCD_MDD = kh.CCCD_MDD,
               DanToc = kh.DanToc,
               FullAddress = $"{kh.DiaChi}, {kh.Ward.name}, {kh.Ward.District.name}, {kh.Ward.District.Province.name}"
           })
           .ToListAsync();
            return Ok(khachHangs);
        }

        [HttpPost("LoginKH")]
        public async Task<IActionResult> LoginKH([FromBody] LoginKhachHang loginKhachHang)
        {
            if (loginKhachHang == null || string.IsNullOrWhiteSpace(loginKhachHang.Sdt) || string.IsNullOrWhiteSpace(loginKhachHang.Password))
            {
                return BadRequest("Thông tin đăng nhập không hợp lệ.");
            }

            var login = await _context.QLyTaiKhoanKH
                .Include(kh => kh.KhachHang)
                .FirstOrDefaultAsync(kh => kh.SDT == loginKhachHang.Sdt && kh.MatKhau == loginKhachHang.Password);

            if (login == null)
            {
                return Unauthorized("Thông tin đăng nhập không chính xác.");
            }

            LoginResult loginResult = new LoginResult { 
                ID = login.KhachHang.IDKH,
                Role ="KhachHang",
                Username = login.KhachHang.TenKhachHang
                
            };
            return Ok(loginResult);
        }

        [HttpPost("LoginNV")]
        public async Task<IActionResult> LoginNV([FromBody] LoginNhanVien loginNhanVien)
        {
            if (loginNhanVien == null || string.IsNullOrWhiteSpace(loginNhanVien.Email) || string.IsNullOrWhiteSpace(loginNhanVien.Password))
            {
                return BadRequest("Thông tin đăng nhập không hợp lệ.");
            }

            var login = await _context.QLyTaiKhoanNV
                .Include(nv => nv.NhanVien)
                .ThenInclude(nv=> nv.ChucDanh)
                .FirstOrDefaultAsync(nv => nv.Email == loginNhanVien.Email && nv.MatKhau == loginNhanVien.Password);

            if (login == null)
            {
                return Unauthorized("Thông tin đăng nhập không chính xác.");
            }

            LoginResult loginResult = new LoginResult
            {
                ID = login.NhanVien.IDNV,
                Role = login.NhanVien.ChucDanh.TenChucDanh,
                Username = login.NhanVien.TenNhanVien

            };
            return Ok(loginResult);
        }
        [HttpGet("CheckEmail/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var khachhang = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.Email == email);
            if (khachhang != null)
            {
                return BadRequest("Email đã tồn tại.");
            }

            return Ok("Email hợp lệ.");
        }

        [HttpGet("CheckPhone/{sdt}")]
        public async Task<IActionResult> CheckPhone(string sdt)
        {
            var khachhang = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.SoDienThoai == sdt);
            if (khachhang != null)
            {
                return BadRequest("Số điện thoại đã tồn tại.");
            }

            return Ok("Số điện thoại hợp lệ.");
        }

        [HttpGet("CheckCCCD/{cccd}")]
        public async Task<IActionResult> CheckCCCD(string cccd)
        {
            var khachhang = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.CCCD_MDD == cccd);
            if (khachhang != null)
            {
                return BadRequest("CCCD/Mã định danh đã tồn tại.");
            }

            return Ok("CCCD/Mã định danh hợp lệ.");
        }


        [HttpPost("khachHangRegister")]
        public async Task<IActionResult> khachHangRegister([FromBody] RegisterDto khachHangRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var hd = await _context.KhachHang.FirstOrDefaultAsync(kh=> kh.Email == khachHangRegister.Email || kh.SoDienThoai == khachHangRegister.SoDienThoai || kh.CCCD_MDD == khachHangRegister.CCCD_MDD);
            if (hd == null)
            {
                var ward = await _context.wards.FindAsync(khachHangRegister.IDXP);
                if (ward == null)
                {
                    return NotFound(new { Message = "Ward không tồn tại." });
                }
                string newIDKH = await GenerateNewIDKH();

                var newKhachHang = new KhachHang
                {
                    IDKH = newIDKH,  // Sử dụng IDKH mới
                    IDXP = khachHangRegister.IDXP,
                    Ward = ward,
                    TenKhachHang = khachHangRegister.TenKhachHang,
                    NgaySinh = khachHangRegister.NgaySinh,
                    GioiTinh = khachHangRegister.GioiTinh,
                    DiaChi = khachHangRegister.DiaChi,
                    SoDienThoai = khachHangRegister.SoDienThoai,
                    Email = khachHangRegister.Email,
                    CCCD_MDD = khachHangRegister.CCCD_MDD,
                    DanToc = khachHangRegister.DanToc
                };
               
                // Thêm Khách Hàng vào Database
                await _context.KhachHang.AddAsync(newKhachHang);
                await _context.SaveChangesAsync();
                string newIDTKKH = await GenerateNewIDIKKH();
                QLyTaiKhoanKH ang = new QLyTaiKhoanKH
                {
                    IDTKKH = newIDTKKH,
                    IDKH = newIDKH,
                    MatKhau = khachHangRegister.Password,
                    SDT = khachHangRegister.SoDienThoai
                };
                await _context.QLyTaiKhoanKH.AddAsync(ang);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound("thông tin khách hàng dã tồn tài");
           
        }
        private async Task<string> GenerateNewIDIKKH()
        {
            var lastTaiKhoan = await _context.QLyTaiKhoanKH
                .OrderByDescending(tk => tk.IDTKKH)
                .FirstOrDefaultAsync();

            string newIDTKKH;

            if (lastTaiKhoan != null)
            {
                string prefix = new string(lastTaiKhoan.IDTKKH.TakeWhile(c => !char.IsDigit(c)).ToArray());
                string numericPart = new string(lastTaiKhoan.IDTKKH.SkipWhile(c => !char.IsDigit(c)).ToArray());

                if (int.TryParse(numericPart, out int number))
                {
                    number++;
                    newIDTKKH = $"{prefix}{number:D3}";
                }
                else
                {
                    throw new FormatException("Numeric part of the IDTKKH is invalid.");
                }
            }
            else
            {
                newIDTKKH = "TKKH001";
            }

            return newIDTKKH;
        }

        private async Task<string> GenerateNewIDKH()
        {
            var lastKhachHang = await _context.KhachHang
                .OrderByDescending(kh => kh.IDKH)
                .FirstOrDefaultAsync();

            string newIDKH;

            if (lastKhachHang != null)
            {
                string prefix = new string(lastKhachHang.IDKH.TakeWhile(c => !char.IsDigit(c)).ToArray());
                string numericPart = new string(lastKhachHang.IDKH.SkipWhile(c => !char.IsDigit(c)).ToArray());

                if (int.TryParse(numericPart, out int number))
                {
                    number++; // Tăng số lên 1
                    newIDKH = $"{prefix}{number:D3}"; // D3 giữ 3 chữ số
                }
                else
                {
                    throw new FormatException("Numeric part of the IDKH is invalid.");
                }
            }
            else
            {
                // Nếu chưa có khách hàng nào, bắt đầu từ KH001
                newIDKH = "KH001";
            }

            return newIDKH;
        }

    }
}
