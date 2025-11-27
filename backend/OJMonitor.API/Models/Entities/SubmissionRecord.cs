namespace OJMonitor.API.Models.Entities
{
    public class SubmissionRecord
    {
        public int Id { get; set; }
        public int UserPlatformId { get; set; }
        public string ProblemId { get; set; } = string.Empty;
        public string ProblemName { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int ExecutionTime { get; set; }
        public int MemoryUsage { get; set; }
        public DateTime SubmitTime { get; set; }
        
        // 导航属性
        public virtual UserPlatform UserPlatform { get; set; } = null!;
    }
}