using JobManager.Domain.JobSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSetup.Configuration;
internal sealed class JobConfigConfiguration : IEntityTypeConfiguration<JobConfig>
{
    public void Configure(EntityTypeBuilder<JobConfig> builder)
    {
        builder.ToTable(nameof(JobConfig));
        builder.HasKey(jobConfig => jobConfig.Id);
    }
}
