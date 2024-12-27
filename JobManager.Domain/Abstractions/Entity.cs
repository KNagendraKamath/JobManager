namespace JobManager.Domain.Abstractions;

public abstract class Entity
{
    public long Id { get; init; } 
    public DateTimeOffset CreatedTime { get; init; }
    public DateTimeOffset? UpdatedTime { get; set; }
    public long CreatedById { get; init; }
    public long? UpdatedById { get; set; }
    public bool Active { get; set; }
}
