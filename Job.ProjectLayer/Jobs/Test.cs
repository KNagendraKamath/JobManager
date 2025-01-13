using JobManager.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;
namespace Job.ProjectLayer;

public class Test : BaseJobInstance<TestParam>
{
    public override Task Execute()
    {
      
        File.WriteAllText("C:\\Users\\nagka\\source\\repos\\JobManager\\JobRunner\\test.txt", $"{DateTime.Now.ToString("F")}");
        return Task.CompletedTask;
    }
}

public class TestParam
{
}
