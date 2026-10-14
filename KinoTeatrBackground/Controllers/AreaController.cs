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
    public class AreaController : Controller
    {
        private readonly AppDataContext _context;

        public AreaController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet("getAreas")]
        public async Task<ActionResult<IEnumerable<AreaDTO>>> GetAllAreas()
        {

            try 
            {
                var result = await (from a in _context.Areas
                                    join
                                    t in _context.TypeAreas on a.TypeAreaId equals t.Id
                                    join
                                    s in _context.Streets on a.StreetId equals s.Id
                                    join c in _context.Towns on s.TownId equals c.Id
                                    select new AreaDTO()
                                    {
                                        Id = a.Id,
                                        TypeAreaId = a.TypeAreaId,
                                        Adress = a.Adress,
                                        StreetId = a.StreetId,
                                        DayCost = a.DayCost,
                                        Capacity = a.Capacity,

                                        TypeAreaName = t.Name,
                                        StreetName = s.Name
                                    }).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet("getStreets")]   
        public async Task<ActionResult<IEnumerable<StreetDTO>>> GetAllStreets()
        {
            try
            {
                var streetList = await (from s in _context.Streets
                                        join
                                        t in _context.Towns on s.TownId equals t.Id select new StreetDTO()
                                        {
                                            Id= s.Id,
                                            Name = s.Name,
                                            TownId = s.TownId,
                                            TownName = t.Name
                                        }).ToListAsync();
                return Ok(streetList);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet("getTypes")]
        public async Task<ActionResult<IEnumerable<TypeArea>>> GetAllTypes()
        {
            try
            {
                var streetList = await (from s in _context.TypeAreas select s).ToListAsync();

                return Ok(streetList);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("addArea")]
        public async Task<IActionResult> AddNewArea([FromBody] NewAreaRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Area area = new Area()
                {
                    TypeAreaId = request.TypeAreaId,
                    Adress = request.Adress,
                    StreetId = request.StreetId,
                    DayCost = request.DayCost,
                    Capacity = request.Capacity
                };

                _context.Add(area);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500);
            }
        }
    }
}
