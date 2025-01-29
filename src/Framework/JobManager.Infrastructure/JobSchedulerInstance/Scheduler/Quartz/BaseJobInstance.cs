using System.Text.Json;
using Quartz;
using Exceptions = JobManager.Framework.Application.Abstractions.Exceptions;

namespace JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

[DisallowConcurrentExecution]
public abstract class BaseJobInstance<TParameter> : IJob
{
    protected TParameter? Parameter { get; private set; }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Parameter = JsonSerializer.Deserialize<TParameter>(dataMap.GetString("jsonParameter") ?? "{}");
            await Execute();
        }
        catch (Exception ex)
        {
            string exceptionDetails = Exceptions.ValidationException.GetExceptionDetails(ex);
            throw new Exception($"An error occurred: {exceptionDetails}");
        }
    }

    public abstract Task Execute();
}
