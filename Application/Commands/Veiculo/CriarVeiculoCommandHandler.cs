using Application.Common;
using Dominio.Interfaces.Infra.Data;
using VeiculoEntity = Dominio.Entidades.Veiculos.Veiculo;
using Dominio.ValueObjects;
using MediatR;

namespace Application.Commands.Veiculo
{
    public class CriarVeiculoCommandHandler : IRequestHandler<CriarVeiculoCommand, BaseResponse<long>>
    {
        private readonly IGenericRepository<VeiculoEntity> _veiculoRepository;

        public CriarVeiculoCommandHandler(
            IGenericRepository<VeiculoEntity> veiculoRepository)
        {
            _veiculoRepository = veiculoRepository;
        }

        public async Task<BaseResponse<long>> Handle(CriarVeiculoCommand request, CancellationToken cancellationToken)
        {
            var placa = new Placa(request.Placa);
            
            var veiculo = Dominio.Entidades.Veiculos.Veiculo.CriarVeiculo(
                placa,
                request.Modelo,
                request.Marca,
                request.NumeroChassi,
                request.AnoModelo,
                request.AnoFabricacao,
                request.Cor,
                request.Renavam,
                request.VencimentoLicenciamento,
                request.TipoCombustivel,
                request.Status,
                request.Observacao,
                request.ModeloVeiculoId);

            await _veiculoRepository.AdicionarAsync(veiculo);
            return BaseResponse<long>.Ok(veiculo.Id);
        }
    }
} 
