using Dominio.Interfaces.Service.Hangfire;

namespace Service.Services.Hangifre;

public class HangfireService : IHangfireService
{
    public Task ExecutarJobAsync(string jobName, object data)
    {
        // Implementação básica do Hangfire
        return Task.CompletedTask;
    }

    public Task AgendarJobAsync(string jobName, object data, DateTime agendamento)
    {
        // Implementação básica do Hangfire
        return Task.CompletedTask;
    }

    public Task CancelarJobAsync(string jobId)
    {
        // Implementação básica do Hangfire
        return Task.CompletedTask;
    }
} 