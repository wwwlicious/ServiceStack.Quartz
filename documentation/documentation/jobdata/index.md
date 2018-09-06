<!--title: Job Data-->

If you need to pass data to your jobs, in Quartz.Net, this is done via the `JobDetail` class.

Here is how you can pass a string to a job.

```csharp
public class AppHost : AppHostBase
{
    public override void Configure(Container container)
    {
        var quartzFeature = new QuartzFeature();
        
        // set up some job data
        var jobData = JobBuilder.Create<MyJob>().UsingJobData("Name", "Sharon").Build();
        
        // create a trigger
        var myTrigger = TriggerBuilder.Create()
                        .WithCronSchedule("0 0 0/1 1/1 * ? *")
                        .Build();
                
        // register the trigger with the job data
        quartzFeature.RegisterJob<MyJob>(myTrigger, jobData);
        
        Plugins.Add(quartzFeature);
    }
}
```

In order to get the value within the Job, we can use the `MergedJobDataMap` on the `IJobExecutionContext` Execute method parameter.

There is also a more convenient method `Get(string keyName)` which we can use

```csharp
public class HelloJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        // get job data
        var contextMergedJobData = context.MergedJobDataMap["Name"]?.ToString();

        // or using Get(key)
        var jobDataName = context.Get("Name");

        return context.AsTaskResult();
    }
}
```

