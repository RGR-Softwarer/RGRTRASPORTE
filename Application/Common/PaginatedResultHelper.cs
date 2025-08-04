using Application.Dtos;
using Application.Queries.Base;

namespace Application.Common
{
    public static class PaginatedResultHelper
    {
        public static ResponseGridDto<T> CreatePaginatedResult<T>(
            IEnumerable<T> items, 
            int total, 
            QueryPaginado<object> request)
        {
            return ResponseGridDto<T>.Create(
                items, 
                total, 
                request.PaginaAtual, 
                request.TamanhoPagina);
        }

        public static ResponseGridDto<T> CreatePaginatedResult<T>(
            IEnumerable<T> items, 
            int total, 
            int paginaAtual, 
            int tamanhoPagina)
        {
            return ResponseGridDto<T>.Create(items, total, paginaAtual, tamanhoPagina);
        }

        public static ResponseGridDto<T> CreateEmptyResult<T>(int paginaAtual = 1, int tamanhoPagina = 10)
        {
            return ResponseGridDto<T>.CreateEmpty(paginaAtual, tamanhoPagina);
        }
    }
} 