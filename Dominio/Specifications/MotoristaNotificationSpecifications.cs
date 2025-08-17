using Dominio.Entidades.Pessoas;
using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;
using Dominio.Interfaces;
using Dominio.ValueObjects;

namespace Dominio.Specifications
{
    public class MotoristaDadosBasicosSpecification : NotificationSpecification<(string nome, CPF cpf, string rg, string telefone, string email)>
    {
        public override bool IsSatisfiedBy((string nome, CPF cpf, string rg, string telefone, string email) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.nome) &&
                   dados.nome.Length <= 100 &&
                   dados.cpf != null &&
                   !string.IsNullOrWhiteSpace(dados.rg) &&
                   dados.rg.Length <= 20 &&
                   !string.IsNullOrWhiteSpace(dados.telefone) &&
                   !string.IsNullOrWhiteSpace(dados.email);
        }

        public override string ErrorMessage => "Dados básicos do motorista são inválidos";

        public override void ValidateAndNotify((string nome, CPF cpf, string rg, string telefone, string email) dados, IDomainNotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.nome))
                notificationContext.AddNotification("Nome do motorista é obrigatório");

            if (dados.nome?.Length > 100)
                notificationContext.AddNotification("Nome do motorista deve ter no máximo 100 caracteres");

            if (dados.cpf == null)
                notificationContext.AddNotification("CPF do motorista é obrigatório");

            if (string.IsNullOrWhiteSpace(dados.rg))
                notificationContext.AddNotification("RG do motorista é obrigatório");

            if (dados.rg?.Length > 20)
                notificationContext.AddNotification("RG deve ter no máximo 20 caracteres");

            if (string.IsNullOrWhiteSpace(dados.telefone))
                notificationContext.AddNotification("Telefone do motorista é obrigatório");

            if (string.IsNullOrWhiteSpace(dados.email))
                notificationContext.AddNotification("Email do motorista é obrigatório");
        }
    }

    public class MotoristaCNHValidaNotificationSpecification : NotificationSpecification<(string cnh, CategoriaCNHEnum categoria, DateTime validade)>
    {
        public override bool IsSatisfiedBy((string cnh, CategoriaCNHEnum categoria, DateTime validade) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.cnh) &&
                   dados.cnh.Length == 11 &&
                   dados.cnh.All(char.IsDigit) &&
                   dados.validade >= DateTime.Today &&
                   Enum.IsDefined(typeof(CategoriaCNHEnum), dados.categoria);
        }

        public override string ErrorMessage => "Dados da CNH são inválidos";

        public override void ValidateAndNotify((string cnh, CategoriaCNHEnum categoria, DateTime validade) dados, IDomainNotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.cnh))
                notificationContext.AddNotification("Número da CNH é obrigatório");
            else if (dados.cnh.Length != 11)
                notificationContext.AddNotification("CNH deve ter 11 dígitos");
            else if (!dados.cnh.All(char.IsDigit))
                notificationContext.AddNotification("CNH deve conter apenas números");

            if (dados.validade < DateTime.Today)
                notificationContext.AddNotification("Data de validade da CNH não pode ser anterior à data atual");

            if (!Enum.IsDefined(typeof(CategoriaCNHEnum), dados.categoria))
                notificationContext.AddNotification("Categoria da CNH é inválida");
        }
    }

    public class MotoristaCNHPodeSerRenovadaSpecification : NotificationSpecification<(Motorista motorista, DateTime novaValidade)>
    {
        public override bool IsSatisfiedBy((Motorista motorista, DateTime novaValidade) dados)
        {
            return dados.motorista.Situacao &&
                   dados.novaValidade > DateTime.Today &&
                   dados.novaValidade > dados.motorista.ValidadeCNH;
        }

        public override string ErrorMessage => "CNH não pode ser renovada";

        public override void ValidateAndNotify((Motorista motorista, DateTime novaValidade) dados, IDomainNotificationContext notificationContext)
        {
            if (!dados.motorista.Situacao)
                notificationContext.AddNotification("Não é possível renovar CNH de motorista inativo");

            if (dados.novaValidade <= DateTime.Today)
                notificationContext.AddNotification("Nova data de validade deve ser futura");

            if (dados.novaValidade <= dados.motorista.ValidadeCNH)
                notificationContext.AddNotification("Nova data de validade deve ser posterior à validade atual");
        }
    }

    public class MotoristaHabilitadoParaVeiculoNotificationSpecification : NotificationSpecification<(Motorista motorista, TipoModeloVeiculoEnum tipoVeiculo)>
    {
        public override bool IsSatisfiedBy((Motorista motorista, TipoModeloVeiculoEnum tipoVeiculo) dados)
        {
            return dados.motorista.PodeDirigirVeiculo(dados.tipoVeiculo) && !dados.motorista.CNHExpirada;
        }

        public override string ErrorMessage => "Motorista não está habilitado para dirigir este tipo de veículo";

        public override void ValidateAndNotify((Motorista motorista, TipoModeloVeiculoEnum tipoVeiculo) dados, IDomainNotificationContext notificationContext)
        {
            if (!dados.motorista.PodeDirigirVeiculo(dados.tipoVeiculo))
                notificationContext.AddNotification($"Categoria da CNH ({dados.motorista.CategoriaCNH}) não permite dirigir {dados.tipoVeiculo}");

            if (dados.motorista.CNHExpirada)
                notificationContext.AddNotification("CNH do motorista está expirada");
        }
    }
}
