using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Enums.Data;

namespace Dominio.Interfaces.Infra.Data.Viagens.Gatilho
{
    public interface IGatilhoViagemRepository : IGenericRepository<GatilhoViagem>
    {
        Task<GatilhoViagem> ObterGatilhoCompletoPorIdAsync(long id);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorVeiculoAsync(long veiculoId);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorMotoristaAsync(long motoristaId);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorLocalidadeAsync(long localidadeId, bool origem = true);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorDiaSemanaAsync(DiaSemanaEnum diaSemana);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorHorarioAsync(TimeSpan horarioInicio, TimeSpan horarioFim);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorValorPassagemAsync(decimal valorMinimo, decimal valorMaximo);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorQuantidadeVagasAsync(int quantidadeMinima, int quantidadeMaxima);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorDistanciaAsync(decimal distanciaMinima, decimal distanciaMaxima);
        Task<IEnumerable<GatilhoViagem>> ObterGatilhosPorAtivoAsync(bool ativo);
        Task<bool> ExisteGatilhoConflitanteAsync(TimeSpan horarioSaida, TimeSpan horarioChegada, long veiculoId, long? gatilhoIdExcluir = null);
        Task<bool> ExisteGatilhoAgendadoParaVeiculo(long veiculoId, DiaSemanaEnum diaSemana);
        Task<bool> ExisteGatilhoAgendadoParaMotorista(long motoristaId, DiaSemanaEnum diaSemana);
    }
}