using Application.Common;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Passageiro;

public class CriarPassageiroCommandHandler : IRequestHandler<CriarPassageiroCommand, BaseResponse<long>>
{
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly ILogger<CriarPassageiroCommandHandler> _logger;

    public CriarPassageiroCommandHandler(
        IPassageiroRepository passageiroRepository,
        ILogger<CriarPassageiroCommandHandler> logger)
    {
        _passageiroRepository = passageiroRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<long>> Handle(CriarPassageiroCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando criação do passageiro {Nome}", request.Nome);

            var passageiro = new Dominio.Entidades.Pessoas.Passageiros.Passageiro(
                request.Nome,
                request.CPF,
                request.Telefone,
                request.Email,
                request.Sexo,
                request.LocalidadeId,
                request.LocalidadeEmbarqueId,
                request.LocalidadeDesembarqueId,
                request.Observacao,
                request.Situacao);

            await _passageiroRepository.AdicionarAsync(passageiro);

            _logger.LogInformation("Passageiro {Nome} criado com sucesso. ID: {PassageiroId}", request.Nome, passageiro.Id);

            return BaseResponse<long>.Ok(passageiro.Id, "Passageiro criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar passageiro {Nome}", request.Nome);
            return BaseResponse<long>.Erro("Erro ao criar passageiro", new List<string> { ex.Message });
        }
    }
} 
