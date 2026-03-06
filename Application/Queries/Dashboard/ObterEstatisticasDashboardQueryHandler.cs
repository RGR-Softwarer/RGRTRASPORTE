using Application.Common;
using Dominio.Enums.Viagens;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Dashboard;

public class ObterEstatisticasDashboardQueryHandler : IRequestHandler<ObterEstatisticasDashboardQuery, BaseResponse<EstatisticasDashboardDto>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly IGenericRepository<Dominio.Entidades.Veiculos.Veiculo> _veiculoRepository;
    private readonly IGenericRepository<Dominio.Entidades.Pessoas.Motorista> _motoristaRepository;
    private readonly IGenericRepository<Dominio.Entidades.Localidades.Localidade> _localidadeRepository;
    private readonly ILogger<ObterEstatisticasDashboardQueryHandler> _logger;

    public ObterEstatisticasDashboardQueryHandler(
        IViagemRepository viagemRepository,
        IPassageiroRepository passageiroRepository,
        IGenericRepository<Dominio.Entidades.Veiculos.Veiculo> veiculoRepository,
        IGenericRepository<Dominio.Entidades.Pessoas.Motorista> motoristaRepository,
        IGenericRepository<Dominio.Entidades.Localidades.Localidade> localidadeRepository,
        ILogger<ObterEstatisticasDashboardQueryHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _passageiroRepository = passageiroRepository;
        _veiculoRepository = veiculoRepository;
        _motoristaRepository = motoristaRepository;
        _localidadeRepository = localidadeRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<EstatisticasDashboardDto>> Handle(ObterEstatisticasDashboardQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando estatísticas do dashboard");

            var hoje = DateTime.Today;
            var amanha = hoje.AddDays(1);

            // Contagens de viagens
            var todasViagens = await _viagemRepository.ObterTodosAsync();
            var viagensHoje = await _viagemRepository.ObterViagensPorDataAsync(hoje, amanha);
            var viagensEmAndamento = await _viagemRepository.ObterViagensEmAndamentoAsync();
            var viagensAgendadas = await _viagemRepository.ObterViagensAgendadasAsync(hoje, amanha.AddDays(30));

            // Contagens de passageiros
            var todosPassageiros = await _passageiroRepository.ObterTodosAsync();
            var passageirosAtivos = todosPassageiros.Where(p => p.Situacao).ToList();

            // Contagens de veículos
            var todosVeiculos = await _veiculoRepository.ObterTodosAsync();
            var veiculoIdsEmViagem = viagensEmAndamento.Select(v => v.VeiculoId).Distinct().ToList();
            var veiculosDisponiveis = todosVeiculos.Where(v => v.Situacao && !veiculoIdsEmViagem.Contains(v.Id)).ToList();
            var veiculosEmViagem = todosVeiculos.Where(v => veiculoIdsEmViagem.Contains(v.Id) && v.Situacao).ToList();

            // Contagens de motoristas
            var todosMotoristas = await _motoristaRepository.ObterTodosAsync();
            var motoristasAtivos = todosMotoristas.Where(m => m.Situacao).ToList();

            // Contagens de localidades
            var todasLocalidades = await _localidadeRepository.ObterTodosAsync();

            var estatisticas = new EstatisticasDashboardDto
            {
                TotalViagens = todasViagens.Count(),
                ViagensHoje = viagensHoje.Count(),
                ViagensEmAndamento = viagensEmAndamento.Count(),
                ViagensAgendadas = viagensAgendadas.Count(),
                TotalPassageiros = todosPassageiros.Count(),
                PassageirosAtivos = passageirosAtivos.Count,
                TotalVeiculos = todosVeiculos.Count(),
                VeiculosDisponiveis = veiculosDisponiveis.Count,
                VeiculosEmViagem = veiculosEmViagem.Count,
                TotalMotoristas = todosMotoristas.Count(),
                MotoristasAtivos = motoristasAtivos.Count,
                TotalLocalidades = todasLocalidades.Count()
            };

            _logger.LogInformation("Estatísticas do dashboard obtidas com sucesso");

            return BaseResponse<EstatisticasDashboardDto>.Ok(estatisticas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar estatísticas do dashboard");
            return BaseResponse<EstatisticasDashboardDto>.Erro("Erro ao buscar estatísticas do dashboard", new List<string> { ex.Message });
        }
    }
}

