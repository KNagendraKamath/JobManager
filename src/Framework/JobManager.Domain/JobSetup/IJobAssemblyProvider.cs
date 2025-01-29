using System.Collections.Frozen;

namespace JobManager.Framework.Domain.JobSetup;

public interface IJobAssemblyProvider
{
    static FrozenDictionary<string, string?> JobNameAssemblyDictionary { get; }
    string GetAssemblyName(string className);
    Task LoadJobsFromAssemblyAsync(CancellationToken cancellationToken = default);
}
