namespace ServiceStack.Quartz
{
    using System;

    public class JobExecutionHistory
    {
        public string Id { get; set; }
        public JobKeySummary JobKey { get; set; }
        public TimeSpan RunTime { get; set; }
        public JobDataMapSummary Data { get; set; }
        public JobExecutionStatus Status { get; set; }
        public Exception Exception { get; set; }
    }

    public enum JobExecutionStatus
    {
        None,
        Vetoed,
        Failed,
        Success
    }
}