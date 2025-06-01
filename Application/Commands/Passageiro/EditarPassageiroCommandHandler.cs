using Application.Common;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Passageiro;

public class EditarPassageiroCommandHandler : IRequestHandler<EditarPassageiroCommand, BaseResponse<bool>>
{
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly ILogger<EditarPassageiroCommandHandler> _logger;

    public EditarPassageiroCommandHandler(
        IPassageiroRepository passageiroRepository,
        ILogger<EditarPassageiroCommandHandler> logger)
    {
        _passageiroRepository = passageiroRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(EditarPassageiroCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando edição do passageiro {PassageiroId}", request.Id);

            var passageiro = await _passageiroRepository.ObterPorIdAsync(request.Id);
            if (passageiro == null)
                return BaseResponse<bool>.Erro("Passageiro não encontrado");

            passageiro.Atualizar(
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

            await _passageiroRepository.AtualizarAsync(passageiro);

            _logger.LogInformation("Passageiro {PassageiroId} atualizado com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Passageiro atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao editar passageiro {PassageiroId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao editar passageiro", new List<string> { ex.Message });
        }
    }
} 