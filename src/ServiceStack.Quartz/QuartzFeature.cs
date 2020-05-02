// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/. 
namespace ServiceStack.Quartz
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Reflection;
    using global::Quartz;
    using ServiceStack.Quartz.Funq;

    /// <summary>
    /// Sets up a quartz.net scheduler and scans assemblies to register IJob implementations by default
    /// Quartz.net standard app config is automatically used
    /// </summary>
    public class QuartzFeature : IPlugin, IPreInitPlugin
    {
        private Dictionary<string, string> _config = new Dictionary<string, string>();
        private string _apiRestPath = "quartz/api";
        internal Dictionary<JobKey, JobInstance> Jobs { get; } = new Dictionary<JobKey, JobInstance>();
        
        /// <summary>
        /// Set quartz specific config overriding defaults
        /// By default, all keys starting with "quartz." are read from configuration
        /// For details of quartz config keys, see 
        /// </summary>
        public NameValueCollection Config
        {
            get => _config.ToNameValueCollection();
            set => _config = value.ToDictionary();
        }

        /// <summary>
        /// The assemblies to scan for IJob implementations
        /// </summary>
        public Assembly[] JobAssemblies { get; set; }
        
        /// <summary>
        /// Add addition trigger listeners to the scheduler
        /// </summary>
        public List<ITriggerListener> TriggerListeners { get; } = new List<ITriggerListener>();
        
        /// <summary>
        /// Add addition jobs listeners to the scheduler. InMemoryJobListener added by default 
        /// </summary>
        public List<IJobListener> JobListeners { get; } = new List<IJobListener> {new InMemoryJobListener()}; 

        /// <summary>
        /// checks all assemblies for exported public IJob concrete types
        /// True by default
        /// </summary>
        public bool ScanAppHostAssemblies { get; set; } = true;
        
        /// <summary>
        /// Limit access to /quartz service to these roles
        /// </summary>
        public string[] RequiredRoles { get; set; }

        /// <summary>
        /// Executed before any plugin is registered
        /// </summary>
        /// <param name="appHost"></param>
        public void BeforePluginsLoaded(IAppHost appHost)
        {
        }

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
            // Wire up common logging from Quartz -> ServiceStack
            // Register service for getting info from scheduler
            var serviceType = typeof(QuartzService);
            if (!RequiredRoles.IsEmpty())
            {
                if (!appHost.HasPlugin<AuthFeature>())
                    throw new System.Configuration.ConfigurationErrorsException(
                        "RequiredRoles has been specified but AuthFeature plugin has been registered to handle authorization"); 

                serviceType.AddAttributes(new RequiresAnyRoleAttribute(RequiredRoles));
            }

            appHost.RegisterService(serviceType, _apiRestPath);
            
            // add endpoint link to metadata page
            ConfigurePluginLinks(appHost);
            
            appHost.AfterInitCallbacks.Add(AfterInit);

            // add dispose delegate to shutdown scheduler
            appHost.OnDisposeCallbacks.Add(ShutdownQuartzScheduler);
        }

        private void ConfigurePluginLinks(IAppHost appHost)
        {
            appHost.GetPlugin<MetadataFeature>().AddPluginLink(_apiRestPath, "Quartz Api");
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
        /// The scheduler is started after all the plugins have been loaded
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
            
            // register additional listeners
            JobListeners.Each(x => scheduler.ListenerManager.AddJobListener(x));
            TriggerListeners.Each(x => scheduler.ListenerManager.AddTriggerListener(x));
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