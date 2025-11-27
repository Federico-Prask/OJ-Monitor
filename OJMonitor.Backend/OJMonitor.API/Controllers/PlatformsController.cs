using Microsoft.AspNetCore.Mvc;
using OJMonitor.API.Models.DTOs;
using OJMonitor.API.Services.Interfaces;

namespace OJMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformService;

        public PlatformsController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlatformInfoDto>>> GetPlatforms()
        {
            var platforms = await _platformService.GetAvailablePlatformsAsync();
            return Ok(platforms);
        }

        [HttpGet("{platformId}/users/{username}/stats")]
        public async Task<ActionResult<UserStatsDto>> GetUserStats(string platformId, string username)
        {
            try
            {
                var stats = await _platformService.GetUserStatsAsync(platformId, username);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{platformId}/users/{username}/submissions")]
        public async Task<ActionResult<List<SubmissionRecordDto>>> GetRecentSubmissions(
            string platformId, string username, [FromQuery] int limit = 10)
        {
            try
            {
                var submissions = await _platformService.GetRecentSubmissionsAsync(platformId, username, limit);
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{platformId}/users/{username}/validate")]
        public async Task<ActionResult<bool>> ValidateUser(string platformId, string username)
        {
            try
            {
                var isValid = await _platformService.ValidateUserAsync(platformId, username);
                return Ok(isValid);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}