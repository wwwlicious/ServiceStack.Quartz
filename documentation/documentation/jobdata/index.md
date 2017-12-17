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

In order to get the value within the Job, we can use the `MergedDataMap` on the `IExecutionContext` argument.
There is also a more convenient method `Get(string keyName)` which we can use

