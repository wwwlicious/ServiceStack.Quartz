namespace ServiceStack.Quartz
{
    [Route("/quartz/api/jobgroup/{GroupName}/{JobName}", "GET", Summary = "Gets a summary for a specific job")]
    public class GetQuartzJob : IGet, IReturn<GetQuartzJobResponse>
    {
        public string JobName { get; set; }
        public string GroupName { get; set; }
    }
}