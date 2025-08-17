using Dominio.Entidades.Localidades;
using Dominio.Interfaces;
using Dominio.Specifications;
using Dominio.ValueObjects;

namespace Dominio.Services
{
    public class LocalidadeValidationService
    {
        public bool ValidarCriacao(
            string nome,
            Endereco endereco,
            decimal latitude,
            decimal longitude,
            IDomainNotificationContext notificationContext)
        {
            var dadosBasicosSpec = new LocalidadeDadosBasicosSpecification();
            var dados = (nome, endereco, latitude, longitude);

            dadosBasicosSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacao(
            Localidade localidade,
            string nome,
            Endereco endereco,
            decimal latitude,
            decimal longitude,
            IDomainNotificationContext notificationContext)
        {
            // Validar se a localidade pode ser editada
            var podeSerEditadaSpec = new LocalidadePodeSerEditadaSpecification();
            if (!podeSerEditadaSpec.IsSatisfiedBy(localidade))
            {
                notificationContext.AddNotification(podeSerEditadaSpec.ErrorMessage);
                return false;
            }

            // Validar dados básicos
            var dadosBasicosSpec = new LocalidadeDadosBasicosSpecification();
            var dados = (nome, endereco, latitude, longitude);

            dadosBasicosSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtivacao(
            Localidade localidade,
            IDomainNotificationContext notificationContext)
        {
            var podeSerAtivadaSpec = new LocalidadePodeSerAtivadaNotificationSpecification();
            podeSerAtivadaSpec.ValidateAndNotify(localidade, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarInativacao(
            Localidade localidade,
            IDomainNotificationContext notificationContext)
        {
            var podeSerInativadaSpec = new LocalidadePodeSerInativadaNotificationSpecification();
            podeSerInativadaSpec.ValidateAndNotify(localidade, notificationContext);

            return !notificationContext.HasNotifications();
        }
    }
} 
