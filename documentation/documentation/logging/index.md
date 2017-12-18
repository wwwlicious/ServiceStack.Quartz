<!--title: Logging-->

If you require logging during your `IJob` executions, you can simply use the default [ServiceStack logging](http://docs.servicestack.net/logging)

```csharp
ILog log = LogManager.GetLogger(GetType());

log.Debug("Debug Event Log Entry.");
log.Warn("Warning Event Log Entry.");
```

<[info]>
Quartz.Net v3.x and above now uses [LibLog](https://github.com/damianh/LibLog) for it's own internal logging
which will be picked up automatically by most common logging libraries and should appear in the ServiceStack logs by default.
<[/info]>
