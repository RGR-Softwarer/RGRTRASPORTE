using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;
using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class CNH
    {
        public string Numero { get; private set; }
        public CategoriaCNHEnum Categoria { get; private set; }
        public DateTime Validade { get; private set; }

        private CNH() { } // Para EF Core

        public CNH(string numero, CategoriaCNHEnum categoria, DateTime validade)
        {
            ValidarNumero(numero);
            ValidarValidade(validade);

            Numero = numero;
            Categoria = categoria;
            Validade = validade;
        }

        public bool Expirada => Validade < DateTime.Today;
        public int DiasParaVencer => (Validade - DateTime.Today).Days;
        public bool VencendoEm30Dias => DiasParaVencer <= 30 && DiasParaVencer > 0;

        public string NumeroFormatado => $"{Numero.Substring(0, 3)}.{Numero.Substring(3, 3)}.{Numero.Substring(6, 3)}-{Numero.Substring(9, 2)}";

        public bool PodeConduzirTipoVeiculo(TipoModeloVeiculoEnum tipoVeiculo)
        {
            return tipoVeiculo switch
            {
                TipoModeloVeiculoEnum.Carro => Categoria >= CategoriaCNHEnum.B,
                TipoModeloVeiculoEnum.Van => Categoria == CategoriaCNHEnum.B || Categoria == CategoriaCNHEnum.D,
                TipoModeloVeiculoEnum.MicroOnibus => Categoria == CategoriaCNHEnum.D,
                TipoModeloVeiculoEnum.Onibus => Categoria == CategoriaCNHEnum.D,
                _ => false
            };
        }

        private static void ValidarNumero(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new DomainException("Número da CNH é obrigatório");

            // Remove formatação se houver
            numero = numero.Replace(".", "").Replace("-", "").Trim();

            if (numero.Length != 11)
                throw new DomainException("CNH deve ter 11 dígitos");

            if (!numero.All(char.IsDigit))
                throw new DomainException("CNH deve conter apenas números");

            // Verifica se não são todos os dígitos iguais
            if (new string(numero[0], 11) == numero)
                throw new DomainException("CNH inválida");
        }

        private static void ValidarValidade(DateTime validade)
        {
            if (validade < DateTime.Today)
                throw new DomainException("Data de validade da CNH não pode ser anterior à data atual");

            if (validade > DateTime.Today.AddYears(10))
                throw new DomainException("Data de validade da CNH não pode ser superior a 10 anos");
        }

        public CNH RenovarCNH(DateTime novaValidade)
        {
            return new CNH(Numero, Categoria, novaValidade);
        }

        public override bool Equals(object? obj)
        {
            return obj is CNH cnh && 
                   Numero == cnh.Numero && 
                   Categoria == cnh.Categoria && 
                   Validade == cnh.Validade;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Numero, Categoria, Validade);
        }

        public override string ToString()
        {
            return $"CNH: {NumeroFormatado} - Categoria: {Categoria} - Validade: {Validade:dd/MM/yyyy}";
        }

        public static implicit operator string(CNH cnh) => cnh.Numero;
        public static implicit operator CNH(string numero) => new(numero, CategoriaCNHEnum.B, DateTime.Today.AddYears(5));
    }
}
