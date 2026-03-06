using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Enums.Viagens;
using Dominio.Exceptions;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPassageiro : BaseEntity
    {
        public long ViagemId { get; private set; }
        public long PassageiroId { get; private set; }
        public DateTime DataReserva { get; private set; }
        public bool Confirmado { get; private set; }
        public DateTime? DataConfirmacao { get; private set; }
        public string? Observacao { get; private set; }
        public StatusConfirmacaoEnum StatusConfirmacao { get; private set; }
        public DateTime? DataLimiteConfirmacao { get; private set; }
        public bool PassageiroFixo { get; private set; }

        // Navega��o
        public Viagem Viagem { get; private set; }
        public Passageiro Passageiro { get; private set; }

        private ViagemPassageiro() { } // Para EF

        public ViagemPassageiro(Viagem viagem, long passageiroId, bool passageiroFixo = false)
        {
            ValidarCriacao(viagem, passageiroId);

            ViagemId = viagem.Id;
            Viagem = viagem;
            PassageiroId = passageiroId;
            DataReserva = DateTime.UtcNow;
            Confirmado = false;
            StatusConfirmacao = StatusConfirmacaoEnum.AguardandoConfirmacao;
            PassageiroFixo = passageiroFixo;
            
            // Define data limite de confirmacao como 20h do dia anterior
            var dataViagem = viagem.Periodo.Data.Date;
            DataLimiteConfirmacao = dataViagem.AddDays(-1).AddHours(20);
        }

        private void ValidarCriacao(Viagem viagem, long passageiroId)
        {
            if (viagem == null)
                throw new DomainException("Viagem � obrigat�ria");

            if (passageiroId <= 0)
                throw new DomainException("Passageiro � obrigat�rio");
        }

        public void ConfirmarReserva()
        {
            if (Confirmado)
                throw new DomainException("Reserva j� est� confirmada");

            Confirmado = true;
            DataConfirmacao = DateTime.UtcNow;
        }

        public void CancelarReserva()
        {
            if (!Confirmado)
                throw new DomainException("Reserva n�o est� confirmada");

            Confirmado = false;
            DataConfirmacao = null;
            StatusConfirmacao = StatusConfirmacaoEnum.Cancelado;
        }

        public void ConfirmarPresenca()
        {
            if (StatusConfirmacao == StatusConfirmacaoEnum.Confirmado)
                throw new DomainException("Presena j est confirmada");

            if (DataLimiteConfirmacao.HasValue && DateTime.UtcNow > DataLimiteConfirmacao.Value)
                throw new DomainException("Prazo para confirmao expirado");

            StatusConfirmacao = StatusConfirmacaoEnum.Confirmado;
            DataConfirmacao = DateTime.UtcNow;
            Confirmado = true;
        }

        public void CancelarPresenca(string? motivo = null)
        {
            if (StatusConfirmacao == StatusConfirmacaoEnum.Cancelado)
                throw new DomainException("Presena j est cancelada");

            StatusConfirmacao = StatusConfirmacaoEnum.Cancelado;
            DataConfirmacao = null;
            Confirmado = false;
            
            if (!string.IsNullOrEmpty(motivo))
                AdicionarObservacao($"Cancelado: {motivo}");
        }

        public void MarcarComoNaoVai()
        {
            if (StatusConfirmacao == StatusConfirmacaoEnum.NaoVai)
                throw new DomainException("J est marcado como no vai");

            StatusConfirmacao = StatusConfirmacaoEnum.NaoVai;
            DataConfirmacao = null;
            Confirmado = false;
        }

        public void AdicionarObservacao(string observacao)
        {
            if (string.IsNullOrEmpty(observacao))
                throw new DomainException("Observa��o � obrigat�ria");

            if (observacao.Length > 500)
                throw new DomainException("Observa��o n�o pode ter mais que 500 caracteres");

            Observacao = observacao;
        }
    }
}
