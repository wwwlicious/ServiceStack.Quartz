namespace ServiceStack.Quartz.NetCore.Quartz.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Quartz;
    using ServiceStack;
    using ServiceStack.Quartz.ServiceInterface;
    using ServiceStack.Quartz.ServiceModel;

    public class HelloJob : IJob
    {
        /// <summary>
        /// TODO Is null?
        /// </summary>
        public IServiceGateway Gateway { get; set; }

        /// <summary>
        /// Is injected ok
        /// </summary>
        public MyServices Services { get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            Thread.Sleep(TimeSpan.FromMinutes(1));

            // get job data
            var contextMergedJobData = context.MergedJobDataMap["Name"]?.ToString();
            var jobDataName = context.Get("Name");

            // do some work, call a service
            var response = Services.Any(new Hello
            {
                Name = contextMergedJobData ?? "there!"
            });
            context.Result = response.Result;

            return context.AsTaskResult();
        }
    }
}