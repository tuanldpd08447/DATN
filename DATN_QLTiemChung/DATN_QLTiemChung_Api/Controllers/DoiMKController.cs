using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoiMKController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly ILogger<DoiMKController> _logger;

        public DoiMKController(DBContext context, ILogger<DoiMKController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet("GetTKKhachHang")]
        public async Task<IActionResult> GetTKKhachHang(string IDKH)
        {
            if (string.IsNullOrEmpty(IDKH))
            {
                _logger.LogWarning("IDKH không hợp lệ.");
                return BadRequest("IDKH không hợp lệ.");
            }

            var taiKhoanKhachHang = await _context.QLyTaiKhoanKH
                .Select(tk => new QLyTaiKhoanKHDTO
                {
                    IDKH = tk.IDKH,
                    IDTKKH = tk.IDTKKH,
                    SDT = tk.SDT,
                    MatKhau = tk.MatKhau,
                    TenKhachHang = tk.KhachHang.TenKhachHang 
                })
                .FirstOrDefaultAsync(tk => tk.IDKH == IDKH);

            if (taiKhoanKhachHang == null)
            {
                _logger.LogWarning("Không tìm thấy tài khoản khách hàng với IDKH: {IDKH}", IDKH);
                return NotFound("Không tìm thấy tài khoản khách hàng.");
            }

            return Ok(taiKhoanKhachHang);
        }


        [HttpPost("DoiMatKhau")]
        public async Task<IActionResult> DoiMatKhau([FromBody] DoiMatKhauRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Dữ liệu yêu cầu không hợp lệ.");
                return BadRequest("Dữ liệu yêu cầu không hợp lệ.");
            }

            var taiKhoanKhachHang = await _context.QLyTaiKhoanKH
                .FirstOrDefaultAsync(tk => tk.IDKH == request.IDKH);

            if (taiKhoanKhachHang == null)
            {
                _logger.LogWarning("Không tìm thấy tài khoản khách hàng với IDKH: {IDKH}", request.IDKH);
                return NotFound("Không tìm thấy tài khoản khách hàng.");
            }

            if (taiKhoanKhachHang.MatKhau != request.MatKhauCu)
            {
                _logger.LogWarning("Mật khẩu cũ không đúng cho khách hàng với IDKH: {IDKH}", request.IDKH);
                return BadRequest("Mật khẩu cũ không đúng.");
            }

            if (string.IsNullOrEmpty(request.MatKhauMoi) || request.MatKhauMoi.Length < 6)
            {
                _logger.LogWarning("Mật khẩu mới không hợp lệ cho khách hàng với IDKH: {IDKH}", request.IDKH);
                return BadRequest("Mật khẩu mới phải có ít nhất 6 ký tự.");
            }

            try
            {
                // Cập nhật mật khẩu cho tài khoản khách hàng
                taiKhoanKhachHang.MatKhau = request.MatKhauMoi;

                // Cập nhật tài khoản khách hàng trong database
                _context.QLyTaiKhoanKH.Update(taiKhoanKhachHang);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Mật khẩu đã được thay đổi thành công cho khách hàng với IDKH: {IDKH}", request.IDKH);
                return Ok($"Mật khẩu đã được thay đổi thành công cho khách hàng: {request.IDKH}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi thay đổi mật khẩu cho khách hàng với IDKH: {IDKH}", request.IDKH);
                return StatusCode(500, "Đã xảy ra lỗi khi thay đổi mật khẩu.");
            }
        }

    }
}
