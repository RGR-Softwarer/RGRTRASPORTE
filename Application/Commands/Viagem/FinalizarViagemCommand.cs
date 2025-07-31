using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class FinalizarViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public FinalizarViagemCommand(
        long id)
    {
        Id = id;
    }
} 
