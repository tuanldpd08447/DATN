using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DATN_QLTiemChung_Api.Models;
using DATN_QLTiemChung_Api;

[Route("api/[controller]")]
[ApiController]
public class TTCaNhanController : Controller
{
    private readonly DBContext _context;
    private readonly ILogger<TTCaNhanController> _logger;

    public TTCaNhanController(DBContext context, ILogger<TTCaNhanController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/TTCaNhan/GetKhachHang
    [HttpGet("GetKhachHang")]
    public async Task<IActionResult> GetKhachHang(string IDKH)
    {
        if (string.IsNullOrEmpty(IDKH))
        {
            _logger.LogWarning("GetKhachHang: IDKH is empty or null.");
            return BadRequest("IDKH cannot be empty.");
        }

        var khachHang = await _context.KhachHang
            .Include(kh => kh.Ward) // Include Ward
                .ThenInclude(w => w.District) // Include District
                    .ThenInclude(d => d.Province) // Include Province
            .Where(kh => kh.IDKH == IDKH)
            .Select(kh => new KhachHangDTo2
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
                IDXP = kh.Ward.code,
                IDQH = kh.Ward.District.code,
                IDTTP = kh.Ward.District.Province.code,
                NameXP = kh.Ward.name,
                NameQH = kh.Ward.District.name,
                NameTTP = kh.Ward.District.Province.name,
            })
            .FirstOrDefaultAsync();

        if (khachHang == null)
        {
            _logger.LogWarning("GetKhachHang: Customer not found for IDKH = {IDKH}", IDKH);
            return NotFound("Customer not found.");
        }

        return Ok(khachHang);
    }
    [HttpPut("UpdateKhachHang")]
    public async Task<IActionResult> UpdateKhachHang([FromBody] KhachHangUpdateDTO updatedKhachHang)
    {
        if (updatedKhachHang == null || string.IsNullOrEmpty(updatedKhachHang.IDKH))
        {
            return BadRequest("Invalid customer data.");
        }

        var khachHang = await _context.KhachHang
            .Include(kh => kh.Ward)
            .FirstOrDefaultAsync(kh => kh.IDKH == updatedKhachHang.IDKH);

        if (khachHang == null)
        {
            return NotFound("Customer not found.");
        }

        // Update properties
        khachHang.TenKhachHang = updatedKhachHang.TenKhachHang;
        khachHang.NgaySinh = updatedKhachHang.NgaySinh;
        khachHang.CCCD_MDD = updatedKhachHang.CCCD_MDD;
        khachHang.SoDienThoai = updatedKhachHang.SoDienThoai;
        khachHang.IDXP = updatedKhachHang.IDXP;
        khachHang.DanToc = updatedKhachHang.DanToc;
        khachHang.GioiTinh = updatedKhachHang.GioiTinh;
        khachHang.Email = updatedKhachHang.Email;
        khachHang.DiaChi = updatedKhachHang.DiaChi;

        _context.Update(khachHang); // Save changes
        await _context.SaveChangesAsync();

        return Ok("Customer updated successfully.");
    }




}

