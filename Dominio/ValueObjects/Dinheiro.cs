using Dominio.Exceptions;
using System.Globalization;

namespace Dominio.ValueObjects
{
    public class Dinheiro
    {
        public decimal Valor { get; private set; }
        public string Moeda { get; private set; }

        private Dinheiro() { } // Para EF Core

        public Dinheiro(decimal valor, string moeda = "BRL")
        {
            ValidarValor(valor);
            ValidarMoeda(moeda);

            Valor = Math.Round(valor, 2, MidpointRounding.AwayFromZero);
            Moeda = moeda.ToUpperInvariant();
        }

        public bool EhZero => Valor == 0;
        public bool EhPositivo => Valor > 0;
        public bool EhNegativo => Valor < 0;

        public string ValorFormatado => Moeda switch
        {
            "BRL" => Valor.ToString("C", new CultureInfo("pt-BR")),
            "USD" => Valor.ToString("C", new CultureInfo("en-US")),
            "EUR" => Valor.ToString("C", new CultureInfo("de-DE")),
            _ => $"{Valor:F2} {Moeda}"
        };

        public string SimboloMoeda => Moeda switch
        {
            "BRL" => "R$",
            "USD" => "$",
            "EUR" => "€",
            "GBP" => "£",
            _ => Moeda
        };

        private static void ValidarValor(decimal valor)
        {
            if (valor < 0)
                throw new DomainException("Valor monetário não pode ser negativo");

            if (valor > 999999999.99m)
                throw new DomainException("Valor monetário não pode ser maior que R$ 999.999.999,99");
        }

        private static void ValidarMoeda(string moeda)
        {
            if (string.IsNullOrWhiteSpace(moeda))
                throw new DomainException("Código da moeda é obrigatório");

            if (moeda.Length != 3)
                throw new DomainException("Código da moeda deve ter 3 caracteres");

            var moedasValidas = new[] { "BRL", "USD", "EUR", "GBP", "JPY", "CHF", "CAD", "AUD" };
            if (!moedasValidas.Contains(moeda.ToUpperInvariant()))
                throw new DomainException($"Moeda '{moeda}' não é suportada");
        }

        public Dinheiro Somar(Dinheiro outro)
        {
            ValidarMesmaMoeda(outro);
            return new Dinheiro(Valor + outro.Valor, Moeda);
        }

        public Dinheiro Subtrair(Dinheiro outro)
        {
            ValidarMesmaMoeda(outro);
            var novoValor = Valor - outro.Valor;
            if (novoValor < 0)
                throw new DomainException("Resultado da subtração não pode ser negativo");
            
            return new Dinheiro(novoValor, Moeda);
        }

        public Dinheiro Multiplicar(decimal fator)
        {
            if (fator < 0)
                throw new DomainException("Fator de multiplicação não pode ser negativo");
            
            return new Dinheiro(Valor * fator, Moeda);
        }

        public Dinheiro Dividir(decimal divisor)
        {
            if (divisor <= 0)
                throw new DomainException("Divisor deve ser maior que zero");
            
            return new Dinheiro(Valor / divisor, Moeda);
        }

        public Dinheiro AplicarDesconto(decimal percentualDesconto)
        {
            if (percentualDesconto < 0 || percentualDesconto > 100)
                throw new DomainException("Percentual de desconto deve estar entre 0 e 100");
            
            var valorDesconto = Valor * (percentualDesconto / 100);
            return new Dinheiro(Valor - valorDesconto, Moeda);
        }

        public Dinheiro AplicarAcrescimo(decimal percentualAcrescimo)
        {
            if (percentualAcrescimo < 0)
                throw new DomainException("Percentual de acréscimo não pode ser negativo");
            
            var valorAcrescimo = Valor * (percentualAcrescimo / 100);
            return new Dinheiro(Valor + valorAcrescimo, Moeda);
        }

        private void ValidarMesmaMoeda(Dinheiro outro)
        {
            if (Moeda != outro.Moeda)
                throw new DomainException($"Não é possível operar com moedas diferentes: {Moeda} e {outro.Moeda}");
        }

        public static Dinheiro Zero(string moeda = "BRL") => new(0, moeda);
        public static Dinheiro Real(decimal valor) => new(valor, "BRL");
        public static Dinheiro Dolar(decimal valor) => new(valor, "USD");
        public static Dinheiro Euro(decimal valor) => new(valor, "EUR");

        public override bool Equals(object? obj)
        {
            return obj is Dinheiro dinheiro &&
                   Valor == dinheiro.Valor &&
                   Moeda == dinheiro.Moeda;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Valor, Moeda);
        }

        public override string ToString()
        {
            return ValorFormatado;
        }

        // Operadores
        public static Dinheiro operator +(Dinheiro a, Dinheiro b) => a.Somar(b);
        public static Dinheiro operator -(Dinheiro a, Dinheiro b) => a.Subtrair(b);
        public static Dinheiro operator *(Dinheiro a, decimal b) => a.Multiplicar(b);
        public static Dinheiro operator /(Dinheiro a, decimal b) => a.Dividir(b);
        
        public static bool operator ==(Dinheiro a, Dinheiro b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(Dinheiro a, Dinheiro b) => !(a == b);
        public static bool operator >(Dinheiro a, Dinheiro b) => a.Valor > b.Valor && a.Moeda == b.Moeda;
        public static bool operator <(Dinheiro a, Dinheiro b) => a.Valor < b.Valor && a.Moeda == b.Moeda;
        public static bool operator >=(Dinheiro a, Dinheiro b) => a.Valor >= b.Valor && a.Moeda == b.Moeda;
        public static bool operator <=(Dinheiro a, Dinheiro b) => a.Valor <= b.Valor && a.Moeda == b.Moeda;

        public static implicit operator decimal(Dinheiro dinheiro) => dinheiro.Valor;
        public static implicit operator Dinheiro(decimal valor) => new(valor);
    }
}
