using MediatR;

namespace Application.Commands.Base;

public abstract class BaseCommand<TResponse> : IRequest<TResponse>
{
    public DateTime DataCriacao { get; private set; }
    public string UsuarioCriacao { get; private set; }
    public string UsuarioId { get; private set; }

    protected BaseCommand()
    {
        DataCriacao = DateTime.UtcNow;
        UsuarioCriacao = "Sistema"; // Pode ser substituído pelo usuário real da requisição
        UsuarioId = "0";
    }

    protected BaseCommand(string usuarioId, string usuarioCriacao)
    {
        DataCriacao = DateTime.UtcNow;
        UsuarioCriacao = string.IsNullOrEmpty(usuarioCriacao) ? "Sistema" : usuarioCriacao;
        UsuarioId = string.IsNullOrEmpty(usuarioId) ? "1" : usuarioId;
    }
}