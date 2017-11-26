namespace ServiceStack.Quartz
{
    using global::Quartz;

    public static class MappingExtensions
    {
        public static JobDataMapSummary Map(this JobDataMap jobDataMap)
        {
            return new JobDataMapSummary
            {
                Count = jobDataMap.Count,
                Dirty = jobDataMap.Dirty,
                IsEmpty = jobDataMap.IsEmpty,
                Keys = jobDataMap.Keys.ToArray(),
                Values = jobDataMap.Values.ToArray(),
                Data = jobDataMap.WrappedMap.ToJson(),
            };
        }

        public static TriggerSummary Map(this ITrigger trigger)
        {
            return new TriggerSummary
            {
                CalendarName = trigger.CalendarName,
                Description = trigger.Description,
                EndTimeUtc = trigger.EndTimeUtc,
                FinalFireTimeUtc = trigger.FinalFireTimeUtc,
                HasMillisecondPrecision = trigger.HasMillisecondPrecision,
                JobDataMap = trigger.JobDataMap.Map()
            };
        }

        public static JobExecutionSummary Map(this IJobExecutionContext context)
        {
            return new JobExecutionSummary
            {
                FireInstanceId = context.FireInstanceId,
                FireTimeUtc = context.FireTimeUtc,
                JobRunTime = context.JobRunTime,
                NextFireTimeUtc = context.NextFireTimeUtc,
                PreviousFireTimeUtc = context.PreviousFireTimeUtc,
                Recovering = context.Recovering,
                RecoveringTriggerKey = $"{context.RecoveringTriggerKey.Group}:{context.RecoveringTriggerKey.Name}",
                RefireCount = context.RefireCount,
                ScheduledFireTimeUtc = context.ScheduledFireTimeUtc,
                Trigger = context.Trigger.Map(),
            };
        }

        public static SchedulerSummary Map(this SchedulerMetaData x)
        {
            return new SchedulerSummary
            {
                InStandbyMode = x.InStandbyMode,
                JobStoreClustered = x.JobStoreClustered,
                JobStoreSupportsPersistence= x.JobStoreSupportsPersistence,
                JobStoreType = x.JobStoreType.FullName,
                NumberOfJobsExecuted = x.NumberOfJobsExecuted,
                RunningSince = x.RunningSince,
                SchedulerInstanceId = x.SchedulerInstanceId,
                SchedulerName = x.SchedulerName,
                SchedulerRemote = x.SchedulerRemote,
                SchedulerType = x.SchedulerType.FullName,
                Shutdown = x.Shutdown,
                Started = x.Started,
                ThreadPoolSize = x.ThreadPoolSize,
                ThreadPoolType = x.ThreadPoolType.FullName,
                Version = x.Version,
                Summary = x.GetSummary()
            };
        }

        public static JobKeySummary Map(this JobKey key)
        {
            return new JobKeySummary
            {
                Group = key.Group,
                Name = key.Name
            };
        }
    }
}