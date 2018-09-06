<!--title: Job Registration-->

## Automatic job registration

There are two options for registering Jobs.

By default, the plugin will scan all the application's assemblies for implementations of the `IJob` interface and register them with ServiceStack's IoC container.

```csharp
public class AppHost : AppHostBase
{
    public override void Configure(Container container)
    {
        Plugins.Add(new QuartzFeature());
    }
}
```

## Manual job registration

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

Registration will take care of creating instances of your Jobs, including injecting any dependencies into them, but without triggers, your jobs will never run.

Lets see how we schedule our jobs to run next...