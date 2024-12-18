using System.Text.Json;
using JobScheduler.Application.Abstractions;
using JobScheduler.Domain.Jobs;

namespace JobmanagerTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // var jobparameter = new UpdateInterestRateParam() { InterestRate = 0.5 };
        // var JobSteps = [new() {JobConfigId = 1, JobParameter=JsonSerializer.Serialize(jobparameter)}];
        // var Job = Job.Create("TestJob", "TestJob Description", DateTime.Now, JobType.Onetime, null, JobSteps);
        //
        // JobScheduler.Run(Job.Id);
    }
}
