using KinoTeatrBackground.Data;
using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BCrypt;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

namespace KinoTeatrBackground.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffGroupController : Controller
    {
        private readonly AppDataContext _context;

        public StaffGroupController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet("getGroups")]
        public async Task<ActionResult<IEnumerable<CinemaGroupDTO>>> GetAllGroups()
        {
            try
            {
                var groupList = await (from g in _context.Groups
                                       join
                                       sg in _context.StaffGroups on g.Id equals sg.GroupId
                                       join
                                       v in _context.VideoProducts on sg.VideoProductId equals v.Id
                                       select new CinemaGroupDTO()
                                       {
                                           Id = g.Id,
                                           Name = g.Name,

                                           VideoId = v.Id,
                                           VideoName = v.Name
                                       }).ToListAsync();
                if (groupList == null)
                {
                    Debug.WriteLine("Ошибка получения групп");
                    return BadRequest();
                }
                return Ok(groupList);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addGroup")]
        public async Task<IActionResult> AddNewGroup([FromBody] NewGroupRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var group = new CinemaGroup()
                {
                    Name = request.Name
                };
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
                var staffGroup = new StaffGroup()
                {
                    VideoProductId = request.ProductId,
                    GroupId = group.Id
                };
                _context.StaffGroups.Add(staffGroup);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Успешная регистрация группы" });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new {message = "Ошибка добавления группы" + ex.Message});
            }
        }

        [HttpPost("updateGroup")]
        public async Task<IActionResult> UpdateGroup([FromBody] NewGroupRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            if(request == null)
            {
                return BadRequest();
            }
            try
            {
                CinemaGroup? group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId);
                if (group == null)
                {
                    return BadRequest();
                }
                group.Name = request.Name;
                await _context.SaveChangesAsync();

                List<StaffGroup>? listStaffGroup = await _context.StaffGroups.Where(sg => sg.GroupId == request.GroupId).ToListAsync();
                foreach (StaffGroup? staff in listStaffGroup)
                {
                    staff.VideoProductId = request.ProductId;
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Вроде все в порядке" });
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("getStaffGroup")]
        public async Task<ActionResult<IEnumerable<StaffGroupDTO>>> GetStaffGroup([FromBody] NewStaffGroupRequest request)
        {
            try
            {
                var staffGrList = await (from sg in _context.StaffGroups
                                         join s in _context.Staffs on sg.StaffId equals s.Id
                                         join
                                         v in _context.VideoProducts on sg.VideoProductId equals v.Id
                                         join
                                         r in _context.Specializations on sg.SpecializationId equals r.Id
                                         join
                                         g in _context.Groups on sg.GroupId equals g.Id
                                         where sg.GroupId == request.GroupId
                                         select new StaffGroupDTO()
                                         {
                                             Id = sg.Id,
                                             StaffId = sg.StaffId,
                                             VideoProductId = sg.VideoProductId,
                                             SpecializationId = sg.SpecializationId,
                                             GroupId = sg.GroupId,

                                             VideoProductName = v.Name,
                                             StaffFamily = s.Family,
                                             StaffName = s.Name,
                                             StaffFather = s.Father,
                                             StaffRoleName = r.Name,
                                             StaffOklad = r.Oklad
                                            

                                         }).ToListAsync();
                return Ok(staffGrList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("addStaffGroup")]
        public async Task<IActionResult> AddStaffInGroup([FromBody] NewStaffInGroupRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                StaffGroup staffGroup = new StaffGroup()
                {
                    StaffId = request.StaffId,
                    GroupId = request.GroupId,
                    SpecializationId = request.RoleId,
                    VideoProductId = request.ProductId
                };

                _context.StaffGroups.Add(staffGroup);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new {message = "Регистрация прошла успешно"});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка добавления сотрудника в группу" + ex.Message });
            }
        }
    }
}
