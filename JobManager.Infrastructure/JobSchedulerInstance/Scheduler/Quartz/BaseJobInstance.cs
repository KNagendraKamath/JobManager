using System.Text.Json;
using JobManager.Application.JobSchedulerInstance.CreateJobInstance;
using JobManager.Application.JobSchedulerInstance.UpdateJobInstance;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace JobManager.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;

[DisallowConcurrentExecution]
public abstract class BaseJobInstance<TParameter> : IJob
{
    protected TParameter? Parameter { get; private set; }

    public async Task Execute(IJobExecutionContext context)
    {
        JobDataMap dataMap = context.JobDetail.JobDataMap;
        Parameter = JsonSerializer.Deserialize<TParameter>(dataMap.GetString("jsonParameter") ?? "{}");
        await Execute();
    }

    public abstract Task Execute();
}
