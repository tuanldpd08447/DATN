using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataQLKhoVaccineController : ControllerBase
    {
        private readonly DBContext _context;
        public DataQLKhoVaccineController(DBContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllVatTu")]
        public async Task<IActionResult>GetAllVatTu ()
        {
            var vatTu = await _context.VatTuYTe
                  .Include(hd => hd.LoaivatTu)
                  .Include(hd => hd.NguonCungCap)
                  .Include(hd => hd.NhaCungCap)
                  .Include(hd => hd.XuatXu)
                  .Select(hd => new VatTuDTO

                  {

                      IDVT = hd.IDVT.ToString(),
                      IDTL = hd.IDTL,
                      IDNGC = hd.IDNGC,
                      IDNHC = hd.IDNHC,
                      IDXX = hd.IDXX,
                      TenVatTu = hd.TenVatTu,
                      DonGia = hd.DonGia,
                      HanSuDung = hd.HanSuDung,
                      SoLuong= hd.SoLuong,
                      GhiChu = hd.GhiChu

                  }).ToListAsync();

            return Ok(vatTu);
        }
        [HttpGet("GetAllBYIDVT/{IDVT}")]
        public async Task<ActionResult<VatTuDTO>> GetAllBYIDVT(string IDVT)
        {
            var vattu = await _context.VatTuYTe
                .Include(hd => hd.LoaivatTu)
                .Include(hd => hd.NguonCungCap)
                .Include(hd => hd.  NhaCungCap)
                .Include(hd => hd.XuatXu)
                .FirstOrDefaultAsync(hd => hd.IDVT == IDVT);


            if (vattu == null)
            {
                return NotFound(new { message = "Hóa đơn không tồn tại" });
            }
            var dto = new VatTuDTO()
            {
                IDVT = vattu.IDVT,
                IDTL = vattu.IDTL,
                IDNHC = vattu.IDNHC,
                IDNGC = vattu.IDNGC,
                IDXX = vattu.IDXX,
                TenVatTu = vattu.TenVatTu,
                DonGia = vattu.DonGia,
                HanSuDung = vattu.HanSuDung,
                SoLuong= vattu.SoLuong,
                GhiChu = vattu.GhiChu
            };
                  return Ok(dto);

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
