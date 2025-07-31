namespace Application.Dtos
{
    public class ResponseGridDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)Total / TamanhoPagina);
        public bool TemProximaPagina => Pagina < TotalPaginas;
        public bool TemPaginaAnterior => Pagina > 1;
    }
} 
