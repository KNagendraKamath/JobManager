namespace JobScheduler.Application.Abstractions;
public abstract class BaseJobInstance<T> where T : IJobParameter
{
    protected T JobParameter { get; set; }

    public abstract Task Execute();
}
