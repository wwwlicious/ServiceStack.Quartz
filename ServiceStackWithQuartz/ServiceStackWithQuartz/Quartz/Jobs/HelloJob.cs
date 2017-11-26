using Quartz;
using ServiceStack.Text;
using ServiceStackWithQuartz.ServiceInterface;

namespace ServiceStackWithQuartz
{
    public class HelloJob : IJob
    {
        private MyServices MyServices { get; set; }
        public HelloJob(MyServices myServices)
        {
            MyServices = myServices;
        }

        public virtual void Execute(IJobExecutionContext context)
        {
            using (var service = MyServices)
            {
                var response = MyServices.Any(new ServiceModel.Hello
                {
                    Name = "CodeRevver"
                });

                response.PrintDump();
            }
        }
    }
}
