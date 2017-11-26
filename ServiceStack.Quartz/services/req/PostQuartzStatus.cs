namespace ServiceStack.Quartz
{
    public class PostQuartzStatus : IPost, IReturn<PostQuartzStatusResponse>
    {
        public bool Standby { get; set; }
        public bool Shutdown { get; set; }
        public string[] PauseGroups { get; set; }
        public string[] ResumeGroups { get; set; }
        public bool PauseAll { get; set; }
        public bool ResumeAll { get; set; }
        public string[] PauseJobs { get; set; }
        public string[] ResumeJobs { get; set; }
    }
}