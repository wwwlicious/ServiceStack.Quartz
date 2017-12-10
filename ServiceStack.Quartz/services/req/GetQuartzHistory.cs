namespace ServiceStack.Quartz
{
    [Route("/quartz/api/history", "GET")]
    public class GetQuartzHistory : IGet, IReturn<GetQuartzHistoryResponse>
    {
    }
}