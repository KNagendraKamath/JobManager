using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSetup.Configuration;
internal sealed class JobConfiguration : BaseConfiguration<Job>
{
    public override void ConfigureBuilder(EntityTypeBuilder<Job> builder)
    {

        builder.ToTable("job");

        builder.Property(job => job.Type)
               .HasConversion(
                v => v.ToString(), // Convert enum to string for storage
                v => (JobType)Enum.Parse(typeof(JobType), v!))
             .HasColumnName("type");

        builder.HasOne(job => job.RecurringDetail)
               .WithOne(RecurringDetail => RecurringDetail.Job)
               .HasForeignKey<RecurringDetail>(RecurringDetail => RecurringDetail.JobId);

        builder.HasMany(job => job.JobSteps)
            .WithOne(jobStep => jobStep.Job)
            .HasForeignKey(jobStep => jobStep.JobId);
    }
}
