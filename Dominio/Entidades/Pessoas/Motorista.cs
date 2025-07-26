using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;
using Dominio.Exceptions;

namespace Dominio.Entidades.Pessoas
{
    public class Motorista : Pessoa
    {
        protected Motorista() { } // Construtor protegido para EF Core

        public Motorista(
            string nome,
            string cpf,
            string rg,
            string telefone,
            string email,
            SexoEnum sexo,
            string cnh,
            CategoriaCNHEnum categoriaCNH,
            DateTime validadeCNH,
            string observacao) : base(nome, cpf, telefone, email, sexo, observacao)
        {
            ValidarRG(rg);
            ValidarCNH(cnh);
            ValidarValidadeCNH(validadeCNH);

            RG = rg;
            CNH = cnh;
            CategoriaCNH = categoriaCNH;
            ValidadeCNH = validadeCNH;
        }

        public string RG { get; private set; }
        public string CNH { get; private set; }
        public CategoriaCNHEnum CategoriaCNH { get; private set; }
        public DateTime ValidadeCNH { get; private set; }

        public void AtualizarDocumentos(
            string rg,
            string cnh,
            CategoriaCNHEnum categoriaCNH,
            DateTime validadeCNH)
        {
            ValidarRG(rg);
            ValidarCNH(cnh);
            ValidarValidadeCNH(validadeCNH);

            RG = rg;
            CNH = cnh;
            CategoriaCNH = categoriaCNH;
            ValidadeCNH = validadeCNH;
        }

        public void RenovarCNH(DateTime novaValidade)
        {
            ValidarValidadeCNH(novaValidade);
            ValidadeCNH = novaValidade;
        }

        public bool CNHExpirada => ValidadeCNH < DateTime.Today;

        public bool PodeDirigirVeiculo(TipoModeloVeiculoEnum tipoVeiculo)
        {
            return tipoVeiculo switch
            {
                TipoModeloVeiculoEnum.Van => CategoriaCNH == CategoriaCNHEnum.B || CategoriaCNH == CategoriaCNHEnum.D,
                TipoModeloVeiculoEnum.Onibus => CategoriaCNH == CategoriaCNHEnum.D,
                _ => false
            };
        }

        protected override string DescricaoFormatada => $"{Nome} ({CPF_Formatado})";

        private void ValidarRG(string rg)
        {
            if (string.IsNullOrWhiteSpace(rg))
                throw new DomainException("RG é obrigatório.");

            if (rg.Length > 20)
                throw new DomainException("RG deve ter no máximo 20 caracteres.");
        }

        private void ValidarCNH(string cnh)
        {
            if (string.IsNullOrWhiteSpace(cnh))
                throw new DomainException("CNH é obrigatória.");

            if (cnh.Length != 11)
                throw new DomainException("CNH deve ter 11 caracteres.");

            if (!cnh.All(char.IsDigit))
                throw new DomainException("CNH deve conter apenas números.");
        }

        private void ValidarValidadeCNH(DateTime validade)
        {
            if (validade < DateTime.Today)
                throw new DomainException("Data de validade da CNH não pode ser anterior à data atual.");

            if (validade > DateTime.Today.AddYears(10))
                throw new DomainException("Data de validade da CNH não pode ser superior a 10 anos.");
        }

        #region Propriedades Virtuais

        public string DescricaoAtivo
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }

        #endregion
    }
}
