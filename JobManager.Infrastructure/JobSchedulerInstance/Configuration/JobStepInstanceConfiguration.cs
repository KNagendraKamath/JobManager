using JobManager.Domain.JobSchedulerInstance;
using JobManager.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSchedulerInstance.Configuration;
internal class JobStepInstanceConfiguration : BaseConfiguration<JobStepInstance>//IEntityTypeConfiguration<JobStepInstance>
{
    public override void ConfigureBuilder(EntityTypeBuilder<JobStepInstance> builder)
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
