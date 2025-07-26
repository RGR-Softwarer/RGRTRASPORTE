using Dominio.Enums.Viagens;
using Dominio.Exceptions;
using Dominio.ObjetosDeValor.Logistica;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPosicao : BaseEntity
    {
        public long ViagemId { get; private set; }
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public DateTime DataHora { get; private set; }
                
        // Navegação
        public Viagem Viagem { get; private set; }

        private ViagemPosicao() { } // Para EF

        public ViagemPosicao(Viagem viagem, decimal latitude, decimal longitude, DateTime dataHora)
        {
            ValidarCriacao(viagem, latitude, longitude, dataHora);

            ViagemId = viagem.Id;
            Viagem = viagem;
            Latitude = latitude;
            Longitude = longitude;
            DataHora = dataHora;
        }

        private void ValidarCriacao(Viagem viagem, decimal latitude, decimal longitude, DateTime dataHora)
        {
            if (viagem == null)
                throw new DomainException("Viagem é obrigatória");

            if (latitude < -90 || latitude > 90)
                throw new DomainException("Latitude inválida");

            if (longitude < -180 || longitude > 180)
                throw new DomainException("Longitude inválida");

            if (dataHora > DateTime.UtcNow)
                throw new DomainException("Data/hora não pode ser futura");

            if (viagem.Situacao != SituacaoViagemEnum.EmAndamento)
                throw new DomainException("Apenas viagens em andamento podem receber posições");
        }
       

        public decimal CalcularDistancia(ViagemPosicao outraPosicao)
        {
            if (outraPosicao == null)
                throw new DomainException("Posição de referência é obrigatória");

            const double raioTerra = 6371; // Raio da Terra em quilômetros
            var lat1 = (double)Latitude * Math.PI / 180;
            var lat2 = (double)outraPosicao.Latitude * Math.PI / 180;
            var deltaLat = ((double)outraPosicao.Latitude - (double)Latitude) * Math.PI / 180;
            var deltaLon = ((double)outraPosicao.Longitude - (double)Longitude) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distancia = raioTerra * c;

            return (decimal)distancia;
        }
    }
}
