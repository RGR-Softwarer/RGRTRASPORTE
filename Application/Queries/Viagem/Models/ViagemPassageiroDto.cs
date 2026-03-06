using Dominio.Enums.Viagens;

namespace Application.Queries.Viagem.Models;

public class ViagemPassageiroDto
{
    public long Id { get; set; }
    public long ViagemId { get; set; }
    public long PassageiroId { get; set; }
    public string NomePassageiro { get; set; }
    public string CPFPassageiro { get; set; }
    public bool Ativo { get; set; }
    public StatusConfirmacaoEnum StatusConfirmacao { get; set; }
    public DateTime? DataLimiteConfirmacao { get; set; }
    public bool PassageiroFixo { get; set; }
    public string StatusConfirmacaoDescricao => StatusConfirmacao switch
    {
        StatusConfirmacaoEnum.AguardandoConfirmacao => "Aguardando Confirmação",
        StatusConfirmacaoEnum.Confirmado => "Confirmado",
        StatusConfirmacaoEnum.NaoVai => "Não Vai",
        StatusConfirmacaoEnum.Cancelado => "Cancelado",
        _ => "Desconhecido"
    };
} 
