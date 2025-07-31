using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Enums.Veiculo;

namespace Dominio.Specifications
{
    public class PessoaPodeSerAtivadaSpecification : Specification<Pessoa>
    {
        public override bool IsSatisfiedBy(Pessoa pessoa)
        {
            return !pessoa.Situacao;
        }

        public override string ErrorMessage => "Pessoa já está ativa.";
    }

    public class PessoaPodeSerInativadaSpecification : Specification<Pessoa>
    {
        public override bool IsSatisfiedBy(Pessoa pessoa)
        {
            return pessoa.Situacao;
        }

        public override string ErrorMessage => "Pessoa já está inativa.";
    }

    public class PessoaPodeSerEditadaSpecification : Specification<Pessoa>
    {
        public override bool IsSatisfiedBy(Pessoa pessoa)
        {
            // Pessoas podem sempre ser editadas (adicionar regras específicas se necessário)
            return true;
        }

        public override string ErrorMessage => "Pessoa não pode ser editada.";
    }

    public class NomeValidoSpecification : Specification<string>
    {
        public override bool IsSatisfiedBy(string nome)
        {
            return !string.IsNullOrWhiteSpace(nome) && nome.Length <= 100;
        }

        public override string ErrorMessage => "Nome inválido.";
    }

    public class TelefoneValidoSpecification : Specification<string>
    {
        public override bool IsSatisfiedBy(string telefone)
        {
            return !string.IsNullOrWhiteSpace(telefone) &&
                   telefone.Length >= 10 && telefone.Length <= 11 &&
                   telefone.All(char.IsDigit);
        }

        public override string ErrorMessage => "Telefone inválido.";
    }

    public class EmailValidoSpecification : Specification<string>
    {
        public override bool IsSatisfiedBy(string email)
        {
            return !string.IsNullOrWhiteSpace(email) &&
                   email.Length <= 100 &&
                   email.Contains("@") && email.Contains(".");
        }

        public override string ErrorMessage => "E-mail inválido.";
    }

    public class RGValidoSpecification : Specification<string>
    {
        public override bool IsSatisfiedBy(string rg)
        {
            return !string.IsNullOrWhiteSpace(rg) && rg.Length <= 20;
        }

        public override string ErrorMessage => "RG inválido.";
    }

    public class CNHValidaSpecification : Specification<string>
    {
        public override bool IsSatisfiedBy(string cnh)
        {
            return !string.IsNullOrWhiteSpace(cnh) &&
                   cnh.Length == 11 &&
                   cnh.All(char.IsDigit);
        }

        public override string ErrorMessage => "CNH inválida.";
    }

    public class ValidadeCNHValidaSpecification : Specification<DateTime>
    {
        public override bool IsSatisfiedBy(DateTime validade)
        {
            return validade >= DateTime.Today &&
                   validade <= DateTime.Today.AddYears(10);
        }

        public override string ErrorMessage => "Data de validade da CNH inválida.";
    }

    public class MotoristaPodeDirigirVeiculoSpecification : Specification<(Motorista motorista, TipoModeloVeiculoEnum tipoVeiculo)>
    {
        public override bool IsSatisfiedBy((Motorista motorista, TipoModeloVeiculoEnum tipoVeiculo) dados)
        {
            return dados.motorista.PodeDirigirVeiculo(dados.tipoVeiculo);
        }

        public override string ErrorMessage => "Motorista não habilitado para dirigir este tipo de veículo.";
    }

    public class CNHNaoExpiradaSpecification : Specification<Motorista>
    {
        public override bool IsSatisfiedBy(Motorista motorista)
        {
            return !motorista.CNHExpirada;
        }

        public override string ErrorMessage => "CNH expirada.";
    }

    public class MotoristaPodeRenovarCNHSpecification : Specification<Motorista>
    {
        public override bool IsSatisfiedBy(Motorista motorista)
        {
            return motorista.Situacao; // Só renova CNH de motoristas ativos
        }

        public override string ErrorMessage => "Não é possível renovar CNH de motorista inativo.";
    }
} 
