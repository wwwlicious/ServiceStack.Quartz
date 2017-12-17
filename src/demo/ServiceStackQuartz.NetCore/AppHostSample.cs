// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/. 
namespace ServiceStack.Quartz.NetCore
{
    using System;
    using System.Threading.Tasks;
    using global::Funq;
    using global::Quartz;
    using ServiceStack.Quartz.ServiceInterface;

    // SAMPLE: GettingStarted-AppHost
    public class AppHostSample : AppHostBase
    {
        public AppHostSample() : base("Quartz Sample", typeof(MyServices).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            var quartzFeature = new QuartzFeature();

            // create a simple job trigger to repeat every minute 
            quartzFeature.RegisterJob<MyJob>(
                trigger =>
                    trigger.WithSimpleSchedule(s =>
                            s.WithInterval(TimeSpan.FromMinutes(1))
                                .RepeatForever()
                        )
                        .Build()
            );

            // register the plugin
            Plugins.Add(quartzFeature);
        }
    }
    // ENDSAMPLE
    
    // SAMPLE: GettingStarted-BasicJob
    public class MyJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            // ... job code goes here
            return context.AsTaskResult();
        }
    }
    // ENDSAMPLE
}