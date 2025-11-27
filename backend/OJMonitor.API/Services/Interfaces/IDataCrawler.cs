namespace OJMonitor.API.Services.Interfaces
{
    public interface IDataCrawler
    {
        string PlatformId { get; }
        Task<object> GetUserStatsAsync(string username);
        Task<List<object>> GetRecentSubmissionsAsync(string username, int limit = 10);
        Task<bool> ValidateUserAsync(string username);
    }
}