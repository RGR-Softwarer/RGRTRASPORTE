using Application.Commands.Localidade;
using Application.Validators.Base;
using FluentValidation;

namespace Application.Validators.Localidade;

public class CriarLocalidadeCommandValidator : BaseValidator<CriarLocalidadeCommand>
{
    public CriarLocalidadeCommandValidator()
    {
        ValidarTextoObrigatorio(RuleFor(x => x.Nome), "nome", 100);

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("O estado é obrigatório")
            .Length(2)
            .WithMessage("O estado deve ter 2 caracteres");

        ValidarTextoObrigatorio(RuleFor(x => x.Cidade), "cidade", 100);

        RuleFor(x => x.Cep)
            .NotEmpty()
            .WithMessage("O CEP é obrigatório")
            .Length(8)
            .WithMessage("O CEP deve ter 8 caracteres")
            .Matches(@"^\d{8}$")
            .WithMessage("O CEP deve conter apenas números");

        ValidarTextoObrigatorio(RuleFor(x => x.Bairro), "bairro", 100);
        ValidarTextoObrigatorio(RuleFor(x => x.Logradouro), "logradouro", 100);
        ValidarTextoObrigatorio(RuleFor(x => x.Numero), "número", 10);

        RuleFor(x => x.Complemento)
            .MaximumLength(100)
            .WithMessage("O complemento deve ter no máximo 100 caracteres");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("A latitude deve estar entre -90 e 90 graus");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("A longitude deve estar entre -180 e 180 graus");
    }
} 