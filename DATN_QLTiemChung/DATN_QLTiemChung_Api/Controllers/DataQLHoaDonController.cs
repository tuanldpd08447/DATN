using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DATN_QLTiemChung_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataQLHoaDonController : ControllerBase
    {
        private readonly DBContext _context;

        public DataQLHoaDonController(DBContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll/{IDHD}")]
        public async Task<ActionResult<HoaDonDTO>> GetAll(string IDHD)
        {
            var hoaDon = await _context.HoaDon
                .Include(hd => hd.KhachHang)
                .Include(hd => hd.NhanVien)
                .Include(hd => hd.HoaDonChiTiets)
                .FirstOrDefaultAsync(hd => hd.IDHD == IDHD);


            if (hoaDon == null)
            {
                return NotFound(new { message = "Hóa đơn không tồn tại" });
            }


            var dto = new HoaDonDTO
            {

                IDHD = hoaDon.IDHD,
                IDKH = hoaDon.KhachHang?.IDKH,
                IDNV = hoaDon.NhanVien?.IDNV,
                ThoiGian = hoaDon.ThoiGian,
                GhiChu = hoaDon.GhiChu,
                NoiDung = hoaDon.NoiDung,
                TongTien = hoaDon.TongTien,
                TrangThai = hoaDon.TrangThai,




                // Ánh xạ các thuộc tính cần thiết cho Khách hàng
                KhachHang = hoaDon.KhachHang != null ? new KhachHang
                {
                    IDKH = hoaDon.KhachHang.IDKH,
                    TenKhachHang = hoaDon.KhachHang.TenKhachHang,
                    SoDienThoai = hoaDon.KhachHang.SoDienThoai,
                    NgaySinh = hoaDon.KhachHang.NgaySinh,
                    IDXP = hoaDon.KhachHang.IDXP,

                    DiaChi = hoaDon.KhachHang.DiaChi
                } : null,

                // Ánh xạ các thuộc tính cần thiết cho Nhân viên
                NhanVien = hoaDon.NhanVien != null ? new NhanVien
                {
                    IDNV = hoaDon.NhanVien.IDNV,

                } : null,

                // Ánh xạ danh sách chi tiết hóa đơn
                HoaDonChiTiets = hoaDon.HoaDonChiTiets?.Select(hdct => new HoaDonChiTiet
                {
                    IDHDCT = hdct.IDHDCT,
                    IDVT = hdct.IDVT,
                    SoLuong = hdct.SoLuong,
                    DonGia = hdct.DonGia,
                    ThanhTien = hdct.ThanhTien,
                    GhiChu = hdct.GhiChu
                }).ToList() ?? new List<HoaDonChiTiet>()
            };

            return Ok(dto);
        }
        [HttpGet("GetAllBYIDKh/{IDKH}")]
        public async Task<ActionResult<HoaDonDTO>> GetAllBYIDKh(string IDKH)
        {
            try
            {
                var hoaDon = await _context.HoaDon
                    .AsNoTracking()
                    .Include(hd => hd.KhachHang)
                    .Include(hd => hd.NhanVien)
                    .Include(hd => hd.HoaDonChiTiets)
                    .FirstOrDefaultAsync(hd => hd.IDKH == IDKH && hd.ThanhToan == false);

                if (hoaDon == null)
                {
                    return NotFound(new { message = "Hóa đơn không tồn tại" });
                }

                var dto = new HoaDonDTO
                {
                    IDHD = hoaDon.IDHD,
                    IDKH = hoaDon.KhachHang?.IDKH,
                    IDNV = hoaDon.NhanVien?.IDNV,
                    ThoiGian = hoaDon.ThoiGian,
                    GhiChu = hoaDon.GhiChu,
                    NoiDung = hoaDon.NoiDung,
                    TongTien = hoaDon.TongTien,
                    TrangThai = hoaDon.TrangThai,
                    ThanhToan = hoaDon.ThanhToan,
                    KhachHang = hoaDon.KhachHang != null ? new KhachHang
                    {
                        IDKH = hoaDon.KhachHang.IDKH,
                        TenKhachHang = hoaDon.KhachHang.TenKhachHang,
                        SoDienThoai = hoaDon.KhachHang.SoDienThoai,
                        NgaySinh = hoaDon.KhachHang.NgaySinh,
                        IDXP = hoaDon.KhachHang.IDXP,
                        DiaChi = hoaDon.KhachHang.DiaChi
                    } : null,
                    NhanVien = hoaDon.NhanVien != null ? new NhanVien
                    {
                        IDNV = hoaDon.NhanVien.IDNV
                    } : null,
                    HoaDonChiTiets = hoaDon.HoaDonChiTiets?.Select(hdct => new HoaDonChiTiet
                    {
                        IDHDCT = hdct.IDHDCT,
                        IDVT = hdct.IDVT,
                        SoLuong = hdct.SoLuong,
                        DonGia = hdct.DonGia,
                        ThanhTien = hdct.ThanhTien,
                        GhiChu = hdct.GhiChu
                    }).ToList() ?? new List<HoaDonChiTiet>()
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống" });
            }
        }


        [HttpGet("GetKHBYID/{IDKH}")]
        public async Task<ActionResult<KhachHang>> GetKHBYID(string IDKH)
        {
            var khachhang = await _context.KhachHang
                .FirstOrDefaultAsync(hd => hd.IDKH == IDKH);


            if (khachhang == null)
            {
                return NotFound(new { message = "Khách hàng không tồn tại" });
            }


            var dto = new KHDTO
            {

                IDKH = khachhang.IDKH,
                IDXP = khachhang.IDXP,
                TenKhachHang = khachhang.TenKhachHang,
                NgaySinh = khachhang.NgaySinh,
                GioiTinh = khachhang.GioiTinh,
                DiaChi = khachhang.DiaChi,
                SoDienThoai = khachhang.SoDienThoai,
                Email = khachhang.Email,
                CCCD_MDD = khachhang.CCCD_MDD,
                DanToc = khachhang.DanToc,

            };




            return Ok(dto);
        }
        [HttpGet("GetDiaChi")]
        public async Task<IActionResult> GetDiaChi(string idxp)
        {
            var ward = await _context.wards
               .Include(w => w.District)
                   .ThenInclude(d => d.Province).Select(diachi => new DiaChiDTO
                   {
                       IDXP = diachi.code,
                       TenXaPhuong = diachi.full_name,
                       IDQH = diachi.District.code,
                       TenQuanHuyen = diachi.District.full_name,
                       IDTP = diachi.District.Province.code,
                       TenTinhThanhPho = diachi.District.Province.full_name
                   }).FirstOrDefaultAsync(w => w.IDXP == idxp)
           ;

            return Ok(ward);
        }
        [HttpGet("GetAllVatTu")]
        public async Task<IActionResult> GetAllVatTu()
        {
            var VatTuYTe = await _context.VatTuYTe.ToListAsync();

            return Ok(VatTuYTe);
        }
        [HttpPost("AddHoaDon")]
        public async Task<IActionResult> AddHoaDon([FromBody] HoaDonCreateDTO HoaDonDto)
        {
            // Kiểm tra dữ liệu đầu vào
            if (HoaDonDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu hóa đơn không hợp lệ." });
            }

            // Kiểm tra hóa đơn có tồn tại không

            // Tạo hóa đơn mới
            string newIDHD = await GenerateNewIDHD();
            var newHoaDon = new HoaDon
            {
                IDHD = newIDHD,
                IDKH = HoaDonDto.IDKH,
                IDNV = HoaDonDto.IDNV,
                ThoiGian = HoaDonDto.ThoiGian,
                GhiChu = HoaDonDto.GhiChu, 
                NoiDung = HoaDonDto.NoiDung, // Tên vaccine
                TongTien = HoaDonDto.TongTien,
                TrangThai = HoaDonDto.TrangThai,
                ThanhToan= HoaDonDto.ThanhToan,
            };

            // Tạo hóa đơn chi tiết mới
            string newIDHDCT = await GenerateNewIDHDCT();
            var newHoaDonChiTiet = new HoaDonChiTiet
            {
                IDHDCT = newIDHDCT,
                IDHD = newIDHD, // Liên kết đúng hóa đơn
                IDVT = HoaDonDto.IDVT,
                SoLuong = HoaDonDto.SoLuong,
                DonGia = HoaDonDto.DonGia,
                ThanhTien = HoaDonDto.ThanhTien,
                GhiChu = HoaDonDto.GhiChu
            };

            // Thêm dữ liệu vào cơ sở dữ liệu
            await _context.HoaDon.AddAsync(newHoaDon);
            await _context.HoaDonChiTiet.AddAsync(newHoaDonChiTiet);
            await _context.SaveChangesAsync();

            // Phản hồi thành công
            return Ok(new { HoaDon = newHoaDon, HoaDonChiTiet = newHoaDonChiTiet });
        }



        private async Task<string> GenerateNewIDHD()
        {
            // Lấy khách hàng có IDHD lớn nhất hiện tại
            var lastHDKH = await _context.HoaDon
                .OrderByDescending(HD => HD.IDHD)
                .FirstOrDefaultAsync();

            string newIDHD;

            if (lastHDKH != null)
            {
                // Lấy phần số cuối cùng từ IDHD (giả sử dạng HD001, HD002,...)
                string lastNumber = lastHDKH.IDHD.Substring(2); // Lấy phần sau "HD"
                int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
                newIDHD = "HD" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
            }
            else
            {
                // Nếu chưa có HDách hàng nào, bắt đầu từ HD001
                newIDHD = "HD001";
            }

            return newIDHD;
        }
        private async Task<string> GenerateNewIDHDCT()
        {
            // Lấy khách hàng có IDHD lớn nhất hiện tại
            var lastHDKH = await _context.HoaDonChiTiet
                .OrderByDescending(HD => HD.IDHDCT)
                .FirstOrDefaultAsync();

            string newIDHDCT;

            if (lastHDKH != null)
            {
                // Lấy phần số cuối cùng từ IDHD (giả sử dạng HD001, HD002,...)
                string lastNumber = lastHDKH.IDHD.Substring(2); // Lấy phần sau "HD"
                int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
                newIDHDCT = "HDCT" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
            }
            else
            {
                // Nếu chưa có HDách hàng nào, bắt đầu từ HD001
                newIDHDCT = "HDCT001";
            }

            return newIDHDCT;
        }
        [HttpPut("CancelHoaDon/{id}")]
        public async Task<IActionResult> CancelHoaDon(string id, [FromBody] bool TrangThai)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hd = await _context.HoaDon.FirstOrDefaultAsync(hd => hd.IDHD == id);
            if (hd == null)
            {
                return NotFound(new { Message = "Hóa đơn không tồn tại." });
            }

            hd.TrangThai = TrangThai;

            try
            {
                // Lưu thay đổi
                _context.HoaDon.Update(hd);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Cập nhật trạng thái thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi cập nhật trạng thái.", Error = ex.Message });
            }

        }

        [HttpPut("UpdateHoaDon/{id}")]
        public async Task<IActionResult> UpdateHoaDon(string id, [FromBody] UpdateHoaDonRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ.", Errors = ModelState });
            }

            try
            {
                // Tìm hóa đơn
                var hd = await _context.HoaDon.FirstOrDefaultAsync(h => h.IDHD == id);
                if (hd == null)
                {
                    return NotFound(new { Message = "Hóa đơn không tồn tại." });
                }

                // Cập nhật thông tin
                hd.ThanhToan = request.ThanhToan;
                hd.GhiChu = request.GhiChu;

                // Lưu thay đổi
                _context.HoaDon.Update(hd);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Cập nhật hóa đơn thành công.", HoaDon = hd });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi cập nhật hóa đơn.", Error = ex.Message });
            }
        }


    }
}