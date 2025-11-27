namespace OJMonitor.API.Models.DTOs
{
    public class UserStatsDto
    {
        public int Solved { get; set; }
        public int Submissions { get; set; }
        public double Accuracy { get; set; }
        public int Ranking { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}