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
    public class ProcatController : Controller
    {
        private readonly AppDataContext _context;

        public ProcatController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet("getVideoCinemas")]
        public async Task<ActionResult<IEnumerable<VideoProductInCinemasDTO>>> GetVideoCinemas()
        {
            try
            {
                    var videoCin = await (from d in _context.VideoInCinemas
                                                           join
                                                           v in _context.VideoProducts on d.VideoProductId equals v.Id
                                                           join
                                                           c in _context.Cinemas on d.CinemaId equals c.Id
                                                           select new VideoProductInCinemasDTO()
                                                           {
                                                                Id = d.Id,
                                                                VideoProductId = d.VideoProductId,
                                                                CinemaId = d.CinemaId,
                                                                Revenue = d.Revenue,
                                                                Rating = d.Rating,

                                                                VideoName = v.Name,
                                                                CinemaName = c.Name,
                                                                DateProduct = v.DateEnd
                                                           }).ToListAsync();
                return Ok(videoCin);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("getCinemas")]
        public async Task<ActionResult<IEnumerable<Cinema>>> GetCinemas()
        {
            try
            {
                var listCinemas = await (from c in _context.Cinemas select c).ToListAsync();
                return Ok(listCinemas);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }   
        }

        [HttpPost("addNewProcat")]
        public async Task<IActionResult> AddNewProcat([FromBody] NewProcatRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                VideoProductInCinemas video = new VideoProductInCinemas()
                {
                    VideoProductId = request.VideoProductId,
                    CinemaId = request.CinemaId,
                    Revenue = request.Revenue,
                    Rating = request.Rating
                };
                _context.VideoInCinemas.Add(video);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return StatusCode(200);
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
    }
}
