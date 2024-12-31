using JobManager.Domain.JobSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSetup.Configuration;
internal sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {

        builder.ToTable("job");

        builder.Property(job => job.Type)
               .HasConversion(
                v => v.ToString(), // Convert enum to string for storage
                v => (JobType)Enum.Parse(typeof(JobType), v!))
             .HasColumnName("type");

        builder.Property(job => job.RecurringType)
            .HasConversion(
                v => v.ToString(), // Convert enum to string for storage
                v => (RecurringType)Enum.Parse(typeof(RecurringType), v!))
            .HasColumnName("recurring_type"); 

        builder.HasKey(job => job.Id);

        builder.HasMany(job => job.JobSteps)
            .WithOne(jobStep => jobStep.Job)
            .HasForeignKey(jobStep => jobStep.JobId);
    }
}
