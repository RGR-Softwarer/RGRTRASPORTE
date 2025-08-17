using Application.Common;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;
using Infra.CrossCutting.Handlers.Notifications;
using Dominio.Interfaces;
using PassageiroEntity = Dominio.Entidades.Pessoas.Passageiros.Passageiro;

namespace Application.Commands.Passageiro;

public class CriarPassageiroCommandHandler : IRequestHandler<CriarPassageiroCommand, BaseResponse<long>>
{
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly ILogger<CriarPassageiroCommandHandler> _logger;
    private readonly INotificationContext _notificationContext;

    public CriarPassageiroCommandHandler(
        IPassageiroRepository passageiroRepository,
        ILogger<CriarPassageiroCommandHandler> logger,
        INotificationContext notificationContext)
    {
        _passageiroRepository = passageiroRepository;
        _logger = logger;
        _notificationContext = notificationContext;
    }

    public async Task<BaseResponse<long>> Handle(CriarPassageiroCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando criação do passageiro {Nome}", request.Nome);

            // Criar adapter para converter interface
            var domainNotificationContext = new DomainNotificationContextAdapter(_notificationContext);

            // Usar Factory Method com validação
            var (passageiro, sucesso) = PassageiroEntity.CriarPassageiroComValidacao(
                request.Nome,
                request.CPF,
                request.Telefone,
                request.Email,
                request.Sexo,
                request.LocalidadeId,
                request.LocalidadeEmbarqueId,
                request.LocalidadeDesembarqueId,
                request.Observacao,
                domainNotificationContext);

            if (!sucesso || passageiro == null)
            {
                _logger.LogWarning("Falha na validação do passageiro {Nome}. Total de erros: {Count}", 
                    request.Nome, domainNotificationContext.GetNotificationCount());
                return BaseResponse<long>.Erro("Dados inválidos para criação do passageiro", new List<string> { "Falha na validação dos dados" });
            }

            await _passageiroRepository.AdicionarAsync(passageiro);

            _logger.LogInformation("Passageiro {Nome} criado com sucesso. ID: {PassageiroId}", 
                request.Nome, passageiro.Id);

            return BaseResponse<long>.Ok(passageiro.Id, "Passageiro criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar passageiro {Nome}", request.Nome);
            return BaseResponse<long>.Erro("Erro ao criar passageiro", new List<string> { ex.Message });
        }
    }
} 
