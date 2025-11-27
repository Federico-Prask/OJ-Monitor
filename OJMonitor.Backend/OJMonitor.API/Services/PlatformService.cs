using OJMonitor.API.Models.DTOs;
using OJMonitor.API.Services.Interfaces;

namespace OJMonitor.API.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IEnumerable<IDataCrawler> _crawlers;

        public PlatformService(IEnumerable<IDataCrawler> crawlers)
        {
            _crawlers = crawlers;
        }

        public async Task<List<PlatformInfoDto>> GetAvailablePlatformsAsync()
        {
            return new List<PlatformInfoDto>
            {
                new PlatformInfoDto
                {
                    Id = "luogu",
                    Name = "洛谷",
                    Description = "国内知名的算法竞赛社区",
                    BaseUrl = "https://www.luogu.com.cn",
                    Color = "#1a9c55",
                    IsEnabled = true
                },
                new PlatformInfoDto
                {
                    Id = "lsyoi",
                    Name = "LSYOJ",
                    Description = "立山中学在线评测系统",
                    BaseUrl = "http://lsyoi.top:81",
                    Color = "#3b82f6",
                    IsEnabled = true
                },
                new PlatformInfoDto
                {
                    Id = "oiclass",
                    Name = "OI Class",
                    Description = "信息学竞赛在线学习平台",
                    BaseUrl = "https://oiclass.com",
                    Color = "#8b5cf6",
                    IsEnabled = true
                }
            };
        }

        public async Task<UserStatsDto> GetUserStatsAsync(string platformId, string username)
        {
            var crawler = _crawlers.FirstOrDefault(c => c.PlatformId == platformId);
            if (crawler == null)
            {
                throw new ArgumentException($"Unsupported platform: {platformId}");
            }

            return await crawler.GetUserStatsAsync(username);
        }

        public async Task<List<SubmissionRecordDto>> GetRecentSubmissionsAsync(string platformId, string username, int limit = 10)
        {
            var crawler = _crawlers.FirstOrDefault(c => c.PlatformId == platformId);
            if (crawler == null)
            {
                throw new ArgumentException($"Unsupported platform: {platformId}");
            }

            return await crawler.GetRecentSubmissionsAsync(username, limit);
        }

        public async Task<bool> ValidateUserAsync(string platformId, string username)
        {
            var crawler = _crawlers.FirstOrDefault(c => c.PlatformId == platformId);
            if (crawler == null)
            {
                return false;
            }

            return await crawler.ValidateUserAsync(username);
        }
    }
}