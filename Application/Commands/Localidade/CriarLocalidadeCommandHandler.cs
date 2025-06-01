using Application.Common;
using Dominio.Interfaces.Infra.Data.Localidades;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Localidade;

public class CriarLocalidadeCommandHandler : IRequestHandler<CriarLocalidadeCommand, BaseResponse<long>>
{
    private readonly ILocalidadeRepository _localidadeRepository;
    private readonly ILogger<CriarLocalidadeCommandHandler> _logger;

    public CriarLocalidadeCommandHandler(
        ILocalidadeRepository localidadeRepository,
        ILogger<CriarLocalidadeCommandHandler> logger)
    {
        _localidadeRepository = localidadeRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<long>> Handle(CriarLocalidadeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando criação da localidade {Nome}", request.Nome);

            var localidade = new Dominio.Entidades.Localidades.Localidade(
                request.Nome,
                request.Estado,
                request.Cidade,
                request.Cep,
                request.Bairro,
                request.Logradouro,
                request.Numero,
                request.Complemento,
                request.Latitude,
                request.Longitude);

            await _localidadeRepository.AdicionarAsync(localidade);

            _logger.LogInformation("Localidade {Nome} adicionada ao contexto com sucesso", request.Nome);

            return BaseResponse<long>.Ok(localidade.Id, "Localidade criada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar localidade {Nome}", request.Nome);
            return BaseResponse<long>.Erro("Erro ao criar localidade", new List<string> { ex.Message });
        }
    }
} 