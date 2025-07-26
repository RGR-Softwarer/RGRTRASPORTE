using Dominio.Enums.Veiculo;
using Dominio.Exceptions;

namespace Dominio.Entidades.Veiculos
{
    public class Veiculo : BaseEntity
    {
        protected Veiculo() { } // Construtor protegido para EF Core

        public Veiculo(
            string placa,
            string modelo,
            string marca,
            string numeroChassi,
            int anoModelo,
            int anoFabricacao,
            string cor,
            string renavam,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId)
        {
            ValidarPlaca(placa);
            ValidarModelo(modelo);
            ValidarMarca(marca);
            ValidarNumeroChassi(numeroChassi);
            ValidarAnos(anoModelo, anoFabricacao);
            ValidarCor(cor);
            ValidarRenavam(renavam);

            Placa = placa;
            Modelo = modelo;
            Marca = marca;
            NumeroChassi = numeroChassi;
            AnoModelo = anoModelo;
            AnoFabricacao = anoFabricacao;
            Cor = cor;
            Renavam = renavam;
            VencimentoLicenciamento = vencimentoLicenciamento;
            TipoCombustivel = tipoCombustivel;
            Status = status;
            Observacao = observacao;
            ModeloVeiculoId = modeloVeiculoId;
            Situacao = true;
        }

        public string Placa { get; private set; }
        public string Modelo { get; private set; }
        public string Marca { get; private set; }
        public string NumeroChassi { get; private set; }
        public int AnoModelo { get; private set; }
        public int AnoFabricacao { get; private set; }
        public string Cor { get; private set; }
        public string Renavam { get; private set; }
        public virtual DateTime? VencimentoLicenciamento { get; private set; }
        public TipoCombustivelEnum TipoCombustivel { get; private set; }
        public StatusVeiculoEnum Status { get; private set; }
        public string Observacao { get; private set; }
        public long? ModeloVeiculoId { get; private set; }
        public virtual ModeloVeicular ModeloVeiculo { get; private set; }
        public bool Situacao { get; private set; }

        public virtual string PlacaFormatada => string.IsNullOrWhiteSpace(Placa) ? string.Empty : $"{Placa.Substring(0, 3)}-{Placa.Substring(3, 4)}";

        protected override string DescricaoFormatada => Placa;

        public void Atualizar(
            string modelo,
            string marca,
            string cor,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId)
        {
            ValidarModelo(modelo);
            ValidarMarca(marca);
            ValidarCor(cor);

            Modelo = modelo;
            Marca = marca;
            Cor = cor;
            VencimentoLicenciamento = vencimentoLicenciamento;
            TipoCombustivel = tipoCombustivel;
            Status = status;
            Observacao = observacao;
            ModeloVeiculoId = modeloVeiculoId;
        }

        public void AtualizarLicenciamento(DateTime vencimento)
        {
            if (vencimento < DateTime.Today)
                throw new DomainException("Data de vencimento do licenciamento não pode ser anterior à data atual.");

            VencimentoLicenciamento = vencimento;
        }

        public void Ativar()
        {
            if (Situacao)
                throw new DomainException("Veículo já está ativo.");

            Situacao = true;
        }

        public void Inativar()
        {
            if (!Situacao)
                throw new DomainException("Veículo já está inativo.");

            Situacao = false;
        }

        private void ValidarPlaca(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                throw new DomainException("Placa é obrigatória.");

            if (placa.Length != 7)
                throw new DomainException("Placa deve ter 7 caracteres.");

            if (!placa.All(c => char.IsLetterOrDigit(c)))
                throw new DomainException("Placa deve conter apenas letras e números.");
        }

        private void ValidarModelo(string modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo))
                throw new DomainException("Modelo é obrigatório.");

            if (modelo.Length > 50)
                throw new DomainException("Modelo deve ter no máximo 50 caracteres.");
        }

        private void ValidarMarca(string marca)
        {
            if (string.IsNullOrWhiteSpace(marca))
                throw new DomainException("Marca é obrigatória.");

            if (marca.Length > 50)
                throw new DomainException("Marca deve ter no máximo 50 caracteres.");
        }

        private void ValidarNumeroChassi(string numeroChassi)
        {
            if (string.IsNullOrWhiteSpace(numeroChassi))
                throw new DomainException("Número do chassi é obrigatório.");

            if (numeroChassi.Length != 17)
                throw new DomainException("Número do chassi deve ter 17 caracteres.");

            if (!numeroChassi.All(c => char.IsLetterOrDigit(c)))
                throw new DomainException("Número do chassi deve conter apenas letras e números.");
        }

        private void ValidarAnos(int anoModelo, int anoFabricacao)
        {
            var anoAtual = DateTime.Now.Year;

            if (anoModelo < anoFabricacao)
                throw new DomainException("Ano do modelo não pode ser anterior ao ano de fabricação.");

            if (anoModelo > anoAtual + 1)
                throw new DomainException("Ano do modelo não pode ser posterior ao ano seguinte.");

            if (anoFabricacao < 1900 || anoFabricacao > anoAtual)
                throw new DomainException("Ano de fabricação inválido.");
        }

        private void ValidarCor(string cor)
        {
            if (string.IsNullOrWhiteSpace(cor))
                throw new DomainException("Cor é obrigatória.");

            if (cor.Length > 30)
                throw new DomainException("Cor deve ter no máximo 30 caracteres.");
        }

        private void ValidarRenavam(string renavam)
        {
            if (string.IsNullOrWhiteSpace(renavam))
                throw new DomainException("RENAVAM é obrigatório.");

            if (renavam.Length != 11)
                throw new DomainException("RENAVAM deve ter 11 caracteres.");

            if (!renavam.All(char.IsDigit))
                throw new DomainException("RENAVAM deve conter apenas números.");
        }
    }
}
