// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/. 
namespace ServiceStack.Quartz
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Reflection;
    using global::Quartz;
    using ServiceStack.Quartz.Funq;
    using ServiceStack.Text;

    /// <summary>
    /// Sets up a quartz.net scheduler and scans assemblies to register IJob implementations by default
    /// Quartz.net standard app config is automatically used
    /// </summary>
    public class QuartzFeature : IPlugin
    {
        private Dictionary<string, string> _config;
        internal Dictionary<JobKey, JobInstance> Jobs { get; private set; }

        public QuartzFeature()
        {
            _config = new Dictionary<string, string>();
            Jobs = new Dictionary<JobKey, JobInstance>();
        }

        /// <summary>
        /// Set quartz specific config overriding defaults
        /// By default, all keys starting with "quartz." are read from configuration
        /// For details of quartz config keys, see 
        /// </summary>
        public NameValueCollection Config
        {
            get => _config.ToNameValueCollection();
            set => _config = value?.ToStringDictionary();
        }

        /// <summary>
        /// The assemblies to scan for IJob implementations
        /// </summary>
        public Assembly[] JobAssemblies { get; set; }

        /// <summary>
        /// checks all assemblies for exported public IJob concrete types
        /// True by default
        /// </summary>
        public bool ScanAppHostAssemblies { get; set; } = true;

        /// <summary>
        /// Registers the plugin
        /// </summary>
        /// <param name="appHost"></param>
        public void Register(IAppHost appHost)
        {
            // read default config
            // overwrite any config set explicitly in property
            var appConfig = appHost.AppSettings.GetAllKeysStartingWith("quartz.");
            foreach (var kv in appConfig)
            {
                if (_config[kv.Key] == null)
                    _config.Add(kv.Key, kv.Value);
            }

            // TODO list
            // Wire up common logging from Quartz -> SericeStack
            // Register service for getting info from scheduler
            var atRestPath = "/quartz";
            appHost.RegisterService(typeof(QuartzService), atRestPath);
            
            appHost.AfterInitCallbacks.Add(AfterInit);

            // add endpoint link to metadata page
            appHost.GetPlugin<MetadataFeature>()
                .AddPluginLink(atRestPath.TrimStart('/'), "Quartz");

            // add embedded pages for quartz dashboard
            appHost.OnDisposeCallbacks.Add(ShutdownQuartzScheduler);
        }

        /// <summary>
        /// Handles shutting down the scheduler
        /// </summary>
        /// <param name="appHost"></param>
        private void ShutdownQuartzScheduler(IAppHost appHost)
        {
            var scheduler = appHost.GetContainer().Resolve<IScheduler>();
            if(!scheduler.IsShutdown)
                scheduler.Shutdown();
        }

        /// <summary>
        /// The scheduler is started after all the plugsin have been loaded
        /// </summary>
        /// <param name="appHost"></param>
        public void AfterInit(IAppHost appHost)
        {
            // start the scheduler
            var container = appHost.GetContainer();

            // by default, scan all assemblies for jobs, otherwise use property
            if (ScanAppHostAssemblies)
                container.RegisterQuartzScheduler(Config);
            else
                container.RegisterQuartzScheduler(Config, JobAssemblies);

            var scheduler = appHost.GetContainer().Resolve<IScheduler>();
            foreach (var jobInstance in Jobs)
            {
                scheduler.ScheduleJob(jobInstance.Value.JobDetail, new ReadOnlyCollection<ITrigger>(jobInstance.Value.Triggers), true);
            }

            scheduler.Start();
        }

        /// <summary>
        /// Adds a job to run
        /// Note that jobs/triggers will not fire until the apphost has finished initialising
        /// </summary>
        /// <param name="jobDetail">the job detail</param>
        /// <param name="triggers">the job triggers</param>
        public void AddJob(IJobDetail jobDetail, params ITrigger[] triggers)
        {
            Jobs.Add(jobDetail.Key, new JobInstance { JobDetail = jobDetail, Triggers = triggers });
        }
    }
}