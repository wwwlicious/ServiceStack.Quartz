namespace ServiceStack.Quartz
{
    [Route("/quartz/api/status", "POST")]
    public class PostQuartzStatus : IPost, IReturn<PostQuartzStatusResponse>
    {
        public bool Standby { get; set; }
        public bool Shutdown { get; set; }
        public bool PauseAll { get; set; }
        public bool ResumeAll { get; set; }
        public string[] PauseJobGroups { get; set; }
        public string[] PauseTriggerGroups { get; set; }
        public string[] ResumeJobGroups { get; set; }
        public string[] ResumeTriggerGroups { get; set; }
        public string[] PauseJobs { get; set; }
        public string[] PauseTriggers { get; set; }
        public string[] ResumeJobs { get; set; }
        public string[] ResumeTriggers { get; set; }
    }
}