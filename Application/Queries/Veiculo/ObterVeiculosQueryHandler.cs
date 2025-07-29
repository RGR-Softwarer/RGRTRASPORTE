using Application.Common;
using Application.Queries.Veiculo.Models;
using Dominio.Interfaces.Infra.Data;
using VeiculoEntity = Dominio.Entidades.Veiculos.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Veiculo
{
    public class ObterVeiculosQueryHandler : IRequestHandler<ObterVeiculosQuery, BaseResponse<IEnumerable<VeiculoDto>>>
    {
        private readonly IGenericRepository<VeiculoEntity> _veiculoRepository;
        private readonly ILogger<ObterVeiculosQueryHandler> _logger;

        public ObterVeiculosQueryHandler(
            IGenericRepository<VeiculoEntity> veiculoRepository,
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
                    veiculosFiltrados = veiculosFiltrados.Where(v => v.Placa.Numero.ToLower().Contains(request.Placa.ToLower()));

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
                    Id = v.Id,
                    Placa = v.Placa ?? "",
                    PlacaFormatada = !string.IsNullOrEmpty(v.Placa) ? FormatarPlaca(v.Placa) : "",
                    Modelo = v.Modelo ?? "",
                    Marca = v.Marca ?? "",
                    NumeroChassi = v.NumeroChassi ?? "",
                    AnoModelo = v.AnoModelo,
                    AnoFabricacao = v.AnoFabricacao,
                    Cor = v.Cor ?? "",
                    Renavam = v.Renavam ?? "",
                    VencimentoLicenciamento = v.VencimentoLicenciamento,
                    TipoCombustivel = v.TipoCombustivel,
                    TipoCombustivelDescricao = ObterDescricaoEnum(v.TipoCombustivel.ToString()),
                    Status = v.Status,
                    StatusDescricao = ObterDescricaoEnum(v.Status.ToString()),
                    Observacao = v.Observacao ?? "",
                    ModeloVeiculoId = v.ModeloVeiculoId,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt,
                    Capacidade = 0,
                    Ativo = v.Status != Dominio.Enums.Veiculo.StatusVeiculoEnum.Inativo
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

        private string FormatarPlaca(string placa)
        {
            if (string.IsNullOrEmpty(placa))
                return string.Empty;

            // Remove qualquer formatação existente
            var placaLimpa = placa.Replace("-", "").Replace(" ", "").ToUpper();

            // Verifica se tem o tamanho correto (7 caracteres)
            if (placaLimpa.Length != 7)
                return placa; // Retorna original se não estiver no formato esperado

            // Formato brasileiro: ABC-1234 ou ABC1D23 (Mercosul)
            if (char.IsLetter(placaLimpa[4])) // Mercosul: ABC1D23
                return $"{placaLimpa.Substring(0, 3)}{placaLimpa.Substring(3, 1)}{placaLimpa.Substring(4, 1)}{placaLimpa.Substring(5, 2)}";
            else // Formato antigo: ABC-1234
                return $"{placaLimpa.Substring(0, 3)}-{placaLimpa.Substring(3, 4)}";
        }

        private string ObterDescricaoEnum(string valor)
        {
            // Implemente a lógica para obter a descrição do enum com base no valor fornecido
            return valor switch
            {
                "Disponivel" => "Disponível",
                "Indisponivel" => "Indisponível",
                "EmManutencao" => "Em Manutenção",
                "Manutencao" => "Manutenção",
                "EmUso" => "Em Uso",
                "Inativo" => "Inativo",
                "Gasolina" => "Gasolina",
                "Etanol" => "Etanol",
                "Diesel" => "Diesel",
                "GNV" => "Gás Natural Veicular",
                "Flex" => "Flex (Gasolina/Etanol)",
                _ => valor
            };
        }
    }
} 