using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class SeedModeloVeicularCommand : BaseCommand<BaseResponse<string>>
{
    public bool ForcarRecriacao { get; set; } = false;
} 