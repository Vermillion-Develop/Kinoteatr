using KinoTeatrBackground.Data;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BCrypt;

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
                                   where s.Status == true
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
            var staffData = await (from s in _context.Staffs
                                   join r in _context.Specializations on s.SpecializationId equals r.Id
                                   join d in _context.DataLogs on s.DataLogId equals d.DataLogId
                                   where s.DataLogId == request.Login && s.Status == true
                                   select new
                                   {
                                       s.Id,
                                       s.Family,
                                       s.Name,
                                       s.Father,
                                       s.SpecializationId,
                                       s.Phone,
                                       s.Email,
                                       s.Stavka,
                                       s.DataLogId,
                                       SpecializationName = r.Name,
                                       HashedPassword = d.Password
                                   }).FirstOrDefaultAsync();

            if (staffData == null || !BCrypt.Net.BCrypt.Verify(request.Password, staffData.HashedPassword))
            {
                return Unauthorized(new { message = "Неверный логин или пароль" });
            }

            var staffDto = new StaffDTO
            {
                Id = staffData.Id,
                Family = staffData.Family,
                Name = staffData.Name,
                Father = staffData.Father,
                SpecializationId = staffData.SpecializationId,
                Phone = staffData.Phone,
                Email = staffData.Email,
                Stavka = staffData.Stavka,
                DataLogId = staffData.DataLogId,
                SpecializationName = staffData.SpecializationName
            };

            return Ok(staffDto);
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
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
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
                    DataLogId = newLogin.DataLogId,
                    Status = true
                    
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

        [HttpPost("updateStaff")]
        public async Task<IActionResult> UpdateStaff([FromBody] NewStaffRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var selectedUser = await _context.Staffs
                    .FirstOrDefaultAsync(s => s.Id == request.Id);

                var selectedDataLog = await _context.DataLogs
                    .FirstOrDefaultAsync(d => d.DataLogId == selectedUser.DataLogId);

                if (selectedDataLog == null || selectedUser == null)
                {
                    Debug.WriteLine("Ошибка при редактировании данных");
                    return BadRequest();
                }

                selectedUser.DataLogId = null;
                await _context.SaveChangesAsync();

                _context.DataLogs.Remove(selectedDataLog);
                await _context.SaveChangesAsync();

                var newDataLog = new DataLog
                {
                    DataLogId = request.NewDataLogId,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };


                await _context.DataLogs.AddAsync(newDataLog);
                await _context.SaveChangesAsync();


                selectedUser.Family = request.Family;
                selectedUser.Name = request.Name;
                selectedUser.Father = request.Father;

                selectedUser.SpecializationId = request.SpecializationId;
                selectedUser.Phone = request.Phone;
                selectedUser.Email = request.Email;
                selectedUser.Stavka = request.Stavka;


                selectedUser.DataLogId = newDataLog.DataLogId;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { message = "Данные сотрудника успешно обновлены." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                var innerError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Debug.WriteLine($"Ошибка БД: {innerError}");

                return StatusCode(500, $"Внутренняя ошибка сервера: {innerError}");
            }
        }

        [HttpPost("deleteStaff")]
        public async Task<IActionResult> DeleteStaff([FromBody] int selectedStaff)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var selectedUser = await (from s in _context.Staffs where s.Id == selectedStaff select s).FirstOrDefaultAsync();
                if(selectedUser == null)
                {
                    return BadRequest();
                }
                selectedUser.Status = false;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
