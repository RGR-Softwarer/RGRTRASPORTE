using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class ConfirmarPresencaCommand : BaseCommand<BaseResponse<bool>>
{
    public long ViagemId { get; set; }
    public long PassageiroId { get; set; }

    public ConfirmarPresencaCommand()
    {
        // Construtor padrão para model binding
    }

    public ConfirmarPresencaCommand(
        long viagemId,
        long passageiroId)
    {
        ViagemId = viagemId;
        PassageiroId = passageiroId;
    }
}


