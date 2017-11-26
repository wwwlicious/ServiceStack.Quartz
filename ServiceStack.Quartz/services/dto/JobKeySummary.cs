namespace ServiceStack.Quartz
{
    public class JobKeySummary
    {
        public string Group { get; set; }
        public string Name { get; set; }
        
        public string Self => $"http://localhost:5000/quartz/jobs/{Name}";
        public string GroupLink => $"http://localhost:5000/quartz/group/{Group}";
        public string Summary => $"http://localhost:5000/quartz/";
    }
}