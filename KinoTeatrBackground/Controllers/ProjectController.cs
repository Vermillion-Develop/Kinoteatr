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

        [HttpGet("getAdult")]
        public async Task<ActionResult<IEnumerable<AdultVideoProduct>>> GetAllAdults()
        {
            try
            {
                var adultList = await (from s in _context.AdultVideoProducts select s).ToListAsync();
                return Ok(adultList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getVidVideo")]
        public async Task<ActionResult<IEnumerable<VidVideoProduct>>> GetAllVidVideo()
        {
            try
            {
                var vidList = await (from s in _context.VidVideoProducts select s).ToListAsync();
                return Ok(vidList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addNewVideo")]
        public async Task<IActionResult> AddNewProject([FromBody] NewProjectRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newFilm = new VideoProduct()
                {
                    Name = request.Name,
                    Budget = request.Budget,
                    VidVideoProductId = request.VidVideoProductId,
                    DateStart = request.DateStart,
                    DateEnd = request.DateEnd,
                    StatusVideoProductId = request.StatusVideoProductId,
                    AdultVideoProductId = request.AdultVideoProductId,
                    Idea = request.Idea
                };
                _context.VideoProducts.Add(newFilm);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Успешная регистрация сотрудника!" });
            }
            catch (Exception ex) 
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Ошибка: " + ex.Message });
            }
        }

        [HttpPost("updateSelectedVideo")]
        public async Task<IActionResult> UpdateSelectedProject([FromBody] NewProjectRequest request)
        {
            Debug.WriteLine(request.Id);

            if (request == null)
            {
                return BadRequest();
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                VideoProduct? selectedProduct = await _context.VideoProducts.FirstOrDefaultAsync(x => x.Id == request.Id);
                if(selectedProduct == null)
                {
                    return StatusCode(500);
                }
                selectedProduct.Name = request.Name;
                selectedProduct.Budget = request.Budget;
                selectedProduct.VidVideoProductId = request.VidVideoProductId;
                selectedProduct.DateEnd = request.DateEnd;
                selectedProduct.StatusVideoProductId = request.StatusVideoProductId;
                selectedProduct.AdultVideoProductId = request.AdultVideoProductId;
                selectedProduct.Idea = request.Idea;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Успешное обновление проекта" });

            }
            catch (Exception ex) 
            {
                transaction.Rollback();
                return StatusCode(500, new { message = "Ошибка обновления проекта" + ex.Message });
            }
        }
    }
}
