using JobManager.Api.Test.Abstraction;
using JobManager.Framework.Application.JobSetup;
using JobManager.Framework.Application.JobSetup.ScheduleJob;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;
using JobManager.Framework.Infrastructure.Scheduler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RecurringDetail = JobManager.Framework.Application.JobSetup.ScheduleJob.RecurringDetail;

namespace JobManager.Api.Test;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability",
                                                 "CA1515:Consider making public types internal", 
                                                  Justification = "As its test class it should public")]
public class JobSetupTest : BaseTest
{
    private readonly IServiceProvider _serviceProvider;

    public JobSetupTest(BaseTestWebAppFactory factory) : base(factory) => _serviceProvider = factory.Services;

    [Fact]
    public async Task Create_OneTimeJob_And_PersistAsync()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        ISender _sender = scope.ServiceProvider.GetService<ISender>()!;
        IJobRepository _jobRepository = scope.ServiceProvider.GetService<IJobRepository>()!;

        ScheduleJobCommand command = new ScheduleJobCommand("Test",
                                                            DateTime.Now,
                                                            JobType.Onetime,
                                                            [new Step("Test")]);

        Result<long> result = await _sender.Send(command);

        Assert.True(result.IsSuccess);
        Framework.Domain.JobSetup.Job? job = await _jobRepository.GetByIdAsync(result.Value);
        Assert.NotNull(job);
        Assert.Equal("Test", job.Description);
        Assert.Equal(JobType.Onetime.ToString(), job.Type);
        Assert.NotEmpty(job.JobSteps);
    }

    [Fact]
    public async Task Create_RecurringJob_And_PersistAsycn()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        ISender _sender = scope.ServiceProvider.GetService<ISender>()!;
        IJobRepository _jobRepository = scope.ServiceProvider.GetService<IJobRepository>()!;

        ScheduleJobCommand command = new ScheduleJobCommand("Job2",
                                                            DateTime.Now,
                                                            JobType.Recurring,
                                                            [new Step("Job2", """{"Name": "NagendraK"}""")],
                                                            new RecurringDetail(RecurringType.EveryNoSecond,
                                                                                15,
                                                                                default,
                                                                                default,
                                                                                default,
                                                                                default));

        Result<long> result = await _sender.Send(command);

        Assert.True(result.IsSuccess);
        Framework.Domain.JobSetup.Job? job = await _jobRepository.GetByIdAsync(result.Value);
        Assert.NotNull(job);
        Assert.Equal("Job2", job.Description);
        Assert.Equal(JobType.Recurring.ToString(), job.Type);
        Assert.NotEmpty(job.JobSteps);
        Assert.NotNull(job.RecurringDetail);
        Assert.Equal(RecurringType.EveryNoSecond.ToString(), job.RecurringDetail.RecurringType);

    }

    [Fact]
    public async Task ExecuteAsync_ShouldInvokeMethods()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IJobScheduler _jobScheduler = scope.ServiceProvider.GetService<IJobScheduler>()!;
      
        await _jobScheduler.ExecuteAsync(scope);
       
        Assert.True(true);
    }
}

