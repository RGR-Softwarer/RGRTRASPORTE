using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Localidade;

public class RemoverLocalidadeCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }

    public RemoverLocalidadeCommand(long id)
    {
        Id = id;
    }
}