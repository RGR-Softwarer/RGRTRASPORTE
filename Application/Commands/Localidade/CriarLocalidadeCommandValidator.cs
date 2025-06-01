using FluentValidation;

namespace Application.Commands.Localidade;

public class CriarLocalidadeCommandValidator : AbstractValidator<CriarLocalidadeCommand>
{
    public CriarLocalidadeCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório")
            .MaximumLength(200)
            .WithMessage("O nome deve ter no máximo 200 caracteres");

        RuleFor(x => x.Cep)
            .NotEmpty()
            .WithMessage("O CEP é obrigatório")
            .Length(8)
            .WithMessage("O CEP deve ter 8 caracteres")
            .Matches(@"^[0-9]{8}$")
            .WithMessage("O CEP deve conter apenas números");

        RuleFor(x => x.Logradouro)
            .NotEmpty()
            .WithMessage("O logradouro é obrigatório")
            .MaximumLength(200)
            .WithMessage("O logradouro deve ter no máximo 200 caracteres");

        RuleFor(x => x.Numero)
            .NotEmpty()
            .WithMessage("O número é obrigatório")
            .MaximumLength(10)
            .WithMessage("O número deve ter no máximo 10 caracteres");

        RuleFor(x => x.Complemento)
            .MaximumLength(100)
            .WithMessage("O complemento deve ter no máximo 100 caracteres");

        RuleFor(x => x.Bairro)
            .NotEmpty()
            .WithMessage("O bairro é obrigatório")
            .MaximumLength(100)
            .WithMessage("O bairro deve ter no máximo 100 caracteres");

        RuleFor(x => x.Cidade)
            .NotEmpty()
            .WithMessage("A cidade é obrigatória")
            .MaximumLength(100)
            .WithMessage("A cidade deve ter no máximo 100 caracteres");

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("O estado é obrigatório")
            .Length(2)
            .WithMessage("O estado deve ter 2 caracteres");

        RuleFor(x => x.Latitude)
            .NotEmpty()
            .WithMessage("A latitude é obrigatória")
            .InclusiveBetween(-90, 90)
            .WithMessage("A latitude deve estar entre -90 e 90");

        RuleFor(x => x.Longitude)
            .NotEmpty()
            .WithMessage("A longitude é obrigatória")
            .InclusiveBetween(-180, 180)
            .WithMessage("A longitude deve estar entre -180 e 180");
    }
} 