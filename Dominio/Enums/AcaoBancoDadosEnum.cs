namespace Dominio.Enums
{
    public enum AcaoBancoDadosEnum
    {
        Insert = 0,
        Update = 1,
        Delete = 2,
        Registro = 3
    }

    public static class AcaoBancoDadosHelper
    {
        public static string ObterDescricao(this AcaoBancoDadosEnum acao)
        {
            switch (acao)
            {
                case AcaoBancoDadosEnum.Insert: return "Adicionado";
                case AcaoBancoDadosEnum.Update: return "Atualizado";
                case AcaoBancoDadosEnum.Delete: return "Excluido";
                default: return "RegistroDeAcao";
            }
        }
    }
}
