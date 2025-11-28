using OJMonitor.API.Models.DTOs;
using OJMonitor.API.Models.Entities;
using OJMonitor.API.Services.Interfaces;
using System.Collections.Concurrent;

namespace OJMonitor.API.Services
{
    public class WatchlistCrawlerService : IWatchlistCrawlerService
    {
        private readonly IDataCrawler _luoguCrawler;
        private readonly ILogger<WatchlistCrawlerService> _logger;
        
        private ConcurrentDictionary<string, WatchlistSubmission> _lastSubmissions = 
            new ConcurrentDictionary<string, WatchlistSubmission>();
        
        private ConcurrentBag<NewSubmissionNotificationDto> _unnotifiedSubmissions = 
            new ConcurrentBag<NewSubmissionNotificationDto>();
        
        private CancellationTokenSource _crawlerCts;
        private Task _crawlerTask;
        
        private static readonly string[] DifficultyNames = 
        { 
            "暂无评定", "入门", "普及−", "普及/提高−", "普及+/提高", "提高+/省选−", "省选/NOI−", "NOI/NOI+/CTSC"
        };

        public WatchlistCrawlerService(IDataCrawler luoguCrawler, ILogger<WatchlistCrawlerService> logger)
        {
            _luoguCrawler = luoguCrawler;
            _logger = logger;
        }

        public async Task StartWatchlistCrawlerAsync(List<string> usernames, int intervalSeconds = 5)
        {
            if (_crawlerTask != null && !_crawlerTask.IsCompleted)
            {
                _logger.LogWarning("Watchlist crawler is already running");
                return;
            }

            _crawlerCts = new CancellationTokenSource();
            _crawlerTask = CrawlerLoopAsync(usernames, intervalSeconds, _crawlerCts.Token);
            
            _logger.LogInformation($"Watchlist crawler started for {usernames.Count} users with interval {intervalSeconds}s");
            await Task.CompletedTask;
        }

        public async Task StopWatchlistCrawlerAsync()
        {
            if (_crawlerCts != null && !_crawlerCts.Token.IsCancellationRequested)
            {
                _crawlerCts.Cancel();
                if (_crawlerTask != null)
                {
                    try
                    {
                        await _crawlerTask;
                    }
                    catch (OperationCanceledException) { }
                }
                _logger.LogInformation("Watchlist crawler stopped");
            }
        }

        public async Task<List<NewSubmissionNotificationDto>> GetUnnotifiedSubmissionsAsync()
        {
            var result = _unnotifiedSubmissions.ToList();
            // 清空已取出的通知
            _unnotifiedSubmissions = new ConcurrentBag<NewSubmissionNotificationDto>();
            return result;
        }

        public async Task MarkAsNotifiedAsync(string username, string problemId)
        {
            var key = $"{username}:{problemId}";
            if (_lastSubmissions.TryGetValue(key, out var submission))
            {
                submission.IsNotified = true;
            }
            await Task.CompletedTask;
        }

        private async Task CrawlerLoopAsync(List<string> usernames, int intervalSeconds, CancellationToken cancellationToken)
        {
            var interval = TimeSpan.FromSeconds(intervalSeconds);
            var openTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    foreach (var username in usernames)
                    {
                        try
                        {
                            var submissions = await _luoguCrawler.GetRecentSubmissionsAsync(username, limit: 5);
                            
                            foreach (var submission in submissions.OfType<dynamic>())
                            {
                                string problemId = submission.id;
                                string key = $"{username}:{problemId}";

                                if (!_lastSubmissions.ContainsKey(key))
                                {
                                    // 新的提交记录
                                    var newNotification = new NewSubmissionNotificationDto
                                    {
                                        Username = username,
                                        ProblemId = problemId,
                                        ProblemTitle = submission.problem,
                                        Difficulty = Array.IndexOf(DifficultyNames, submission.difficulty),
                                        DifficultyName = submission.difficulty,
                                        SubmitTime = DateTime.Parse(submission.time)
                                    };

                                    _unnotifiedSubmissions.Add(newNotification);
                                    
                                    var submissionRecord = new WatchlistSubmission
                                    {
                                        Username = username,
                                        ProblemId = problemId,
                                        ProblemTitle = submission.problem,
                                        Difficulty = newNotification.Difficulty,
                                        SubmitTime = newNotification.SubmitTime,
                                        IsNotified = false
                                    };

                                    _lastSubmissions[key] = submissionRecord;
                                    
                                    _logger.LogInformation(
                                        $"New submission detected: {username} -> {problemId} ({submission.difficulty}题)");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error crawling {username}: {ex.Message}");
                        }

                        // 避免频繁请求
                        await Task.Delay(500, cancellationToken);
                    }

                    // 等待下一个轮询间隔
                    await Task.Delay(interval, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error in crawler loop: {ex.Message}");
                    await Task.Delay(interval, cancellationToken);
                }
            }
        }
    }
}
