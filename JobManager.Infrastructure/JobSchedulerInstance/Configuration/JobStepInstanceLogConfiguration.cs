using JobManager.Domain.JobSchedulerInstance;
using JobManager.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSchedulerInstance.Configuration;
internal class JobStepInstanceLogConfiguration :BaseConfiguration<JobStepInstanceLog>
{
    public override void ConfigureBuilder(EntityTypeBuilder<JobStepInstanceLog> builder)
    {
        builder.ToTable("job_step_instance_log");
        builder.HasKey(j => j.Id);
        builder.HasOne(j => j.JobStepInstance)
            .WithMany(j => j.JobStepInstanceLogs)
            .HasForeignKey(j => j.JobStepInstanceId);
    }
}
