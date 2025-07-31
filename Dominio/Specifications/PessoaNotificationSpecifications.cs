using Dominio.Entidades.Pessoas;
using Dominio.Interfaces;
using Dominio.ValueObjects;
using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;

namespace Dominio.Specifications
{
    public class PessoaDadosBasicosSpecification : NotificationSpecification<(string nome, CPF cpf, string telefone, string email)>
    {
        public override bool IsSatisfiedBy((string nome, CPF cpf, string telefone, string email) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.nome) && dados.nome.Length <= 100 &&
                   dados.cpf != null &&
                   !string.IsNullOrWhiteSpace(dados.telefone) && dados.telefone.Length >= 10 && dados.telefone.Length <= 11 &&
                   dados.telefone.All(char.IsDigit) &&
                   !string.IsNullOrWhiteSpace(dados.email) && dados.email.Length <= 100 &&
                   dados.email.Contains("@") && dados.email.Contains(".");
        }

        public override string ErrorMessage => "Dados básicos da pessoa são inválidos.";

        public override void ValidateAndNotify((string nome, CPF cpf, string telefone, string email) dados, INotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.nome))
                notificationContext.AddNotification("Nome é obrigatório");
            else if (dados.nome.Length > 100)
                notificationContext.AddNotification("Nome deve ter no máximo 100 caracteres");

            if (dados.cpf == null)
                notificationContext.AddNotification("CPF é obrigatório");

            if (string.IsNullOrWhiteSpace(dados.telefone))
                notificationContext.AddNotification("Telefone é obrigatório");
            else if (dados.telefone.Length < 10 || dados.telefone.Length > 11)
                notificationContext.AddNotification("Telefone deve ter 10 ou 11 caracteres");
            else if (!dados.telefone.All(char.IsDigit))
                notificationContext.AddNotification("Telefone deve conter apenas números");

            if (string.IsNullOrWhiteSpace(dados.email))
                notificationContext.AddNotification("E-mail é obrigatório");
            else if (dados.email.Length > 100)
                notificationContext.AddNotification("E-mail deve ter no máximo 100 caracteres");
            else if (!dados.email.Contains("@") || !dados.email.Contains("."))
                notificationContext.AddNotification("E-mail inválido");
        }
    }

    public class MotoristaDadosEspecificosSpecification : NotificationSpecification<(string rg, string cnh, DateTime validadeCNH)>
    {
        public override bool IsSatisfiedBy((string rg, string cnh, DateTime validadeCNH) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.rg) && dados.rg.Length <= 20 &&
                   !string.IsNullOrWhiteSpace(dados.cnh) && dados.cnh.Length == 11 && dados.cnh.All(char.IsDigit) &&
                   dados.validadeCNH >= DateTime.Today && dados.validadeCNH <= DateTime.Today.AddYears(10);
        }

        public override string ErrorMessage => "Dados específicos do motorista são inválidos.";

        public override void ValidateAndNotify((string rg, string cnh, DateTime validadeCNH) dados, INotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.rg))
                notificationContext.AddNotification("RG é obrigatório");
            else if (dados.rg.Length > 20)
                notificationContext.AddNotification("RG deve ter no máximo 20 caracteres");

            if (string.IsNullOrWhiteSpace(dados.cnh))
                notificationContext.AddNotification("CNH é obrigatória");
            else if (dados.cnh.Length != 11)
                notificationContext.AddNotification("CNH deve ter 11 caracteres");
            else if (!dados.cnh.All(char.IsDigit))
                notificationContext.AddNotification("CNH deve conter apenas números");

            if (dados.validadeCNH < DateTime.Today)
                notificationContext.AddNotification("Data de validade da CNH não pode ser anterior à data atual");

            if (dados.validadeCNH > DateTime.Today.AddYears(10))
                notificationContext.AddNotification("Data de validade da CNH não pode ser superior a 10 anos");
        }
    }

    public class PessoaPodeSerAtivadaNotificationSpecification : NotificationSpecification<Pessoa>
    {
        public override bool IsSatisfiedBy(Pessoa pessoa)
        {
            return !pessoa.Situacao;
        }

        public override string ErrorMessage => "Pessoa já está ativa.";

        public override void ValidateAndNotify(Pessoa pessoa, INotificationContext notificationContext)
        {
            if (pessoa.Situacao)
                notificationContext.AddNotification("Pessoa já está ativa");
        }
    }

    public class PessoaPodeSerInativadaNotificationSpecification : NotificationSpecification<Pessoa>
    {
        public override bool IsSatisfiedBy(Pessoa pessoa)
        {
            return pessoa.Situacao;
        }

        public override string ErrorMessage => "Pessoa já está inativa.";

        public override void ValidateAndNotify(Pessoa pessoa, INotificationContext notificationContext)
        {
            if (!pessoa.Situacao)
                notificationContext.AddNotification("Pessoa já está inativa");
        }
    }

    public class MotoristaPodeRenovarCNHNotificationSpecification : NotificationSpecification<(Motorista motorista, DateTime novaValidade)>
    {
        public override bool IsSatisfiedBy((Motorista motorista, DateTime novaValidade) dados)
        {
            return dados.motorista.Situacao && 
                   dados.novaValidade >= DateTime.Today && 
                   dados.novaValidade <= DateTime.Today.AddYears(10);
        }

        public override string ErrorMessage => "Não é possível renovar CNH.";

        public override void ValidateAndNotify((Motorista motorista, DateTime novaValidade) dados, INotificationContext notificationContext)
        {
            if (!dados.motorista.Situacao)
                notificationContext.AddNotification("Não é possível renovar CNH de motorista inativo");

            if (dados.novaValidade < DateTime.Today)
                notificationContext.AddNotification("Data de validade da CNH não pode ser anterior à data atual");

            if (dados.novaValidade > DateTime.Today.AddYears(10))
                notificationContext.AddNotification("Data de validade da CNH não pode ser superior a 10 anos");
        }
    }
} 
