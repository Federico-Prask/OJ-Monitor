using HtmlAgilityPack;
using OJMonitor.API.Models.DTOs;
using OJMonitor.API.Services.Interfaces;

namespace OJMonitor.API.Services.Crawlers
{
    public class LuoguCrawler : IDataCrawler
    {
        private readonly HttpClient _httpClient;
        public string PlatformId => "luogu";

        public LuoguCrawler(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://www.luogu.com.cn/");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", 
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        }

        public async Task<UserStatsDto> GetUserStatsAsync(string username)
        {
            try
            {
                // 使用洛谷的API或解析HTML页面获取用户数据
                var response = await _httpClient.GetAsync($"user/{username}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch user data: {response.StatusCode}");
                }

                var htmlContent = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // 解析HTML获取统计数据
                // 这里需要根据洛谷的实际HTML结构进行调整
                var solvedNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'solved')]//span");
                var submissionsNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'submissions')]//span");

                return new UserStatsDto
                {
                    Solved = int.Parse(solvedNode?.InnerText.Trim() ?? "0"),
                    Submissions = int.Parse(submissionsNode?.InnerText.Trim() ?? "0"),
                    Accuracy = CalculateAccuracy(htmlContent),
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to crawl Luogu data for user {username}: {ex.Message}", ex);
            }
        }

        public async Task<List<SubmissionRecordDto>> GetRecentSubmissionsAsync(string username, int limit = 10)
        {
            var records = new List<SubmissionRecordDto>();
            
            try
            {
                // 调用洛谷API获取最近提交记录
                // 实际实现需要根据洛谷API文档进行调整
                var response = await _httpClient.GetAsync($"api/record/list?user={username}&limit={limit}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    // 解析JSON响应
                    records = ParseSubmissionRecords(json);
                }
            }
            catch (Exception ex)
            {
                // 记录日志
                Console.WriteLine($"Error fetching submissions: {ex.Message}");
            }

            return records;
        }

        public async Task<bool> ValidateUserAsync(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"user/{username}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private double CalculateAccuracy(string htmlContent)
        {
            // 实现正确率计算逻辑
            // 这需要根据洛谷页面的具体结构来解析
            return 0.75; // 示例值
        }

        private List<SubmissionRecordDto> ParseSubmissionRecords(string json)
        {
            // 解析JSON并转换为DTO列表
            // 实际实现需要根据洛谷API的响应格式
            return new List<SubmissionRecordDto>
            {
                new SubmissionRecordDto
                {
                    Problem = "A+B Problem",
                    ProblemId = "P1001",
                    Difficulty = "简单",
                    SubmitTime = DateTime.UtcNow.AddHours(-1),
                    Status = "通过",
                    Language = "C++",
                    ExecutionTime = 15,
                    MemoryUsage = 2048
                }
            };
        }
    }
}
