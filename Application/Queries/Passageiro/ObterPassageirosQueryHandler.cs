using Application.Common;
using Application.Queries.Passageiro.Models;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Passageiro
{
    public class ObterPassageirosQueryHandler : IRequestHandler<ObterPassageirosQuery, BaseResponse<IEnumerable<PassageiroDto>>>
    {
        private readonly IPassageiroRepository _passageiroRepository;
        private readonly ILogger<ObterPassageirosQueryHandler> _logger;

        public ObterPassageirosQueryHandler(
            IPassageiroRepository passageiroRepository,
            ILogger<ObterPassageirosQueryHandler> logger)
        {
            _passageiroRepository = passageiroRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<IEnumerable<PassageiroDto>>> Handle(ObterPassageirosQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando passageiros com filtros - Nome: {Nome}, CPF: {CPF}, Situacao: {Situacao}", 
                    request.Nome, request.CPF, request.Situacao);

                var passageiros = await _passageiroRepository.ObterTodosAsync(cancellationToken);

                var passageirosFiltrados = passageiros.AsQueryable();

                if (!string.IsNullOrEmpty(request.Nome))
                    passageirosFiltrados = passageirosFiltrados.Where(p => p.Nome.ToLower().Contains(request.Nome.ToLower()));

                if (!string.IsNullOrEmpty(request.CPF))
                    passageirosFiltrados = passageirosFiltrados.Where(p => p.CPF.Contains(request.CPF));

                if (request.Situacao.HasValue)
                    passageirosFiltrados = passageirosFiltrados.Where(p => p.Situacao == request.Situacao.Value);

                var passageirosDto = passageirosFiltrados.Select(p => new PassageiroDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Documento = p.CPF,
                    Telefone = p.Telefone,
                    Email = p.Email,
                    Endereco = p.Observacao,
                    DataCadastro = p.CreatedAt,
                    Ativo = p.Situacao
                }).ToList();

                _logger.LogInformation("Passageiros encontrados com sucesso. Total: {Total}", passageirosDto.Count);

                return BaseResponse<IEnumerable<PassageiroDto>>.Ok(passageirosDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar passageiros");
                return BaseResponse<IEnumerable<PassageiroDto>>.Erro("Erro ao buscar passageiros", new List<string> { ex.Message });
            }
        }
    }
} 