using JobManager.Framework.Domain.Abstractions;
using MediatR;

namespace JobManager.Framework.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
