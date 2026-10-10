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
    public class ProjectController : Controller
    {
        private readonly AppDataContext _context;

        public ProjectController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet("getProjects")]
        public async Task<ActionResult<IEnumerable<VideoProductDTO>>> GetAllProjects()
        {
            try
            {
                var projectsList = await (from p in _context.VideoProducts
                                          join
                                          v in _context.VidVideoProducts on p.VidVideoProductId equals v.Id
                                          join
                                          s in _context.StatusesVideoProduct on p.StatusVideoProductId equals s.Id
                                          join
                                          a in _context.AdultVideoProducts on p.AdultVideoProductId equals a.Id

                                          select new VideoProductDTO()
                                          {
                                              Id = p.Id,
                                              Name = p.Name,
                                              Budget = p.Budget,
                                              VidVideoProductId = p.VidVideoProductId,
                                              DateStart = p.DateStart,
                                              DateEnd = p.DateEnd,
                                              StatusVideoProductId = p.StatusVideoProductId,
                                              AdultVideoProductId = p.AdultVideoProductId,
                                              Idea = p.Idea,

                                              StatusVideoName = s.Name,
                                              AdultVideoName = a.Name,
                                              VidVideoName = v.Name,

                                          }).ToListAsync();
                return Ok(projectsList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getStatuses")]
        public async Task<ActionResult<IEnumerable<StatusVideoProduct>>> GetAllStatusVideo()
        {
            try
            {
                var statusList = await (from s in _context.StatusesVideoProduct select s).ToListAsync();
                return Ok(statusList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
