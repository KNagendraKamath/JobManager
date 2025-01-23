using JobManager.Framework.Application.Abstractions.Messaging;
using JobManager.Framework.Domain.JobSetup;


namespace JobManager.Framework.Application.JobSetup.GetAllJob;
public record GetAllJobQuery(int Page=0,int PageSize=100) : IQuery<IEnumerable<Job>>;
