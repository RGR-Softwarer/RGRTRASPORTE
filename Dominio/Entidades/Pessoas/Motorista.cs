using Dominio.Enums.Veiculo;

namespace Dominio.Entidades.Pessoas
{
    public class Motorista : BaseEntity
    {
        public string Nome { get; private set; }
        public bool Situacao { get; private set; }
        public string CPF { get; private set; }
        public string RG { get; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public string CNH { get; set; }
        public CategoriaCNHEnum CategoriaCNH { get; private set; }
        public DateTime ValidadeCNH { get; private set; }
        public string Observacao { get; private set; }

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
