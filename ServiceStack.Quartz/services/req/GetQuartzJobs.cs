namespace ServiceStack.Quartz
{
    [Route("/quartz/api/jobs", "GET")]
    [Route("/quartz/api/jobgroup/{Group}", "GET")]
    public class GetQuartzJobs : IGet, IReturn<GetQuartzJobsResponse>
    {
        [ApiMember(Description = "Get a summary for all jobs in a specific group", IsRequired = false)]
        public string Group { get; set; }

        [ApiMember(Description = "Get a summary for all jobs in a specific group", IsRequired = false)]
        public string JobName { get; set; }

        [ApiMember(Description = "Return only executing jobs", IsRequired = false)]
        public bool Executing { get; set; } 
    }
}