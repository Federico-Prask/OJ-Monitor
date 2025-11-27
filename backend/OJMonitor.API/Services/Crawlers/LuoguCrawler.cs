using OJMonitor.API.Services.Interfaces;

namespace OJMonitor.API.Services.Crawlers
{
    public class LuoguCrawler : IDataCrawler
    {
        public string PlatformId => "luogu";

        public async Task<object> GetUserStatsAsync(string username)
        {
            await Task.Delay(100); // 模拟网络请求
            return new { solved = 156, submissions = 423, accuracy = 78 };
        }

        public async Task<List<object>> GetRecentSubmissionsAsync(string username, int limit = 10)
        {
            await Task.Delay(100); // 模拟网络请求
            return new List<object>
            {
                new
                {
                    problem = "A+B Problem",
                    id = "P1001",
                    difficulty = "简单",
                    time = DateTime.UtcNow.AddHours(-1).ToString("yyyy-MM-dd HH:mm"),
                    status = "通过"
                }
            };
        }

        public async Task<bool> ValidateUserAsync(string username)
        {
            await Task.Delay(100);
            return !string.IsNullOrEmpty(username);
        }
    }
}