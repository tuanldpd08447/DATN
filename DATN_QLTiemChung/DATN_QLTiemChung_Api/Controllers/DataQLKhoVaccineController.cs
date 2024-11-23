using DATN_QLTiemChung_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
     


        
    }
}
