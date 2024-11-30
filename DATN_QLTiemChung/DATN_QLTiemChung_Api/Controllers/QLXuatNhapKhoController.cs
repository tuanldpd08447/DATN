using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLXuatNhapKhoController : ControllerBase
    {
        private readonly DBContext _context;

        public QLXuatNhapKhoController(DBContext context)
        {
            _context = context;
        }
        [HttpGet("GetLoaiVatTu")]
        public async Task<IActionResult> GetLoaiVatTu()
        {
            var dsvattu = await _context.LoaiVatTu.ToListAsync();
            return Ok(dsvattu);
        }


        [HttpGet("GetNguonCap")]
        public async Task<IActionResult> GetNguonCap()
        {
            var dsnguoncap = await _context.NguonCungCap.ToListAsync();
            return Ok(dsnguoncap);
        }
        [HttpGet("GetNhacap")]
        public async Task<IActionResult> GetNhaCap()
        {
            var dsnhacap = await _context.NhaCungCap.ToListAsync();
            return Ok(dsnhacap);
        }
    }
}
