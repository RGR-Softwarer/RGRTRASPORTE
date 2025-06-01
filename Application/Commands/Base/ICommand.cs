using MediatR;

namespace Application.Commands.Base
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
} 