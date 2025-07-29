using Dominio.Enums.Pessoas;
using Dominio.ValueObjects;

namespace Dominio.Interfaces.Service.Pessoas;

public interface IPessoa
{
    long Id { get; }
    string Nome { get; }
    CPF CPF { get; }
    string Telefone { get; }
    string Email { get; }
    SexoEnum Sexo { get; }
    bool Situacao { get; }
    string Observacao { get; }
} 