using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class AdicionarPassageiroViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long ViagemId { get; private set; }
    public long PassageiroId { get; private set; }

    public AdicionarPassageiroViagemCommand(
        long viagemId,
        long passageiroId)
    {
        ViagemId = viagemId;
        PassageiroId = passageiroId;
    }
} 