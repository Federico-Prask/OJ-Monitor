namespace OJMonitor.API.Services.Interfaces
{
    public interface IPlatformService
    {
        Task<List<PlatformInfoDto>> GetAvailablePlatformsAsync();
        Task<UserStatsDto> GetUserStatsAsync(string platformId, string username);
        Task<List<SubmissionRecordDto>> GetRecentSubmissionsAsync(string platformId, string username, int limit = 10);
        Task<bool> ValidateUserAsync(string platformId, string username);
    }
}