using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class CapacidadeVeiculo
    {
        public int AssentosDisponiveis { get; private set; }
        public int PassageirosEmPe { get; private set; }
        public int CapacidadeMaxima => AssentosDisponiveis + PassageirosEmPe;

        private CapacidadeVeiculo() { } // Para EF Core

        public CapacidadeVeiculo(int assentosDisponiveis, int passageirosEmPe)
        {
            ValidarCapacidade(assentosDisponiveis, passageirosEmPe);
            
            AssentosDisponiveis = assentosDisponiveis;
            PassageirosEmPe = passageirosEmPe;
        }

        public bool TemCapacidadePara(int quantidadePassageiros)
        {
            return quantidadePassageiros <= CapacidadeMaxima;
        }

        public int VagasRemanescentesPara(int passageirosEmbarcados)
        {
            return Math.Max(0, CapacidadeMaxima - passageirosEmbarcados);
        }

        public bool EstaLotado(int passageirosEmbarcados)
        {
            return passageirosEmbarcados >= CapacidadeMaxima;
        }

        public double TaxaOcupacao(int passageirosEmbarcados)
        {
            if (CapacidadeMaxima == 0) return 0;
            return Math.Min(100, (double)passageirosEmbarcados / CapacidadeMaxima * 100);
        }

        public string DescricaoCapacidade => PassageirosEmPe > 0 
            ? $"{AssentosDisponiveis} assentos + {PassageirosEmPe} em pé = {CapacidadeMaxima} total"
            : $"{AssentosDisponiveis} assentos";

        private static void ValidarCapacidade(int assentos, int emPe)
        {
            if (assentos <= 0)
                throw new DomainException("Quantidade de assentos deve ser maior que zero");

            if (emPe < 0)
                throw new DomainException("Quantidade de passageiros em pé não pode ser negativa");

            if (assentos > 100)
                throw new DomainException("Quantidade de assentos não pode ser maior que 100");

            if (emPe > 50)
                throw new DomainException("Quantidade de passageiros em pé não pode ser maior que 50");

            var capacidadeTotal = assentos + emPe;
            if (capacidadeTotal > 150)
                throw new DomainException("Capacidade total não pode ser maior que 150 passageiros");
        }

        public CapacidadeVeiculo AtualizarAssentos(int novosAssentos)
        {
            return new CapacidadeVeiculo(novosAssentos, PassageirosEmPe);
        }

        public CapacidadeVeiculo AtualizarPassageirosEmPe(int novosPassageirosEmPe)
        {
            return new CapacidadeVeiculo(AssentosDisponiveis, novosPassageirosEmPe);
        }

        public override bool Equals(object? obj)
        {
            return obj is CapacidadeVeiculo capacidade &&
                   AssentosDisponiveis == capacidade.AssentosDisponiveis &&
                   PassageirosEmPe == capacidade.PassageirosEmPe;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AssentosDisponiveis, PassageirosEmPe);
        }

        public override string ToString()
        {
            return DescricaoCapacidade;
        }
    }
}
