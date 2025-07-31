namespace Dominio.Enums.Veiculo
{
    public enum CategoriaCNHEnum
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        AB = 6,
        AC = 7,
        AD = 8,
        AE = 9
    }
    public static class CategoriaCNHEnumHelper
    {
        public static string ObterDescricao(this CategoriaCNHEnum situacao)
        {
            switch (situacao)
            {
                case CategoriaCNHEnum.A: return "A";
                case CategoriaCNHEnum.B: return "B";
                case CategoriaCNHEnum.C: return "C";
                case CategoriaCNHEnum.D: return "D";
                case CategoriaCNHEnum.E: return "E";
                case CategoriaCNHEnum.AB: return "AB";
                case CategoriaCNHEnum.AC: return "AC";
                case CategoriaCNHEnum.AD: return "AD";
                case CategoriaCNHEnum.AE: return "AE";
                default: return string.Empty;
            }
        }
    }
}
