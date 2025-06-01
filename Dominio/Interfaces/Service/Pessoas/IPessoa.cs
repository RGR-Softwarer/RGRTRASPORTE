using Dominio.Enums.Pessoas;

namespace Dominio.Interfaces.Service.Pessoas;

public interface IPessoa
{
    long Id { get; }
    string Nome { get; }
    string CPF { get; }
    string Telefone { get; }
    string Email { get; }
    SexoEnum Sexo { get; }
    bool Situacao { get; }
    string Observacao { get; }
} 