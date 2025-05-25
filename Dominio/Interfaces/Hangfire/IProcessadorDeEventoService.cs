using Dominio.Dtos.Viagens;

namespace Dominio.Interfaces.Hangfire
{
    public interface IProcessadorDeEventoService
    {
        Task ProcessarViagemCriada(ViagemCriadaJobData data);
    }

}
