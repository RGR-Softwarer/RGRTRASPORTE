using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class AdicionarPassageiroViagemApiCommand : BaseCommand<BaseResponse<bool>>
{
    public long ViagemId { get; set; }
    public long PassageiroId { get; set; }

    public AdicionarPassageiroViagemApiCommand()
    {
        // Construtor padrão para model binding
    }

    public AdicionarPassageiroViagemApiCommand(long viagemId, long passageiroId)
    {
        ViagemId = viagemId;
        PassageiroId = passageiroId;
    }
} 
