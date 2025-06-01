using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;

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
            string observacao,
            bool situacao) : base(nome, cpf, telefone, email, sexo, observacao, situacao)
        {
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
            RG = rg;
            CNH = cnh;
            CategoriaCNH = categoriaCNH;
            ValidadeCNH = validadeCNH;
        }

        #region Propriedades Virtuais

        protected override string DescricaoFormatada
        {
            //get { return $"{Nome} ({CPF_Formatado})"; }
            get { return $"{Nome}"; }
        }

        public string DescricaoAtivo
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }

        #endregion
    }
}
