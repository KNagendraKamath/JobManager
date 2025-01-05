using JobManager.Infrastructure.JobSchedulerInstance.Scheduler;
namespace Job.ProjectLayer;

public class Test : BaseJobInstance<TestParam>
{
  
    public Test(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        
    }

    public override Task Execute()
    {
      
        File.WriteAllText("C:\\Users\\nagka\\source\\repos\\JobManager\\JobRunner\\test.txt", $"{DateTime.Now.ToString("F")}");
        return Task.CompletedTask;
    }
}

public class TestParam
{
}
