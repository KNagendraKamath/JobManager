using JobManager.Domain.Abstractions;
using MediatR;

namespace JobManager.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
