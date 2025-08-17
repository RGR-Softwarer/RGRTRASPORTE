using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class Chassi
    {
        public string Numero { get; private set; }

        private Chassi() { } // Para EF Core

        public Chassi(string numero)
        {
            ValidarChassi(numero);
            Numero = numero.Replace(" ", "").Replace("-", "").ToUpperInvariant();
        }

        public string NumeroFormatado => Numero.Length == 17 ? 
            $"{Numero.Substring(0, 3)} {Numero.Substring(3, 6)} {Numero.Substring(9, 2)} {Numero.Substring(11, 6)}" : 
            Numero;

        private static void ValidarChassi(string chassi)
        {
            if (string.IsNullOrWhiteSpace(chassi))
                throw new DomainException("Número do chassi é obrigatório");

            // Remove espaços e traços
            chassi = chassi.Replace(" ", "").Replace("-", "").Trim();

            if (chassi.Length != 17)
                throw new DomainException("Chassi deve ter 17 caracteres");

            if (!chassi.All(c => char.IsLetterOrDigit(c)))
                throw new DomainException("Chassi deve conter apenas letras e números");

            // Chassi não pode conter I, O, Q (padrão internacional)
            if (chassi.Contains('I') || chassi.Contains('O') || chassi.Contains('Q'))
                throw new DomainException("Chassi não pode conter as letras I, O ou Q");

            // Verifica se não são todos os caracteres iguais
            if (chassi.All(c => c == chassi[0]))
                throw new DomainException("Chassi inválido - todos os caracteres são iguais");
        }

        public bool IsFormatoVIN()
        {
            return Numero.Length == 17 && 
                   Numero.All(c => char.IsLetterOrDigit(c)) &&
                   !Numero.Contains('I') && !Numero.Contains('O') && !Numero.Contains('Q');
        }

        public string ExtrairAnoModelo()
        {
            if (!IsFormatoVIN() || Numero.Length < 10)
                return "Não disponível";

            // No padrão VIN, o 10º caractere representa o ano
            var anoChar = Numero[9];
            
            // Mapear caractere para ano (simplificado)
            return anoChar switch
            {
                'A' => "2010", 'B' => "2011", 'C' => "2012", 'D' => "2013", 'E' => "2014",
                'F' => "2015", 'G' => "2016", 'H' => "2017", 'J' => "2018", 'K' => "2019",
                'L' => "2020", 'M' => "2021", 'N' => "2022", 'P' => "2023", 'R' => "2024",
                _ when char.IsDigit(anoChar) => $"20{anoChar}X",
                _ => "Não identificado"
            };
        }

        public override bool Equals(object? obj)
        {
            return obj is Chassi chassi && Numero == chassi.Numero;
        }

        public override int GetHashCode()
        {
            return Numero.GetHashCode();
        }

        public override string ToString()
        {
            return NumeroFormatado;
        }

        public static implicit operator string(Chassi chassi) => chassi.Numero;
        public static implicit operator Chassi(string numero) => new(numero);
    }
}
