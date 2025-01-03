
using JobManager.Domain.JobSetup;
using JobManager.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManager.Infrastructure.JobSetup.Configuration;
internal sealed class RecurringDetailConfiguration : BaseConfiguration<RecurringDetail>
{
    public override void ConfigureBuilder(EntityTypeBuilder<RecurringDetail> builder)
    {
        builder.ToTable("recurring_detail");

        builder.HasOne(rd => rd.Job)
          .WithOne(j => j.RecurringDetail)
          .HasForeignKey<RecurringDetail>(rd => rd.JobId);


        builder.Property(rd => rd.RecurringType)
            .HasConversion(
                v => v.ToString(),
                v => (RecurringType)Enum.Parse(typeof(RecurringType), v));

        builder.Property(rd => rd.DayOfWeek)
            .HasConversion(
                v => v.ToString(),
                v => (DayOfWeek)Enum.Parse(typeof(DayOfWeek),v));


    }
}
