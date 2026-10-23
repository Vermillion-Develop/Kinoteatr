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
    public class GraphicController : Controller
    {
        private readonly AppDataContext _context;

        public GraphicController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet("getGraphic")]
        public async Task<ActionResult<IEnumerable<GraphicVideoDTO>>> GetGraphicVideos()
        {
            try
            {
                var graphList = await (from g in _context.GraphicVideos
                                       join s in _context.StaffGroups on g.StaffGroupId equals s.Id
                                       join v in _context.VideoProducts on g.VideoProductId equals v.Id
                                       join a in _context.Areas on g.AreaId equals a.Id
                                       join c in _context.Scenes on g.SceneId equals c.Id
                                       join r in _context.Groups on s.GroupId equals r.Id
                                       join j in _context.Streets on a.StreetId equals j.Id
                                       join t in _context.Towns on j.TownId equals t.Id
                                       select new GraphicVideoDTO()
                                       {
                                           Id = g.Id,
                                           StaffGroupId = g.StaffGroupId,
                                           VideoProductId = g.VideoProductId,
                                           AreaId = g.AreaId,
                                           SceneId = g.SceneId,
                                           Date = g.Date,
                                           Time = g.Time,

                                           StaffName = r.Name,
                                           VideoName = v.Name,
                                           AreaName = a.Adress,
                                           SceneName = c.Name,
                                           StreetName = j.Name,
                                           TownName = t.Name
                                       }).ToListAsync();
                return Ok(graphList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getScenes")]
        public async Task<ActionResult<IEnumerable<Scene>>> GetAllScenes()
        {
            try
            {
                var sceneList = await (from s in _context.Scenes select s).ToListAsync();
                return Ok(sceneList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addGraphic")]
        public async Task<IActionResult> AddNewGraphic([FromBody] NewGraphicRequest request)
        {
            using var  transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                GraphicVideo graph = new GraphicVideo()
                {
                    StaffGroupId = request.StaffGroupId,
                    VideoProductId = request.VideoProductId,
                    AreaId = request.AreaId,
                    SceneId = request.SceneId,
                    Date = request.Date,
                    Time = request.Time
                };

                _context.Add(graph);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return StatusCode(200);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                return StatusCode(500);
            }
        }


    }
}
