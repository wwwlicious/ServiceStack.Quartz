using ServiceStack;
using ServiceStackWithQuartz.ServiceModel;

namespace ServiceStackWithQuartz.ServiceInterface
{
    public class MyServices : Service
    {
        public HelloResponse Any(Hello request)
        {
            return new HelloResponse { Result = "Hello, {0}!".Fmt(request.Name) };
        }
    }
}