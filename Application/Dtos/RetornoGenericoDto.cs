namespace Application.Dtos
{
    public class RetornoGenericoDto<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }
        public List<string> Erros { get; set; } = new();

        public static RetornoGenericoDto<T> ComSucesso(T dados, string mensagem = "")
        {
            return new RetornoGenericoDto<T>
            {
                Sucesso = true,
                Dados = dados,
                Mensagem = mensagem
            };
        }

        public static RetornoGenericoDto<T> ComErro(string mensagem, List<string>? erros = null)
        {
            return new RetornoGenericoDto<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Erros = erros ?? new List<string>()
            };
        }
    }
} 
