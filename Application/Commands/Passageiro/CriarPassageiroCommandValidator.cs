using FluentValidation;

namespace Application.Commands.Passageiro;

public class CriarPassageiroCommandValidator : AbstractValidator<CriarPassageiroCommand>
{
    public CriarPassageiroCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório")
            .MaximumLength(200)
            .WithMessage("O nome deve ter no máximo 200 caracteres");

        RuleFor(x => x.CPF)
            .NotEmpty()
            .WithMessage("O documento é obrigatório")
            .MaximumLength(20)
            .WithMessage("O documento deve ter no máximo 20 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O email é obrigatório")
            .EmailAddress()
            .WithMessage("O email informado não é válido")
            .MaximumLength(100)
            .WithMessage("O email deve ter no máximo 100 caracteres");

        RuleFor(x => x.Telefone)
            .NotEmpty()
            .WithMessage("O telefone é obrigatório")
            .MaximumLength(20)
            .WithMessage("O telefone deve ter no máximo 20 caracteres");

        RuleFor(x => x.LocalidadeId)
            .NotEmpty()
            .WithMessage("O endereço é obrigatório")
            .WithMessage("O endereço deve ter no máximo 500 caracteres");
    }
} 
