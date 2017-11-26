ServiceStackWithQuartz [https://github.com/CodeRevver/ServiceStackWithQuartz]

ServiceStack.Funq.Quartz allows an easy registration of all Quartz Jobs within ServiceStack's Funq container. This allows for dependency injection within any Quartz Job.

This repository goes alongside my blog post here: Creating a ServiceStack Windows Service that uses Quartz

You can install this package via Nuget with: install-package ServiceStack.Funq.Quartz

How to use

Register your jobs with the Funq container by calling the RegisterQuartzScheduler with the assembly that contains your Jobs:

    //// This method scans the assembly for the Jobs
    container.RegisterQuartzScheduler(typeof(HelloJob));

    //// Resolve the Quartz Scheduler as normal
    var scheduler = container.Resolve<IScheduler>();

    //// Start Quartz Scheduler
    scheduler.Start();