using Dominio.Entidades.Auditoria;
using Dominio.Entidades;
using Dominio.Enums.Auditoria;

namespace Dominio.Services
{
    public interface IAuditoriaService
    {
        Task RegistrarAlteracaoAsync(
            string nomeEntidade,
            long entidadeId,
            AcaoBancoDadosEnum acao,
            object dadosOriginais,
            object dadosNovos,
            string usuario,
            string ip,
            CancellationToken cancellationToken = default);

        Task RegistrarEventoAsync(
            string evento,
            long entidadeId,
            string dadosEvento,
            string usuario,
            string ip,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<RegistroAuditoria>> ObterHistoricoAsync(
            string nomeEntidade,
            long entidadeId,
            CancellationToken cancellationToken = default);
    }
} 
