using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class CPF
    {
        public string Numero { get; private set; } = string.Empty;

        private CPF() { } // Para EF

        public CPF(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new DomainException("CPF é obrigatório.");

            // Remove any formatting
            numero = numero.Replace(".", "").Replace("-", "").Trim();

            if (numero.Length != 11)
                throw new DomainException("CPF deve ter 11 caracteres.");

            if (!numero.All(char.IsDigit))
                throw new DomainException("CPF deve conter apenas números.");

            if (!ValidarCpf(numero))
                throw new DomainException("CPF inválido.");

            Numero = numero;
        }

        public static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove any formatting
            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
                return false;

            // Check for known invalid CPFs (all same digits)
            if (new string(cpf[0], 11) == cpf)
                return false;

            // Validate first digit
            var sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * (10 - i);
            
            var remainder = sum % 11;
            var digit1 = remainder < 2 ? 0 : 11 - remainder;
            
            if (int.Parse(cpf[9].ToString()) != digit1)
                return false;

            // Validate second digit
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * (11 - i);
            
            remainder = sum % 11;
            var digit2 = remainder < 2 ? 0 : 11 - remainder;
            
            return int.Parse(cpf[10].ToString()) == digit2;
        }

        public string NumeroFormatado => $"{Numero.Substring(0, 3)}.{Numero.Substring(3, 3)}.{Numero.Substring(6, 3)}-{Numero.Substring(9, 2)}";

        public override string ToString() => NumeroFormatado;

        public override bool Equals(object? obj)
        {
            return obj is CPF other && Numero == other.Numero;
        }

        public override int GetHashCode()
        {
            return Numero.GetHashCode();
        }

        public static implicit operator string(CPF cpf) => cpf.Numero;
        public static implicit operator CPF(string numero) => new(numero);
    }
} 