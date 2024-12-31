﻿using JobManager.Domain.JobSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSetup.Configuration;
internal sealed class JobConfigConfiguration : IEntityTypeConfiguration<JobConfig>
{
    public void Configure(EntityTypeBuilder<JobConfig> builder)
    {
        builder.ToTable("job_config");
        builder.HasKey(jobConfig => jobConfig.Id);
    }
}
