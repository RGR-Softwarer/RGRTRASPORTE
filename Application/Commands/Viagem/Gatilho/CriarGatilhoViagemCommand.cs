using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem.Gatilho;

public class CriarGatilhoViagemCommand : BaseCommand<BaseResponse<long>>
{
    public long ViagemId { get; private set; }
    public string Descricao { get; private set; }

    public CriarGatilhoViagemCommand(
        long viagemId,
        string descricao,
        bool ativo)
    {
        ViagemId = viagemId;
        Descricao = descricao;
    }
} 
