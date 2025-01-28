using System.Collections.Frozen;
using JobManager.Framework.Application.JobSetup.ConfigureJob;
using JobManager.Framework.Domain.Abstractions;
using JobManager.Framework.Domain.JobSetup;
using JobManager.Framework.Infrastructure.JobSchedulerInstance.Scheduler.Quartz;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Job.Modules.Common;

internal sealed class JobAssemblyProvider : IJobAssemblyProvider
{
    public FrozenDictionary<string, string?> JobNameAssemblyDictionary { get; private set; }
    private readonly IServiceProvider _serviceProvider;


    public JobAssemblyProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task LoadJobsFromAssemblyAsync(CancellationToken cancellationToken = default)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
            
        ISender _sender = scope.ServiceProvider.GetRequiredService<ISender>();
      
        try
        {
            Type BaseType = typeof(BaseJobInstance<>);
            IEnumerable<Type> Types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetExportedTypes()
                    .Where(t => !t.IsAbstract
                                && t.BaseType is not null
                                && t.BaseType.IsGenericType
                                && t.BaseType.GetGenericTypeDefinition() == BaseType));

            IEnumerable<string> repeatedTypeNames = Types
                .GroupBy(type => type.Name)
                .Where(group => group.Count() > 1)
                .Select(g => g.Key);

            if (repeatedTypeNames.Any())
                throw new InvalidOperationException($"The following job name(s) are repeated: {string.Join(", ", repeatedTypeNames)}");

            JobNameAssemblyDictionary = Types.ToFrozenDictionary(type => type.Name, type => type.FullName);
            await SyncJobConfigToDatabase(_sender, JobNameAssemblyDictionary.Select(x => x.Key));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    private async Task SyncJobConfigToDatabase(ISender _sender, IEnumerable<string> jobNames)
    {
        Result result = await _sender.Send(new ConfigureJobCommand(jobNames));
        if (result.IsFailure)
            throw new InvalidOperationException($"Failed to sync job config to database {result.Error}");
    }

    public string GetAssemblyName(string className)
    {
        JobNameAssemblyDictionary.TryGetValue(className, out string assemblyName);
        return assemblyName;
    }
}
