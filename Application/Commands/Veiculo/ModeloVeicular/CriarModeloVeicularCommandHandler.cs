using Application.Common;
using Dominio.Interfaces.Infra.Data;
using ModeloVeicularEntity = Dominio.Entidades.Veiculos.ModeloVeicular;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class CriarModeloVeicularCommandHandler : IRequestHandler<CriarModeloVeicularCommand, BaseResponse<long>>
{
    private readonly IGenericRepository<ModeloVeicularEntity> _modeloVeicularRepository;
    private readonly ILogger<CriarModeloVeicularCommandHandler> _logger;

    public CriarModeloVeicularCommandHandler(
        IGenericRepository<ModeloVeicularEntity> modeloVeicularRepository,
        ILogger<CriarModeloVeicularCommandHandler> logger)
    {
        _modeloVeicularRepository = modeloVeicularRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<long>> Handle(CriarModeloVeicularCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando criação do modelo veicular {Nome}", request.Descricao);

            var modeloVeicular = new Dominio.Entidades.Veiculos.ModeloVeicular(
                request.Descricao,
                Dominio.Enums.Veiculo.TipoModeloVeiculoEnum.Onibus,
                request.QuantidadeAssento,
                request.QuantidadeEixo,
                request.CapacidadeMaxima,
                request.PassageirosEmPe,
                request.PossuiBanheiro,
                request.PossuiClimatizador,
                request.Situacao);

            await _modeloVeicularRepository.AdicionarAsync(modeloVeicular);

            _logger.LogInformation("Modelo veicular {Nome} adicionado ao contexto com sucesso", request.Descricao);

            return BaseResponse<long>.Ok(modeloVeicular.Id, "Modelo veicular criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar modelo veicular {Nome}", request.Descricao);
            return BaseResponse<long>.Erro("Erro ao criar modelo veicular", new List<string> { ex.Message });
        }
    }
} 