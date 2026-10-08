using KinoTeatrBackground.Data;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KinoTeatrBackground.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : Controller
    {
        private readonly AppDataContext _context;

        public StaffController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet("getStaff")]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetAllStaff()
        {
            var staffList = await (from s in _context.Staffs
                                   join
                                   r in _context.Specializations on s.SpecializationId equals r.Id
                                   select new StaffDTO()
                                   {
                                        Id = s.Id,
                                        Family = s.Family,
                                        Name = s.Name,
                                        Father = s.Father,
                                        SpecializationId = s.SpecializationId,
                                        Phone = s.Phone,
                                        Email = s.Email,
                                        Stavka = s.Stavka,
                                        DataLogId = s.DataLogId,
                                        ///////////////////////////
                                        SpecializationName = r.Name
                                   }).ToListAsync();
            return Ok(staffList);
        }

        [HttpPost("loginStaff")]
        public async Task<IActionResult> GetCurrentStaff([FromBody] NewLoginRequest request)
        {
            if(request == null)
            {
                return BadRequest();
            }
            StaffDTO? staff = await (from s in _context.Staffs
                                    join
                                    r in _context.Specializations on s.SpecializationId equals r.Id
                                    join
                                    d in _context.DataLogs on s.DataLogId equals d.DataLogId
                                    where s.DataLogId == request.Login && d.Password == request.Password
                                    select new StaffDTO() 
                                    {
                                        Id = s.Id,
                                        Family = s.Family,
                                        Name= s.Name,
                                        Father= s.Father,
                                        SpecializationId = s.SpecializationId,
                                        Phone = s.Phone,
                                        Email= s.Email,
                                        Stavka= s.Stavka,
                                        DataLogId= s.DataLogId,

                                        SpecializationName= r.Name

                                    }).FirstOrDefaultAsync();

            if (staff == null)
            {
                return Unauthorized(new { message = "Неверный логин или пароль" });
            }
            else
            {
                return Ok(staff);
            }
        }
    }
}
