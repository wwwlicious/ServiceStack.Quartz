namespace ServiceStack.Quartz
{
    public class GetQuartzSummaryResponse
    {
        public JobKeySummary[] JobKeys { get; set; }
        public TriggerKeySummary[] TriggerKeys { get; set; }
        public bool IsInStandbyMode { get; set; }
        public bool IsShutdown { get; set; }
        public bool IsStarted { get; set; }
        public SchedulerSummary Scheduler { get; set; }
        public string[] CalendarNames { get; set; }
        public string[] JobGroups { get; set; }
        public string[] TriggerGroups { get; set; }
        public string[] PausedTriggerGroups { get; set; }
        public JobExecutionSummary[] CurrentlyExecutingJobs { get; set; }
    }
}