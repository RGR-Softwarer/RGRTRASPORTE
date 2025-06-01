using Application.Common;
using Dominio.Interfaces.Infra.Data.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Veiculo;

public class EditarVeiculosEmLoteCommandHandler : IRequestHandler<EditarVeiculosEmLoteCommand, BaseResponse<bool>>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly ILogger<EditarVeiculosEmLoteCommandHandler> _logger;

    public EditarVeiculosEmLoteCommandHandler(
        IVeiculoRepository veiculoRepository,
        ILogger<EditarVeiculosEmLoteCommandHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(EditarVeiculosEmLoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando edição em lote de {Quantidade} veículos", request.Veiculos.Count());

            var veiculos = request.Veiculos.Select(v => {
                var veiculo = new Dominio.Entidades.Veiculos.Veiculo(
                    v.Placa,
                    v.Modelo,
                    v.Marca,
                    v.NumeroChassi,
                    v.AnoModelo,
                    v.AnoFabricacao,
                    v.Cor,
                    v.Renavam,
                    v.VencimentoLicenciamento,
                    v.TipoCombustivel,
                    v.Status,
                    v.Observacao,
                    v.ModeloVeiculoId);
                
                veiculo.Id = v.Id;
                return veiculo;
            });

            await _veiculoRepository.AtualizarEmLoteAsync(veiculos.ToList());

            _logger.LogInformation("Veículos atualizados com sucesso");

            return BaseResponse<bool>.Ok(true, "Veículos atualizados com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar veículos em lote");
            return BaseResponse<bool>.Erro("Erro ao atualizar veículos em lote", new List<string> { ex.Message });
        }
    }
} 