namespace ServiceStack.Quartz
{
    using System.Linq;
    using System.Threading;
    using global::Quartz;
    using global::Quartz.Impl.Matchers;
    using global::Quartz.Util;

    public class QuartzService : Service
    {
        public IScheduler Scheduler { get; set; }

        public object Get(GetQuartzSummary request)
        {
            return new GetQuartzSummaryResponse
            {
                Scheduler = Scheduler.GetMetaData().Result.Map(),
                IsInStandbyMode = Scheduler.InStandbyMode,
                IsShutdown = Scheduler.IsShutdown,
                IsStarted = Scheduler.IsStarted,
                CalendarNames = Scheduler.GetCalendarNames().Result.ToArray(),
                CurrentlyExecutingJobs = Scheduler.GetCurrentlyExecutingJobs().Result.Select(x => x.Map()).ToArray(),
                JobGroups = Scheduler.GetJobGroupNames().Result.ToArray(),
                JobKeys = Scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result.Select(k => k.Map()).ToArray()
            };
        }

        public object Get(GetQuartzJob request)
        {
            var jobDetail = Scheduler.GetJobDetail(JobKey.Create(request.JobName)).Result;
            return new GetQuartzJobResponse
            {
                Job = new JobSummary
                {
                    Key = jobDetail.Key.Map(),
                    ConcurrentExecutionDisallowed = jobDetail.ConcurrentExecutionDisallowed,
                    Description = jobDetail.Description,
                    Durable = jobDetail.Durable,
                    JobType = jobDetail.JobType.AssemblyQualifiedName,
                    PersistJobDataAfterExecution = jobDetail.PersistJobDataAfterExecution,
                    RequestsRecovery = jobDetail.RequestsRecovery,
                    JobData = jobDetail.JobDataMap.Map(),
                }
            };
        }

        public object Get(GetQuartzJobs request)
        {
            var groupMatcher = GroupMatcher<JobKey>.AnyGroup();

            if (!request.Group.IsNullOrWhiteSpace())
            {
                groupMatcher = GroupMatcher<JobKey>.GroupEquals(request.Group);
            }

            var jobKeys = Scheduler.GetJobKeys(groupMatcher).Result.Select(x => x.Map()).ToArray();

            return new GetQuartzJobsResponse
            {
                JobKeys = jobKeys
            };
        }

        public object Post(PostQuartzStatus request)
        {
            if (request.Standby)
                Scheduler.Standby(CancellationToken.None);
            
            if (request.Shutdown)
                Scheduler.Shutdown(CancellationToken.None);
            
            if (!request.PauseGroups.IsEmpty())
                request.PauseGroups.Each(x => Scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(x)).Wait());
            
            if (!request.ResumeGroups.IsEmpty())
                request.PauseGroups.Each(x => Scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals(x)).Wait());
            
            if (request.PauseAll)
                Scheduler.PauseAll();
            
            if (request.ResumeAll)
                Scheduler.ResumeAll();
            
            if (!request.PauseJobs.IsEmpty())
                request.PauseJobs.Each(x => Scheduler.PauseJob(JobKey.Create(x), CancellationToken.None));
            
            if (!request.ResumeJobs.IsEmpty())
                request.ResumeJobs.Each(x => Scheduler.ResumeJob(JobKey.Create(x), CancellationToken.None));
            
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