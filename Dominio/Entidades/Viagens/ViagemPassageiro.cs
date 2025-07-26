using Dominio.Entidades.Pessoas.Passageiros;
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

        // Navegação
        public Viagem Viagem { get; private set; }
        public Passageiro Passageiro { get; private set; }

        private ViagemPassageiro() { } // Para EF

        public ViagemPassageiro(Viagem viagem, long passageiroId)
        {
            ValidarCriacao(viagem, passageiroId);

            ViagemId = viagem.Id;
            Viagem = viagem;
            PassageiroId = passageiroId;
            DataReserva = DateTime.UtcNow;
            Confirmado = false;
        }

        private void ValidarCriacao(Viagem viagem, long passageiroId)
        {
            if (viagem == null)
                throw new DomainException("Viagem é obrigatória");

            if (passageiroId <= 0)
                throw new DomainException("Passageiro é obrigatório");
        }

        public void ConfirmarReserva()
        {
            if (Confirmado)
                throw new DomainException("Reserva já está confirmada");

            Confirmado = true;
            DataConfirmacao = DateTime.UtcNow;
        }

        public void CancelarReserva()
        {
            if (!Confirmado)
                throw new DomainException("Reserva não está confirmada");

            Confirmado = false;
            DataConfirmacao = null;
        }

        public void AdicionarObservacao(string observacao)
        {
            if (string.IsNullOrEmpty(observacao))
                throw new DomainException("Observação é obrigatória");

            if (observacao.Length > 500)
                throw new DomainException("Observação não pode ter mais que 500 caracteres");

            Observacao = observacao;
        }
    }
}
