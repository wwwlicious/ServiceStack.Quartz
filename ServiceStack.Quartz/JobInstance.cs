namespace ServiceStack.Quartz
{
    using global::Quartz;

    internal class JobInstance
    {
        public IJobDetail JobDetail { get; set; }
        public ITrigger[] Triggers { get; set; }
    }
}