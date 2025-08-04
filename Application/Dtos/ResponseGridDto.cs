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

        public ResponseGridDto()
        {
        }

        public ResponseGridDto(IEnumerable<T> items, int total, int pagina, int tamanhoPagina)
        {
            Items = items ?? new List<T>();
            Total = total;
            Pagina = pagina;
            TamanhoPagina = tamanhoPagina;
        }

        public static ResponseGridDto<T> Create(IEnumerable<T> items, int total, int pagina, int tamanhoPagina)
        {
            return new ResponseGridDto<T>(items, total, pagina, tamanhoPagina);
        }

        public static ResponseGridDto<T> CreateEmpty(int pagina = 1, int tamanhoPagina = 10)
        {
            return new ResponseGridDto<T>(new List<T>(), 0, pagina, tamanhoPagina);
        }
    }
} 
