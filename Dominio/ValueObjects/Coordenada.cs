using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class Coordenada
    {
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }

        public Coordenada(decimal latitude, decimal longitude)
        {
            ValidarCoordenada(latitude, longitude);
            Latitude = latitude;
            Longitude = longitude;
        }

        private void ValidarCoordenada(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new DomainException("Latitude deve estar entre -90 e 90 graus");

            if (longitude < -180 || longitude > 180)
                throw new DomainException("Longitude deve estar entre -180 e 180 graus");
        }

        public decimal CalcularDistancia(Coordenada outraCoordenada)
        {
            if (outraCoordenada == null)
                throw new DomainException("Coordenada de referência é obrigatória");

            const double raioTerra = 6371; // Raio da Terra em quilômetros
            var lat1 = (double)Latitude * Math.PI / 180;
            var lat2 = (double)outraCoordenada.Latitude * Math.PI / 180;
            var deltaLat = ((double)outraCoordenada.Latitude - (double)Latitude) * Math.PI / 180;
            var deltaLon = ((double)outraCoordenada.Longitude - (double)Longitude) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distancia = raioTerra * c;

            return (decimal)distancia;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coordenada coord && 
                   Latitude == coord.Latitude && 
                   Longitude == coord.Longitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }

        public override string ToString()
        {
            return $"{Latitude:F6}, {Longitude:F6}";
        }
    }
} 
