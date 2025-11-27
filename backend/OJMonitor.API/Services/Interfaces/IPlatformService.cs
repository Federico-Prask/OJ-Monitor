namespace OJMonitor.API.Services.Interfaces
{
    public interface IPlatformService
    {
        Task<List<object>> GetAvailablePlatformsAsync();
        Task<object> GetUserStatsAsync(string platformId, string username);
        Task<List<object>> GetRecentSubmissionsAsync(string platformId, string username, int limit = 10);
        Task<bool> ValidateUserAsync(string platformId, string username);
    }
}