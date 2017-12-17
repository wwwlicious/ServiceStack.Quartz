<!--title: Getting Started-->

<[warning]>
Uses Quartz.Net v3.x which requires net 4.61 and above.
<[/warning]>

Install the [nuget package](https://www.nuget.org/packages/ServiceStack.Quartz/) into your ServiceStack `AppHost` project
 
```powershell

install-package ServiceStack.Quartz

```
Create a basic job
<[sample:GettingStarted-BasicJob]>

Then inside your AppHost, register the plugin and a job trigger.

<[sample:GettingStarted-AppHost]>

That's it. The job will now execute every minute while the apphost is running.
