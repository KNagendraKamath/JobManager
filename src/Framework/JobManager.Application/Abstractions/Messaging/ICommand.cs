using JobManager.Framework.Domain.Abstractions;
using MediatR;

namespace JobManager.Framework.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand;
public interface ICommand<TReponse> : IRequest<Result<TReponse>>, IBaseCommand;
public interface IBaseCommand;
