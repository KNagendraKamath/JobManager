using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSetup.CreateJob;
public record UnscheduleJobCommand(long JobId,string? JobName):ICommand;

