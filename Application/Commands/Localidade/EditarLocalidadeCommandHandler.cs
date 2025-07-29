using Application.Common;
using Dominio.Interfaces.Infra.Data;
using LocalidadeEntity = Dominio.Entidades.Localidades.Localidade;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Localidade;

public class EditarLocalidadeCommandHandler : IRequestHandler<EditarLocalidadeCommand, BaseResponse<bool>>
{
    private readonly IGenericRepository<LocalidadeEntity> _localidadeRepository;
    private readonly ILogger<EditarLocalidadeCommandHandler> _logger;

    public EditarLocalidadeCommandHandler(
        IGenericRepository<LocalidadeEntity> localidadeRepository,
        ILogger<EditarLocalidadeCommandHandler> logger)
    {
        _localidadeRepository = localidadeRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(EditarLocalidadeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando edição da localidade {LocalidadeId}", request.Id);

            var localidade = await _localidadeRepository.ObterPorIdAsync(request.Id);
            if (localidade == null)
                return BaseResponse<bool>.Erro("Localidade não encontrada");

            var endereco = new Dominio.ValueObjects.Endereco(
                request.Estado,
                request.Cidade,
                request.Cep,
                request.Bairro,
                request.Logradouro,
                request.Numero,
                request.Complemento);

            localidade.Atualizar(
                request.Nome,
                endereco,
                request.Latitude,
                request.Longitude,
                request.Ativo);

            await _localidadeRepository.AtualizarAsync(localidade);

            _logger.LogInformation("Localidade {LocalidadeId} atualizada com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Localidade atualizada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao editar localidade {LocalidadeId}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao editar localidade", new List<string> { ex.Message });
        }
    }
} 