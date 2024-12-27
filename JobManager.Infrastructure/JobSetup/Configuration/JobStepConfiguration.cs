using JobManager.Domain.JobSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSetup.Configuration;
internal class JobStepConfiguration : IEntityTypeConfiguration<JobStep>
{
    public void Configure(EntityTypeBuilder<JobStep> builder)
    {
        builder.ToTable("jobstep");

        builder.HasKey(jobStep => jobStep.Id);

        builder.Property(jobStep => jobStep.JsonParameter).IsRequired();

        builder.HasOne(jobStep => jobStep.Job)
            .WithMany(job => job.JobSteps)
            .HasForeignKey(jobStep => jobStep.JobId);

        builder.HasOne(jobStep => jobStep.JobConfig);
    }
}
