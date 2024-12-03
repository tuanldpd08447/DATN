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
                newId = "TC001";
            }
            else
            {
                string idTCNumber = idTC.IDTC.Substring(2);
                int number = int.Parse(idTCNumber) + 1;

                newId = $"TC{number:D3}";
            }

            return newId;
        }
        private async Task<string> GenerateNewIDSTAsync()
        {
            // Lấy ID lớn nhất từ database
            var idTC = await _context.TheoDoiSauTiem
                .OrderByDescending(o => o.IDST)
                .FirstOrDefaultAsync();

            string newId;
            if (idTC == null || string.IsNullOrEmpty(idTC.IDST))
            {
                newId = "ST001";
            }
            else
            {
                string idTCNumber = idTC.IDST.Substring(2);
                int number = int.Parse(idTCNumber) + 1;

                newId = $"ST{number:D3}";
            }

            return newId;
        }
        [HttpPost("CreateTiemChung")]
        public async Task<IActionResult> CreateTiemChung([FromBody] createTiemChung request)
        {
            if (request == null )
            {
                return BadRequest("Thông tin không hợp lệ.");
            }

            string newIDTC = await GenerateNewIDTCAsync();

           

            var tiemChung = new TiemChung
            {
                IDTC = newIDTC,
                IDKH = request.IDKH,
                IDDK = request.IDDK,
                IDNV = request.IDNV,
                ThoiGian = request.ThoiGian,
                TrangThai = request.TrangThai
            };
            var TheoDoiSauTiem = new TheoDoiSauTiem
            {
                IDST = await GenerateNewIDSTAsync(),
                IDTC = newIDTC,
                IDKH = request.IDKH,
                IDNV = request.IDNV,
                ThoiGian = request.ThoiGian.TimeOfDay,
                TrangThai = request.TrangThai
            };
            _context.TiemChung.Add(tiemChung);
            _context.TheoDoiSauTiem.Add(TheoDoiSauTiem);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                TiemChung = tiemChung,
            });
        }
        [HttpPut("UpdateTheoDoiSauTiem")]
        public async Task<IActionResult> UpdateTheoDoiSauTiem([FromBody] createTheoDoi request)
        {
            if (request == null || string.IsNullOrEmpty(request.IDST))
            {
                return BadRequest("Thông tin không hợp lệ hoặc thiếu IDST.");
            }

            // Tìm bản ghi TheoDoiSauTiem dựa trên IDST
            var existingRecord = await _context.TheoDoiSauTiem.FindAsync(request.IDST);
            if (existingRecord == null)
            {
                return NotFound($"Không tìm thấy TheoDoiSauTiem với IDST = {request.IDST}");
            }

            // Cập nhật thông tin từ request
            existingRecord.IDTC = request.IDTC;
            existingRecord.IDNV = request.IDNV;
            existingRecord.IDKH = request.IDKH;
            existingRecord.ThoiGian = request.ThoiGian;
            existingRecord.TrangThai = request.TrangThai;
            existingRecord.GhiChu = request.GhiChu;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.TheoDoiSauTiem.Update(existingRecord);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Cập nhật thành công",
                TheoDoiSauTiem = existingRecord
            });
        }


        [HttpGet("TheoDoiSauTiemByIDDK/{IDDK}")]
        public async Task<ActionResult> TheoDoiSauTiemByIDDK( string IDDK)
        {
            var TiemChung = await _context.TheoDoiSauTiem.Include(st=>st.TiemChung)
                .Include(st => st.KhachHang)
                .Include(st => st.NhanVien)
                .FirstOrDefaultAsync(st => st.TiemChung.IDDK == IDDK);
            return Ok(TiemChung); 
        }
    }
}
