using FluentValidation;

namespace Application.Commands.Viagem;

public class CriarViagemCommandValidator : AbstractValidator<CriarViagemCommand>
{
    public CriarViagemCommandValidator()
    {
        RuleFor(x => x.DataViagem)
            .NotEmpty()
            .WithMessage("A data da viagem é obrigatória")
            .GreaterThan(DateTime.Now)
            .WithMessage("A data da viagem deve ser maior que a data atual");

        RuleFor(x => x.VeiculoId)
            .NotEmpty()
            .WithMessage("O veículo é obrigatório");

        RuleFor(x => x.MotoristaId)
            .NotEmpty()
            .WithMessage("O motorista é obrigatório");

        RuleFor(x => x.LocalidadeOrigemId)
            .NotEmpty()
            .WithMessage("A origem é obrigatória")
            .WithMessage("A origem deve ter no máximo 200 caracteres");

        RuleFor(x => x.LocalidadeDestinoId)
            .NotEmpty()
            .WithMessage("O destino é obrigatório")
            .WithMessage("O destino deve ter no máximo 200 caracteres");
    }
} 