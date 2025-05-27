using AutoMapper;
using Dominio.Dtos;
using Dominio.Dtos.Veiculo;
using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data.Veiculo;
using Dominio.Interfaces.Service;
using Dominio.Validators;
using Infra.CrossCutting.Handlers.Notifications;
using System.Linq.Expressions;
using System.Net;

namespace Service.Services
{
    public class VeiculoService : ServiceBase, IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;

        public VeiculoService(INotificationHandler notificationHandler, IVeiculoRepository veiculoRepository, IMapper mapper) : base(notificationHandler)
        {
            _veiculoRepository = veiculoRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<ResponseGridDto<VeiculoDto>> ObterPaginadoAsync(
        ParametrosBuscaDto parametros,
        CancellationToken cancellationToken = default)
        {
            // Cria expressão de filtro baseada nos parâmetros
            Expression<Func<Veiculo, bool>> filter = null;
            if (parametros.Filtros?.Any() == true)
            {
                var parameter = Expression.Parameter(typeof(Veiculo), "x");
                Expression filterExpression = null;

                foreach (var filtro in parametros.Filtros)
                {
                    var property = Expression.Property(parameter, filtro.Campo);
                    var value = Expression.Constant(filtro.Valor);
                    var comparison = Expression.Call(
                        property,
                        "Contains",
                        Type.EmptyTypes,
                        value);

                    filterExpression = filterExpression == null
                        ? comparison
                        : Expression.AndAlso(filterExpression, comparison);
                }

                if (filterExpression != null)
                    filter = Expression.Lambda<Func<Veiculo, bool>>(filterExpression, parameter);
            }

            // Obtém dados paginados
            var (items, total) = await _veiculoRepository.GetPaginatedAsync(
                parametros.PaginaAtual,
                parametros.TamanhoPagina,
                parametros.CampoOrdenacao,
                parametros.Descendente,
                filter);

            // Mapeia para DTOs
            return new ResponseGridDto<VeiculoDto>
            {
                Items = _mapper.Map<List<VeiculoDto>>(items),
                Total = total,
                Pagina = parametros.PaginaAtual,
                TamanhoPagina = parametros.TamanhoPagina
            };
        }

        public async Task<List<VeiculoDto>> ObterTodosAsync()
        {
            var veiculos = await _veiculoRepository.ObterTodosAsync();

            if (veiculos?.Count > 0)
                return _mapper.Map<List<VeiculoDto>>(veiculos);

            return new List<VeiculoDto>();
        }

        public async Task<VeiculoDto> ObterPorIdAsync(long id)
        {
            var veiculo = await _veiculoRepository.ObterPorIdAsync(id);

            VeiculoDto veiculoDto = null;

            if (veiculo != null)
                return _mapper.Map<VeiculoDto>(veiculo);


            return veiculoDto;
        }

        public async Task AdicionarAsync(VeiculoDto dto)
        {
            var veiculo = _mapper.Map<Veiculo>(dto);

            Validar(veiculo, new VeiculoValidator());

            await _veiculoRepository.AdicionarAsync(veiculo, Auditado);
        }

        public async Task AdicionarEmLoteAsync(List<VeiculoDto> dto)
        {
            var veiculo = _mapper.Map<List<Veiculo>>(dto);
            await _veiculoRepository.AdicionarEmLoteAsync(veiculo);
        }

        public async Task EditarAsync(VeiculoDto dto)
        {
            if (dto != null)
            {
                await _veiculoRepository.AtualizarAsync(_mapper.Map<Veiculo>(dto), Auditado);
            }
        }

        public async Task EditarEmLoteAsync(List<VeiculoDto> dto)
        {
            if (dto.Count > 0)
                await _veiculoRepository.AtualizarEmLoteAsync(_mapper.Map<List<Veiculo>>(dto));
        }

        public async Task RemoverAsync(long id)
        {
            var veiculo = await _veiculoRepository.ObterPorIdAsync(id);

            if (veiculo == null)
            {
                _notificationHandler.AddNotification("Veículo não encontrado", HttpStatusCode.NotFound);
                return;
            }

            await _veiculoRepository.RemoverAsync(veiculo);
        }

    }
}
