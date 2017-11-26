namespace ServiceStack.Quartz
{
    public class JobSummary
    {
        public JobKeySummary Key { get; set; }
        public bool ConcurrentExecutionDisallowed { get; set; }
        public string Description { get; set; }
        public bool Durable { get; set; }
        public string JobType { get; set; }
        public bool PersistJobDataAfterExecution { get; set; }
        public bool RequestsRecovery { get; set; }
        public JobDataMapSummary JobData { get; set; }
    }
}