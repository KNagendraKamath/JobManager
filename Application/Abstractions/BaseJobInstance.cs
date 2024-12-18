using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler.Application.Abstractions;
public abstract class BaseJobInstance<T> where T : IJobParameter
{
    protected T JobParameter { get; set; }

    public abstract Task Execute();
}
