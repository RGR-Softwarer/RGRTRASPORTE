using Application.Common;
using Dominio.Interfaces.Infra.Data;
using LocalidadeEntity = Dominio.Entidades.Localidades.Localidade;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Localidade;

public class RemoverLocalidadeCommandHandler : IRequestHandler<RemoverLocalidadeCommand, BaseResponse<bool>>
{
    private readonly IGenericRepository<LocalidadeEntity> _localidadeRepository;
    private readonly ILogger<RemoverLocalidadeCommandHandler> _logger;

    public RemoverLocalidadeCommandHandler(
        IGenericRepository<LocalidadeEntity> localidadeRepository,
        ILogger<RemoverLocalidadeCommandHandler> logger)
    {
        _localidadeRepository = localidadeRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverLocalidadeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando remoção da localidade {LocalidadeId}", request.Id);

            var localidade = await _localidadeRepository.ObterPorIdAsync(request.Id);
            if (localidade == null)
                return BaseResponse<bool>.Erro("Localidade não encontrada");

            await _localidadeRepository.RemoverAsync(localidade);

            _logger.LogInformation("Localidade {LocalidadeId} removida com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Localidade removida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover localidade {LocalidadeId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao remover localidade", new List<string> { ex.Message });
        }
    }
} 