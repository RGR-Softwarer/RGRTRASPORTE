using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class IniciarViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public IniciarViagemCommand(
        long id)
    {
        Id = id;
    }
} 