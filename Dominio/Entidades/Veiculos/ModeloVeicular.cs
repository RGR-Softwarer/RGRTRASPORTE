using Dominio.Enums.Veiculo;
using Dominio.Exceptions;
using Dominio.Events.Base;
using Dominio.Events.Veiculos;

namespace Dominio.Entidades.Veiculos
{
    public class ModeloVeicular : AggregateRoot
    {
        protected ModeloVeicular() { } // Construtor protegido para EF Core

        public ModeloVeicular(
            string descricaoModelo,
            TipoModeloVeiculoEnum tipo,
            int quantidadeAssento,
            int quantidadeEixo,
            int capacidadeMaxima,
            int passageirosEmPe,
            bool possuiBanheiro,
            bool possuiClimatizador,
            bool situacao)
        {
            ValidarDescricao(descricaoModelo);
            ValidarQuantidades(quantidadeAssento, quantidadeEixo, capacidadeMaxima, passageirosEmPe);

            Descricao = descricaoModelo;
            Tipo = tipo;
            QuantidadeAssento = quantidadeAssento;
            QuantidadeEixo = quantidadeEixo;
            CapacidadeMaxima = capacidadeMaxima;
            PassageirosEmPe = passageirosEmPe;
            PossuiBanheiro = possuiBanheiro;
            PossuiClimatizador = possuiClimatizador;
            Situacao = situacao;
            Situacao = true;
            Veiculos = new List<Veiculo>();

            AddDomainEvent(new ModeloVeicularCriadoEvent(Id, descricaoModelo, tipo));
        }

        public bool Situacao { get; private set; }
        public string Descricao { get; private set; } = null!;
        public TipoModeloVeiculoEnum Tipo { get; private set; }
        public int QuantidadeAssento { get; private set; }
        public int QuantidadeEixo { get; private set; }
        public int CapacidadeMaxima { get; private set; }
        public int PassageirosEmPe { get; private set; }
        public bool PossuiBanheiro { get; private set; }
        public bool PossuiClimatizador { get; private set; }
        public ICollection<Veiculo> Veiculos { get; private set; } = null!;

        public string DescricaoAtivo => Situacao ? "Ativo" : "Inativo";

        public void Atualizar(
            string descricao,
            TipoModeloVeiculoEnum tipo,
            int quantidadeAssento,
            int quantidadeEixo,
            int capacidadeMaxima,
            int passageirosEmPe,
            bool possuiBanheiro,
            bool possuiClimatizador,
            bool situacao)
        {
            ValidarDescricao(descricao);
            ValidarQuantidades(quantidadeAssento, quantidadeEixo, capacidadeMaxima, passageirosEmPe);

            Descricao = descricao;
            Tipo = tipo;
            QuantidadeAssento = quantidadeAssento;
            QuantidadeEixo = quantidadeEixo;
            CapacidadeMaxima = capacidadeMaxima;
            PassageirosEmPe = passageirosEmPe;
            PossuiBanheiro = possuiBanheiro;
            PossuiClimatizador = possuiClimatizador;
            
            UpdateTimestamp();
            AddDomainEvent(new ModeloVeicularAtualizadoEvent(Id, descricao));
        }

        public void Ativar()
        {
            if (Situacao)
                throw new DomainException("Modelo j� est� ativo.");

            Situacao = true;
            UpdateTimestamp();
            AddDomainEvent(new ModeloVeicularAtivadoEvent(Id, Descricao));
        }

        public void Inativar()
        {
            if (!Situacao)
                throw new DomainException("Modelo j� est� inativo.");

            if (Veiculos.Any(v => v.Situacao))
                throw new DomainException("N�o � poss�vel inativar um modelo que possui ve�culos ativos.");

            Situacao = false;
            UpdateTimestamp();
            AddDomainEvent(new ModeloVeicularInativadoEvent(Id, Descricao));
        }

        private void ValidarDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new DomainException("Descri��o � obrigat�ria.");

            if (descricao.Length > 100)
                throw new DomainException("Descri��o deve ter no m�ximo 100 caracteres.");
        }

        private void ValidarQuantidades(int quantidadeAssento, int quantidadeEixo, int capacidadeMaxima, int passageirosEmPe)
        {
            if (quantidadeAssento <= 0)
                throw new DomainException("Quantidade de assentos deve ser maior que zero.");

            if (quantidadeEixo <= 0)
                throw new DomainException("Quantidade de eixos deve ser maior que zero.");

            if (capacidadeMaxima <= 0)
                throw new DomainException("Capacidade m�xima deve ser maior que zero.");

            if (passageirosEmPe < 0)
                throw new DomainException("Quantidade de passageiros em p� n�o pode ser negativa.");

            if (capacidadeMaxima < quantidadeAssento + passageirosEmPe)
                throw new DomainException("Capacidade m�xima deve ser maior ou igual � soma de assentos e passageiros em p�.");
        }

        protected override string DescricaoFormatada => $"{Descricao} ({Tipo})";
    }
}
