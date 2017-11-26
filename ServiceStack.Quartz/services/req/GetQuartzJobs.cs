namespace ServiceStack.Quartz
{
    [Route("/quartz/jobs", "GET")]
    [Route("/quartz/group/{Group}", "GET")]
    public class GetQuartzJobs : IGet, IReturn<GetQuartzJobsResponse>
    {
        [ApiMember(Description = "Get a summary for all jobs in a specific group", IsRequired = false)]
        public string Group { get; set; }

        [ApiMember(Description = "Get a summary for all jobs in a specific group", IsRequired = false)]
        public string JobName { get; set; }
    }
}