namespace Dominio.Dtos
{
    public class ParametrosBuscaDto
    {
        public List<FiltroGridDto> Filtros { get; set; } = new();
        public int PaginaAtual { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
        public string? CampoOrdenacao { get; set; }
        public bool Descendente { get; set; }
    }
}
