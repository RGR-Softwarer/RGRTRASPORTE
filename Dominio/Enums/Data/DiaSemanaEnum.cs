namespace Dominio.Enums.Data
{
    public enum DiaSemanaEnum
    {
        Domingo = 1,
        Segunda = 2,
        Terca = 3,
        Quarta = 4,
        Quinta = 5,
        Sexta = 6,
        Sabado = 7,
    }

    public static class DiaSemanaEnumHelper
    {
        public static string ObterDescricao(this DiaSemanaEnum diaSemana)
        {
            switch (diaSemana)
            {
                case DiaSemanaEnum.Domingo: return "Domingo";
                case DiaSemanaEnum.Segunda: return "Segunda";
                case DiaSemanaEnum.Terca: return "Terça";
                case DiaSemanaEnum.Quarta: return "Quarta";
                case DiaSemanaEnum.Quinta: return "Quinta";
                case DiaSemanaEnum.Sexta: return "Sexta";
                case DiaSemanaEnum.Sabado: return "Sábado";
                default: return string.Empty;
            }
        }

        public static string ObterDescricao(DateTime data)
        {
            DiaSemanaEnum diaSemana = ObterDiaSemana(data);

            return diaSemana.ObterDescricao();
        }

        public static string ObterDescricaoResumida(this DiaSemanaEnum diaSemana)
        {
            switch (diaSemana)
            {
                case DiaSemanaEnum.Domingo: return "Domingo";
                case DiaSemanaEnum.Segunda: return "Segunda";
                case DiaSemanaEnum.Terca: return "Terca";
                case DiaSemanaEnum.Quarta: return "Quarta";
                case DiaSemanaEnum.Quinta: return "Quinta";
                case DiaSemanaEnum.Sexta: return "Sexta";
                case DiaSemanaEnum.Sabado: return "Sábado";
                default: return string.Empty;
            }
        }

        public static DiaSemanaEnum ObterDiaSemana(DateTime data)
        {
            int dia = (int)data.DayOfWeek + 1;

            return (DiaSemanaEnum)dia;
        }
    }
}
