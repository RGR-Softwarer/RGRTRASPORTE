using Dominio.Entidades.Veiculos;

namespace Dominio.Specifications
{
    public class VeiculoPodeSerAtivadoSpecification : Specification<Veiculo>
    {
        public override bool IsSatisfiedBy(Veiculo veiculo)
        {
            return !veiculo.Situacao;
        }

        public override string ErrorMessage => "Veículo já está ativo.";
    }

    public class VeiculoPodeSerInativadoSpecification : Specification<Veiculo>
    {
        public override bool IsSatisfiedBy(Veiculo veiculo)
        {
            return veiculo.Situacao;
        }

        public override string ErrorMessage => "Veículo já está inativo.";
    }

    public class VeiculoPodeSerEditadoSpecification : Specification<Veiculo>
    {
        public override bool IsSatisfiedBy(Veiculo veiculo)
        {
            // Veículos podem sempre ser editados (adicionar regras específicas se necessário)
            return true;
        }

        public override string ErrorMessage => "Veículo não pode ser editado.";
    }

    public class VeiculoPodeAtualizarLicenciamentoSpecification : Specification<Veiculo>
    {
        public override bool IsSatisfiedBy(Veiculo veiculo)
        {
            return veiculo.Situacao; // Só atualiza licenciamento de veículos ativos
        }

        public override string ErrorMessage => "Não é possível atualizar licenciamento de veículo inativo.";
    }

    public class VencimentoLicenciamentoValidoSpecification : Specification<DateTime>
    {
        public override bool IsSatisfiedBy(DateTime vencimento)
        {
            return vencimento >= DateTime.Today;
        }

        public override string ErrorMessage => "Data de vencimento do licenciamento não pode ser anterior à data atual.";
    }

    public class ChassiValidoSpecification : Specification<string>
    {
        public override bool IsSatisfiedBy(string chassi)
        {
            return !string.IsNullOrWhiteSpace(chassi) &&
                   chassi.Length == 17 &&
                   chassi.All(c => char.IsLetterOrDigit(c));
        }

        public override string ErrorMessage => "Número do chassi inválido.";
    }

    public class RenavamValidoSpecification : Specification<string>
    {
        public override bool IsSatisfiedBy(string renavam)
        {
            return !string.IsNullOrWhiteSpace(renavam) &&
                   renavam.Length == 11 &&
                   renavam.All(char.IsDigit);
        }

        public override string ErrorMessage => "RENAVAM inválido.";
    }

    public class AnosVeiculoValidosSpecification : Specification<(int anoModelo, int anoFabricacao)>
    {
        public override bool IsSatisfiedBy((int anoModelo, int anoFabricacao) anos)
        {
            var anoAtual = DateTime.Now.Year;
            return anos.anoModelo >= anos.anoFabricacao &&
                   anos.anoModelo <= anoAtual + 1 &&
                   anos.anoFabricacao >= 1900 &&
                   anos.anoFabricacao <= anoAtual;
        }

        public override string ErrorMessage => "Anos do veículo são inválidos.";
    }
} 
