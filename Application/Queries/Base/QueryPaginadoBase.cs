using Application.Common;
using Application.Dtos;
using MediatR;

namespace Application.Queries.Base
{
    public abstract class QueryPaginadoBase<TDto> : QueryPaginado<BaseResponse<ResponseGridDto<TDto>>>
    {
        protected QueryPaginadoBase()
        {
        }

        protected QueryPaginadoBase(
            List<FiltroGrid> filtros,
            int paginaAtual = 1,
            int tamanhoPagina = 10,
            string campoOrdenacao = "Id",
            bool descendente = false) : base(filtros, paginaAtual, tamanhoPagina, campoOrdenacao, descendente)
        {
        }
    }
} 