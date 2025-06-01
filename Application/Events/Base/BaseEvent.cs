using MediatR;

namespace Application.Events.Base;

public abstract class BaseEvent : INotification
{
    public DateTime DataOcorrencia { get; private set; }
    public string Usuario { get; private set; }

    protected BaseEvent()
    {
        DataOcorrencia = DateTime.UtcNow;
        Usuario = "Sistema"; // Pode ser substituído pelo usuário real da requisição
    }
} 