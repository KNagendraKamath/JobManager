using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;

namespace JobManager.Framework.Application.JobSetup.ConfigureJob;
public sealed class ConfigureJobCommandHandler : ICommandHandler<ConfigureJobCommand>
{
    private readonly IJobConfigRepository _jobConfigRepository;

    public ConfigureJobCommandHandler(IJobConfigRepository jobConfigRepository) => 
        _jobConfigRepository = jobConfigRepository;

    public async Task<Result> Handle(ConfigureJobCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<JobConfig> JobConfigs = request.Names.Select(name => JobConfig.Create(name));
        await _jobConfigRepository.AddJobConfig(JobConfigs,cancellationToken);
        return Result.Success();
    }
}
