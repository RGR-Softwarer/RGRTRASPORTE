namespace Dominio.Dtos
{
    public class ResponseGridDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
    }
}
