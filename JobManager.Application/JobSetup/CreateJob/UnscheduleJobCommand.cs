using JobManager.Application.Abstractions.Messaging;

namespace JobManager.Application.JobSetup.CreateJob;
public record UnscheduleJobCommand(long JobId,string? JobName):ICommand;

