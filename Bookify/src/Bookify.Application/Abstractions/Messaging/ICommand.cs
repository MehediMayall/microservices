using Bookify.Domain;
using MediatR;

namespace Bookify.Application;

public interface ICommand: IRequest<Result>, IBaseCommand{}
public interface ICommand<TResponse>: IRequest<Result<TResponse>>, IBaseCommand{}

public interface IBaseCommand{}

