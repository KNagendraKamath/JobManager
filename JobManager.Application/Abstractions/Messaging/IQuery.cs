using JobManager.Framework.Domain.Abstractions;
using MediatR;

namespace JobManager.Framework.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
