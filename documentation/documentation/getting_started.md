<!--title: Getting Started-->

<[warning]>
Uses Quartz.Net v3.x which requires net 4.52 and above.
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

# Defaults

## Jobs

By default, the plugin will scan all assemblies for implementations of Quartz.Net's `IJob` interface and register them with [ServiceStack's IoC container](http://docs.servicestack.net/ioc).

Your `IJob` implementations will use the IoC container to resolve any constructor or property injection dependencies automatically.

## Configuration

By default, the plugin will use the standard Quartz.Net's configuration (if specified).

## Lifecycle

By default, the scheduler is started after the the `AppHost` has initialised and shutdown when the `AppHost` is disposed.

## Persistence

By default, there is no scheduler persistence for Jobs. This can be configured using the standard Quartz.Net configuration options.