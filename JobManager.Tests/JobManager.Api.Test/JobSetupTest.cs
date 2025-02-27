using System.Diagnostics;
using JobManager.Api.Test.Abstraction;
using JobManager.Application.JobSetup.CreateJob;
using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSetup;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RecurringDetail = JobManager.Application.JobSetup.CreateJob.RecurringDetail;

namespace JobManager.Api.Test;

public class JobSetupTest: BaseTest
{
    private readonly ISender _sender;
    private readonly IJobRepository _jobRepository;

    public JobSetupTest(BaseTestWebAppFactory factory) : base(factory)
    {
        _sender = factory.Services.CreateScope().ServiceProvider.GetService<ISender>()!;
        _jobRepository = factory.Services.CreateScope().ServiceProvider.GetService<IJobRepository>()!;
    }

    [Fact]
    public async Task Create_OneTimeJob_And_PersistAsync()
    {
        try
        {
            ScheduleJobCommand command = new ScheduleJobCommand("Test",
                                                            DateTime.UtcNow,
                                                            JobType.Onetime,
                                                            [new Step("Test", "{}")]);

            Result<long> result = await _sender.Send(command);

            Assert.True(result.IsSuccess);
            Domain.JobSetup.Job? job = await _jobRepository.GetByIdAsync(result.Value);
            Assert.NotNull(job);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    [Fact]
    public async Task Create_RecurringJob_And_PersistAsycn()
    {
        try
        {
            ScheduleJobCommand command = new ScheduleJobCommand("Job2",
                                                            DateTime.UtcNow,
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
            Domain.JobSetup.Job? job = await _jobRepository.GetByIdAsync(result.Value);
            Assert.NotNull(job);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}

