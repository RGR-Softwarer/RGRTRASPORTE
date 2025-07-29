using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class Placa
    {
        public string Numero { get; private set; }

        public Placa(string numero)
        {
            ValidarPlaca(numero);
            Numero = numero.ToUpperInvariant();
        }

        public string Formatada => $"{Numero.Substring(0, 3)}-{Numero.Substring(3, 4)}";

        private void ValidarPlaca(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new DomainException("Placa é obrigatória.");

            if (placa.Length != 7)
                throw new DomainException("Placa deve ter 7 caracteres.");

            if (!placa.All(c => char.IsLetterOrDigit(c)))
                throw new DomainException("Placa deve conter apenas letras e números.");
        }

        public override bool Equals(object? obj)
        {
            return obj is Placa placa && Numero == placa.Numero;
        }

        public override int GetHashCode()
        {
            return Numero.GetHashCode();
        }

        public override string ToString()
        {
            return Formatada;
        }

        public static implicit operator string(Placa placa) => placa.Numero;
        public static implicit operator Placa(string numero) => new(numero);
    }
} 