namespace ServiceStack.Quartz
{
    using System;
    using global::Funq;
    using global::Quartz;

    public static class FunqQuartzExtensions
    {
        /// <summary>
        /// Registers a job with the quartz scheduler
        /// </summary>
        /// <param name="quartzFeature">The quartz feature</param>
        /// <param name="trigger">the job trigger</param>
        public static void RegisterJob<TJob>(this QuartzFeature quartzFeature, ITrigger trigger) where TJob : IJob
        {
            // Register job with Trigger
            quartzFeature.RegisterJob(trigger, JobBuilder.Create<TJob>().Build());
        }

        /// <summary>
        /// Registers a job with the quartz scheduler
        /// </summary>
        /// <param name="quartzFeature">The quartz feature</param>
        /// <param name="trigger">the job trigger</param>
        /// <param name="jobDetail">the job detail</param>
        public static void RegisterJob<TJob>(this QuartzFeature quartzFeature, ITrigger trigger, IJobDetail jobDetail) where TJob : IJob
        {
            // Register job with Trigger
            quartzFeature.RegisterJob(trigger, jobDetail);
        }

        /// <summary>
        /// Registers a job with the quartz scheduler
        /// </summary>
        /// <param name="quartzFeature">The quertz feature</param>
        /// <param name="triggerFunc">the job trigger delegate</param>
        public static void RegisterJob<TJob>(this QuartzFeature quartzFeature, Func<TriggerBuilder,ITrigger> triggerFunc) where TJob : IJob
        {
            // Register job with Trigger
            quartzFeature.RegisterJob(
                triggerFunc.Invoke(TriggerBuilder.Create()),
                JobBuilder.Create<TJob>().WithIdentity($"{typeof(TJob).Name}-{Guid.NewGuid()}").Build());
        }

        /// <summary>
        /// Registers a job with the quartz scheduler
        /// </summary>
        /// <param name="quartzFeature">The quartz feature</param>
        /// <param name="trigger">the job trigger</param>
        /// <param name="jobDetail">the job detail</param>
        public static void RegisterJob(this QuartzFeature quartzFeature, ITrigger trigger, IJobDetail jobDetail)
        {
            // Add the job
            quartzFeature.AddJob(jobDetail, trigger);
        }

        /// <summary>
        /// Registers a job with the quartz scheduler
        /// </summary>
        /// <typeparam name="TJob">The job type</typeparam>
        /// <param name="quartzFeature">the funq container</param>
        /// <param name="triggerFunc">the trigger delegate</param>
        /// <param name="jobDetailFunc">the jobdetail delegate</param>
        public static void RegisterJob<TJob>(this QuartzFeature quartzFeature, Func<TriggerBuilder, ITrigger> triggerFunc, Func<JobBuilder, IJobDetail> jobDetailFunc)
            where TJob : IJob
        {
            var triggers = triggerFunc.Invoke(TriggerBuilder.Create());
            var jobDetail = jobDetailFunc.Invoke(JobBuilder.Create<TJob>());
            quartzFeature.RegisterJob(triggers, jobDetail);
        }
    }
}