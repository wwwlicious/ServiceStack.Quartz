namespace ServiceStack.Quartz
{
    using global::Quartz;

    public static class MappingExtensions
    {
        public static JobSummary Map(this IJobDetail jobDetail, TriggerSummary[] triggers)
        {
            return new JobSummary
            {
                Key = jobDetail.Key.Map(),
                Description = jobDetail.Description,
                ConcurrentExecutionDisallowed = jobDetail.ConcurrentExecutionDisallowed,
                JobType = jobDetail.JobType.FullName,
                Durable = jobDetail.Durable,
                PersistJobDataAfterExecution = jobDetail.PersistJobDataAfterExecution,
                RequestsRecovery = jobDetail.RequestsRecovery,
                JobData = jobDetail.JobDataMap.Map(),
                Triggers = triggers
            };
        }
        public static JobDataMapSummary Map(this JobDataMap jobDataMap)
        {
            return new JobDataMapSummary
            {
                Count = jobDataMap.Count,
                Dirty = jobDataMap.Dirty,
                IsEmpty = jobDataMap.IsEmpty,
                Data = jobDataMap.WrappedMap.ToJson(),
            };
        }

        public static TriggerSummary Map(this ITrigger trigger)
        {
            return new TriggerSummary
            {
                Key = trigger.Key.Map(),
                JobKey = trigger.JobKey.Map(),
                Description = trigger.Description,
                CalendarName = trigger.CalendarName,
                StartTimeUtc = trigger.StartTimeUtc,
                EndTimeUtc = trigger.EndTimeUtc,
                FinalFireTimeUtc = trigger.FinalFireTimeUtc,
                HasMillisecondPrecision = trigger.HasMillisecondPrecision,
                MisfireInstruction = trigger.MisfireInstruction,
                Priority = trigger.Priority,
                JobDataMap = trigger.JobDataMap.Map(),
                MayFireAgain = trigger.GetMayFireAgain(),
                NextFireTimeUtc = trigger.GetNextFireTimeUtc(),
                PreviousFireTimeUtc = trigger.GetPreviousFireTimeUtc()
            };
        }

        public static TriggerKeySummary Map(this TriggerKey key)
        {
            return new TriggerKeySummary
            {
                Group = key.Group,
                Name = key.Name
            };
        }

        public static JobExecutionSummary Map(this IJobExecutionContext context)
        {
            return new JobExecutionSummary
            {
                FireInstanceId = context.FireInstanceId,
                JobRunTime = context.JobRunTime,
                FireTimeUtc = context.FireTimeUtc,
                PreviousFireTimeUtc = context.PreviousFireTimeUtc,
                NextFireTimeUtc = context.NextFireTimeUtc,
                ScheduledFireTimeUtc = context.ScheduledFireTimeUtc,
                Recovering = context.Recovering,
                RecoveringTriggerKey = context.Recovering ? context.RecoveringTriggerKey.Map() : null,
                RefireCount = context.RefireCount,
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