namespace OJMonitor.API.Services.Interfaces
{
    public interface IDataCrawler
    {
        string PlatformId { get; }
        Task<UserStatsDto> GetUserStatsAsync(string username);
        Task<List<SubmissionRecordDto>> GetRecentSubmissionsAsync(string username, int limit = 10);
        Task<bool> ValidateUserAsync(string username);
    }
}