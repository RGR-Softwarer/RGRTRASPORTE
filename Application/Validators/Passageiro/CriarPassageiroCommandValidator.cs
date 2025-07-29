using Application.Commands.Passageiro;
using Application.Validators.Base;
using FluentValidation;

namespace Application.Validators.Passageiro;

public class CriarPassageiroCommandValidator : BaseValidator<CriarPassageiroCommand>
{
    public CriarPassageiroCommandValidator()
    {
        // Usando os métodos do BaseValidator para evitar duplicação
        ValidarTextoObrigatorio(RuleFor(x => x.Nome), "nome", 100);
        
        ValidarCpf(RuleFor(x => x.CPF));
        
        ValidarTelefone(RuleFor(x => x.Telefone));
        
        ValidarEmail(RuleFor(x => x.Email));

        RuleFor(x => x.Sexo)
            .IsInEnum()
            .WithMessage("O sexo informado não é válido");

        ValidarIdObrigatorio(RuleFor(x => x.LocalidadeId), "localidade");
        
        ValidarIdObrigatorio(RuleFor(x => x.LocalidadeEmbarqueId), "localidade de embarque");
        
        ValidarIdObrigatorio(RuleFor(x => x.LocalidadeDesembarqueId), "localidade de desembarque");
        
        RuleFor(x => x.LocalidadeDesembarqueId)
            .NotEqual(x => x.LocalidadeEmbarqueId)
            .WithMessage("A localidade de desembarque deve ser diferente da localidade de embarque");
    }
} 