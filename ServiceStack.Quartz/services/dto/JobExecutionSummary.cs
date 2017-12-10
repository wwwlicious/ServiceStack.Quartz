namespace ServiceStack.Quartz
{
    using System;

    public class JobExecutionSummary
    {
        public string FireInstanceId { get; set; }
        public TimeSpan JobRunTime { get; set; }
        public DateTimeOffset FireTimeUtc { get; set; }
        public DateTimeOffset? PreviousFireTimeUtc { get; set; }
        public DateTimeOffset? NextFireTimeUtc { get; set; }
        public DateTimeOffset? ScheduledFireTimeUtc { get; set; }
        public bool Recovering { get; set; }
        public TriggerKeySummary RecoveringTriggerKey { get; set; }
        public int RefireCount { get; set; }
        public TriggerSummary Trigger { get; set; }
    }
}