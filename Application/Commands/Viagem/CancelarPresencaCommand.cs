using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class CancelarPresencaCommand : BaseCommand<BaseResponse<bool>>
{
    public long ViagemId { get; set; }
    public long PassageiroId { get; set; }
    public string? Motivo { get; set; }

    public CancelarPresencaCommand()
    {
        // Construtor padrão para model binding
    }

    public CancelarPresencaCommand(
        long viagemId,
        long passageiroId,
        string? motivo = null)
    {
        ViagemId = viagemId;
        PassageiroId = passageiroId;
        Motivo = motivo;
    }
}


