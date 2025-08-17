using MediatR;
using Application.Common;
using Dominio.Interfaces.Infra.Data;
using Microsoft.Extensions.Logging;
using Dominio.Enums.Veiculo;
using Dominio.Enums.Data;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class SeedModeloVeicularCommandHandler : IRequestHandler<SeedModeloVeicularCommand, BaseResponse<string>>
{
    private readonly IGenericRepository<Dominio.Entidades.Veiculos.ModeloVeicular> _modeloVeicularRepository;
    private readonly ILogger<SeedModeloVeicularCommandHandler> _logger;

    public SeedModeloVeicularCommandHandler(
        IGenericRepository<Dominio.Entidades.Veiculos.ModeloVeicular> modeloVeicularRepository,
        ILogger<SeedModeloVeicularCommandHandler> logger)
    {
        _modeloVeicularRepository = modeloVeicularRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<string>> Handle(SeedModeloVeicularCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando seed de modelos veiculares");

            // Remove dados existentes se solicitado
            var modelosExistentes = await _modeloVeicularRepository.ObterTodosAsync();
            if (modelosExistentes.Any())
            {
                foreach (var model in modelosExistentes)
                {
                    await _modeloVeicularRepository.RemoverAsync(model);
                }
            }

            // Cria os dados de teste usando Factory Methods
            var modelosVeiculares = new List<Dominio.Entidades.Veiculos.ModeloVeicular>();

            // Ônibus
            var onibusStandard = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Ônibus Urbano Standard",
                TipoModeloVeiculoEnum.Onibus,
                50, 2, 80, 30, true, true);
            if (onibusStandard != null) modelosVeiculares.Add(onibusStandard);

            var onibusArticulado = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Ônibus Urbano Articulado",
                TipoModeloVeiculoEnum.Onibus,
                90, 3, 150, 60, true, true);
            if (onibusArticulado != null) modelosVeiculares.Add(onibusArticulado);

            var onibusExecutivo = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Ônibus Rodoviário Executivo",
                TipoModeloVeiculoEnum.Onibus,
                42, 2, 42, 0, true, true);
            if (onibusExecutivo != null) modelosVeiculares.Add(onibusExecutivo);

            // Microônibus
            var microOnibus20 = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Microônibus 20 Lugares",
                TipoModeloVeiculoEnum.MicroOnibus,
                20, 2, 25, 5, false, true);
            if (microOnibus20 != null) modelosVeiculares.Add(microOnibus20);

            var microOnibusEscolar = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Microônibus Escolar",
                TipoModeloVeiculoEnum.MicroOnibus,
                24, 2, 24, 0, false, false);
            if (microOnibusEscolar != null) modelosVeiculares.Add(microOnibusEscolar);

            // Van
            var van15 = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Van 15 Lugares",
                TipoModeloVeiculoEnum.Van,
                15, 2, 15, 0, false, true);
            if (van15 != null) modelosVeiculares.Add(van15);

            var vanExecutiva = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Van Executiva",
                TipoModeloVeiculoEnum.Van,
                8, 2, 8, 0, false, true);
            if (vanExecutiva != null) modelosVeiculares.Add(vanExecutiva);

            var van12 = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Van 12 Lugares",
                TipoModeloVeiculoEnum.Van,
                12, 2, 12, 0, false, true);
            if (van12 != null) modelosVeiculares.Add(van12);

            // Carro
            var sedan = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Sedan 5 Lugares",
                TipoModeloVeiculoEnum.Carro,
                5, 2, 5, 0, false, true);
            if (sedan != null) modelosVeiculares.Add(sedan);

            var suv = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "SUV 7 Lugares",
                TipoModeloVeiculoEnum.Carro,
                7, 2, 7, 0, false, true);
            if (suv != null) modelosVeiculares.Add(suv);

            // Modelo descontinuado para teste
            var hatchDescontinuado = Dominio.Entidades.Veiculos.ModeloVeicular.CriarModeloVeicular(
                "Hatch 5 Lugares (Descontinuado)",
                TipoModeloVeiculoEnum.Carro,
                5, 2, 5, 0, false, false);
            if (hatchDescontinuado != null) 
            {
                hatchDescontinuado.Inativar(); // Inativa após a criação
                modelosVeiculares.Add(hatchDescontinuado);
            }

            // Adiciona todos os modelos criados
            foreach (var modelo in modelosVeiculares)
            {
                await _modeloVeicularRepository.AdicionarAsync(modelo);
            }

            _logger.LogInformation("Seed de modelos veiculares concluído. {Count} modelos criados", modelosVeiculares.Count);

            return BaseResponse<string>.Ok($"Seed concluído com sucesso. {modelosVeiculares.Count} modelos veiculares criados.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar seed de modelos veiculares");
            return BaseResponse<string>.Erro("Erro ao executar seed", new List<string> { ex.Message });
        }
    }
}