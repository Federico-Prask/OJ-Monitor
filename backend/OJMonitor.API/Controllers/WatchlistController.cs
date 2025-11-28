using Microsoft.AspNetCore.Mvc;
using OJMonitor.API.Models.DTOs;
using OJMonitor.API.Services.Interfaces;

namespace OJMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistCrawlerService _watchlistCrawlerService;
        private readonly ILogger<WatchlistController> _logger;

        public WatchlistController(IWatchlistCrawlerService watchlistCrawlerService, ILogger<WatchlistController> logger)
        {
            _watchlistCrawlerService = watchlistCrawlerService;
            _logger = logger;
        }

        /// <summary>
        /// 启动监视列表爬虫
        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<object>> StartWatchlist([FromBody] WatchlistStartRequest request)
        {
            if (request?.Usernames == null || request.Usernames.Count == 0)
            {
                return BadRequest(new { error = "Usernames list is required" });
            }

            try
            {
                var interval = request.IntervalSeconds > 0 ? request.IntervalSeconds : 5;
                await _watchlistCrawlerService.StartWatchlistCrawlerAsync(request.Usernames, interval);
                
                return Ok(new 
                { 
                    message = "Watchlist crawler started",
                    userCount = request.Usernames.Count,
                    intervalSeconds = interval
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error starting watchlist: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// 停止监视列表爬虫
        /// </summary>
        [HttpPost("stop")]
        public async Task<ActionResult<object>> StopWatchlist()
        {
            try
            {
                await _watchlistCrawlerService.StopWatchlistCrawlerAsync();
                return Ok(new { message = "Watchlist crawler stopped" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error stopping watchlist: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// 获取未通知的新提交
        /// </summary>
        [HttpGet("notifications")]
        public async Task<ActionResult<List<NewSubmissionNotificationDto>>> GetNotifications()
        {
            try
            {
                var notifications = await _watchlistCrawlerService.GetUnnotifiedSubmissionsAsync();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting notifications: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// 标记提交为已通知
        /// </summary>
        [HttpPost("mark-notified")]
        public async Task<ActionResult<object>> MarkNotified([FromBody] MarkNotifiedRequest request)
        {
            if (string.IsNullOrEmpty(request?.Username) || string.IsNullOrEmpty(request?.ProblemId))
            {
                return BadRequest(new { error = "Username and ProblemId are required" });
            }

            try
            {
                await _watchlistCrawlerService.MarkAsNotifiedAsync(request.Username, request.ProblemId);
                return Ok(new { message = "Marked as notified" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking as notified: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class WatchlistStartRequest
    {
        public List<string> Usernames { get; set; }
        public int IntervalSeconds { get; set; } = 5;
    }

    public class MarkNotifiedRequest
    {
        public string Username { get; set; }
        public string ProblemId { get; set; }
    }
}
