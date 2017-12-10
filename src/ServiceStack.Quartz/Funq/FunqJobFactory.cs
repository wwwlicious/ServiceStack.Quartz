namespace ServiceStack.Quartz.Funq
{
    using System;
    using global::Funq;
    using global::Quartz;
    using global::Quartz.Spi;
    using ServiceStack.Logging;

    /// <summary>
    /// Resolve Quartz Job and it's dependencies from Funq container
    /// Source and credit: https://github.com/CodeRevver/ServiceStackWithQuartz
    /// </summary>
    internal class FunqJobFactory : IJobFactory
    {
        private readonly Container _container;

        /// <summary>
        /// Initialises a new instance of the FunqJobFactory
        /// </summary>
        /// <param name="container"></param>
        public FunqJobFactory(Container container)
        {
            _container = container;
        }

        /// <summary>
        /// Called by the Quartz Scheduler at the time of the trigger firing
        /// in order to produce a IJob instance on which to call execute
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            bundle.ThrowIfNull(nameof(bundle));
            scheduler.ThrowIfNull(nameof(scheduler));

            var jobDetail = bundle.JobDetail;
            var newJob = (IJob)_container.TryResolve(jobDetail.JobType);

            if (newJob == null)
                throw new SchedulerConfigException(
                    $"Failed to instantiate Job {jobDetail.Key} of type {jobDetail.JobType}");

            LogManager.GetLogger(typeof(FunqJobFactory)).DebugFormat("Created new Job {JobGroup}:{JobName} of {JobType}", jobDetail.Key.Group, jobDetail.Key.Name, jobDetail.JobType);
            return newJob;
        }

        /// <summary>
        /// Allows the job factory to destroy/cleanup the job if needed
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            if (job is IDisposable)
            {
                var disposableJob = (IDisposable)job;
                disposableJob.Dispose();
                LogManager.GetLogger(typeof(FunqJobFactory)).DebugFormat("Disposed Job {JobType}", job.GetType());
            }
        }
    }
}
