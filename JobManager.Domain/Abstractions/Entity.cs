namespace JobManager.Domain.Abstractions;

public abstract class Entity
{
    public long Id { get; set; } 
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset? UpdatedTime { get; set; }
    public long CreatedById { get; set; }
    public long? UpdatedById { get; set; }
    public bool Active { get; set; }

    public virtual void Deactivate()
    {
        Active = false;
    }
}
