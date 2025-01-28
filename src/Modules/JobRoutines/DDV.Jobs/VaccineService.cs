using Microsoft.Extensions.Logging;

namespace DDV.Jobs;

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


