using JobManager.Domain.Abstractions;
using JobManager.Domain.JobSchedulerInstance;
using JobManager.Domain.JobSetup;
using Microsoft.EntityFrameworkCore;

namespace JobManager.Infrastructure;

public class JobDbContext : DbContext,IUnitOfWork
{
    public JobDbContext(DbContextOptions<JobDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JobDbContext).Assembly);

    public DbSet<JobConfig> JobConfigs { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobStep> JobSteps { get; set; }
    public DbSet<JobInstance> JobInstances { get; set; }
    public DbSet<JobStepInstance> JobStepsInstances { get; set; }
    public DbSet<JobStepInstanceLog> JobStepInstanceLogs { get; set; }

}
