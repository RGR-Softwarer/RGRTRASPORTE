using MediatR;

namespace Application.Queries.Base
{
    public abstract class QueryPaginado<TResponse> : IRequest<TResponse>
    {
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }
        public string CampoOrdenacao { get; set; }
        public bool Descendente { get; set; }
    }
} 
