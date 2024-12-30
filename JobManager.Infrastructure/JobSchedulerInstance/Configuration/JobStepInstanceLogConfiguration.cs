using JobManager.Domain.JobSchedulerInstance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSchedulerInstance.Configuration;
internal class JobStepInstanceLogConfiguration : IEntityTypeConfiguration<JobStepInstanceLog>
{
    public void Configure(EntityTypeBuilder<JobStepInstanceLog> builder)
    {
        builder.ToTable(nameof(JobStepInstanceLog));
        builder.HasKey(j => j.Id);
        builder.HasOne(j => j.JobStepInstance)
            .WithMany(j => j.JobStepInstanceLogs)
            .HasForeignKey(j => j.JobStepInstanceId);
    }
}
