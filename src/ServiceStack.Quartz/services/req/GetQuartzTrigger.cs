namespace ServiceStack.Quartz
{
    [Route("/quartz/api/triggergroup/{GroupName}/{TriggerName}", "GET", Summary = "Gets a summary for a specific trigger")]
    public class GetQuartzTrigger : IGet, IReturn<GetQuartzTriggerResponse>
    {
        public string TriggerName { get; set; }
        public string GroupName { get; set; }
    }
}