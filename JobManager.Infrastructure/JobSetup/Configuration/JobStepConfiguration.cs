using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSetup.Configuration;
internal class JobStepConfiguration : BaseConfiguration<JobStep>
{
    public override void ConfigureBuilder(EntityTypeBuilder<JobStep> builder)
    {
        builder.ToTable("job_step");

        builder.HasKey(jobStep => jobStep.Id);

        builder.Property(jobStep => jobStep.JsonParameter).IsRequired();

        builder.HasOne(jobStep => jobStep.Job)
            .WithMany(job => job.JobSteps)
            .HasForeignKey(jobStep => jobStep.JobId);

        builder.HasOne(jobStep => jobStep.JobConfig);

    }
}
