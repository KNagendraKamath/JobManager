namespace JobScheduler.Application.Abstractions;
public abstract class BaseJobInstance<T> where T : class
{
    protected T Parameter { get; set; }

    public abstract Task Execute();
}
