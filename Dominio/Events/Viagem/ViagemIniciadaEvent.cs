using MediatR;

namespace Dominio.Events.Viagem;

public class ViagemIniciadaEvent : INotification
{
    public long ViagemId { get; }
    public string UsuarioOperacao { get; }

    public ViagemIniciadaEvent(long viagemId, string usuarioOperacao)
    {
        ViagemId = viagemId;
        UsuarioOperacao = usuarioOperacao;
    }
} 