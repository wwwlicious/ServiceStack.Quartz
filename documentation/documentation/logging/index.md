<!--title: Logging-->

If you require logging during your `IJob` executions, use the default [ServiceStack logging](http://docs.servicestack.net/logging)

```csharp
ILog log = LogManager.GetLogger(GetType());

log.Debug("Debug Event Log Entry.");
log.Warn("Warning Event Log Entry.");
```

```Verilog
[22:40:48 INF] Quartz scheduler 'QuartzScheduler' initialized
[22:40:48 INF] Quartz scheduler version: 3.0.0.0
[22:40:48 INF] JobFactory set to: ServiceStack.Quartz.Funq.FunqJobFactory
[22:40:48 INF] Scheduler QuartzScheduler_$_NON_CLUSTERED started.
[22:40:49 DBG] Hosting environment: Production
Content root path: C:\dev\ServiceStack.Quartz\src\demo\ServiceStackQuartz.NetCore
Now listening on: http://localhost:5000
Now listening on: https://localhost:5001
Application started. Press Ctrl+C to shut down.
Created new Job ServiceStack.Quartz.NetCore.Quartz.Jobs:HelloJob_dreamy_franklin of ServiceStack.Quartz.NetCore.Quartz.Jobs.HelloJob
[22:40:49 DBG] Batch acquisition of 1 triggers
[22:40:49 DBG] Calling Execute on job ServiceStack.Quartz.NetCore.Quartz.Jobs.HelloJob_dreamy_franklin
```

<[info]>
Quartz.Net v3.x and above uses [LibLog](https://github.com/damianh/LibLog) for it's own internal logging. This will be picked up automatically by most common logging libraries and should appear in the ServiceStack logs by default.
<[/info]>
