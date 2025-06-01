using FluentValidation;

namespace Application.Validators.Base;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected void ValidarGuid(string propertyName)
    {
        RuleFor(x => x.GetType().GetProperty(propertyName).GetValue(x, null))
            .NotEmpty()
            .WithMessage($"O {propertyName} é obrigatório")
            .Must(id => id is Guid && (Guid)id != Guid.Empty)
            .WithMessage($"O {propertyName} informado não é válido");
    }

    protected void ValidarString(string propertyName, int maxLength)
    {
        RuleFor(x => x.GetType().GetProperty(propertyName).GetValue(x, null).ToString())
            .NotEmpty()
            .WithMessage($"O {propertyName} é obrigatório")
            .MaximumLength(maxLength)
            .WithMessage($"O {propertyName} deve ter no máximo {maxLength} caracteres");
    }

    protected void ValidarEmail(string propertyName)
    {
        RuleFor(x => x.GetType().GetProperty(propertyName).GetValue(x, null).ToString())
            .NotEmpty()
            .WithMessage($"O {propertyName} é obrigatório")
            .EmailAddress()
            .WithMessage($"O {propertyName} informado não é válido")
            .MaximumLength(100)
            .WithMessage($"O {propertyName} deve ter no máximo 100 caracteres");
    }

    protected void ValidarData(string propertyName)
    {
        RuleFor(x => (DateTime)x.GetType().GetProperty(propertyName).GetValue(x, null))
            .NotEmpty()
            .WithMessage($"A {propertyName} é obrigatória")
            .Must(data => data != DateTime.MinValue)
            .WithMessage($"A {propertyName} informada não é válida");
    }

    protected void ValidarDecimal(string propertyName, decimal minValue = 0)
    {
        RuleFor(x => (decimal)x.GetType().GetProperty(propertyName).GetValue(x, null))
            .NotEmpty()
            .WithMessage($"O {propertyName} é obrigatório")
            .GreaterThan(minValue)
            .WithMessage($"O {propertyName} deve ser maior que {minValue}");
    }

    protected void ValidarInt(string propertyName, int minValue = 0)
    {
        RuleFor(x => (int)x.GetType().GetProperty(propertyName).GetValue(x, null))
            .NotEmpty()
            .WithMessage($"O {propertyName} é obrigatório")
            .GreaterThan(minValue)
            .WithMessage($"O {propertyName} deve ser maior que {minValue}");
    }
} 