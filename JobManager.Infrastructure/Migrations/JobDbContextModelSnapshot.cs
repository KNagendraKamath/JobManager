﻿// <auto-generated />
using System;
using JobManager.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobManager.Infrastructure.Migrations
{
    [DbContext(typeof(JobDbContext))]
    partial class JobDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("JobManager.Domain.JobSchedulerInstance.JobInstance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_id");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time");

                    b.Property<long>("JobId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_id");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<long?>("UpdatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_id");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time");

                    b.HasKey("Id")
                        .HasName("pk_job_instance");

                    b.HasIndex("JobId")
                        .HasDatabaseName("ix_job_instance_job_id");

                    b.ToTable("job_instance", (string)null);
                });

            modelBuilder.Entity("JobManager.Domain.JobSchedulerInstance.JobStepInstance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_id");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time");

                    b.Property<DateTimeOffset?>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time");

                    b.Property<long>("JobInstanceId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_instance_id");

                    b.Property<long>("JobStepId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_step_id");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<long?>("UpdatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_id");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time");

                    b.HasKey("Id")
                        .HasName("pk_job_step_instance");

                    b.HasIndex("JobInstanceId")
                        .HasDatabaseName("ix_job_step_instance_job_instance_id");

                    b.HasIndex("JobStepId")
                        .HasDatabaseName("ix_job_step_instance_job_step_id");

                    b.ToTable("job_step_instance", (string)null);
                });

            modelBuilder.Entity("JobManager.Domain.JobSchedulerInstance.JobStepInstanceLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_id");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time");

                    b.Property<long>("JobStepInstanceId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_step_instance_id");

                    b.Property<string>("Log")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("log");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<long?>("UpdatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_id");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time");

                    b.HasKey("Id")
                        .HasName("pk_job_step_instance_log");

                    b.HasIndex("JobStepInstanceId")
                        .HasDatabaseName("ix_job_step_instance_log_job_step_instance_id");

                    b.ToTable("job_step_instance_log", (string)null);
                });

            modelBuilder.Entity("JobManager.Domain.JobSetup.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_id");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime>("EffectiveDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("effective_date_time");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<long?>("UpdatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_id");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time");

                    b.HasKey("Id")
                        .HasName("pk_job");

                    b.ToTable("job", (string)null);
                });

            modelBuilder.Entity("JobManager.Domain.JobSetup.JobConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<string>("Assembly")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("assembly");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_id");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<long?>("UpdatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_id");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time");

                    b.HasKey("Id")
                        .HasName("pk_job_config");

                    b.ToTable("job_config", (string)null);
                });

            modelBuilder.Entity("JobManager.Domain.JobSetup.JobStep", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_id");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time");

                    b.Property<long>("JobConfigId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_config_id");

                    b.Property<long>("JobId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_id");

                    b.Property<string>("JsonParameter")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("json_parameter");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<long?>("UpdatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_id");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time");

                    b.HasKey("Id")
                        .HasName("pk_job_step");

                    b.HasIndex("JobConfigId")
                        .HasDatabaseName("ix_job_step_job_config_id");

                    b.HasIndex("JobId")
                        .HasDatabaseName("ix_job_step_job_id");

                    b.ToTable("job_step", (string)null);
                });

            modelBuilder.Entity("JobManager.Domain.JobSetup.RecurringDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<long>("CreatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_id");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time");

                    b.Property<int>("Day")
                        .HasColumnType("integer")
                        .HasColumnName("day");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("day_of_week");

                    b.Property<int>("Hours")
                        .HasColumnType("integer")
                        .HasColumnName("hours");

                    b.Property<long>("JobId")
                        .HasColumnType("bigint")
                        .HasColumnName("job_id");

                    b.Property<int>("Minutes")
                        .HasColumnType("integer")
                        .HasColumnName("minutes");

                    b.Property<string>("RecurringType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("recurring_type");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<int>("Second")
                        .HasColumnType("integer")
                        .HasColumnName("second");

                    b.Property<long?>("UpdatedById")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_id");

                    b.Property<DateTimeOffset?>("UpdatedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time");

                    b.HasKey("Id")
                        .HasName("pk_recurring_detail");

                    b.HasIndex("JobId")
                        .IsUnique()
                        .HasDatabaseName("ix_recurring_detail_job_id");

                    b.ToTable("recurring_detail", (string)null);
                });

            modelBuilder.Entity("JobManager.Domain.JobSchedulerInstance.JobInstance", b =>
                {
                    b.HasOne("JobManager.Domain.JobSetup.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_job_instance_jobs_job_id");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("JobManager.Domain.JobSchedulerInstance.JobStepInstance", b =>
                {
                    b.HasOne("JobManager.Domain.JobSchedulerInstance.JobInstance", "JobInstance")
                        .WithMany("JobStepInstances")
                        .HasForeignKey("JobInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_job_step_instance_job_instance_job_instance_id");

                    b.HasOne("JobManager.Domain.JobSetup.JobStep", "JobStep")
                        .WithMany()
                        .HasForeignKey("JobStepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_job_step_instance_job_steps_job_step_id");

                    b.Navigation("JobInstance");

                    b.Navigation("JobStep");
                });

            modelBuilder.Entity("JobManager.Domain.JobSchedulerInstance.JobStepInstanceLog", b =>
                {
                    b.HasOne("JobManager.Domain.JobSchedulerInstance.JobStepInstance", "JobStepInstance")
                        .WithMany("JobStepInstanceLogs")
                        .HasForeignKey("JobStepInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_job_step_instance_log_job_step_instance_job_step_instance_id");

                    b.Navigation("JobStepInstance");
                });

            modelBuilder.Entity("JobManager.Domain.JobSetup.JobStep", b =>
                {
                    b.HasOne("JobManager.Domain.JobSetup.JobConfig", "JobConfig")
                        .WithMany()
                        .HasForeignKey("JobConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_job_step_job_config_job_config_id");

                    b.HasOne("JobManager.Domain.JobSetup.Job", "Job")
                        .WithMany("JobSteps")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_job_step_job_job_id");

                    b.Navigation("Job");

                    b.Navigation("JobConfig");
                });

            modelBuilder.Entity("JobManager.Domain.JobSetup.RecurringDetail", b =>
                {
                    b.HasOne("JobManager.Domain.JobSetup.Job", "Job")
                        .WithOne("RecurringDetail")
                        .HasForeignKey("JobManager.Domain.JobSetup.RecurringDetail", "JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_recurring_detail_jobs_job_id");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("JobManager.Domain.JobSchedulerInstance.JobInstance", b =>
                {
                    b.Navigation("JobStepInstances");
                });

            modelBuilder.Entity("JobManager.Domain.JobSchedulerInstance.JobStepInstance", b =>
                {
                    b.Navigation("JobStepInstanceLogs");
                });

            modelBuilder.Entity("JobManager.Domain.JobSetup.Job", b =>
                {
                    b.Navigation("JobSteps");

                    b.Navigation("RecurringDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
