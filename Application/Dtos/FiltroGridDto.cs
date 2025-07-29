namespace Application.Dtos
{
    public class FiltroGridDto
    {
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
        public string OrdenarPor { get; set; } = string.Empty;
        public bool Decrescente { get; set; } = false;
    }
} 