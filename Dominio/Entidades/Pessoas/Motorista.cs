using Dominio.Enums.Veiculo;

namespace Dominio.Entidades.Pessoas
{
    public class Motorista : BaseEntity
    {
        public string Nome { get; }
        public bool Situacao { get; }
        public string CPF { get; }
        public string RG { get; }
        public string Telefone { get; }
        public string Email { get; }
        public string CNH { get; set; }
        public CategoriaCNHEnum CategoriaCNH { get; }
        public DateTime ValidadeCNH { get; }
        public string Observacao { get; }

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
