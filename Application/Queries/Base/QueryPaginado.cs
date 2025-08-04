using Application.Common;
using MediatR;

namespace Application.Queries.Base
{
    public abstract class QueryPaginado<TResponse> : IRequest<TResponse>
    {
        public List<FiltroGrid> Filtros { get; set; } = new();
        public int PaginaAtual { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
        public string CampoOrdenacao { get; set; } = "Id";
        public bool Descendente { get; set; } = false;

        protected QueryPaginado()
        {
        }

        protected QueryPaginado(
            List<FiltroGrid> filtros,
            int paginaAtual = 1,
            int tamanhoPagina = 10,
            string campoOrdenacao = "Id",
            bool descendente = false)
        {
            Filtros = filtros ?? new List<FiltroGrid>();
            PaginaAtual = paginaAtual;
            TamanhoPagina = tamanhoPagina;
            CampoOrdenacao = campoOrdenacao;
            Descendente = descendente;
        }
    }
} 
