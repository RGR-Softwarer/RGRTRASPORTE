using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class Distancia
    {
        public decimal Quilometros { get; private set; }

        public Distancia(decimal quilometros)
        {
            ValidarDistancia(quilometros);
            Quilometros = quilometros;
        }

        public decimal Metros => Quilometros * 1000;
        public string Formatada => $"{Quilometros:F2} km";

        private void ValidarDistancia(decimal quilometros)
        {
            if (quilometros <= 0)
                throw new DomainException("Distância deve ser maior que zero");

            if (quilometros > 10000)
                throw new DomainException("Distância não pode ser maior que 10.000 km");
        }

        public Distancia Somar(Distancia outraDistancia)
        {
            return new Distancia(Quilometros + outraDistancia.Quilometros);
        }

        public override bool Equals(object? obj)
        {
            return obj is Distancia distancia && Quilometros == distancia.Quilometros;
        }

        public override int GetHashCode()
        {
            return Quilometros.GetHashCode();
        }

        public override string ToString()
        {
            return Formatada;
        }

        public static implicit operator decimal(Distancia distancia) => distancia.Quilometros;
        public static implicit operator Distancia(decimal quilometros) => new(quilometros);
    }
} 