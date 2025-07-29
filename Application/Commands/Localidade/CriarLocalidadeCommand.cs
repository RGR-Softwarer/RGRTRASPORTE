using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Localidade;

public class CriarLocalidadeCommand : BaseCommand<BaseResponse<long>>
{
    public string Nome { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public CriarLocalidadeCommand()
    {
        // Construtor padrão para model binding
    }

    public CriarLocalidadeCommand(
        string nome,
        string estado,
        string cidade,
        string cep,
        string bairro,
        string logradouro,
        string numero,
        string complemento,
        decimal latitude,
        decimal longitude)
    {
        Nome = nome;
        Estado = estado;
        Cidade = cidade;
        Cep = cep;
        Bairro = bairro;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Latitude = latitude;
        Longitude = longitude;
    }
} 