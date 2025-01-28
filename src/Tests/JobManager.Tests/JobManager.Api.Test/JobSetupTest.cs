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
    private readonly ISender _sender;
    private readonly IJobRepository _jobRepository;

    public JobSetupTest(BaseTestWebAppFactory factory) : base(factory)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        _sender = scope.ServiceProvider.GetService<ISender>()!;
        _jobRepository = scope.ServiceProvider.GetService<IJobRepository>()!;
    }




    [Fact]
    public async Task Create_OneTimeJob_And_PersistAsync()
    {
        try
        {
            ScheduleJobCommand command = new ScheduleJobCommand("Test",
                                                            DateTime.Now,
                                                            JobType.Onetime,
                                                            [new Step("Test")]);

            Result<long> result = await _sender.Send(command);

            Assert.True(result.IsSuccess);
            Job? job = await _jobRepository.GetByIdAsync(result.Value);
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
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}

