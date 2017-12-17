<!--title: Configuration-->

## From AppSettings 

By default, the plugin will use any configuration keys starting with `quartz.` available from ServiceStack's `IAppSettings`.

The following example will configure Quartz.Net's threadpool settings

```xml
<appsettings>
    <add key="quartz.threadPool.type" value="Quartz.Simpl.SimpleThreadPool, Quartz" />
    <add key="quartz.threadPool.threadCount" value="5" />
</appsettings>
```
To learn more about the available Quartz configuration settings see the [Quartz.Net configuration docs](https://www.quartz-scheduler.net/documentation/quartz-2.x/quick-start.html#configuration)

## From code

Configuration can also be passed directly to the plugin. 
This will **override** any duplicate settings from the application configuration above.

```csharp
var quartzConfig = new NameValueCollection();
properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
properties["quartz.threadPool.threadCount"] = "5";
    
var quartzFeature = new QuartzFeature { Config = quartzConfig };
```
