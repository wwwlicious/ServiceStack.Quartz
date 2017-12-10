namespace ServiceStack.Quartz.Tests
{
    using System;
    using FluentAssertions;
    using ServiceStack;
    using ServiceStack.Quartz.ServiceInterface;
    using ServiceStack.Quartz.ServiceModel;
    using ServiceStack.Testing;
    using Xunit;

    public class UnitTests : IDisposable
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [Fact]
        public void TestMethod1()
        {
            var service = appHost.Container.Resolve<MyServices>();

            var response = (HelloResponse)service.Any(new Hello { Name = "World" });

            response.Result.Should().Be("Hello, World!");
        }

        public void Dispose()
        {
            appHost?.Dispose();
        }
    }
}
