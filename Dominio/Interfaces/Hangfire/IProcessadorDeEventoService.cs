namespace Dominio.Interfaces.Hangfire
{
    public interface IProcessadorDeEventoService
    {
        Task ProcessarViagemCriada(long viagemId);
    }

}
