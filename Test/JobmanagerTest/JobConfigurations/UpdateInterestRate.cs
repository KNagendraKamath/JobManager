using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Application.Abstractions;

namespace JobmanagerTest.JobConfigurations;
internal class UpdateInterestRate : BaseJobInstance<UpdateInterestRateParam>
{
    public override Task Execute()
    {
        JobParameter.InterestRate = 0.05;
        return Task.CompletedTask;
    }
}
