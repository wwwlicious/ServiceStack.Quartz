namespace ServiceStack.Quartz
{
    [Route("/quartz/api/triggers", "GET")]
    [Route("/quartz/api/triggergroup/{Group}", "GET")]
    public class GetQuartzTriggers : IGet, IReturn<GetQuartzTriggersResponse>
    {
        [ApiMember(Description = "Get a summary for all triggers in a specific group", IsRequired = false)]
        public string Group { get; set; }

        [ApiMember(Description = "Get a summary for all triggers in a specific group", IsRequired = false)]
        public string JobName { get; set; }
    }
}