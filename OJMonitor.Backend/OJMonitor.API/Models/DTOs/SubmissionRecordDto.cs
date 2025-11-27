namespace OJMonitor.API.Models.DTOs
{
    public class SubmissionRecordDto
    {
        public string Problem { get; set; } = string.Empty;
        public string ProblemId { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public DateTime SubmitTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int ExecutionTime { get; set; }
        public int MemoryUsage { get; set; }
    }
}
