using System.Collections.Generic;

namespace OJMonitor.API.Models.DTOs
{
    public class WatchlistSubmissionDto
    {
        public string Username { get; set; }
        public string ProblemId { get; set; }
        public string ProblemTitle { get; set; }
        public int Difficulty { get; set; }
        public DateTime SubmitTime { get; set; }
    }

    public class NewSubmissionNotificationDto
    {
        public string Username { get; set; }
        public string ProblemId { get; set; }
        public string ProblemTitle { get; set; }
        public int Difficulty { get; set; }
        public string DifficultyName { get; set; }
        public DateTime SubmitTime { get; set; }
    }
}
