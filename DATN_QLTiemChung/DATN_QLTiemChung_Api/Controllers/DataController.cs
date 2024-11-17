using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly DBContext _context;

        public DataController(DBContext context)
        {
            _context = context;
        }

        // Lấy tất cả ChucDanhs
        [HttpGet("chucdanh")]
        public async Task<ActionResult<IEnumerable<ChucDanh>>> GetChucDanhs()
        {
            return await _context.ChucDanh.ToListAsync();
        }

        // Lấy tất cả ChungTus
        [HttpGet("chungtu")]
        public async Task<ActionResult<IEnumerable<ChungTu>>> GetChungTus()
        {
            return await _context.ChungTu.ToListAsync();
        }

        // Lấy tất cả DangKyTiemChungs
        [HttpGet("dangkytimchung")]
        public async Task<ActionResult<IEnumerable<DangKyTiemChung>>> GetDangKyTiemChungs()
        {
            return await _context.DangKyTiemChung.ToListAsync();
        }

        // Lấy tất cả DangKyVaccines
        [HttpGet("dangkyvaccine")]
        public async Task<ActionResult<IEnumerable<DangKyVaccine>>> GetDangKyVaccines()
        {
            return await _context.DangKyVaccine.ToListAsync();
        }

        // Lấy tất cả Districts
        [HttpGet("district")]
        public async Task<ActionResult<IEnumerable<District>>> GetDistricts()
        {
            return await _context.districts.ToListAsync();
        }

        // Lấy tất cả HoaDons
        [HttpGet("hoadon")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDons()
        {
            return await _context.HoaDon.ToListAsync();
        }

        // Lấy tất cả HoaDonChiTiets
        [HttpGet("hoadonchitiet")]
        public async Task<ActionResult<IEnumerable<HoaDonChiTiet>>> GetHoaDonChiTiets()
        {
            return await _context.HoaDonChiTiet.ToListAsync();
        }

        // Lấy tất cả HoanTras
        [HttpGet("hoantra")]
        public async Task<ActionResult<IEnumerable<HoanTra>>> GetHoanTras()
        {
            return await _context.HoanTra.ToListAsync();
        }

        // Lấy tất cả KhachHangs
        [HttpGet("khachhang")]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetKhachHangs()
        {
            return await _context.KhachHang.ToListAsync();
        }

        // Lấy tất cả KhamSangLocs
        [HttpGet("khamsangloc")]
        public async Task<ActionResult<IEnumerable<KhamSangLoc>>> GetKhamSangLocs()
        {
            return await _context.KhamSangLoc.ToListAsync();
        }

        // Lấy tất cả LoaiVatTus
        [HttpGet("loaivattu")]
        public async Task<ActionResult<IEnumerable<LoaivatTu>>> GetLoaiVatTus()
        {
            return await _context.LoaiVatTu.ToListAsync();
        }

        // Lấy tất cả NguonCungCaps
        [HttpGet("nguoncungcap")]
        public async Task<ActionResult<IEnumerable<NguonCungCap>>> GetNguonCungCaps()
        {
            return await _context.NguonCungCap.ToListAsync();
        }

        // Lấy tất cả NhaCungCaps
        [HttpGet("nhacungcap")]
        public async Task<ActionResult<IEnumerable<NhaCungCap>>> GetNhaCungCaps()
        {
            return await _context.NhaCungCap.ToListAsync();
        }

        // Lấy tất cả NhanViens
        [HttpGet("nhanvien")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanViens()
        {
            return await _context.NhanVien.ToListAsync();
        }

        // Lấy tất cả Orders
        [HttpGet("order")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Order.ToListAsync();
        }

        // Lấy tất cả Provinces
        [HttpGet("province")]
        public async Task<ActionResult<IEnumerable<Province>>> GetProvinces()
        {
            return await _context.provinces.ToListAsync();
        }

        // Lấy tất cả PhongBans
        [HttpGet("phongban")]
        public async Task<ActionResult<IEnumerable<PhongBan>>> GetPhongBans()
        {
            return await _context.PhongBan.ToListAsync();
        }

        // Lấy tất cả QLyTaiKhoanKHs
        [HttpGet("qlytkkh")]
        public async Task<ActionResult<IEnumerable<QLyTaiKhoanKH>>> GetQLyTaiKhoanKHs()
        {
            return await _context.QLyTaiKhoanKH.ToListAsync();
        }

        // Lấy tất cả QLyTaiKhoanNVs
        [HttpGet("qlytknv")]
        public async Task<ActionResult<IEnumerable<QLyTaiKhoanNV>>> GetQLyTaiKhoanNVs()
        {
            return await _context.QLyTaiKhoanNV.ToListAsync();
        }

        // Lấy tất cả TiemChungs
        [HttpGet("tiemchung")]
        public async Task<ActionResult<IEnumerable<TiemChung>>> GetTiemChungs()
        {
            return await _context.TiemChung.ToListAsync();
        }

        // Lấy tất cả TheoDoiSauTiems
        [HttpGet("theodoisautiem")]
        public async Task<ActionResult<IEnumerable<TheoDoiSauTiem>>> GetTheoDoiSauTiems()
        {
            return await _context.TheoDoiSauTiem.ToListAsync();
        }

        // Lấy tất cả VatTuYTes
        [HttpGet("vattuyte")]
        public async Task<ActionResult<IEnumerable<VatTuYTe>>> GetVatTuYTes()
        {
            return await _context.VatTuYTe.ToListAsync();
        }

        // Lấy tất cả Wards
        [HttpGet("ward")]
        public async Task<ActionResult<IEnumerable<Ward>>> GetWards()
        {
            var wards = await _context.wards.ToListAsync();
            return Ok(wards);
        }

        // Lấy tất cả XuatXus
        [HttpGet("xuatxu")]
        public async Task<ActionResult<IEnumerable<XuatXu>>> GetXuatXus()
        {
            return await _context.XuatXu.ToListAsync();
        }
    }
}
