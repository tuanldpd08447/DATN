using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLTiemChungController : ControllerBase
    {
        private readonly DBContext _context;
        public QLTiemChungController(DBContext context)
        {
            _context = context;
        }

        [HttpGet("DSKhamSangLoc")]
        public async Task<ActionResult> DSKhamSangLoc()
        {
            var khachHangKSL = await _context.KhamSangLoc.
                Include(kh => kh.KhachHang).
                Select(kh => new DSKhamSangLocDTO
                {
                    IDKH = kh.IDKH.ToString(),
                    ThoiGian = kh.ThoiGian,
                    KhachHang = kh.KhachHang != null ? new KhachHang
                    {
                        IDKH = kh.KhachHang.IDKH,
                        NgaySinh = kh.KhachHang.NgaySinh,
                        TenKhachHang = kh.KhachHang.TenKhachHang,
                    } : null,
                })
                .ToListAsync();
            return Ok(khachHangKSL); ;
        }

        [HttpGet("KQKhamSangLoc/{IDKH}")]
        public async Task<ActionResult> KQKhamSangLoc(string IDKH)
        {
            var kQKhamSangLoc = await _context.KhamSangLoc.Include(kh => kh.KhachHang).FirstOrDefaultAsync(kh => kh.IDKH == IDKH);

            if (kQKhamSangLoc == null)
            {
                return NotFound();
            }

            KQKhamSangLocDTO kQKhamSangLocDTO = new KQKhamSangLocDTO
            {
                IDKH = kQKhamSangLoc.IDKH.ToString(),
                TinhTrangSucKhoe = kQKhamSangLoc.TinhTrangSucKhoe,
                KetQua = kQKhamSangLoc.KetQua,
                TrangThai = kQKhamSangLoc.TrangThai,
                ChieuCao = kQKhamSangLoc.ChieuCao,
                CanNang = kQKhamSangLoc.CanNang,
                KhachHang = kQKhamSangLoc.KhachHang != null ? new KhachHang
                {
                    IDKH = kQKhamSangLoc.KhachHang.IDKH,
                    TenKhachHang = kQKhamSangLoc.KhachHang.TenKhachHang,
                    GioiTinh = kQKhamSangLoc.KhachHang.GioiTinh,
                } : null
            };


            return Ok(kQKhamSangLocDTO);
        }

        [HttpGet("CDVaccine/{IDKH}")]
        public async Task<ActionResult> CDVaccine(string IDKH)
        {
            var cdVaccine = await _context.DangKyTiemChung.Include(kh => kh.KhachHang).
                Include(kh => kh.DangKyVaccine).Include(kh => kh.NhanVien).
                FirstOrDefaultAsync(kh => kh.IDKH == IDKH);
            if (cdVaccine == null)
            {
                return NotFound();
            }

            CDVaccineDTO cdVaccineDTO = new CDVaccineDTO
            {
                IDDK = cdVaccine.IDDK.ToString(),
                IDKH = cdVaccine.IDKH,
                IDDKVC = cdVaccine.IDDKVC,
                IDNV = cdVaccine.IDNV,
                KhachHang = cdVaccine.KhachHang != null ? new KhachHang
                {
                    IDKH = cdVaccine.KhachHang.IDKH,
                }: null,

                DangKyVaccine = cdVaccine.DangKyVaccine != null ? new DangKyVaccine
                {
                    IDDKVC = cdVaccine.DangKyVaccine.IDDKVC,
                    TenVaccine = cdVaccine.DangKyVaccine.TenVaccine,
                    SoLuong = cdVaccine.DangKyVaccine.SoLuong,
                }: null,

                NhanVien = cdVaccine.NhanVien != null ? new NhanVien
                {
                    IDNV = cdVaccine.NhanVien.IDNV,
                    TenNhanVien = cdVaccine.NhanVien.TenNhanVien,
                }: null
            };
            
            return Ok(cdVaccineDTO);
        }

        private async Task<string> GenerateNewIDTCAsync()
        {
            // Lấy ID lớn nhất từ database
            var idTC = await _context.TiemChung
                .OrderByDescending(o => o.IDTC)
                .FirstOrDefaultAsync();

            string newId;
            if (idTC == null || string.IsNullOrEmpty(idTC.IDTC))
            {
                newId = "OD001";
            }
            else
            {
                string idTCNumber = idTC.IDTC.Substring(2);
                int number = int.Parse(idTCNumber) + 1;

                newId = $"TC{number:D3}";
            }

            return newId;
        }

        [HttpPost("CreateTiemChung")]
        public async Task<IActionResult> CreateTiemChung([FromBody] createTiemChung request)
        {
            if (request == null || string.IsNullOrEmpty(request.IDTC))
            {
                return BadRequest("Thông tin không hợp lệ.");
            }

            string newIDTC = await GenerateNewIDTCAsync();

            TimeSpan thoiGian;
            if (!TimeSpan.TryParse(request.ThoiGian, out thoiGian))
            {
                return BadRequest("Giá trị ThoiGian không hợp lệ.");
            }

            var tiemChung = new TiemChung
            {
                IDTC = newIDTC,
                IDKH = request.IDKH,
                IDDK = request.IDDK,
                IDNV = request.IDNV,
                ThoiGian = thoiGian,
            };
            _context.TiemChung.Add(tiemChung);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                TiemChung = tiemChung,
            });
        }
    }
}
