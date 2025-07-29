using Dominio.Enums.Viagens;
using Dominio.Exceptions;
using Dominio.ValueObjects;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPosicao : BaseEntity
    {
        public long ViagemId { get; private set; }
        public Coordenada Coordenada { get; private set; }
        public DateTime DataHora { get; private set; }
                
        // Navegação
        public Viagem Viagem { get; private set; }

        private ViagemPosicao() { } // Para EF

        public ViagemPosicao(Viagem viagem, decimal latitude, decimal longitude, DateTime dataHora)
        {
            ValidarCriacao(viagem, dataHora);
            
            ViagemId = viagem.Id;
            Viagem = viagem;
            Coordenada = new Coordenada(latitude, longitude);
            DataHora = dataHora;
        }

        private void ValidarCriacao(Viagem viagem, DateTime dataHora)
        {
            if (viagem == null)
                throw new DomainException("Viagem é obrigatória");

            if (dataHora > DateTime.UtcNow)
                throw new DomainException("Data/hora não pode ser futura");

            if (viagem.Situacao != SituacaoViagemEnum.EmAndamento)
                throw new DomainException("Apenas viagens em andamento podem receber posições");
        }

        public decimal CalcularDistancia(ViagemPosicao outraPosicao)
        {
            if (outraPosicao == null)
                throw new DomainException("Posição de referência é obrigatória");

            return Coordenada.CalcularDistancia(outraPosicao.Coordenada);
        }

        // Propriedades para compatibilidade com o código existente
        public decimal Latitude => Coordenada.Latitude;
        public decimal Longitude => Coordenada.Longitude;
    }
}
