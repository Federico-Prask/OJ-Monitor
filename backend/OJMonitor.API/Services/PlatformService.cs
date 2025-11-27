using OJMonitor.API.Services.Interfaces;

namespace OJMonitor.API.Services
{
    public class PlatformService : IPlatformService
    {
        public async Task<List<object>> GetAvailablePlatformsAsync()
        {
            // 添加异步操作以避免警告
            await Task.Delay(1);
            
            return new List<object>
            {
                new
                {
                    id = "luogu",
                    name = "洛谷",
                    description = "国内知名的算法竞赛社区",
                    baseUrl = "https://www.luogu.com.cn",
                    color = "#1a9c55",
                    isEnabled = true
                },
                new
                {
                    id = "lsyoi",
                    name = "LSYOJ",
                    description = "立山中学在线评测系统",
                    baseUrl = "http://lsyoi.top:81",
                    color = "#3b82f6",
                    isEnabled = true
                },
                new
                {
                    id = "oiclass",
                    name = "OI Class",
                    description = "信息学竞赛在线学习平台",
                    baseUrl = "https://oiclass.com",
                    color = "#8b5cf6",
                    isEnabled = true
                }
            };
        }

        public async Task<object> GetUserStatsAsync(string platformId, string username)
        {
            // 添加异步操作以避免警告
            await Task.Delay(1);
            
            // 返回模拟数据
            return platformId switch
            {
                "luogu" => new { solved = 156, submissions = 423, accuracy = 78 },
                "lsyoi" => new { solved = 89, submissions = 201, accuracy = 82 },
                "oiclass" => new { solved = 124, submissions = 298, accuracy = 75 },
                _ => new { solved = 0, submissions = 0, accuracy = 0 }
            };
        }

        public async Task<List<object>> GetRecentSubmissionsAsync(string platformId, string username, int limit = 10)
        {
            // 添加异步操作以避免警告
            await Task.Delay(1);
            
            // 返回模拟数据
            var submissions = new List<object>
            {
                new
                {
                    problem = "A+B Problem",
                    id = "P1001",
                    difficulty = "简单",
                    time = DateTime.UtcNow.AddHours(-1).ToString("yyyy-MM-dd HH:mm"),
                    status = "通过",
                    language = "C++",
                    executionTime = 15,
                    memoryUsage = 2048
                },
                new
                {
                    problem = "斐波那契数列",
                    id = "P1002",
                    difficulty = "简单",
                    time = DateTime.UtcNow.AddHours(-2).ToString("yyyy-MM-dd HH:mm"),
                    status = "通过",
                    language = "Python",
                    executionTime = 23,
                    memoryUsage = 4096
                }
            };

            return submissions;
        }

        public async Task<bool> ValidateUserAsync(string platformId, string username)
        {
            // 添加异步操作以避免警告
            await Task.Delay(100);
            return !string.IsNullOrEmpty(username);
        }
    }
}