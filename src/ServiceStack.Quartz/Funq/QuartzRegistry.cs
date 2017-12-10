namespace ServiceStack.Quartz.Funq
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reflection;
    using global::Funq;
    using global::Quartz;
    using global::Quartz.Impl;
    using global::Quartz.Spi;

    /// <summary>
    /// Registers Quartz Scheduler and Jobs with the ServiceStack Funq container
    /// Source and credit: https://github.com/CodeRevver/ServiceStackWithQuartz
    /// </summary>
    internal static class QuartzRegistry
    {
        /// <summary>
        /// Registers Quartz Scheduler and Jobs from specified assembly, with specified config
        /// </summary>
        /// <param name="container">the ioc container</param>
        /// <param name="quartzConfig">quartz config</param>
        internal static void RegisterQuartzScheduler(this Container container, NameValueCollection quartzConfig)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).ToArray();
            container.RegisterQuartzScheduler(quartzConfig, assemblies);
        }

        /// <summary>
        /// Registers Quartz Scheduler and Jobs from specified assembly, with specified config
        /// </summary>
        /// <param name="container">the ioc container</param>
        /// <param name="jobsAssembly">the assemblies containing the jobs</param>
        /// <param name="quartzConfig">quartz config</param>
        internal static void RegisterQuartzScheduler(this Container container, NameValueCollection quartzConfig, Assembly[] jobsAssembly)
        {
            jobsAssembly.ThrowIfNull(nameof(jobsAssembly));

            // register the job factory and jobs 
            container.RegisterAs<FunqJobFactory, IJobFactory>();
            var jobs = jobsAssembly.SelectMany(x => x.GetExportedTypes())
                .Where(type => !type.IsAbstract && typeof(IJob).IsAssignableFrom(type))
                .ToArray();

            jobs.Each(x => container.RegisterAutoWiredType(x));

            // register the default scheduler factory
            ISchedulerFactory schedFact = quartzConfig != null
                ? new StdSchedulerFactory(quartzConfig)
                : new StdSchedulerFactory();

            var scheduler = schedFact.GetScheduler().Result;
            scheduler.JobFactory = container.Resolve<IJobFactory>();
            container.Register<IScheduler>(scheduler);
        }
    }
}