using Application.Commands.Passageiro;
using FluentValidation;

namespace Application.Validators.Passageiro;

public class EditarPassageiroCommandValidator : AbstractValidator<EditarPassageiroCommand>
{
    public EditarPassageiroCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID do passageiro é obrigatório");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório")
            .MaximumLength(100)
            .WithMessage("O nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.CPF)
            .NotEmpty()
            .WithMessage("O CPF é obrigatório")
            .Length(11)
            .WithMessage("O CPF deve ter 11 caracteres");

        RuleFor(x => x.Telefone)
            .NotEmpty()
            .WithMessage("O telefone é obrigatório")
            .MaximumLength(20)
            .WithMessage("O telefone deve ter no máximo 20 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório")
            .EmailAddress()
            .WithMessage("O e-mail informado não é válido")
            .MaximumLength(100)
            .WithMessage("O e-mail deve ter no máximo 100 caracteres");

        RuleFor(x => x.Sexo)
            .IsInEnum()
            .WithMessage("O sexo informado não é válido");

        RuleFor(x => x.LocalidadeId)
            .GreaterThan(0)
            .WithMessage("A localidade é obrigatória");

        RuleFor(x => x.LocalidadeEmbarqueId)
            .GreaterThan(0)
            .WithMessage("A localidade de embarque é obrigatória");

        RuleFor(x => x.LocalidadeDesembarqueId)
            .GreaterThan(0)
            .WithMessage("A localidade de desembarque é obrigatória")
            .NotEqual(x => x.LocalidadeEmbarqueId)
            .WithMessage("A localidade de desembarque deve ser diferente da localidade de embarque");

        RuleFor(x => x.Observacao)
            .MaximumLength(500)
            .WithMessage("A observação deve ter no máximo 500 caracteres");
    }
} 