using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class RemoverPassageiroViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long ViagemId { get; private set; }
    public long PassageiroId { get; private set; }

    public RemoverPassageiroViagemCommand(
        long viagemId,
        long passageiroId)
    {
        ViagemId = viagemId;
        PassageiroId = passageiroId;
    }
} 