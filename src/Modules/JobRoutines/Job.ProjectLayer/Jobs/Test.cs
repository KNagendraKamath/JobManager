using System.Globalization;
using JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

namespace Job.ProjectLayer;

public class Test : BaseJobInstance<TestParam>
{
    public override async Task Execute()
    {
        await File.WriteAllTextAsync("C:\\Users\\nagka\\source\\repos\\JobManager\\JobRunner\\test.txt", $"{DateTime.Now.ToString("F",CultureInfo.InvariantCulture)}");

    }
}

public class TestParam
{
}
