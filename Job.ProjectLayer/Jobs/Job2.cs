using JobManager.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;
using Microsoft.Extensions.Logging;

namespace Job.ProjectLayer;
public class Job2 : BaseJobInstance<Job2Param>
{
    private readonly ILogger<Job2> _logger;

    public Job2(ILogger<Job2> logger) 
    {
        _logger = logger;
    }

    public override Task Execute()
    {
        
        _logger.LogInformation("Job2 execution started.");

        // Add your job execution logic here
        _logger.LogInformation($"Parameter passed is {Parameter?.Name}");

        _logger.LogInformation("Job2 execution completed.");
        return Task.CompletedTask;
    }
}

public class Job2Param
{
    public string Name { get; set; }
}
