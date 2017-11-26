namespace ServiceStack.Quartz
{
    public class PostQuartzStatusResponse
    {
        public bool InStandbyMode { get; set; }
        public bool IsShutdown { get; set; }
        public bool IsStarted { get; set; }
        public string[] PausedGroupJobs { get; set; }
    }
}