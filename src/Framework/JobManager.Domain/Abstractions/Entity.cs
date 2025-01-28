namespace JobManager.Framework.Domain.Abstractions;

public abstract class Entity
{
    protected Entity()
    {
        CreatedTime = DateTime.UtcNow;
        Active = true;
    }

    public long Id { get; set; } 
    public DateTimeOffset CreatedTime { get; init; }
    public DateTimeOffset? UpdatedTime { get; set; }
    public bool Active { get; set; }

    public uint RowVersion { get; init; }

    public virtual void Deactivate() => 
        Active = false;
}
