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

        [HttpGet("getRoles")]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations()
        {
            var roleList = await (from r in _context.Specializations select r).ToListAsync();
            return Ok(roleList);
        }

        [HttpPost("newStaff")]
        public async Task<IActionResult> RegisterStaff([FromBody] NewStaffRequest request)
        {
            var existingLogin = await _context.DataLogs.FirstOrDefaultAsync(l => l.DataLogId == request.DataLogId);

            if (existingLogin != null)
            {
                return BadRequest(new { message = "Пользователь с таким email уже существует!" });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newLogin = new DataLog
                {
                    DataLogId = request.DataLogId,
                    Password = request.Password
                };
                _context.DataLogs.Add(newLogin);
                await _context.SaveChangesAsync();

                var newUser = new Staff
                {
                    Family = request.Family,
                    Name = request.Name,
                    Father = request.Father,
                    SpecializationId = request.SpecializationId,
                    Phone = request.Phone,
                    Email = request.Email,
                    Stavka = request.Stavka,
                    DataLogId = newLogin.DataLogId
                };
                _context.Staffs.Add(newUser);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { message = "Успешная регистрация сотрудника!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = $"Ошибка на сервере: {ex.Message}" });
            }
        }
    }
}
