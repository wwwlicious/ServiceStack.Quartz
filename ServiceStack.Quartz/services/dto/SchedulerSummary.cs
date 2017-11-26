namespace ServiceStack.Quartz
{
    using System;

    public class SchedulerSummary
    {
        public bool InStandbyMode { get; set; }
        public bool JobStoreClustered { get; set; }
        public bool JobStoreSupportsPersistence { get; set; }
        public string JobStoreType { get; set; }
        public int NumberOfJobsExecuted { get; set; }
        public DateTimeOffset? RunningSince { get; set; }
        public string SchedulerInstanceId { get; set; }
        public string SchedulerName { get; set; }
        public bool SchedulerRemote { get; set; }
        public string SchedulerType { get; set; }
        public bool Shutdown { get; set; }
        public bool Started { get; set; }
        public int ThreadPoolSize { get; set; }
        public string ThreadPoolType { get; set; }
        public string Version { get; set; }
        public string Summary { get; set; }
    }
}