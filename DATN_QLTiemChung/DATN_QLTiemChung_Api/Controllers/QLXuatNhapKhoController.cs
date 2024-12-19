using DATN_QLTiemChung_Api.Models;
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

        [HttpPut("CapNhatTrangThai/{id}")]
        public async Task<IActionResult> CapNhatTrangThai(string id)
        {
            try
            {
                var chungTu = await _context.ChungTu.FirstOrDefaultAsync(ct => ct.IDXCT == id);

                if (chungTu == null)
                {
                    return NotFound(new { message = "Không tìm thấy chứng từ với ID đã cung cấp." });
                }

                chungTu.TrangThai = false;

                _context.ChungTu.Update(chungTu);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật trạng thái thành công.", TrangThai = chungTu.TrangThai });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi cập nhật trạng thái.", error = ex.Message });
            }
        }

        private async Task<string> GenerateNewOrderIdAsync()
        {
            // Lấy ID lớn nhất từ database
            var lastOrder = await _context.Order
                .OrderByDescending(o => o.IDOD)
                .FirstOrDefaultAsync();

            string newId;
            if (lastOrder == null || string.IsNullOrEmpty(lastOrder.IDOD))
            {
                newId = "OD001";
            }
            else
            {
                string lastIdNumber = lastOrder.IDOD.Substring(2); 
                int number = int.Parse(lastIdNumber) + 1;

                newId = $"OD{number:D3}"; 
            }

            return newId;
        }

        [HttpPost("CreateChungTu")]
        public async Task<IActionResult> CreateChungTu([FromBody] crteateChungTu request)
        {
            if (request == null || string.IsNullOrEmpty(request.IDVT))
            {
                return BadRequest("Thông tin không hợp lệ.");
            }

            string newOrderId = await GenerateNewOrderIdAsync();

            var order = new Order
            {
                IDOD = newOrderId,
                IDVT = request.IDVT,
                SoLuong = request.SoLuongXuatNhap,
                GhiChu = request.GhiChu
            };

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            double thanhTien = request.SoLuongXuatNhap * request.DonGia;

            var chungTu = new ChungTu
            {
                IDXCT = request.IDXCT,
                IDNV = request.IDNV,
                IDOD = newOrderId,
                LoaiChungTu = request.LoaiChungTu,
                ThoiGian = request.ThoiGian,
                DonGia = request.DonGia,
                ThanhTien = thanhTien,
                TrangThai = request.TrangThai,
                GhiChu = request.GhiChu,
                HinhAnh = request.HinhAnh
            };

            _context.ChungTu.Add(chungTu);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tạo chứng từ thành công",
                ChungTu = chungTu,
                Order = order
            });
        }

        [HttpGet("GetAllChungTu")]
        public async Task<IActionResult> GetAllChungTu()
        {
            var ListChungTu = await _context.ChungTu
         .Include(ct => ct.Order)
         .ThenInclude(od => od.VatTuYTe)
         .Select(ct => new ChungTuDetail
         {
             IDXCT = ct.IDXCT,
             IDNV = ct.IDNV,
             IDVT = ct.Order.IDVT,
             IDTL = ct.Order.VatTuYTe.IDTL,
             IDNHC = ct.Order.VatTuYTe.IDNHC,
             IDNGC = ct.Order.VatTuYTe.IDNGC,
             IDXX = ct.Order.VatTuYTe.IDXX,
             TenVatTu = ct.Order.VatTuYTe.TenVatTu,
             HanSuDung = ct.Order.VatTuYTe.HanSuDung,
             GhiChu = ct.Order.VatTuYTe.GhiChu,
             DonGia = ct.Order.VatTuYTe.DonGia,
             SoLuongXuatNhap = ct.Order.SoLuong,
             SoLuongTonKho = ct.Order.VatTuYTe.SoLuong,
             LoaiChungTu = ct.LoaiChungTu,
             ThoiGianXuatNhap = ct.ThoiGian,
             DonGiaXuatNhap = ct.DonGia,
             ThanhTien = ct.ThanhTien,
             TrangThai = ct.TrangThai,
             HinhAnh = ct.HinhAnh
         })
         .ToListAsync();

            return Ok(ListChungTu);
        }

        [HttpPost("SearchChungTu")]
        public async Task<IActionResult> SearchChungTu([FromBody] SearchChungTuModel searchCt)
        {
            var query = _context.ChungTu
                .Include(ct => ct.Order)
                .ThenInclude(od => od.VatTuYTe)
                .AsQueryable();

            // Lọc theo số phiếu
            if (!string.IsNullOrEmpty(searchCt.SoPhieu))
            {
                query = query.Where(ct => ct.IDXCT.Contains(searchCt.SoPhieu));
            }

            // Lọc theo loại chứng từ
            if (searchCt.Loai.HasValue)
            {
                query = query.Where(ct => ct.LoaiChungTu == searchCt.Loai.Value);
            }

            // Lọc theo ID vật tư
            if (!string.IsNullOrEmpty(searchCt.IdVatTu))
            {
                query = query.Where(ct => ct.Order.VatTuYTe.IDVT == searchCt.IdVatTu);
            }

            // Lọc theo ngày xuất/nhập
            if (searchCt.NgayXuatNhap.HasValue)
            {
                query = query.Where(ct => ct.ThoiGian.Date == searchCt.NgayXuatNhap.Value.Date);
            }

            // Lọc theo trạng thái (TrangThai)
            if (searchCt.TrangThai.HasValue)
            {
                query = query.Where(ct => ct.TrangThai == searchCt.TrangThai.Value);
            }

            // Thực hiện truy vấn và lấy kết quả
            var filteredList = await query
                .Select(ct => new ChungTuDetail
                {
                    IDXCT = ct.IDXCT,
                    IDNV = ct.IDNV,
                    IDVT = ct.Order.IDVT,
                    IDTL = ct.Order.VatTuYTe.IDTL,
                    IDNHC = ct.Order.VatTuYTe.IDNHC,
                    IDNGC = ct.Order.VatTuYTe.IDNGC,
                    IDXX = ct.Order.VatTuYTe.IDXX,
                    TenVatTu = ct.Order.VatTuYTe.TenVatTu,
                    HanSuDung = ct.Order.VatTuYTe.HanSuDung,
                    GhiChu = ct.Order.VatTuYTe.GhiChu,
                    DonGia = ct.Order.VatTuYTe.DonGia,
                    SoLuongXuatNhap = ct.Order.SoLuong,
                    SoLuongTonKho = ct.Order.VatTuYTe.SoLuong,
                    LoaiChungTu = ct.LoaiChungTu,
                    ThoiGianXuatNhap = ct.ThoiGian,
                    DonGiaXuatNhap = ct.DonGia,
                    ThanhTien = ct.ThanhTien,
                    TrangThai = ct.TrangThai,
                    HinhAnh = ct.HinhAnh
                })
                .ToListAsync();

            // Nếu danh sách trống hoặc null, trả về NotFound hoặc null
            if (filteredList == null || filteredList.Count == 0)
            {
                return Ok(null); 
            }

            return Ok(filteredList);
        }


        [HttpGet("GetChungTuById/{id}")]
        public async Task<IActionResult> GetChungTuById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID không được để trống.");
            }

            var ListChungTu = await _context.ChungTu
                .Include(ct => ct.Order)
                .ThenInclude(od => od.VatTuYTe)
                .Where(ct => ct.IDXCT == id) 
                .Select(ct => new ChungTuDetail
                {
                    IDXCT = ct.IDXCT,
                    IDNV = ct.IDNV,
                    IDVT = ct.Order.IDVT,
                    IDTL = ct.Order.VatTuYTe.IDTL,
                    IDNHC = ct.Order.VatTuYTe.IDNHC,
                    IDNGC = ct.Order.VatTuYTe.IDNGC,
                    IDXX = ct.Order.VatTuYTe.IDXX,
                    TenVatTu = ct.Order.VatTuYTe.TenVatTu,
                    HanSuDung = ct.Order.VatTuYTe.HanSuDung,
                    GhiChu = ct.Order.VatTuYTe.GhiChu,
                    DonGia = ct.Order.VatTuYTe.DonGia,
                    SoLuongXuatNhap = ct.Order.SoLuong,
                    SoLuongTonKho = ct.Order.VatTuYTe.SoLuong,
                    LoaiChungTu = ct.LoaiChungTu,
                    ThoiGianXuatNhap = ct.ThoiGian,
                    DonGiaXuatNhap = ct.DonGia,
                    ThanhTien = ct.ThanhTien,
                    TrangThai = ct.TrangThai,
                    HinhAnh = ct.HinhAnh
                })
                .FirstOrDefaultAsync();

            if (ListChungTu == null)
            {
                return NotFound("Không tìm thấy chứng từ với ID cung cấp.");
            }

            return Ok(ListChungTu);
        }

        [HttpGet("GenerateNewId")]
        public async Task<IActionResult> GenerateNewId()
        {
            // Lấy ID lớn nhất hiện tại
            var maxId = await _context.ChungTu
                .OrderByDescending(ct => ct.IDXCT)
                .Select(ct => ct.IDXCT)
                .FirstOrDefaultAsync();

            string newId;
            if (string.IsNullOrEmpty(maxId))
            {
                // Nếu chưa có ID nào, bắt đầu từ CT001
                newId = "CT001";
            }
            else
            {
                // Tăng giá trị số trong ID
                var numericPart = int.Parse(maxId.Substring(2)); // Bỏ "CT" và chuyển phần còn lại sang số
                newId = $"CT{(numericPart + 1):D3}"; // Định dạng lại với 3 chữ số
            }

            return Ok(newId);
        }

        [HttpGet("GetLoaiVatTu")]
        public async Task<IActionResult> GetLoaiVatTu()
        {
            var dsvattu = await _context.LoaiVatTu.ToListAsync();
            return Ok(dsvattu);
        }
        [HttpGet("GetXuatXu")]
        public async Task<IActionResult> GetXuatXu()
        {
            var xuatXus = await _context.XuatXu.ToListAsync();
            return Ok(xuatXus);
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
