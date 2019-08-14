<!--title: Job Tiggers-->

Job aren't much use without triggers. These tell Quartz when to run the jobs.
You configure triggers in ServiceStack's `AppHost` configuration.

```csharp
// first let's create the plugin and add it to our apphost
var quartzFeature = new QuartzFeature();
Plugins.Add(quartzFeature);

// now let's create a trigger to run HelloJob every minute
quartzFeature.RegisterJob<HelloJob>(
    trigger =>
        trigger
        .WithSimpleSchedule(s => s.WithInterval(TimeSpan.FromMinutes(1)).RepeatForever())
        .Build()
);
```

You can create triggers separately if you want to assign them to mulitple jobs too.

```csharp
// Here is a cron schedule trigger which executes
// at second :00, at minute :00, every hour starting at 00am, 
// every day starting on the 1st, every month
var cronTrigger = TriggerBuilder.Create()
    .WithCronSchedule("0 0 0/1 1/1 * ? *")
    .Build();
quartzFeature.RegisterJob<HelloJob>(cronTrigger);
quartzFeature.RegisterJob<AnotherJob>(cronTrigger);
```

You can customise how the triggers are registered in the scheduler too.
Here we are setting the identity and description for our trigger

```csharp
// you can setup jobs with data and triggers however you like
// this lets create a trigger with our preferred identity
var everyHourTrigger = TriggerBuilder.Create()
    .WithDescription("This is my week day trigger, on the hour, every hour")
    .WithIdentity("Every Hour", "Week days")
    .WithDailyTimeIntervalSchedule(x => x.OnMondayThroughFriday().WithIntervalInHours(1))
    .Build();
```
