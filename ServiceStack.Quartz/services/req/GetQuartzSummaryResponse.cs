namespace ServiceStack.Quartz
{
    public class GetQuartzSummaryResponse
    {
        public bool IsInStandbyMode { get; set; }
        public bool IsShutdown { get; set; }
        public bool IsStarted { get; set; }
        public string[] CalendarNames { get; set; }
        public JobExecutionSummary[] CurrentlyExecutingJobs { get; set; }
        public string[] JobGroups { get; set; }
        public JobKeySummary[] JobKeys { get; set; }
        public SchedulerSummary Scheduler { get; set; }
    }
}