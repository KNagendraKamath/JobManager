using JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;
using Microsoft.Extensions.Logging;

namespace DDV.Jobs;

public class MetadataServiceUpdateTask : BaseJobInstance<MetadataServiceUpdateTaskParam>
{
    private readonly IVaccineService _vaccineService;

    public MetadataServiceUpdateTask(IVaccineService vaccineService)
    {
        _vaccineService = vaccineService;
    }
    public override async Task Execute()
    {
        await _vaccineService.UpdateVaccineMetadata(Parameter!.Url);
    }
}

public record MetadataServiceUpdateTaskParam(Uri Url);

public interface IVaccineService
{
    Task UpdateVaccineMetadata(Uri url);
}

public class VaccineService : IVaccineService
{
    private readonly ILogger<VaccineService> _logger;
    public string Url { get; set; }

    public VaccineService(ILogger<VaccineService> logger) =>
        _logger = logger;

    public async Task UpdateVaccineMetadata(Uri url)
    {
        await Task.Delay(100);
        _logger.LogInformation("Vaccine metadata update started {Name}", url);
    }
}


