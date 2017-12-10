namespace ServiceStack.Quartz
{
    using System;
    using System.Linq;
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
            quartzFeature.RegisterJob(trigger,
                JobBuilder.Create<TJob>()
                    .WithIdentity(quartzFeature.GetJobIdentity<TJob>()).Build());
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
                JobBuilder.Create<TJob>().WithIdentity(quartzFeature.GetJobIdentity<TJob>()).Build());
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
            var triggers = triggerFunc.Invoke(TriggerBuilder.Create().WithIdentity(quartzFeature.GetTriggerIdentity<TJob>()));
            var jobDetail = jobDetailFunc.Invoke(JobBuilder.Create<TJob>().WithIdentity(quartzFeature.GetJobIdentity<TJob>()));
            quartzFeature.RegisterJob(triggers, jobDetail);
        }

        /// <summary>
        /// /// Generate a random key for the job avoiding collisions with existing keys
        /// </summary>
        /// <param name="quartzFeature">the quartz plugin</param>
        /// <typeparam name="T">the job type</typeparam>
        /// <returns>the job key</returns>
        private static JobKey GetJobIdentity<T>(this QuartzFeature quartzFeature)
        {
            var groupName = $"{typeof(T).Namespace}";
            var jobName = $"{typeof(T).Name}";
            var jobIdentity = JobKey.Create($"{jobName}_{NamesGenerator.GetRandomName()}", groupName);
            while (quartzFeature.Jobs.ContainsKey(jobIdentity))
            {
                jobIdentity = JobKey.Create($"{jobName}_{NamesGenerator.GetRandomName()}", groupName);
            }
            return jobIdentity;
        }
        
        /// <summary>
        /// Generate a random key for the trigger avoiding collisions with existing keys
        /// </summary>
        /// <param name="quartzFeature">the quartz plugin</param>
        /// <typeparam name="T">the job type</typeparam>
        /// <returns>the trigger key</returns>
        private static TriggerKey GetTriggerIdentity<T>(this QuartzFeature quartzFeature)
        {
            var groupName = $"{typeof(T).Namespace}";
            var jobName = $"{typeof(T).Name}";
            var triggerIdentity = new TriggerKey($"{jobName}_{NamesGenerator.GetRandomName()}", groupName);
            var triggers = quartzFeature.Jobs.Values.SelectMany(x => x.Triggers.Select(k => k.Key)).ToArray();
            while (triggers.Any(k => k.Equals(triggerIdentity)))
            {
                triggerIdentity = new TriggerKey($"{jobName}_{NamesGenerator.GetRandomName()}", groupName);
            }
            return triggerIdentity;
        }
    }
}