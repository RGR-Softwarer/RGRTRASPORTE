using MediatR;

namespace Application.Queries.Base;

public abstract class BaseQuery<TResponse> : IRequest<TResponse>
{
    public DateTime DataConsulta { get; private set; }

    protected BaseQuery()
    {
        DataConsulta = DateTime.UtcNow;
    }
} 