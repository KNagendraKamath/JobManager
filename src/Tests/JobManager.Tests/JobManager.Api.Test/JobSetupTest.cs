using System.Diagnostics;
using JobManager.Api.Test.Abstraction;
using JobManager.Framework.Application.JobSetup.ScheduleJob;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RecurringDetail = JobManager.Framework.Application.JobSetup.ScheduleJob.RecurringDetail;

namespace JobManager.Api.Test;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class JobSetupTest : BaseTest
{
    private readonly IServiceProvider _serviceProvider;

    public JobSetupTest(BaseTestWebAppFactory factory) : base(factory)
    {
        _serviceProvider = factory.Services;
      
    }

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
        Job? job = await _jobRepository.GetByIdAsync(result.Value);
        Assert.NotNull(job);
        Assert.Equal("Test", job.Description);
        Assert.Equal(JobType.Onetime, job.Type);
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
        Job? job = await _jobRepository.GetByIdAsync(result.Value);
        Assert.NotNull(job);
        Assert.Equal("Job2", job.Description);
        Assert.Equal(JobType.Recurring, job.Type);
        Assert.NotEmpty(job.JobSteps);
        Assert.NotNull(job.RecurringDetail);
        Assert.Equal(RecurringType.EveryNoSecond, job.RecurringDetail.RecurringType);

    }
}

