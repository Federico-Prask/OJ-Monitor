namespace OJMonitor.API.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; }
        
        // 导航属性
        public virtual ICollection<UserPlatform> UserPlatforms { get; set; } = new List<UserPlatform>();
    }
}