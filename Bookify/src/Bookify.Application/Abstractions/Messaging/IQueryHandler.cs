using Bookify.Domain;
using MediatR;

namespace Bookify.Application;

public interface IQueryHandler<TQuery, TResponse>: IRequestHandler<TQuery, Result<TResponse>> where TQuery: IQuery<TResponse>
{

};