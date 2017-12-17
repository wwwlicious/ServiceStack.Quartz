<!--title: Jobs-->

In Quarz, 'Jobs' are executed by triggers.

In order to create aa Job, create a class that inherits from the `IJob` interface located in the Quartz assembly.

<[sample:GettingStarted-BasicJob]>

As you can see, the `IJob` interface requires that we implement the Execute method.
 