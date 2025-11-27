namespace OJMonitor.API.Models.Entities
{
    public class UserPlatform
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlatformId { get; set; } = string.Empty;
        public string PlatformUsername { get; set; } = string.Empty;
        public DateTime LastSynced { get; set; }
        public bool IsActive { get; set; } = true;
        
        // 导航属性
        public virtual User User { get; set; } = null!;
    }
}