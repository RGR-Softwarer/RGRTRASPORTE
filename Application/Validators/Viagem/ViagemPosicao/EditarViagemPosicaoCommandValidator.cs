using Application.Commands.Viagem.ViagemPosicao;
using FluentValidation;

namespace Application.Validators.Viagem.ViagemPosicao;

public class EditarViagemPosicaoCommandValidator : AbstractValidator<EditarViagemPosicaoCommand>
{
    public EditarViagemPosicaoCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID da posição é obrigatório");

        RuleFor(x => x.Latitude)
            .NotEmpty()
            .WithMessage("A latitude é obrigatória");

        RuleFor(x => x.Longitude)
            .NotEmpty()
            .WithMessage("A longitude é obrigatória");

        RuleFor(x => x.DataPosicao)
            .NotEmpty()
            .WithMessage("A data da posição é obrigatória")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("A data da posição não pode ser maior que a data atual");
    }
} 