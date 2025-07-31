using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Veiculo;

public class RemoverVeiculoCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public RemoverVeiculoCommand(long id)
    {
        Id = id;
    }
} 
