using Dominio.Dtos.Viagens;

namespace Dominio.Interfaces.Hangfire
{
    public interface IProcessadorDeEventoService
    {
        Task ProcessarViagemCriada(ViagemCriadaJobData data);
        Task ProcessarViagemAtualizada(ViagemJobDataBase data);
        Task ProcessarViagemRemovida(ViagemJobDataBase data);
        Task ProcessarViagemCancelada(ViagemJobDataBase data);
        Task ProcessarViagemIniciada(ViagemJobDataBase data);
        Task ProcessarViagemFinalizada(ViagemJobDataBase data);
    }
}
