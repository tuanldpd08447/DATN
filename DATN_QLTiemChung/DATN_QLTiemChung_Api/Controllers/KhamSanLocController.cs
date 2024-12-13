        using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATN_QLTiemChung_Api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhamSanLocController : ControllerBase
    {
        private readonly DBContext _context;

        public KhamSanLocController(DBContext context)
        {
            _context = context;
        }
        [HttpGet("GetKhachHangById/{id}")]
        public async Task<IActionResult> GetKhachHangById(string id)
        {
            var khachHang = await _context.KhachHang
                .Include(kh => kh.Ward).ThenInclude(w => w.District).ThenInclude(d => d.Province)
                .Select(kh => new KhachHangDTo2
                {
                    IDKH = kh.IDKH,
                    TenKhachHang = kh.TenKhachHang,
                    NgaySinh = kh.NgaySinh,
                    GioiTinh = kh.GioiTinh,
                    DiaChi = kh.DiaChi,
                    SoDienThoai = kh.SoDienThoai,
                    Email = kh.Email,
                    CCCD_MDD = kh.CCCD_MDD,
                    DanToc = kh.DanToc,
                    FullAddress = kh.DiaChi + ", " + kh.Ward.name + ", " + kh.Ward.District.name + ", " + kh.Ward.District.Province.name,
                    IDXP = kh.IDXP,
                    NameXP = kh.Ward.name,
                    IDQH = kh.Ward.District.code,
                    NameQH = kh.Ward.District.name,
                    IDTTP = kh.Ward.District.Province.code,
                    NameTTP = kh.Ward.District.Province.name,
                })
                .FirstOrDefaultAsync(kh => kh.IDKH == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return Ok(khachHang);
        }
        [HttpGet("GetAllVatTu")]
        public async Task<IActionResult> GetAllVatTu()
        {
            var vatTu = await _context.VatTuYTe
                  .Include(hd => hd.LoaivatTu)
                  .Include(hd => hd.NguonCungCap)
                  .Include(hd => hd.NhaCungCap)
                  .Include(hd => hd.XuatXu)
                  .Select(hd => new VatTuDTOFull

                  {

                      IDVT = hd.IDVT.ToString(),
                      IDTL = hd.IDTL,
                      TenLoai = hd.LoaivatTu.LoaiVatTu,
                      IDNGC = hd.IDNGC,
                      TenNguon = hd.NguonCungCap.TenNguonCungCap,
                      IDNHC = hd.IDNHC,
                      TenNhaCungCap = hd.NhaCungCap.TenNhaCungCap,
                      IDXX = hd.IDXX,
                      NoiXuatXu = hd.XuatXu.QuocGia,
                      TenVatTu = hd.TenVatTu,
                      DonGia = hd.DonGia,
                      HanSuDung = hd.HanSuDung,
                      SoLuong = hd.SoLuong,
                      GhiChu = hd.GhiChu

                  }).ToListAsync();

            return Ok(vatTu);
        }
        [HttpPost("AddKhamSangLoc")]
        public async Task<IActionResult> AddKhamSangLoc([FromBody] KhamSangLocDTO model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Tạo IDKS tự động tăng
            var lastRecord = await _context.KhamSangLoc
                                            .OrderByDescending(x => x.IDKS)
                                            .Select(x => x.IDKS)
                                            .FirstOrDefaultAsync();

            string newIDKS = GenerateNextID(lastRecord);

            var ksl = new KhamSangLoc
            {
                IDKS = newIDKS,
                IDKH = model.IDKH,
                IDNV = model.IDNV,
                CanNang = model.CanNang,
                ChieuCao = model.ChieuCao,
                ThoiGian = DateTime.Now,
                TrangThai = model.TrangThai,
                KetQua = model.KetQua,
                GhiChu = model.GhiChu,
                TinhTrangSucKhoe = model.TinhTrangSucKhoe
            };

            if (model.TrangThai || model.KetQua)
            {
  
                var vt = await _context.VatTuYTe
                                        .Where(vt => vt.IDVT == model.IDVT)
                                        .Select(vt => new { vt.TenVatTu, vt.DonGia })
                                        .FirstOrDefaultAsync();

                if (vt == null)
                {
                    return BadRequest("Không tìm thấy thông tin vật tư y tế.");
                }

                var lastIDDKVC = await _context.DangKyVaccine
                                                .OrderByDescending(x => x.IDDKVC)
                                                .Select(x => x.IDDKVC)
                                                .FirstOrDefaultAsync();

                string newIDDKVC = GenerateNextIDDKVC(lastIDDKVC);

                var dkvt = new DangKyVaccine
                {
                    IDDKVC = newIDDKVC,
                    IDVT = model.IDVT,
                    TenVaccine = vt.TenVatTu,
                    SoLuong = model.SoLuong,
                    DonGia = vt.DonGia,
                    ThanhTien = vt.DonGia * model.SoLuong,
                    GhiChu = " "
                };

                // Tạo IDDK tự động tăng
                var lastIDDK = await _context.DangKyTiemChung
                                             .OrderByDescending(x => x.IDDK)
                                             .Select(x => x.IDDK)
                                             .FirstOrDefaultAsync();

                string newIDDK = GenerateNextIDDK(lastIDDK);

                var dktc = new DangKyTiemChung
                {
                    IDDK = newIDDK,
                    IDDKVC = newIDDKVC,
                    IDKH = model.IDKH,
                    IDNV = model.IDNV,
                    ThoiGianDK = DateTime.Now,
                    ThoiGianTiem = DateTime.Now,
                    GhiChu = ""
                };

                var lastIDHD = await _context.HoaDon
                                             .OrderByDescending(x => x.IDHD)
                                             .Select(x => x.IDHD)
                                             .FirstOrDefaultAsync();

                string newIDHD = GenerateNextIDHD(lastIDHD);

                var newHoaDon = new HoaDon
                {
                    IDHD = newIDHD,
                    IDKH = model.IDKH,
                    IDNV = model.IDNV,
                    ThoiGian = DateTime.Now,
                    GhiChu = null, 
                    NoiDung = vt.TenVatTu, 
                    TongTien = vt.DonGia * model.SoLuong ?? 0,
                    TrangThai = false,
                    ThanhToan = false
                };

                var lastIDHDCT = await _context.HoaDonChiTiet
                                               .OrderByDescending(x => x.IDHDCT)
                                               .Select(x => x.IDHDCT)
                                               .FirstOrDefaultAsync();

                // Tạo hóa đơn chi tiết mới
                string newIDHDCT = GenerateNextIDHDCT(lastIDHDCT);

                var newHoaDonChiTiet = new HoaDonChiTiet
                {
                    IDHDCT = newIDHDCT,
                    IDHD = newIDHD, 
                    IDVT = model.IDVT,
                    SoLuong = model.SoLuong,
                    DonGia = vt.DonGia ?? 0,
                    ThanhTien = (vt.DonGia * model.SoLuong) ?? 0,
                    GhiChu = null
                };

                _context.DangKyVaccine.Add(dkvt);
                _context.DangKyTiemChung.Add(dktc);
                _context.HoaDon.Add(newHoaDon);
                _context.HoaDonChiTiet.Add(newHoaDonChiTiet);
            }

            try
            {
                // Lưu vào cơ sở dữ liệu
                _context.KhamSangLoc.Add(ksl);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm mới thành công!", data = model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


        private string GenerateNextIDHD(string lastID)
        {
            if (string.IsNullOrEmpty(lastID))
            {
                return "HD001"; // ID đầu tiên
            }

            // Tách tiền tố "DK" và số thứ tự
            string prefix = lastID.Substring(0, 2);
            int number = int.Parse(lastID.Substring(2)) + 1;

            // Tạo ID mới với định dạng "DK###"
            return $"{prefix}{number:000}";
        }
        private string GenerateNextIDHDCT(string lastID)
        {
            if (string.IsNullOrEmpty(lastID))
            {
                return "HDCT001"; // ID đầu tiên
            }

            // Tách tiền tố "DK" và số thứ tự
            string prefix = lastID.Substring(0, 4);
            int number = int.Parse(lastID.Substring(4)) + 1;

            // Tạo ID mới với định dạng "DK###"
            return $"{prefix}{number:000}";
        }

        private string GenerateNextIDDK(string lastID)
        {
            if (string.IsNullOrEmpty(lastID))
            {
                return "DK001"; // ID đầu tiên
            }

            // Tách tiền tố "DK" và số thứ tự
            string prefix = lastID.Substring(0, 2);
            int number = int.Parse(lastID.Substring(2)) + 1;

            // Tạo ID mới với định dạng "DK###"
            return $"{prefix}{number:000}";
        }
        private string GenerateNextID(string lastID )
        {
            if (string.IsNullOrEmpty(lastID))
            {
                return "KS001"; // ID đầu tiên
            }

            string prefix = lastID.Substring(0, 2);
            int number = int.Parse(lastID.Substring(2)) + 1;

            // Tạo ID mới với định dạng "KS###"
            return $"{prefix}{number:000}";
        }
        private string GenerateNextIDDKVC(string lastID)
        {
            if (string.IsNullOrEmpty(lastID))
            {
                return "DKVC001"; // ID đầu tiên
            }

            // Tách tiền tố "DKVC" và số thứ tự
            string prefix = lastID.Substring(0, 4);
            int number = int.Parse(lastID.Substring(4)) + 1;

            // Tạo ID mới với định dạng "DKVC###"
            return $"{prefix}{number:000}";
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
    }
}
