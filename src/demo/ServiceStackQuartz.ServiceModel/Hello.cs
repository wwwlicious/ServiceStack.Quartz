namespace ServiceStack.Quartz.ServiceModel
{
    using ServiceStack;

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