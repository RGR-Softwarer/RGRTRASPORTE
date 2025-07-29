using Dominio.Entidades.Localidades;
using Dominio.Interfaces;
using Dominio.ValueObjects;

namespace Dominio.Specifications
{
    public class LocalidadeDadosBasicosSpecification : NotificationSpecification<(string nome, Endereco endereco, decimal latitude, decimal longitude)>
    {
        public override bool IsSatisfiedBy((string nome, Endereco endereco, decimal latitude, decimal longitude) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.nome) &&
                   dados.nome.Length <= 100 &&
                   dados.endereco != null &&
                   dados.latitude >= -90 && dados.latitude <= 90 &&
                   dados.longitude >= -180 && dados.longitude <= 180;
        }

        public override string ErrorMessage => "Dados básicos da localidade são inválidos.";

        public override void ValidateAndNotify((string nome, Endereco endereco, decimal latitude, decimal longitude) dados, INotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.nome))
                notificationContext.AddNotification("Nome é obrigatório");

            if (!string.IsNullOrWhiteSpace(dados.nome) && dados.nome.Length > 100)
                notificationContext.AddNotification("Nome deve ter no máximo 100 caracteres");

            if (dados.endereco == null)
                notificationContext.AddNotification("Endereço é obrigatório");

            if (dados.latitude < -90 || dados.latitude > 90)
                notificationContext.AddNotification("Latitude deve estar entre -90 e 90");

            if (dados.longitude < -180 || dados.longitude > 180)
                notificationContext.AddNotification("Longitude deve estar entre -180 e 180");
        }
    }

    public class LocalidadePodeSerAtivadaNotificationSpecification : NotificationSpecification<Localidade>
    {
        public override bool IsSatisfiedBy(Localidade localidade)
        {
            return !localidade.Ativo;
        }

        public override string ErrorMessage => "Localidade já está ativa.";

        public override void ValidateAndNotify(Localidade localidade, INotificationContext notificationContext)
        {
            if (localidade.Ativo)
                notificationContext.AddNotification("Localidade já está ativa");
        }
    }

    public class LocalidadePodeSerInativadaNotificationSpecification : NotificationSpecification<Localidade>
    {
        public override bool IsSatisfiedBy(Localidade localidade)
        {
            return localidade.Ativo;
        }

        public override string ErrorMessage => "Localidade já está inativa.";

        public override void ValidateAndNotify(Localidade localidade, INotificationContext notificationContext)
        {
            if (!localidade.Ativo)
                notificationContext.AddNotification("Localidade já está inativa");
        }
    }
} 