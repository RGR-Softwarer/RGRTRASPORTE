using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Localidade;

public class EditarLocalidadeCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }
    public string Nome { get; private set; }
    public string Estado { get; private set; }
    public string Cidade { get; private set; }
    public string Cep { get; private set; }
    public string Bairro { get; private set; }
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public bool Ativo { get; private set; }

    public EditarLocalidadeCommand(
        long id,
        string nome,
        string estado,
        string cidade,
        string cep,
        string bairro,
        string logradouro,
        string numero,
        string complemento,
        decimal latitude,
        decimal longitude,
        bool ativo)
    {
        Id = id;
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
        Ativo = ativo;
    }
} 
