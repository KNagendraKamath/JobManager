using JobManager.Framework.Application.Abstractions.Messaging;

namespace JobManager.Framework.Application.JobSetup.ConfigureJob;
public record ConfigureJobCommand(IEnumerable<string> Names):ICommand;
