namespace Application.Common;

public class BaseResponse<T>
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public T Dados { get; set; }
    public List<string> Erros { get; set; }

    public BaseResponse()
    {
        Sucesso = true;
        Erros = new List<string>();
    }

    public static BaseResponse<T> Ok(T dados, string mensagem = "Operação realizada com sucesso")
    {
        return new BaseResponse<T>
        {
            Sucesso = true,
            Mensagem = mensagem,
            Dados = dados
        };
    }

    public static BaseResponse<T> Erro(string mensagem, List<string> erros = null)
    {
        return new BaseResponse<T>
        {
            Sucesso = false,
            Mensagem = mensagem,
            Erros = erros ?? new List<string>()
        };
    }
} 
