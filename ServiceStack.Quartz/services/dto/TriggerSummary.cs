namespace ServiceStack.Quartz
{
    using System;

    public class TriggerSummary
    {
        public string CalendarName { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? EndTimeUtc { get; set; }
        public DateTimeOffset? FinalFireTimeUtc { get; set; }
        public bool HasMillisecondPrecision { get; set; }
        public JobDataMapSummary JobDataMap { get; set; }
    }
}