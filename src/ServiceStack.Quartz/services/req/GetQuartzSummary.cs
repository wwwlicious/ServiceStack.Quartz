namespace ServiceStack.Quartz
{
    [Route("/quartz/api")]
    public class GetQuartzSummary : IGet, IReturn<GetQuartzSummaryResponse>
    {
    }
}