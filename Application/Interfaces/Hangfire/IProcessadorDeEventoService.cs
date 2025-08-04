using Application.Dtos.Viagens;

namespace Application.Interfaces.Hangfire
{
    public interface IProcessadorDeEventoService
    {
        Task ProcessarViagemAtualizada(ViagemJobDataBase data);
        Task ProcessarViagemRemovida(ViagemJobDataBase data);
        Task ProcessarViagemCancelada(ViagemJobDataBase data);
        Task ProcessarViagemIniciada(ViagemJobDataBase data);
        Task ProcessarViagemFinalizada(ViagemJobDataBase data);
    }
} 