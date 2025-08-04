using Application.Common;
using Application.Dtos;
using Dominio.Entidades;
using Dominio.Interfaces.Infra.Data;
using MediatR;
using System.Linq.Expressions;
using AutoMapper;

namespace Application.Queries.Base
{
    public abstract class QueryPaginadoHandlerBase<TEntity, TDto, TQuery> 
        : IRequestHandler<TQuery, BaseResponse<ResponseGridDto<TDto>>>
        where TEntity : BaseEntity
        where TQuery : QueryPaginadoBase<TDto>
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        protected QueryPaginadoHandlerBase(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<BaseResponse<ResponseGridDto<TDto>>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Monta a expressão de filtro baseada nos filtros recebidos
                Expression<Func<TEntity, bool>>? filter = null;
                
                if (request.Filtros?.Any() == true)
                {
                    filter = BuildFilterExpression(request.Filtros);
                }

                // Mapeia o campo de ordenação para a propriedade correta
                var orderByProperty = MapOrderField(request.CampoOrdenacao);

                // Busca paginada
                var (items, total) = await _repository.GetPaginatedAsync(
                    request.PaginaAtual,
                    request.TamanhoPagina,
                    orderByProperty,
                    request.Descendente,
                    filter,
                    cancellationToken);

                // Converte para DTO usando AutoMapper
                var itemsDto = await MapToDto(items);

                var resultado = PaginatedResultHelper.CreatePaginatedResult(
                    itemsDto, 
                    total, 
                    request.PaginaAtual, 
                    request.TamanhoPagina);

                return BaseResponse<ResponseGridDto<TDto>>.Ok(resultado);
            }
            catch (Exception ex)
            {
                return BaseResponse<ResponseGridDto<TDto>>.Erro($"Erro ao buscar dados: {ex.Message}");
            }
        }

        protected virtual async Task<IEnumerable<TDto>> MapToDto(IEnumerable<TEntity> entities)
        {
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }
        
        protected abstract Expression<Func<TEntity, bool>> BuildFilterExpression(List<FiltroGrid> filtros);
        
        protected abstract string MapOrderField(string campoOrdenacao);
    }
} 