using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public async Task<IActionResult> GetAllVatTu()
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
                      SoLuong = hd.SoLuong,
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
                .Include(hd => hd.NhaCungCap)
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
                SoLuong = vattu.SoLuong,
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

        [HttpGet("Getxuatxu")]
        public async Task<IActionResult> Getxuatxu()
        {
            var dsxuatxu = await _context.XuatXu.ToListAsync();
            return Ok(dsxuatxu);
        }

        [HttpPost("AddVaccine")]
        public async Task<IActionResult> AddVaccine([FromBody] VaccineCreateDTO vaccineDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            string newIDVT = await GenerateNewIDVT();


            var newvaccine = new VatTuYTe
            {

                IDVT = newIDVT,
                IDTL = vaccineDto.IDTL,
                TenVatTu = vaccineDto.TenVatTu,
                IDNGC = vaccineDto.IDNGC,
                IDNHC = vaccineDto.IDNHC,
                DonGia = vaccineDto.DonGia,
                HanSuDung = vaccineDto.HanSuDung,
                IDXX = vaccineDto.IDXX,
                GhiChu = vaccineDto.GhiChu,
                SoLuong = vaccineDto.SoLuong
            };

            // Thêm vào Database
            await _context.VatTuYTe.AddAsync(newvaccine);
            await _context.SaveChangesAsync();

            return Ok(newvaccine);

        }

        [HttpPost("Findvattu")]
        public async Task<IActionResult> Findvattu([FromBody] FindvattuDTO findvattuDto)
        {
            if (findvattuDto == null)
            {
                return BadRequest(new { Message = "Dữ liệu tìm kiếm không hợp lệ." });
            }

            var Mavt = await _context.VatTuYTe
                .Where(vt =>
                    (string.IsNullOrEmpty(findvattuDto.IDVT) || vt.IDVT == findvattuDto.IDVT) &&
                    (string.IsNullOrEmpty(findvattuDto.IDTL) || vt.IDTL == findvattuDto.IDTL) &&
                    (string.IsNullOrEmpty(findvattuDto.TenVatTu) || vt.TenVatTu.Contains(findvattuDto.TenVatTu)) &&
                    (findvattuDto.IDNGC == null || vt.IDNGC == findvattuDto.IDNGC) &&
                    (findvattuDto.IDNHC == null || vt.IDNHC == findvattuDto.IDNHC) &&
                    (findvattuDto.HanSuDung == null || vt.HanSuDung == findvattuDto.HanSuDung))
                .Select(vt => new VatTuDTO
                {
                    IDVT = vt.IDVT,
                    TenVatTu = vt.TenVatTu,
                    DonGia = vt.DonGia,
                    SoLuong = vt.SoLuong,
                    HanSuDung = vt.HanSuDung,
                    GhiChu = vt.GhiChu,
                    IDTL = vt.IDTL,
                    IDNHC = vt.IDNHC,
                    IDNGC = vt.IDNGC,
                    IDXX = vt.IDXX,


                }).ToListAsync();

            if (!Mavt.Any())
            {
                return NotFound(new { Message = "Không tìm thấy vật tư nào phù hợp." });
            }

            return Ok(Mavt);
        }


        [HttpPut("UpdateVaccine/{id}")]
        public async Task<IActionResult> UpdateVaccine(string id, [FromBody] VaccineCreateDTO vaccineDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vt = await _context.VatTuYTe.FirstOrDefaultAsync(vt => vt.IDVT == id);
            if (vt == null)
            {
                return NotFound(new { Message = "Vật tư không tồn tại." });
            }

            var loaivattu = await _context.LoaiVatTu.FirstOrDefaultAsync(lvt => lvt.IDTL == vaccineDto.IDTL);
            var nguoncap = vaccineDto.IDNGC != null ? await _context.NguonCungCap.FirstOrDefaultAsync(nc => nc.IDNGC == vaccineDto.IDNGC) : null;
            var nhacap = vaccineDto.IDNHC != null ? await _context.NhaCungCap.FirstOrDefaultAsync(nhc => nhc.IDNHC == vaccineDto.IDNHC) : null;
            var xuatxu = await _context.XuatXu.FirstOrDefaultAsync(xx => xx.IDXX == vaccineDto.IDXX);

            if (loaivattu == null)
                return NotFound(new { Message = "Loại vật tư không tồn tại." });
            if (vaccineDto.IDNGC != null && nguoncap == null)
                return NotFound(new { Message = "Nguồn cung cấp không tồn tại." });
            if (vaccineDto.IDNHC != null && nhacap == null)
                return NotFound(new { Message = "Nhà cung cấp không tồn tại." });
            if (xuatxu == null)
                return NotFound(new { Message = "Xuất xứ không tồn tại." });

            if (vt.IDTL == vaccineDto.IDTL &&
                vt.TenVatTu == vaccineDto.TenVatTu &&
                vt.IDNGC == vaccineDto.IDNGC &&
                vt.IDNHC == vaccineDto.IDNHC &&
                vt.DonGia == vaccineDto.DonGia &&
                vt.HanSuDung == vaccineDto.HanSuDung &&
                vt.IDXX == vaccineDto.IDXX &&
                vt.GhiChu == vaccineDto.GhiChu &&
                vt.SoLuong == vaccineDto.SoLuong)
            {
                return NoContent();
            }

            vt.IDTL = vaccineDto.IDTL;
            vt.TenVatTu = vaccineDto.TenVatTu;
            vt.IDNGC = vaccineDto.IDNGC;
            vt.IDNHC = vaccineDto.IDNHC;
            vt.DonGia = vaccineDto.DonGia;
            vt.HanSuDung = vaccineDto.HanSuDung;
            vt.IDXX = vaccineDto.IDXX;
            vt.GhiChu = vaccineDto.GhiChu;
            vt.SoLuong = vaccineDto.SoLuong;

            _context.VatTuYTe.Update(vt);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cập nhật thành công.", Data = vt });
        }


        private async Task<string> GenerateNewIDVT()
        {
            // Lấy khách hàng có IDHD lớn nhất hiện tại
            var lastvaccine = await _context.VatTuYTe
                .OrderByDescending(VT => VT.IDVT)
                .FirstOrDefaultAsync();

            string newIDVT;

            if (lastvaccine != null)
            {
                // Lấy phần số cuối cùng từ IDHD (giả sử dạng HD001, HD002,...)
                string lastNumber = lastvaccine.IDVT.Substring(2); // Lấy phần sau "HD"
                int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
                newIDVT = "VT" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
            }
            else
            {
                // Nếu chưa có HDách hàng nào, bắt đầu từ HD001
                newIDVT = "VT001";
            }

            return newIDVT;
        }

    }
}
