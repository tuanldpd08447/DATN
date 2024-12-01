using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace DATN_QLTiemChung_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QLNhanVienController : ControllerBase
    {
        private readonly DBContext _context;
        public QLNhanVienController(DBContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllNhanVien")]
        public async Task<IActionResult> GetAllNhanVien()
        {
            var nhanViens = await _context.NhanVien
                .Include(nv => nv.ChucDanh)
                .Include(nv => nv.PhongBan)
                .Include(nv => nv.Ward) //Lấy thông tin từ bảng Ward
                .ThenInclude(w => w.District)//Lấy thông tin từ bảng District
                .ThenInclude(d => d.Province)//Lấy thông tin từ bảng Province
                .Select(nv => new NhanVienDTO
                {
                    IDNV = nv.IDNV.ToString(),
                    TenNhanVien = nv.TenNhanVien,
                    ChucVu = nv.ChucVu,
                    TenChucDanh =   nv.ChucDanh.TenChucDanh,
                    TenPhongBan = nv.PhongBan.TenPhongBan,
                    DiaChi = nv.DiaChi,
                    CCCD = nv.CCCD,
                    DanToc = nv.DanToc,
                    NgaySinh = nv.NgaySinh,
                    Email = nv.Email,
                    GioiTinh = nv.GioiTinh,
                    SoDienThoai = nv.SoDienThoai,
                    FullAddress = $"{nv.DiaChi}, {nv.Ward.name}, {nv.Ward.District.name}, {nv.Ward.District.Province.name}"
                })
           .ToListAsync();
            return Ok(nhanViens);
        }

        [HttpGet("GetAllBYIDNV/{IDNV}")]
        public async Task<ActionResult<NhanVienDTO>> GetAllBYIDNV(string IDNV)
        {
            var nhanVien = await _context.NhanVien
                .Include(nv => nv.Ward)
                .Include(nv => nv.ChucDanh)
                .Include(nv => nv.PhongBan)
                .FirstOrDefaultAsync(nv => nv.IDNV == IDNV);
            if (nhanVien == null)
            {
                return NotFound(new { message = "Nhân Viên không tồn tại" });
            }
            var dto = new NhanVienDTO
            {
                IDNV = IDNV,  // Sử dụng IDNV mới
                IDXP = nhanVien.IDXP,

                ChucVu = nhanVien.ChucVu,
                TenChucDanh = nhanVien.IDCD,
                TenPhongBan = nhanVien.IDPB,
                TenNhanVien = nhanVien.TenNhanVien,
                NgaySinh = nhanVien.NgaySinh,
                GioiTinh = nhanVien.GioiTinh,
                DiaChi = nhanVien.DiaChi,
                SoDienThoai = nhanVien.SoDienThoai,
                Email = nhanVien.Email,
                CCCD = nhanVien.CCCD,
                DanToc = nhanVien.DanToc
            };

            return Ok(dto);
        }


        [HttpPost("AddNhanVien")]
        public async Task<IActionResult> AddNhanVien([FromBody] NhanVienCreateDTO nhanVienDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Kiểm tra xem Ward có tồn tại không
            var ward = await _context.wards.FindAsync(nhanVienDTO.IDXP);
            if (ward == null)
            {
                return NotFound(new { Message = "Ward không tồn tại." });
            }

            var ChucDanh = await _context.ChucDanh.FindAsync(nhanVienDTO.TenChucDanh);
            if (ChucDanh == null)
            {
                return NotFound(new { Message = "Chuc Danh không tồn tại." });
            }

            var PhongBan = await _context.PhongBan.FindAsync(nhanVienDTO.TenPhongBan);
            if (PhongBan == null)
            {
                return NotFound(new { Message = "Phong Ban không tồn tại." });
            }
            // Tạo IDNV mới
            string newIDNV = await GenerateNewIDNV();

            var newNhanVien = new NhanVien()
            {
                IDNV = newIDNV,  // Sử dụng IDNV mới
                IDXP = nhanVienDTO.IDXP,
                Ward = ward,
                ChucVu = nhanVienDTO.ChucVu,
                IDCD = ChucDanh.IDCD,
                IDPB = PhongBan.IDPB,
                TenNhanVien = nhanVienDTO.TenNhanVien,
                NgaySinh = nhanVienDTO.NgaySinh,
                GioiTinh = nhanVienDTO.GioiTinh,
                DiaChi = nhanVienDTO.DiaChi,
                SoDienThoai = nhanVienDTO.SoDienThoai,
                Email = nhanVienDTO.Email,
                CCCD = nhanVienDTO.CCCD,
                DanToc = nhanVienDTO.DanToc
            };

            // Thêm Nhan Vien vào Database
            await _context.NhanVien.AddAsync(newNhanVien);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNhanVienById), new { id = newNhanVien.IDNV }, newNhanVien);
        
        }

        [HttpPut("UpdateNhanVien")]
        public async Task<IActionResult> UpdateNhanVien( [FromBody] NhanVienCreateDTO nhanVienDTO)
        {
            // Kiểm tra dữ liệu hợp lệ
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tìm nhân viên trong cơ sở dữ liệu theo ID
            var nv = await _context.NhanVien.FindAsync(nhanVienDTO.IDNV);
            if (nv == null)
            {
                return NotFound(new { Message = "Nhân viên không tồn tại." });
            }

            // Kiểm tra nếu dữ liệu gửi đến không thay đổi gì so với dữ liệu cũ
            if (nv.Equals(nhanVienDTO))
            {
                return NoContent(); // Nếu không thay đổi gì, trả về NoContent
            }
            var ward = await _context.wards.FindAsync(nhanVienDTO.IDXP);
            if (ward == null)
            {
                return NotFound(new { Message = "Ward không tồn tại." });
            }

            var ChucDanh = await _context.ChucDanh.FindAsync(nhanVienDTO.TenChucDanh);
            if (ChucDanh == null)
            {
                return NotFound(new { Message = "Chuc Danh không tồn tại." });
            }

            var PhongBan = await _context.PhongBan.FindAsync(nhanVienDTO.TenPhongBan);
            if (PhongBan == null)
            {
                return NotFound(new { Message = "Phong Ban không tồn tại." });
            }
            // Cập nhật các trường thông tin của nhân viên từ DTO
            nv.IDXP = nhanVienDTO.IDXP;
            nv.ChucVu = nhanVienDTO.ChucVu;
            nv.IDCD = ChucDanh.IDCD;
            nv.IDPB = PhongBan.IDPB;
            nv.TenNhanVien = nhanVienDTO.TenNhanVien;
            nv.NgaySinh = nhanVienDTO.NgaySinh;
            nv.GioiTinh = nhanVienDTO.GioiTinh;
            nv.DiaChi = nhanVienDTO.DiaChi;
            nv.SoDienThoai = nhanVienDTO.SoDienThoai;
            nv.Email = nhanVienDTO.Email;
            nv.CCCD = nhanVienDTO.CCCD;
            nv.DanToc = nhanVienDTO.DanToc;
            _context.NhanVien.Update(nv);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected == 0)
            {
                return StatusCode(500, "Không có thay đổi nào được lưu.");
            }

            // Trả về đối tượng nhân viên đã cập nhật
            return Ok(nv);
          
        }




        [HttpGet("GetChucDanh")]
        public async Task<IActionResult> GetChucDanh()
        {
            var dsChucDanh = await _context.ChucDanh.ToListAsync();
            return Ok(dsChucDanh);
        }
        [HttpGet("GetPhongBan")]
        public async Task<IActionResult> GetPhongBan()
        {
            var dsphongBan = await _context.PhongBan.ToListAsync();
            return Ok(dsphongBan);
        }
       
       

        private async Task<string> GenerateNewIDNV()
        {
            // Lấy Nhan Vien có IDNV lớn nhất hiện tại
            var lastNhanVien = await _context.NhanVien
                .OrderByDescending(kh => kh.IDNV)
                .FirstOrDefaultAsync();

            string newIDNV;

            if (lastNhanVien != null)
            {
                // Lấy phần số cuối cùng từ IDNV (giả sử dạng NV001, Nv002,...)
                string lastNumber = lastNhanVien.IDNV.Substring(2); // Lấy phần sau "NV"
                int nextNumber = int.Parse(lastNumber) + 1; // Tăng số lên 1
                newIDNV = "NV" + nextNumber.ToString("D3"); // Đảm bảo có 3 chữ số
            }
            else
            {
                // Nếu chưa có nhan vien nào, bắt đầu từ NV001
                newIDNV = "NV001";
            }

            return newIDNV;
        }

        [HttpGet("GetWardByid/{id}")]
        public async Task<ActionResult<DiaChi>> GetWardByid(string id)
        {
            // Lấy thông tin của ward dựa trên id
            var wards = await _context.wards.FirstOrDefaultAsync(w => w.code == id);
            if (wards == null)
            {
                return NotFound("Không tìm thấy phường/xã với mã " + id);
            }

            // Lấy thông tin của district dựa trên mã của ward
            var districts = await _context.districts.FirstOrDefaultAsync(w => w.code == wards.district_code);
            if (districts == null)
            {
                return NotFound("Không tìm thấy quận/huyện với mã " + wards.district_code);
            }

            // Lấy thông tin của province dựa trên mã của district
            var provinces = await _context.provinces.FirstOrDefaultAsync(w => w.code == districts.province_code);
            if (provinces == null)
            {
                return NotFound("Không tìm thấy tỉnh/thành phố với mã " + districts.province_code);
            }

            // Tạo đối tượng DiaChi
            DiaChi diaChi = new DiaChi
            {
                IDXP = wards.code,
                IDQH = districts.code,
                IDTTP = provinces.code,
                NameXP = wards.name,
                NameQH = districts.name,
                NameTTP = provinces.name,
            };

            return Ok(diaChi);
        }

        // GET: api/NhanVien/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNhanVienById(string id)
        {
            var nhanVien = await _context.NhanVien
                .Include(kh => kh.Ward)
                .FirstOrDefaultAsync(kh => kh.IDNV == id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return Ok(nhanVien);
        }

    }
    }

