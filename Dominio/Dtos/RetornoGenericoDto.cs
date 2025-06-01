using System.Collections.Generic;

namespace Dominio.Dtos
{
    public class RetornoGenericoDto
    {
        public bool Sucesso { get; set; }
        public object Dados { get; set; }
        public List<string> Mensagens { get; set; }
        public int? Pagina { get; set; }
        public int? Limite { get; set; }
        public int? Quantidade { get; set; }
    }
}
