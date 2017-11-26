namespace ServiceStack.Quartz
{
    public class JobDataMapSummary
    {
        public int Count { get; set; }
        public bool Dirty { get; set; }
        public bool IsEmpty { get; set; }
        public string[] Keys { get; set; }
        public object[] Values { get; set; }
        public string Data { get; set; }
    }
}