using JobManager.Framework.Infrastructure.Scheduler.Quartz;

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


