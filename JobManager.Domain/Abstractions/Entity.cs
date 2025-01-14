using System.ComponentModel.DataAnnotations.Schema;

namespace JobManager.Domain.Abstractions;

public abstract class Entity
{
    public Entity()
    {
        CreatedTime = DateTimeOffset.UtcNow;
        Active = true;
    }

    public long Id { get; set; } 
    public DateTimeOffset CreatedTime { get; init; }
    public DateTimeOffset? UpdatedTime { get; set; }
    public long CreatedById { get; set; }
    public long? UpdatedById { get; set; }
    public bool Active { get; set; }

    public uint RowVersion { get; init; }

    public virtual void Deactivate()
    {
        Active = false;
    }
}
