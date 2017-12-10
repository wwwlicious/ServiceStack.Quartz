using System;

namespace ServiceStack.Quartz
{
    public class TriggerSummary
    {
        public TriggerKeySummary Key { get; set; }
        public JobKeySummary JobKey { get; set; }
        public string Description { get; set; }
        public string CalendarName { get; set; }
        public DateTimeOffset StartTimeUtc { get; set; }
        public DateTimeOffset? EndTimeUtc { get; set; }
        public bool MayFireAgain { get; set; }
        public DateTimeOffset? NextFireTimeUtc { get; set; }
        public DateTimeOffset? PreviousFireTimeUtc { get; set; }
        public DateTimeOffset? FinalFireTimeUtc { get; set; }
        public bool HasMillisecondPrecision { get; set; }
        public int MisfireInstruction { get; set; }
        public int Priority { get; set; }
        public JobDataMapSummary JobDataMap { get; set; }
    }
}