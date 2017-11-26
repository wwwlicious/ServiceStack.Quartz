using ServiceStack;

namespace ServiceStackWithQuartz.ServiceModel
{
    [Route("/hello/{Name}")]
    public class Hello : IGet, IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }
}