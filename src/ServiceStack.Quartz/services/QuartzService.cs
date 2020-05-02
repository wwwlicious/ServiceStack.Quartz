namespace ServiceStack.Quartz
{
    using System.Linq;
    using System.Threading;
    using global::Quartz;
    using global::Quartz.Impl.Matchers;
    using global::Quartz.Util;
    using ServiceStack.Templates;

    [Restrict(VisibilityTo = RequestAttributes.None)]
    public class QuartzService : Service
    {
        public IScheduler Scheduler { get; set; }
        
        public object Get(GetQuartzSummary request)
        {
            return new GetQuartzSummaryResponse
            {
                JobKeys = Scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result.Select(k => k.Map()).ToArray(),
                IsInStandbyMode = Scheduler.InStandbyMode,
                IsShutdown = Scheduler.IsShutdown,
                IsStarted = Scheduler.IsStarted,
                Scheduler = Scheduler.GetMetaData().Result.Map(),
                CalendarNames = Scheduler.GetCalendarNames().Result.ToArray(),
                JobGroups = Scheduler.GetJobGroupNames().Result.ToArray(),
                TriggerGroups = Scheduler.GetTriggerGroupNames().Result.ToArray(),
                TriggerKeys = Scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup()).Result.Select(x => x.Map()).ToArray(),
                PausedTriggerGroups = Scheduler.GetPausedTriggerGroups().Result.ToArray(),
                CurrentlyExecutingJobs = Scheduler.GetCurrentlyExecutingJobs().Result.Select(x => x.Map()).ToArray()
            };
        }

        public object Get(GetQuartzJob request)
        {
            var jobKey = JobKey.Create(request.JobName, request.GroupName);
            var jobDetail = Scheduler.GetJobDetail(jobKey).Result;
            return new GetQuartzJobResponse
            {
                Job = jobDetail.Map(Scheduler.GetTriggersOfJob(jobKey).Result.Select(x => x.Map()).ToArray())
            };
        }

        public object Get(GetQuartzTrigger request)
        {
            var trigger = Scheduler.GetTrigger(new TriggerKey(request.TriggerName, request.GroupName)).Result;
            return new GetQuartzTriggerResponse
            {
                Trigger = trigger.Map()
            };
        }

        public object Get(GetQuartzJobs request)
        {
            var groupMatcher = GroupMatcher<JobKey>.AnyGroup();

            if (!request.Group.IsNullOrWhiteSpace())
            {
                groupMatcher = GroupMatcher<JobKey>.GroupEquals(request.Group);
            }

            var jobs = Scheduler.GetJobKeys(groupMatcher).Result.Select(x =>
                    Scheduler.GetJobDetail(x).Result
                        .Map(Scheduler.GetTriggersOfJob(x).Result.Select(t => t.Map()).ToArray()))
                .ToArray();

            return new GetQuartzJobsResponse
            {
                Jobs = jobs
            };
        }

        public object Get(GetQuartzTriggers request)
        {
            var groupMatcher = GroupMatcher<TriggerKey>.AnyGroup();

            if (!request.Group.IsNullOrWhiteSpace())
            {
                groupMatcher = GroupMatcher<TriggerKey>.GroupEquals(request.Group);
            }

            var keys = Scheduler.GetTriggerKeys(groupMatcher).Result.Select(x => x.Map()).ToArray();

            return new GetQuartzTriggersResponse
            {
                Triggers = keys
            };
        }
        
        public object Get(GetQuartzHistory request)
        {
            var listener = Scheduler.ListenerManager.GetJobListener(InMemoryJobListener.ListenerName) as InMemoryJobListener;
            
            return new GetQuartzHistoryResponse
            {
                History = listener?.History.ToArray() ?? new JobExecutionHistory[0]
            };
        }

        public object Post(PostQuartzStatus request)
        {
            if (request.Standby)
                Scheduler.Standby(CancellationToken.None);

            if (request.Shutdown)
                Scheduler.Shutdown(CancellationToken.None);

            if (!request.PauseJobGroups.IsEmpty())
                request.PauseJobGroups.Each(x => Scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(x)).Wait());

            if (!request.ResumeJobGroups.IsEmpty())
                request.PauseJobGroups.Each(x => Scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals(x)).Wait());

            if (!request.PauseTriggerGroups.IsEmpty())
                request.PauseTriggerGroups.Each(x => Scheduler.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(x)).Wait());

            if (!request.ResumeTriggerGroups.IsEmpty())
                request.PauseTriggerGroups.Each(x => Scheduler.ResumeTriggers(GroupMatcher<TriggerKey>.GroupEquals(x)).Wait());

            if (request.PauseAll)
                Scheduler.PauseAll();

            if (request.ResumeAll)
                Scheduler.ResumeAll();

            if (!request.PauseJobs.IsEmpty())
                request.PauseJobs.Each(x => Scheduler.PauseJob(JobKey.Create(x), CancellationToken.None));

            if (!request.ResumeJobs.IsEmpty())
                request.ResumeJobs.Each(x => Scheduler.ResumeJob(JobKey.Create(x), CancellationToken.None));

            if (!request.PauseTriggers.IsEmpty())
                request.PauseTriggers.Each(x => Scheduler.PauseTrigger(new TriggerKey(x), CancellationToken.None));

            if (!request.ResumeTriggers.IsEmpty())
                request.ResumeTriggers.Each(x => Scheduler.ResumeTrigger(new TriggerKey(x), CancellationToken.None));

            return new PostQuartzStatusResponse
            {
                InStandbyMode = Scheduler.InStandbyMode,
                IsShutdown = Scheduler.IsShutdown,
                IsStarted = Scheduler.IsStarted,
                PausedGroupJobs = Scheduler.GetPausedTriggerGroups().Result.ToArray(),
            };
        }
    }
}