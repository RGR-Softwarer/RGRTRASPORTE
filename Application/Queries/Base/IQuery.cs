using MediatR;

namespace Application.Queries.Base
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
} 