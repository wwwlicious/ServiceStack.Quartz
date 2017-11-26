namespace ServiceStack.Quartz
{
    [Route("/quartz")]
    public class GetQuartzSummary : IGet, IReturn<GetQuartzSummaryResponse>
    {
    }
}