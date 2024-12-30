using JobManager.Domain.JobSchedulerInstance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSchedulerInstance.Configuration;
internal class JobInstanceConfiguration : IEntityTypeConfiguration<JobInstance>
{
    public void Configure(EntityTypeBuilder<JobInstance> builder)
    {
        builder.ToTable(nameof(JobInstance));

        builder.Property(j => j.Status)
              .HasConversion(
               v => v.ToString(), // Convert enum to string for storage
               v => (Status)Enum.Parse(typeof(Status), v!))
            .HasColumnName("Status");

        builder.HasKey(j => j.Id);

        builder.HasMany(j => j.JobStepInstances)
            .WithOne(j => j.JobInstance)
            .HasForeignKey(j => j.JobInstanceId);

        builder.HasOne(j => j.Job);
    }
}
