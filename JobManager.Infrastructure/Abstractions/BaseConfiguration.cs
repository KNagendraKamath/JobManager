using JobManager.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.Abstractions;

internal abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        ConfigureBuilder(builder);

        builder.HasKey(j => j.Id);
        builder.Property<uint>("RowVersion")
                    .IsRowVersion();
    }

    public abstract void ConfigureBuilder(EntityTypeBuilder<T> builder);

}
