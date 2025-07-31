namespace Dominio.Interfaces.Service.Hangfire;

public interface IHangfireService
{
    Task ExecutarJobAsync(string jobName, object data);
    Task AgendarJobAsync(string jobName, object data, DateTime agendamento);
    Task CancelarJobAsync(string jobId);
} 
