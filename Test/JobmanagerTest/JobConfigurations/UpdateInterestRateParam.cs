using JobScheduler.Application.Abstractions;

namespace JobmanagerTest.JobConfigurations;

internal class UpdateInterestRateParam: IJobParameter
{
    public double InterestRate { get; set; }
}
