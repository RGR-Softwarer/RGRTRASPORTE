namespace Dominio.Enums.Pessoas
{
    public enum SexoEnum
    {
        Masculino = 1,
        Feminino = 2
    }

    public static class SexoEnumHelper
    {
        public static string ObterDescricao(this SexoEnum sexo)
        {
            switch (sexo)
            {
                case SexoEnum.Masculino: return "Masculino";
                case SexoEnum.Feminino: return "Feminino";
                default: return string.Empty;
            }
        }
    }
}

