using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validators.Base;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    // Regex patterns for common validations
    protected static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    protected static readonly Regex PhonePattern = new(@"^\d{10,11}$", RegexOptions.Compiled);
    protected static readonly Regex CpfPattern = new(@"^\d{11}$", RegexOptions.Compiled);
    protected static readonly Regex CepPattern = new(@"^\d{8}$", RegexOptions.Compiled);
    protected static readonly Regex PlacaPattern = new(@"^[A-Z]{3}\d{4}$|^[A-Z]{3}\d[A-Z]\d{2}$", RegexOptions.Compiled);

    // Extension methods for common validations
    protected IRuleBuilderOptions<T, string> ValidarTextoObrigatorio(
        IRuleBuilder<T, string> ruleBuilder, 
        string fieldName, 
        int maxLength = 100)
    {
        return ruleBuilder
            .NotEmpty().WithMessage($"O {fieldName} é obrigatório")
            .MaximumLength(maxLength).WithMessage($"O {fieldName} deve ter no máximo {maxLength} caracteres");
    }

    protected IRuleBuilderOptions<T, string> ValidarEmail(IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .Matches(EmailPattern).WithMessage("O e-mail informado não é válido")
            .MaximumLength(100).WithMessage("O e-mail deve ter no máximo 100 caracteres");
    }

    protected IRuleBuilderOptions<T, string> ValidarTelefone(IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("O telefone é obrigatório")
            .Matches(PhonePattern).WithMessage("O telefone deve ter 10 ou 11 dígitos");
    }

    protected IRuleBuilderOptions<T, string> ValidarCpf(IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("O CPF é obrigatório")
            .Length(11).WithMessage("O CPF deve ter 11 caracteres")
            .Matches(CpfPattern).WithMessage("O CPF deve conter apenas números")
            .Must(ValidarCpfDigitos).WithMessage("CPF inválido");
    }

    protected IRuleBuilderOptions<T, DateTime> ValidarDataObrigatoria(
        IRuleBuilder<T, DateTime> ruleBuilder, 
        string fieldName)
    {
        return ruleBuilder
            .NotEmpty().WithMessage($"A {fieldName} é obrigatória")
            .Must(data => data != DateTime.MinValue).WithMessage($"A {fieldName} informada não é válida");
    }

    protected IRuleBuilderOptions<T, DateTime> ValidarDataFutura(
        IRuleBuilder<T, DateTime> ruleBuilder, 
        string fieldName)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage($"A {fieldName} deve ser maior ou igual a data atual");
    }

    protected IRuleBuilderOptions<T, long> ValidarIdObrigatorio(
        IRuleBuilder<T, long> ruleBuilder, 
        string fieldName)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage($"O {fieldName} é obrigatório");
    }

    protected IRuleBuilderOptions<T, decimal> ValidarValorPositivo(
        IRuleBuilder<T, decimal> ruleBuilder, 
        string fieldName)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage($"O {fieldName} deve ser maior que zero");
    }

    protected IRuleBuilderOptions<T, int> ValidarQuantidadePositiva(
        IRuleBuilder<T, int> ruleBuilder, 
        string fieldName)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage($"A {fieldName} deve ser maior que zero");
    }

    // CPF validation algorithm
    private static bool ValidarCpfDigitos(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            return false;

        // Check for known invalid CPFs
        if (new string(cpf[0], 11) == cpf)
            return false;

        // Validate first digit
        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;
        
        if (int.Parse(cpf[9].ToString()) != digit1)
            return false;

        // Validate second digit
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;
        
        return int.Parse(cpf[10].ToString()) == digit2;
    }
} 