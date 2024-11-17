using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DATN_QLTiemChung_Api.Models;
using DATN_QLTiemChung_Api;

[Route("api/[controller]")]
[ApiController]
public class QLTiepNhanController : ControllerBase
{
    private readonly DBContext _context;

    public QLTiepNhanController(DBContext context)
    {
        _context = context;
    }
    [HttpGet("GetAllKhachHang")]
    public async Task<IActionResult> GetAllKhachHang()
    {
        var khachHangs = await _context.KhachHang
       .Include(kh => kh.Ward) // Lấy thông tin từ bảng Ward
           .ThenInclude(w => w.District) // Lấy thông tin từ bảng District
               .ThenInclude(d => d.Province) // Lấy thông tin từ bảng Province
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


    [HttpPost("AddKhachHang")]
    public async Task<IActionResult> AddKhachHang([FromBody] KhachHangCreateDTO khachHangDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Kiểm tra xem Ward có tồn tại không
        var ward = await _context.wards.FindAsync(khachHangDTO.IDXP);
        if (ward == null)
        {
            return NotFound(new { Message = "Ward không tồn tại." });
        }

        // Tạo IDKH mới
        string newIDKH = await GenerateNewIDKH();

        var newKhachHang = new KhachHang
        {
            IDKH = newIDKH,  // Sử dụng IDKH mới
            IDXP = khachHangDTO.IDXP,
            Ward = ward,
            TenKhachHang = khachHangDTO.TenKhachHang,
            NgaySinh = khachHangDTO.NgaySinh,
            GioiTinh = khachHangDTO.GioiTinh,
            DiaChi = khachHangDTO.DiaChi,
            SoDienThoai = khachHangDTO.SoDienThoai,
            Email = khachHangDTO.Email,
            CCCD_MDD = khachHangDTO.CCCD_MDD,
            DanToc = khachHangDTO.DanToc
        };

        // Thêm Khách Hàng vào Database
        await _context.KhachHang.AddAsync(newKhachHang);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetKhachHangById), new { id = newKhachHang.IDKH }, newKhachHang);
    }

    private async Task<string> GenerateNewIDKH()
    {
        // Lấy khách hàng có IDKH lớn nhất hiện tại
        var lastKhachHang = await _context.KhachHang
            .OrderByDescending(kh => kh.IDKH)
            .FirstOrDefaultAsync();

        string newIDKH;

        if (lastKhachHang != null)
        {
            // Lấy phần số cuối cùng từ IDKH (giả sử dạng KH001, KH002,...)
            string lastNumber = lastKhachHang.IDKH.Substring(2); // Lấy phần sau "KH"
            int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
            newIDKH = "KH" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
        }
        else
        {
            // Nếu chưa có khách hàng nào, bắt đầu từ KH001
            newIDKH = "KH001";
        }

        return newIDKH;
    }
    // GET: api/KhachHang/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetKhachHangById(string id)
    {
        var khachHang = await _context.KhachHang
            .Include(kh => kh.Ward)
            .FirstOrDefaultAsync(kh => kh.IDKH == id);

        if (khachHang == null)
        {
            return NotFound();
        }

        return Ok(khachHang);
    }

}
