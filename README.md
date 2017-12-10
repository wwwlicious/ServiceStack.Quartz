# ServiceStack.Quartz  [![Build status](https://ci.appveyor.com/api/projects/status/sfp7sg7gh6x1x580/branch/master?svg=true)](https://ci.appveyor.com/project/CodeRevver/servicestackwithquartz/branch/master)

ServiceStack.Quartz is a plugin for [ServiceStack](https://servicestack.net/) that provides simple integration with [Quartz.Net](https://www.quartz-scheduler.net/).

## Compatability

|Name|Min Version|
|---------|-----------|
|.Net Framework|4.5.2|
|.Net Standard |2.0 |
|ServiceStack|4.5.14|

## Dependencies

* Quartz.Net v3.x
* ServiceStack v5.x

## Quick Start

Install the [nuget package](https://www.nuget.org/packages/ServiceStack.Quartz/) into your ServiceStack AppHost project
 
```powershell
install-package ServiceStack.Quartz
```

Then inside your AppHost, register the plugin and a job trigger.

```csharp
// the plugin setup
public class AppHost : AppHostBase
{
    public void Configure(Container container)
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
        
        Plugins.Add(quartzFeature);
    }
}

// a basic job
public class MyJob : IJob 
{
    public Task Execute(IJobExecutionContext context)
    {
        // job code goes here
        
        return context.AsTaskResult();
    }
}
```

## Defaults

* Jobs - By default, the plugin will scan all assemblies for implementations of Quartz.Net's `IJob` interface.
* Configuration - By default, the plugin will read the standard Quartz.Net's configuration (if specified). 
* Lifecycle - By default, the scheduler is started after the the `AppHost` has initialised and shutdown when the `AppHost` is disposed. 

## Api

The plugin exposes an API where you can view and control the scheduler, jobs and triggers.

If you have the metadata plugin enabled, a 'Quartz' link will be available on the default metadata page.

* Scheduler API - *http://{hostaddress}:{port}/quartz*

## Further documentation

* [Configuration](docs/configuration.md)
* [Registration](docs/registration.md)
* [Job Data](docs/jobdata.md)
* [Job Dependencies](docs/dependencies.md)

# Credits 

* [ServiceStackWithQuartz](https://github.com/CodeRevver/ServiceStackWithQuartz)
* [Random names](https://gist.github.com/jesusgoku/7dda3c291229e1280b18)