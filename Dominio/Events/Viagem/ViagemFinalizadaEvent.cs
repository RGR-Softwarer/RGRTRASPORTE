using MediatR;

namespace Dominio.Events.Viagem;

public class ViagemFinalizadaEvent : INotification
{
    public long ViagemId { get; }
    public string UsuarioOperacao { get; }

    public ViagemFinalizadaEvent(long viagemId, string usuarioOperacao)
    {
        ViagemId = viagemId;
        UsuarioOperacao = usuarioOperacao;
    }
} 