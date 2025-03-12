namespace Dominio.Entidades.TiposOperacoes
{
    public class TipoOperacao : BaseEntity
    {
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
    }
}
