using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class CodigoViagem
    {
        public string Valor { get; private set; }

        public CodigoViagem(string codigo)
        {
            ValidarCodigo(codigo);
            Valor = codigo.ToUpperInvariant();
        }

        private void ValidarCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("Código da viagem é obrigatório");

            if (codigo.Length < 5 || codigo.Length > 20)
                throw new DomainException("Código da viagem deve ter entre 5 e 20 caracteres");

            if (!codigo.All(c => char.IsLetterOrDigit(c)))
                throw new DomainException("Código da viagem deve conter apenas letras e números");
        }

        public static CodigoViagem Gerar()
        {
            var codigo = $"VIA{DateTime.UtcNow:yyyyMMddHHmmss}";
            return new CodigoViagem(codigo);
        }

        public override bool Equals(object? obj)
        {
            return obj is CodigoViagem codigo && Valor == codigo.Valor;
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }

        public override string ToString()
        {
            return Valor;
        }

        public static implicit operator string(CodigoViagem codigo) => codigo.Valor;
        public static implicit operator CodigoViagem(string valor) => new(valor);
    }
} 