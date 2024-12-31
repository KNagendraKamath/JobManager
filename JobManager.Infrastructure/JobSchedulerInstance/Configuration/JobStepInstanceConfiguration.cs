using JobManager.Domain.JobSchedulerInstance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSchedulerInstance.Configuration;
internal class JobStepInstanceConfiguration : IEntityTypeConfiguration<JobStepInstance>
{
    public void Configure(EntityTypeBuilder<JobStepInstance> builder)
    {
        builder.ToTable("job_step_instance");

        builder.Property(j => j.Status)
      .HasConversion(
       v => v.ToString(),
       v => (Status)Enum.Parse(typeof(Status), v!))
    .HasColumnName("status");

        builder.HasKey(j => j.Id);

        builder.HasOne(j => j.JobInstance)
            .WithMany(j => j.JobStepInstances)
            .HasForeignKey(j => j.JobInstanceId);
    }
}
