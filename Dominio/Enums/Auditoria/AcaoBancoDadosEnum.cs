namespace Dominio.Enums.Auditoria
{
    public enum AcaoBancoDadosEnum
    {
        Insercao = 1,
        Alteracao = 2,
        Exclusao = 3,
        Evento = 4
    }

    public static class AcaoBancoDadosHelper
    {
        public static string ObterDescricao(this AcaoBancoDadosEnum acao)
        {
            switch (acao)
            {
                case AcaoBancoDadosEnum.Insercao: return "Adicionado";
                case AcaoBancoDadosEnum.Alteracao: return "Atualizado";
                case AcaoBancoDadosEnum.Exclusao: return "Excluido";
                default: return "RegistroDeAcao";
            }
        }
    }
}
