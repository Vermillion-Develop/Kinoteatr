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
    public class ZatratController : Controller
    {
        private readonly AppDataContext _context;

        public ZatratController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet("getZatrats")]
        public async Task<ActionResult<IEnumerable<ZatratDTO>>> GetZatrats()
        {
            try
            {
                var listZatrat = await (from z in _context.Zatrats
                                        join t in _context.TypesZatrats on z.TypeZatratId equals t.Id
                                        join v in _context.VideoProducts on z.VideoProductId equals v.Id
                                        select new ZatratDTO()
                                        {
                                            Id = z.Id,
                                            TypeZatratId = z.TypeZatratId,
                                            Cost = z.Cost,
                                            Date = z.Date,
                                            VideoProductId = z.VideoProductId,

                                            VideoName = v.Name,
                                            TypeName = t.Name
                                            
                                        }).ToListAsync();
                return Ok(listZatrat);
            }
            catch(Exception ex)
            {
                return  BadRequest(ex.Message);
            }
        }

        [HttpGet("getTypesZatrat")]
        public async Task<ActionResult<IEnumerable<ZatratDTO>>> GetTypesZatrat()
        {
            try
            {
                var listZatrat = await (from t in _context.TypesZatrats select t).ToListAsync();
                return Ok(listZatrat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addZatrat")]
        public async Task<IActionResult> AddNewZatrat([FromBody] NewZatratRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Zatrat zat = new Zatrat()
                {
                    TypeZatratId = request.TypeZatratId,
                    Cost = request.Cost,
                    Date = request.Date,
                    VideoProductId= request.VideoProductId
                };
                _context.Zatrats.Add(zat);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return StatusCode(200);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
