using Application.Common;
using Application.Queries.Viagem.Models;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;
using ViagemPassageiroEntity = Dominio.Entidades.Viagens.ViagemPassageiro;

namespace Application.Queries.Viagem.ViagemPassageiro
{
    public class ObterViagemPassageirosQueryHandler : IRequestHandler<ObterViagemPassageirosQuery, BaseResponse<IEnumerable<ViagemPassageiroDto>>>
    {
        private readonly IGenericRepository<ViagemPassageiroEntity> _viagemPassageiroRepository;
        private readonly IPassageiroRepository _passageiroRepository;
        private readonly ILogger<ObterViagemPassageirosQueryHandler> _logger;

        public ObterViagemPassageirosQueryHandler(
            IGenericRepository<ViagemPassageiroEntity> viagemPassageiroRepository,
            IPassageiroRepository passageiroRepository,
            ILogger<ObterViagemPassageirosQueryHandler> logger)
        {
            _viagemPassageiroRepository = viagemPassageiroRepository;
            _passageiroRepository = passageiroRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<IEnumerable<ViagemPassageiroDto>>> Handle(ObterViagemPassageirosQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando passageiros da viagem - ViagemId: {ViagemId}, NomePassageiro: {NomePassageiro}, CPFPassageiro: {CPFPassageiro}", 
                    request.ViagemId, request.NomePassageiro, request.CPFPassageiro);

                var viagemPassageiros = await _viagemPassageiroRepository.ObterTodosAsync(cancellationToken);

                var viagemPassageirosFiltrados = viagemPassageiros.AsQueryable();

                if (request.ViagemId > 0)
                    viagemPassageirosFiltrados = viagemPassageirosFiltrados.Where(vp => vp.ViagemId == request.ViagemId);

                var viagemPassageirosDto = new List<ViagemPassageiroDto>();

                foreach (var vp in viagemPassageirosFiltrados)
                {
                    var passageiro = await _passageiroRepository.ObterPorIdAsync(vp.PassageiroId);
                    
                    if (passageiro != null)
                    {
                        bool incluirPassageiro = true;

                        if (!string.IsNullOrEmpty(request.NomePassageiro))
                            incluirPassageiro &= passageiro.Nome.ToLower().Contains(request.NomePassageiro.ToLower());

                        if (!string.IsNullOrEmpty(request.CPFPassageiro))
                            incluirPassageiro &= passageiro.CPF.Numero.Equals(request.CPFPassageiro);

                        if (incluirPassageiro)
                        {
                            viagemPassageirosDto.Add(new ViagemPassageiroDto
                            {
                                Id = vp.Id,
                                ViagemId = vp.ViagemId,
                                PassageiroId = vp.PassageiroId,
                                NomePassageiro = passageiro.Nome,
                                CPFPassageiro = passageiro.CPF
                            });
                        }
                    }
                }

                _logger.LogInformation("Passageiros da viagem encontrados com sucesso. Total: {Total}", viagemPassageirosDto.Count);

                return BaseResponse<IEnumerable<ViagemPassageiroDto>>.Ok(viagemPassageirosDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar passageiros da viagem");
                return BaseResponse<IEnumerable<ViagemPassageiroDto>>.Erro("Erro ao buscar passageiros da viagem", new List<string> { ex.Message });
            }
        }
    }
} 