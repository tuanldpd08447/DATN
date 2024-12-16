using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
       


        [HttpGet("LsTiem/{IDKH}")]
        public async Task<ActionResult> LsTiem(string IDKH)
        {
            // Lấy dữ liệu từ bảng TheoDoiSauTiem
            var lsTiem = await _context.TheoDoiSauTiem
                .Include(st => st.TiemChung)
                    .ThenInclude(tc => tc.DangKyTiemChung)
                    .ThenInclude(dktc => dktc.DangKyVaccine)
                .Where(st => st.IDKH == IDKH)
                .Select(st => st.TiemChung.IDDK) // Chỉ lấy danh sách IDDK đã tiêm
                .ToListAsync();

            // Lấy dữ liệu từ bảng DangKyTiemChung và loại bỏ các bản ghi đã xuất hiện trong lsTiem
            var lsDK = await _context.DangKyTiemChung
                .Include(dktc => dktc.DangKyVaccine)
                .Where(dktc => dktc.IDKH == IDKH && !lsTiem.Contains(dktc.IDDK)) // Loại bỏ các IDDK đã tiêm
                .Select(dktc => new LichSuTiem
                {
                    ID = dktc.IDDK, // ID của đăng ký
                    IDKH = dktc.IDKH,
                    TenKhachHang = dktc.KhachHang.TenKhachHang,
                    IDVT = dktc.DangKyVaccine.IDVT,
                    TenVacxin = dktc.DangKyVaccine.TenVaccine,
                    XuatXu = dktc.DangKyVaccine.VatTuYTe.XuatXu.QuocGia,
                    NgayTiem = dktc.ThoiGianTiem,
                    DonGia = dktc.DangKyVaccine.DonGia,
                    ThanhTien = dktc.DangKyVaccine.DonGia * dktc.DangKyVaccine.SoLuong,
                    LieuTiem = dktc.DangKyVaccine.SoLuong.ToString(),
                    TrangThaiTiem = false,
                    IDNV = dktc.IDNV,
                    TenNhanVien = dktc.NhanVien.TenNhanVien,
                    GhiChu = dktc.GhiChu
                })
                .ToListAsync();

            // Kết hợp danh sách đã tiêm từ TheoDoiSauTiem
            var lsTiemDetails = await _context.TheoDoiSauTiem
                .Include(st => st.TiemChung)
                    .ThenInclude(tc => tc.DangKyTiemChung)
                    .ThenInclude(dktc => dktc.DangKyVaccine)
                .Where(st => st.IDKH == IDKH)
                .Select(st => new LichSuTiem
                {
                    ID = st.IDST,
                    IDKH = st.IDKH,
                    TenKhachHang = st.KhachHang.TenKhachHang,
                    IDVT = st.TiemChung.DangKyTiemChung.DangKyVaccine.IDVT,
                    TenVacxin = st.TiemChung.DangKyTiemChung.DangKyVaccine.TenVaccine,
                    XuatXu = st.TiemChung.DangKyTiemChung.DangKyVaccine.VatTuYTe.XuatXu.QuocGia,
                    NgayTiem = st.TiemChung.ThoiGian,
                    DonGia = st.TiemChung.DangKyTiemChung.DangKyVaccine.DonGia,
                    ThanhTien = st.TiemChung.DangKyTiemChung.DangKyVaccine.ThanhTien,
                    LieuTiem = st.GhiChu,
                    TrangThaiTiem = st.TiemChung.TrangThai,
                    TrangThaiSauTiem = st.TrangThai,
                    IDNV = st.IDNV,
                    TenNhanVien = st.NhanVien.TenNhanVien,
                    GhiChu = st.TiemChung.GhiChu
                })
                .ToListAsync();

            // Kết hợp danh sách và trả về
            var combinedResult = lsDK.Concat(lsTiemDetails).ToList();

            return Ok(combinedResult);
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
            return Ok(khachHangKSL);
        }

        [HttpGet("KQKhamSangLoc/{IDKH}")]
        public async Task<ActionResult> KQKhamSangLoc(string IDKH)
        {
            var kQKhamSangLoc = await _context.KhamSangLoc
                                           .Include(kh => kh.KhachHang)
                                           .Where(kh => kh.IDKH == IDKH)
                                           .OrderByDescending(kh => kh.ThoiGian)
                                           .FirstOrDefaultAsync();

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
            var cdVaccine = await _context.DangKyTiemChung
                                       .Include(kh => kh.KhachHang)
                                       .Include(kh => kh.DangKyVaccine)
                                       .Include(kh => kh.NhanVien)
                                       .Where(kh => kh.IDKH == IDKH)
                                       .OrderByDescending(kh => kh.ThoiGianDK)
                                       .FirstOrDefaultAsync();
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
                GhiChu = cdVaccine.GhiChu,
                KhachHang = cdVaccine.KhachHang != null ? new KhachHang
                {
                    IDKH = cdVaccine.KhachHang.IDKH,
                } : null,

                DangKyVaccine = cdVaccine.DangKyVaccine != null ? new DangKyVaccine
                {
                    IDDKVC = cdVaccine.DangKyVaccine.IDDKVC,
                    TenVaccine = cdVaccine.DangKyVaccine.TenVaccine,
                    SoLuong = cdVaccine.DangKyVaccine.SoLuong,
                } : null,

                NhanVien = cdVaccine.NhanVien != null ? new NhanVien
                {
                    IDNV = cdVaccine.NhanVien.IDNV,
                    TenNhanVien = cdVaccine.NhanVien.TenNhanVien,
                } : null
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
            if (request == null)
            {
                return BadRequest("Thông tin không hợp lệ.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Sinh ID mới
                    string newIDTC = await GenerateNewIDTCAsync();
                    string newIDST = await GenerateNewIDSTAsync();

                    // Lấy thông tin đăng ký tiêm chủng và vật tư y tế
                    var dk = await _context.DangKyTiemChung
                                           .Include(dk => dk.DangKyVaccine.VatTuYTe)
                                           .FirstOrDefaultAsync(dk => dk.IDDK == request.IDDK);

                    if (dk == null)
                    {
                        return NotFound($"Không tìm thấy đăng ký tiêm chủng với IDDK: {request.IDDK}");
                    }

                    var vt = dk.DangKyVaccine.VatTuYTe;
                    if (vt == null || vt.SoLuong <= 0)
                    {
                        return BadRequest("Số lượng vật tư không đủ.");
                    }

                    // Thêm tiêm chủng
                    var tiemChung = new TiemChung
                    {
                        IDTC = newIDTC,
                        IDKH = request.IDKH,
                        IDDK = request.IDDK,
                        IDNV = request.IDNV,
                        ThoiGian = request.ThoiGian,
                        TrangThai = request.TrangThai
                    };

                    // Thêm theo dõi sau tiêm
                    var theoDoiSauTiem = new TheoDoiSauTiem
                    {
                        IDST = newIDST,
                        IDTC = newIDTC,
                        IDKH = request.IDKH,
                        IDNV = request.IDNV,
                        ThoiGian = request.ThoiGian.TimeOfDay,
                        TrangThai = request.TrangThai
                    };

                    // Cập nhật GhiChu và vật tư y tế
                    dk.GhiChu = (int.TryParse(dk.GhiChu, out int value) ? value + 1 : 1).ToString();
                    vt.SoLuong--;

                    // Lưu vào database
                    _context.TiemChung.Add(tiemChung);
                    _context.TheoDoiSauTiem.Add(theoDoiSauTiem);
                    _context.DangKyTiemChung.Update(dk);
                    _context.VatTuYTe.Update(vt);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(new { TiemChung = tiemChung });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Đã xảy ra lỗi: {ex.Message}");
                }
            }
        }



        [HttpPut("UpdateTheoDoiSauTiem")]
        public async Task<IActionResult> UpdateTheoDoiSauTiem([FromBody] createTheoDoi request)
        {
            if (request == null || string.IsNullOrEmpty(request.IDST))
            {
                return BadRequest("Thông tin không hợp lệ hoặc thiếu IDST.");
            }

            try
            {
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

                // Tìm bản ghi TiemChung
                var existingTC = await _context.TiemChung.FirstOrDefaultAsync(tc => tc.IDTC == request.IDTC);
                if (existingTC == null)
                {
                    return NotFound($"Không tìm thấy TiemChung với IDTC = {request.IDTC}");
                }

                if (request.TrangThai)
                {
                    existingTC.TrangThai = true;
                    var ghiChu = existingRecord.GhiChu; // "Mũi 1", "Mũi 2", ...
                    if (!string.IsNullOrEmpty(ghiChu))
                    {
                        // Sử dụng Regex để lấy số trong chuỗi
                        var match = Regex.Match(ghiChu, @"\d+");
                        if (match.Success && int.TryParse(match.Value, out int soMui))
                        {
                            existingTC.SoMui = soMui;
                        }
                        else
                        {
                            existingTC.SoMui = null;
                        }
                    }
                    else
                    {
                        existingTC.SoMui = null;
                    }
                    _context.TiemChung.Update(existingTC);
                }
                else 
                {
                    existingTC.TrangThai = true;
                    _context.TiemChung.Update(existingTC);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.TheoDoiSauTiem.Update(existingRecord);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Cập nhật thành công",
                    TheoDoiSauTiem = existingRecord,
                    TiemChung = existingTC
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }


        [HttpGet("TheoDoiSauTiemByIDDK/{IDDK}")]
        public async Task<ActionResult> TheoDoiSauTiemByIDDK(string IDDK)
        {
            var TiemChung = await _context.TheoDoiSauTiem.Include(st => st.TiemChung)
                .Include(st => st.KhachHang)
                .Include(st => st.NhanVien)
                .Where(st => st.TiemChung.IDDK == IDDK && !st.TrangThai)
                .OrderByDescending(kh => kh.TiemChung.ThoiGian)
                .FirstOrDefaultAsync();

            return Ok(TiemChung);
        }
    }
}
