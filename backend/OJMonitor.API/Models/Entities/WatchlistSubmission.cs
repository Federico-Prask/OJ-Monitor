namespace OJMonitor.API.Models.Entities
{
    /// <summary>
    /// 监视列表中用户的最新提交记录
    /// </summary>
    public class WatchlistSubmission
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProblemId { get; set; }
        public string ProblemTitle { get; set; }
        public int Difficulty { get; set; } // 0-7 对应难度等级
        public DateTime SubmitTime { get; set; }
        public bool IsNotified { get; set; } = false;
    }
}
