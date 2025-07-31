using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class Polilinha
    {
        public string Rota { get; private set; }

        public Polilinha(string rota)
        {
            ValidarRota(rota);
            Rota = rota;
        }

        private void ValidarRota(string rota)
        {
            if (string.IsNullOrWhiteSpace(rota))
                throw new DomainException("Polilinha da rota é obrigatória");

            if (rota.Length < 10)
                throw new DomainException("Polilinha da rota inválida - muito curta");

            if (rota.Length > 10000)
                throw new DomainException("Polilinha da rota muito longa");
        }

        public bool EhValida()
        {
            // Validação básica de formato de polilinha
            return !string.IsNullOrWhiteSpace(Rota) && 
                   Rota.Length >= 10 && 
                   Rota.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.' || c == '~');
        }

        public override bool Equals(object? obj)
        {
            return obj is Polilinha polilinha && Rota == polilinha.Rota;
        }

        public override int GetHashCode()
        {
            return Rota.GetHashCode();
        }

        public override string ToString()
        {
            return Rota;
        }

        public static implicit operator string(Polilinha polilinha) => polilinha.Rota;
        public static implicit operator Polilinha(string rota) => new(rota);
    }
} 
