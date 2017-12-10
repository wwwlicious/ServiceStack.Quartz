# Registration

* [Jobs](#jobs)
* [Job Data](#job-data)
* [Triggers](#triggers)

## Jobs

There are two options for registering Jobs.

By default, the plugin will scan all the application's assemblies for implementations of the `IJob` interface and register them with Quartz.Net.

```csharp
public class AppHost : AppHostBase
{
    public override void Configure(Container container)
    {
        Plugins.Add(new QuartzFeature());
    }
}
```

### Explicitly registering Job Assesmblies

If you need to explicitly register your assemblies containing your `IJob` implementations, you can 
specify the assemblies.

**NOTE: You must set the plugin property `ScanAppHostAssemblies` to false** 


```csharp
public class AppHost : AppHostBase
{
    public override void Configure(Container container)
    {
        Plugins.Add(new QuartzFeature
        {
            ScanAppHostAssemblies = false,
            JobAssemblies = new[] { typeof(MyJob).Assembly, typeof(AnotherJob).Assembly }
        });
    }
}
```

## Job Data

If you need to pass data to jobs, in Quartz.Net, this is done via the `JobDetail` class.

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

## Triggers

