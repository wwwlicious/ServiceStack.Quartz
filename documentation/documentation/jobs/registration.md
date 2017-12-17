<!--title: Job Registration-->

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


## Triggers

TODO 