using Application.Common;
using Application.Queries.Veiculo.Models;
using Dominio.Interfaces.Infra.Data.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Veiculo
{
    public class ObterVeiculosQueryHandler : IRequestHandler<ObterVeiculosQuery, BaseResponse<IEnumerable<VeiculoDto>>>
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly ILogger<ObterVeiculosQueryHandler> _logger;

        public ObterVeiculosQueryHandler(
            IVeiculoRepository veiculoRepository,
            ILogger<ObterVeiculosQueryHandler> logger)
        {
            _veiculoRepository = veiculoRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<IEnumerable<VeiculoDto>>> Handle(ObterVeiculosQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando veículos com filtros - Placa: {Placa}, Modelo: {Modelo}, Ativo: {Ativo}", 
                    request.Placa, request.Modelo, request.Ativo);

                var veiculos = await _veiculoRepository.ObterTodosAsync(cancellationToken);

                var veiculosFiltrados = veiculos.AsQueryable();

                if (!string.IsNullOrEmpty(request.Placa))
                    veiculosFiltrados = veiculosFiltrados.Where(v => v.Placa.ToLower().Contains(request.Placa.ToLower()));

                if (!string.IsNullOrEmpty(request.Modelo))
                    veiculosFiltrados = veiculosFiltrados.Where(v => v.Modelo.ToLower().Contains(request.Modelo.ToLower()));

                if (request.Ativo.HasValue)
                {
                    // Assumindo que Status ativo é Disponivel ou Alugado (não Inativo)
                    if (request.Ativo.Value)
                        veiculosFiltrados = veiculosFiltrados.Where(v => v.Status != Dominio.Enums.Veiculo.StatusVeiculoEnum.Inativo);
                    else
                        veiculosFiltrados = veiculosFiltrados.Where(v => v.Status == Dominio.Enums.Veiculo.StatusVeiculoEnum.Inativo);
                }

                var veiculosDto = veiculosFiltrados.Select(v => new VeiculoDto
                {
                    Id = v.Id, // Agora ambos são long
                    Placa = v.Placa,
                    Modelo = v.Modelo,
                    Ano = v.AnoModelo,
                    Capacidade = 0, // Não há propriedade equivalente na entidade, usando 0 como padrão
                    Cor = v.Cor,
                    Chassi = v.NumeroChassi,
                    Renavam = v.Renavam,
                    Ativo = v.Status != Dominio.Enums.Veiculo.StatusVeiculoEnum.Inativo,
                    DataCadastro = v.CreatedAt
                }).ToList();

                _logger.LogInformation("Veículos encontrados com sucesso. Total: {Total}", veiculosDto.Count);

                return BaseResponse<IEnumerable<VeiculoDto>>.Ok(veiculosDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar veículos");
                return BaseResponse<IEnumerable<VeiculoDto>>.Erro("Erro ao buscar veículos", new List<string> { ex.Message });
            }
        }
    }
} 