using System.Collections.Specialized;
using Funq;
using Quartz;
using ServiceStack;
using ServiceStack.Funq.Quartz;
using ServiceStackWithQuartz.ServiceInterface;

namespace ServiceStackWithQuartz
{
    public class AppHost : AppSelfHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("ServiceStackWithQuartz", typeof(MyServices).Assembly)
        {

        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            var quartzConfig = ConfigureQuartz();
            container.RegisterQuartzScheduler(typeof(HelloJob), quartzConfig);
            var scheduler = container.Resolve<IScheduler>();
            scheduler.Start();

            /* Schedule HelloJob */
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(10)
                  .RepeatForever())
              .Build();

            scheduler.ScheduleJob(job, trigger);

            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
        }

        /// <summary>
        /// Provides Quartz Configuration Settings
        /// </summary>
        /// <returns></returns>
        private NameValueCollection ConfigureQuartz()
        {
            /* set thread pool info */
            var properties = new NameValueCollection();
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            return properties;
        }
    }
}
