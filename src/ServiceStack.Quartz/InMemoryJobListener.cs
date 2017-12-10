namespace ServiceStack.Quartz
{
    using System.Threading;
    using System.Threading.Tasks;
    using global::Quartz;

    public class InMemoryJobListener : IJobListener
    {
        public static string ListenerName = "InMemoryjobListener";
        public CircularBuffer<JobExecutionHistory> History { get; }
        public string Name { get; }

        public InMemoryJobListener()
        {
            Name = InMemoryJobListener.ListenerName;
            History = new CircularBuffer<JobExecutionHistory>(1000);
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            // Insert history record and retrieve primary key of inserted record.
            context.Put("HistoryIdKey", context.FireInstanceId);
            return context.AsTaskResult();
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            History.PushFront(new JobExecutionHistory
            {
                Id = context.FireInstanceId,
                JobKey = context.JobDetail.Key.Map(),
                RunTime = context.JobRunTime,
                Data = context.MergedJobDataMap.Map(),
                Status = JobExecutionStatus.Vetoed
            });
            return context.AsTaskResult();
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Retrieve history id from context and update history record.
            History.PushFront(new JobExecutionHistory
            {
                Id = context.FireInstanceId,
                JobKey = context.JobDetail.Key.Map(),
                RunTime = context.JobRunTime,
                Data = context.MergedJobDataMap.Map(),
                Exception = jobException.UnwrapIfSingleException(),
                Status = jobException == null ? JobExecutionStatus.Success : JobExecutionStatus.Failed
            });
            
            return context.AsTaskResult();
        }
    }
}