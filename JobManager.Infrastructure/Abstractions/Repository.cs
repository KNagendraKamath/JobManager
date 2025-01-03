using System;
using JobManager.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace JobManager.Infrastructure.Abstractions;

internal abstract class Repository<T>
    where T : Entity
{
    protected readonly JobDbContext DbContext;

    protected Repository(JobDbContext dbContext) => DbContext = dbContext;

    public async Task<T?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<T>()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public virtual void Add(T entity)
    {
        entity.Active = true;
        entity.CreatedById = 1;
        entity.CreatedTime = DateTime.UtcNow;
        DbContext.Add(entity);
    }

    public virtual void Update(T entity)
    {
        entity.UpdatedById = 1;
        entity.UpdatedTime = DateTime.UtcNow;
        DbContext.Update(entity);
    }

    public virtual void DeleteAsync(T entity) => DbContext.Set<T>().Remove(entity);
}
