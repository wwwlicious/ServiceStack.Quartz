
namespace ServiceStack.Quartz
{
    public class GetQuartzHistoryResponse
    {
        public JobExecutionHistory[] History { get; set; } = new JobExecutionHistory[0];
    }
}