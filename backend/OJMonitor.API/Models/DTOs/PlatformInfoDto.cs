namespace OJMonitor.API.Models.DTOs
{
    public class PlatformInfoDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}