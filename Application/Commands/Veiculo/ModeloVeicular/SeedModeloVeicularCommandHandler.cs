using Application.Common;
using Dominio.Entidades.Veiculos;
using Dominio.Enums.Veiculo;
using Dominio.Interfaces.Infra.Data.Veiculo;
using MediatR;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class SeedModeloVeicularCommandHandler : IRequestHandler<SeedModeloVeicularCommand, BaseResponse<string>>
{
    private readonly IModeloVeicularRepository _modeloVeicularRepository;

    public SeedModeloVeicularCommandHandler(IModeloVeicularRepository modeloVeicularRepository)
    {
        _modeloVeicularRepository = modeloVeicularRepository;
    }

    public async Task<BaseResponse<string>> Handle(SeedModeloVeicularCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verifica se já existem dados
            var existingModels = await _modeloVeicularRepository.ObterTodosAsync();
            if (existingModels.Any() && !request.ForcarRecriacao)
            {
                return BaseResponse<string>.Ok("Dados já existem. Use ForcarRecriacao=true para recriar.");
            }

            // Se forçar recreação, remove todos os dados existentes
            if (request.ForcarRecriacao && existingModels.Any())
            {
                foreach (var model in existingModels)
                {
                    await _modeloVeicularRepository.RemoverAsync(model);
                }
            }

            // Cria os dados de teste
            var modelosVeiculares = new List<Dominio.Entidades.Veiculos.ModeloVeicular>
            {
                // Ônibus
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Ônibus Urbano Standard",
                    TipoModeloVeiculoEnum.Onibus,
                    50, // quantidadeAssento
                    2,  // quantidadeEixo
                    80, // capacidadeMaxima
                    30, // passageirosEmPe
                    true, // possuiBanheiro
                    true, // possuiClimatizador
                    true  // situacao
                ),
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Ônibus Urbano Articulado",
                    TipoModeloVeiculoEnum.Onibus,
                    90, // quantidadeAssento
                    3,  // quantidadeEixo
                    150, // capacidadeMaxima
                    60, // passageirosEmPe
                    true, // possuiBanheiro
                    true, // possuiClimatizador
                    true  // situacao
                ),
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Ônibus Rodoviário Executivo",
                    TipoModeloVeiculoEnum.Onibus,
                    42, // quantidadeAssento
                    2,  // quantidadeEixo
                    42, // capacidadeMaxima
                    0,  // passageirosEmPe
                    true, // possuiBanheiro
                    true, // possuiClimatizador
                    true  // situacao
                ),
                
                // Microônibus
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Microônibus 20 Lugares",
                    TipoModeloVeiculoEnum.MicroOnibus,
                    20, // quantidadeAssento
                    2,  // quantidadeEixo
                    25, // capacidadeMaxima
                    5,  // passageirosEmPe
                    false, // possuiBanheiro
                    true,  // possuiClimatizador
                    true   // situacao
                ),
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Microônibus Escolar",
                    TipoModeloVeiculoEnum.MicroOnibus,
                    24, // quantidadeAssento
                    2,  // quantidadeEixo
                    24, // capacidadeMaxima
                    0,  // passageirosEmPe
                    false, // possuiBanheiro
                    false, // possuiClimatizador
                    true   // situacao
                ),
                
                // Van
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Van 15 Lugares",
                    TipoModeloVeiculoEnum.Van,
                    15, // quantidadeAssento
                    2,  // quantidadeEixo
                    15, // capacidadeMaxima
                    0,  // passageirosEmPe
                    false, // possuiBanheiro
                    true,  // possuiClimatizador
                    true   // situacao
                ),
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Van Executiva",
                    TipoModeloVeiculoEnum.Van,
                    8,  // quantidadeAssento
                    2,  // quantidadeEixo
                    8,  // capacidadeMaxima
                    0,  // passageirosEmPe
                    false, // possuiBanheiro
                    true,  // possuiClimatizador
                    true   // situacao
                ),
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Van 12 Lugares",
                    TipoModeloVeiculoEnum.Van,
                    12, // quantidadeAssento
                    2,  // quantidadeEixo
                    12, // capacidadeMaxima
                    0,  // passageirosEmPe
                    false, // possuiBanheiro
                    true,  // possuiClimatizador
                    true   // situacao
                ),
                
                // Carro
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Sedan 5 Lugares",
                    TipoModeloVeiculoEnum.Carro,
                    5,  // quantidadeAssento
                    2,  // quantidadeEixo
                    5,  // capacidadeMaxima
                    0,  // passageirosEmPe
                    false, // possuiBanheiro
                    true,  // possuiClimatizador
                    true   // situacao
                ),
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "SUV 7 Lugares",
                    TipoModeloVeiculoEnum.Carro,
                    7,  // quantidadeAssento
                    2,  // quantidadeEixo
                    7,  // capacidadeMaxima
                    0,  // passageirosEmPe
                    false, // possuiBanheiro
                    true,  // possuiClimatizador
                    true   // situacao
                ),
                
                // Modelo descontinuado para teste
                new Dominio.Entidades.Veiculos.ModeloVeicular(
                    "Hatch 5 Lugares (Descontinuado)",
                    TipoModeloVeiculoEnum.Carro,
                    5,  // quantidadeAssento
                    2,  // quantidadeEixo
                    5,  // capacidadeMaxima
                    0,  // passageirosEmPe
                    false, // possuiBanheiro
                    false, // possuiClimatizador
                    false  // situacao - INATIVO
                )
            };

            // Insere os dados
            foreach (var modelo in modelosVeiculares)
            {
                await _modeloVeicularRepository.AdicionarAsync(modelo);
            }

            return BaseResponse<string>.Ok($"✅ {modelosVeiculares.Count} modelos veiculares criados com sucesso!");
        }
        catch (Exception ex)
        {
            return BaseResponse<string>.Erro($"Erro ao criar dados de teste: {ex.Message}");
        }
    }
} 