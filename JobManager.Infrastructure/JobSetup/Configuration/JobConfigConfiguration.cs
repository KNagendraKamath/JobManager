using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSetup.Configuration;
internal sealed class JobConfigConfiguration : BaseConfiguration<JobConfig>
{
    public override void ConfigureBuilder(EntityTypeBuilder<JobConfig> builder)
    {
        builder.ToTable("job_config");
        builder.HasKey(jobConfig => jobConfig.Id);
    }
}
