using OJMonitor.API.Services.Interfaces;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OJMonitor.API.Services.Crawlers
{
    public class LuoguWatchlistCrawler : IDataCrawler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LuoguWatchlistCrawler> _logger;

        private static readonly string[] DifficultyNames = 
        { 
            "暂无评定", "入门", "普及−", "普及/提高−", "普及+/提高", "提高+/省选−", "省选/NOI−", "NOI/NOI+/CTSC"
        };

        private static readonly string[] DifficultyColors = 
        { 
            "rgb(191, 191, 191)", // 灰
            "rgb(254, 76, 97)",   // 红
            "rgb(243, 156, 17)",  // 橙
            "rgb(255, 193, 22)",  // 黄
            "rgb(82, 196, 26)",   // 绿
            "rgb(52, 152, 219)",  // 蓝
            "rgb(157, 61, 207)",  // 紫
            "rgb(14, 29, 105)"    // 黑
        };

        public string PlatformId => "luogu";

        public LuoguWatchlistCrawler(IHttpClientFactory httpClientFactory, ILogger<LuoguWatchlistCrawler> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<object> GetUserStatsAsync(string username)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                var response = await client.GetAsync($"https://www.luogu.com.cn/user/{username}");
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                return new { solved = 0, submissions = 0, accuracy = 0 };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching stats for {username}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<object>> GetRecentSubmissionsAsync(string username, int limit = 10)
        {
            var submissions = new List<object>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                // 爬取用户的最近提交记录
                for (int page = 1; page <= 2; page++) // 默认爬取前2页
                {
                    try
                    {
                        var url = $"https://www.luogu.com.cn/record/list?user={username}&status=12&page={page}";
                        var response = await client.GetAsync(url);

                        if (!response.IsSuccessStatusCode)
                            break;

                        var content = await response.Content.ReadAsStringAsync();

                        // 提取 JSON 数据（简化版，实际应通过 API 或更复杂的解析）
                        var submissions_temp = ParseLuoguRecords(content);
                        submissions.AddRange(submissions_temp.Take(limit - submissions.Count));

                        if (submissions.Count >= limit)
                            break;

                        await Task.Delay(1000); // 避免频繁请求
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Error fetching page {page} for {username}: {ex.Message}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching submissions for {username}: {ex.Message}");
            }

            return submissions;
        }

        public async Task<bool> ValidateUserAsync(string username)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                var response = await client.GetAsync($"https://www.luogu.com.cn/user/{username}");
                return response.StatusCode != HttpStatusCode.NotFound;
            }
            catch
            {
                return false;
            }
        }

        private List<object> ParseLuoguRecords(string htmlContent)
        {
            var submissions = new List<object>();

            try
            {
                // 使用正则表达式提取 JSON 数据（洛谷页面内嵌 JSON）
                var pattern = @"decodeURIComponent\(""(.*?)""\)";
                var match = Regex.Match(htmlContent, pattern);

                if (match.Success)
                {
                    string encodedJson = match.Groups[1].Value;
                    string decodedJson = WebUtility.UrlDecode(encodedJson);

                    var jsonDoc = JsonDocument.Parse(decodedJson);
                    var records = jsonDoc.RootElement
                        .GetProperty("currentData")
                        .GetProperty("records")
                        .GetProperty("result");

                    foreach (var record in records.EnumerateArray())
                    {
                        try
                        {
                            var problem = record.GetProperty("problem");
                            var difficulty = problem.GetProperty("difficulty").GetInt32();
                            var submitTime = record.GetProperty("submitTime").GetInt64();

                            submissions.Add(new
                            {
                                problem = problem.GetProperty("title").GetString(),
                                id = problem.GetProperty("pid").GetString(),
                                difficulty = DifficultyNames[difficulty],
                                time = UnixTimeStampToDateTime(submitTime).ToString("yyyy-MM-dd HH:mm"),
                                status = "通过"
                            });
                        }
                        catch { /* Skip invalid records */ }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error parsing Luogu records: {ex.Message}");
            }

            return submissions;
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
