using JobManager.Domain.JobSchedulerInstance;
using JobManager.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSchedulerInstance.Configuration;
internal class JobInstanceConfiguration : BaseConfiguration<JobInstance>
{
    public override void ConfigureBuilder(EntityTypeBuilder<JobInstance> builder)
    {
        builder.ToTable("job_instance");

        builder.Property(j => j.Status)
              .HasConversion(
               v => v.ToString(), // Convert enum to string for storage
               v => (Status)Enum.Parse(typeof(Status), v!))
            .HasColumnName("status");

        builder.HasKey(j => j.Id);

        builder.HasMany(j => j.JobStepInstances)
            .WithOne(j => j.JobInstance)
            .HasForeignKey(j => j.JobInstanceId);

        builder.HasOne(j => j.Job);
    }
}
