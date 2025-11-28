using OJMonitor.API.Models.DTOs;

namespace OJMonitor.API.Services.Interfaces
{
    /// <summary>
    /// 监视列表爬虫服务 - 定期爬取用户提交并通知
    /// </summary>
    public interface IWatchlistCrawlerService
    {
        /// <summary>
        /// 开始监视列表爬虫（后台任务）
        /// </summary>
        Task StartWatchlistCrawlerAsync(List<string> usernames, int intervalSeconds = 5);

        /// <summary>
        /// 停止监视列表爬虫
        /// </summary>
        Task StopWatchlistCrawlerAsync();

        /// <summary>
        /// 获取未通知的新提交
        /// </summary>
        Task<List<NewSubmissionNotificationDto>> GetUnnotifiedSubmissionsAsync();

        /// <summary>
        /// 标记提交为已通知
        /// </summary>
        Task MarkAsNotifiedAsync(string username, string problemId);
    }
}
