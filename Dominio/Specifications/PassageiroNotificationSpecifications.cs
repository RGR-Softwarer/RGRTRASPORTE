using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Interfaces;
using Dominio.ValueObjects;

namespace Dominio.Specifications
{
    public class PassageiroDadosBasicosSpecification : NotificationSpecification<(string nome, CPF cpf, string telefone, string email)>
    {
        public override bool IsSatisfiedBy((string nome, CPF cpf, string telefone, string email) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.nome) &&
                   dados.nome.Length <= 100 &&
                   dados.cpf != null &&
                   !string.IsNullOrWhiteSpace(dados.telefone) &&
                   !string.IsNullOrWhiteSpace(dados.email);
        }

        public override string ErrorMessage => "Dados básicos do passageiro são inválidos";

        public override void ValidateAndNotify((string nome, CPF cpf, string telefone, string email) dados, INotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.nome))
                notificationContext.AddNotification("Nome do passageiro é obrigatório");

            if (dados.nome?.Length > 100)
                notificationContext.AddNotification("Nome do passageiro deve ter no máximo 100 caracteres");

            if (dados.cpf == null)
                notificationContext.AddNotification("CPF do passageiro é obrigatório");

            if (string.IsNullOrWhiteSpace(dados.telefone))
                notificationContext.AddNotification("Telefone do passageiro é obrigatório");

            if (string.IsNullOrWhiteSpace(dados.email))
                notificationContext.AddNotification("Email do passageiro é obrigatório");
        }
    }

    public class PassageiroLocalidadesValidasNotificationSpecification : NotificationSpecification<(long localidadeId, long localidadeEmbarqueId, long localidadeDesembarqueId)>
    {
        public override bool IsSatisfiedBy((long localidadeId, long localidadeEmbarqueId, long localidadeDesembarqueId) dados)
        {
            return dados.localidadeId > 0 &&
                   dados.localidadeEmbarqueId > 0 &&
                   dados.localidadeDesembarqueId > 0 &&
                   dados.localidadeEmbarqueId != dados.localidadeDesembarqueId;
        }

        public override string ErrorMessage => "Localidades do passageiro são inválidas";

        public override void ValidateAndNotify((long localidadeId, long localidadeEmbarqueId, long localidadeDesembarqueId) dados, INotificationContext notificationContext)
        {
            if (dados.localidadeId <= 0)
                notificationContext.AddNotification("Localidade principal é obrigatória");

            if (dados.localidadeEmbarqueId <= 0)
                notificationContext.AddNotification("Localidade de embarque é obrigatória");

            if (dados.localidadeDesembarqueId <= 0)
                notificationContext.AddNotification("Localidade de desembarque é obrigatória");

            if (dados.localidadeEmbarqueId == dados.localidadeDesembarqueId)
                notificationContext.AddNotification("Localidade de embarque não pode ser igual à de desembarque");
        }
    }

    public class PassageiroPodeSerInativadoNotificationSpecification : NotificationSpecification<Passageiro>
    {
        public override bool IsSatisfiedBy(Passageiro passageiro)
        {
            return passageiro.Situacao;
        }

        public override string ErrorMessage => "Passageiro não pode ser inativado";

        public override void ValidateAndNotify(Passageiro passageiro, INotificationContext notificationContext)
        {
            if (!passageiro.Situacao)
                notificationContext.AddNotification("Passageiro já está inativo");

            // Adicionar validações específicas como:
            // - Não tem viagens em andamento
            // - Não tem reservas futuras
            // etc.
        }
    }

    public class PassageiroPodeEmbarcarEmViagemSpecification : NotificationSpecification<(Passageiro passageiro, long viagemId)>
    {
        public override bool IsSatisfiedBy((Passageiro passageiro, long viagemId) dados)
        {
            return dados.passageiro.Situacao &&
                   dados.passageiro.LocalidadeEmbarqueId.HasValue &&
                   dados.passageiro.LocalidadeDesembarqueId.HasValue &&
                   dados.viagemId > 0;
        }

        public override string ErrorMessage => "Passageiro não pode embarcar na viagem";

        public override void ValidateAndNotify((Passageiro passageiro, long viagemId) dados, INotificationContext notificationContext)
        {
            if (!dados.passageiro.Situacao)
                notificationContext.AddNotification("Passageiro está inativo");

            if (!dados.passageiro.LocalidadeEmbarqueId.HasValue)
                notificationContext.AddNotification("Passageiro não possui localidade de embarque definida");

            if (!dados.passageiro.LocalidadeDesembarqueId.HasValue)
                notificationContext.AddNotification("Passageiro não possui localidade de desembarque definida");

            if (dados.viagemId <= 0)
                notificationContext.AddNotification("Viagem é inválida");
        }
    }
}
