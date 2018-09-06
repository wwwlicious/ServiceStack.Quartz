<!--title: Jobs-->

In Quarz, 'Jobs' are executed by triggers.

In order to create a Job, create a class that inherits from the `IJob` interface located in the Quartz assembly.

<[sample:GettingStarted-BasicJob]>

As you can see, the `IJob` interface requires that we implement the Execute method.
It requires a task is returned and we return our context as a task result here.

ServiceStack's IoC container will inject any dependencies into your jobs too.

```csharp
public class HelloJob : IJob
{
    /// <summary>
    /// Dependencies are automatically injected
    /// </summary>
    public MyService MyService { get; set; }

    public Task Execute(IJobExecutionContext context)
    {
        // call our injected service
        var response = MyService.Any(new Hello { Name = "Bob" });
        context.Result = response.Result;

        return context.AsTaskResult();
    }
}
``` 