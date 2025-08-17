using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Enums.Pessoas;
using Dominio.Interfaces;
using Dominio.Specifications;
using Dominio.ValueObjects;

namespace Dominio.Services
{
    public class PassageiroValidationService
    {
        public bool ValidarCriacao(
            string nome,
            CPF cpf,
            string telefone,
            string email,
            SexoEnum sexo,
            long localidadeId,
            long localidadeEmbarqueId,
            long localidadeDesembarqueId,
            string observacao,
            IDomainNotificationContext notificationContext)
        {
            var dadosBasicosSpec = new PassageiroDadosBasicosSpecification();
            var localidadesValidasSpec = new PassageiroLocalidadesValidasNotificationSpecification();

            var dadosBasicos = (nome, cpf, telefone, email);
            var localidades = (localidadeId, localidadeEmbarqueId, localidadeDesembarqueId);

            dadosBasicosSpec.ValidateAndNotify(dadosBasicos, notificationContext);
            localidadesValidasSpec.ValidateAndNotify(localidades, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacao(
            Passageiro passageiro,
            string nome,
            CPF cpf,
            string telefone,
            string email,
            SexoEnum sexo,
            long localidadeId,
            long localidadeEmbarqueId,
            long localidadeDesembarqueId,
            string observacao,
            bool situacao,
            IDomainNotificationContext notificationContext)
        {
            // Verificar se o passageiro pode ser editado
            var podeSerEditadoSpec = new PassageiroPodeSerEditadoSpecification();
            if (!podeSerEditadoSpec.IsSatisfiedBy(passageiro))
            {
                notificationContext.AddNotification(podeSerEditadoSpec.ErrorMessage);
                return false;
            }

            // Validar dados básicos
            return ValidarCriacao(nome, cpf, telefone, email, sexo, localidadeId, 
                localidadeEmbarqueId, localidadeDesembarqueId, observacao, notificationContext);
        }

        public bool ValidarAtualizacaoLocalidades(
            Passageiro passageiro,
            long localidadeId,
            long localidadeEmbarqueId,
            long localidadeDesembarqueId,
            IDomainNotificationContext notificationContext)
        {
            var podeAtualizarLocalidadesSpec = new PassageiroPodeAtualizarLocalidadesSpecification();
            if (!podeAtualizarLocalidadesSpec.IsSatisfiedBy(passageiro))
            {
                notificationContext.AddNotification(podeAtualizarLocalidadesSpec.ErrorMessage);
                return false;
            }

            var localidadesValidasSpec = new PassageiroLocalidadesValidasNotificationSpecification();
            var localidades = (localidadeId, localidadeEmbarqueId, localidadeDesembarqueId);
            localidadesValidasSpec.ValidateAndNotify(localidades, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtivacao(
            Passageiro passageiro,
            IDomainNotificationContext notificationContext)
        {
            var podeSerAtivadoSpec = new PassageiroPodeSerAtivadoSpecification();
            if (!podeSerAtivadoSpec.IsSatisfiedBy(passageiro))
            {
                notificationContext.AddNotification(podeSerAtivadoSpec.ErrorMessage);
                return false;
            }

            return true;
        }

        public bool ValidarInativacao(
            Passageiro passageiro,
            IDomainNotificationContext notificationContext)
        {
            var podeSerInativadoSpec = new PassageiroPodeSerInativadoNotificationSpecification();
            podeSerInativadoSpec.ValidateAndNotify(passageiro, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarEmbarqueEmViagem(
            Passageiro passageiro,
            long viagemId,
            IDomainNotificationContext notificationContext)
        {
            var podeEmbarcarSpec = new PassageiroPodeEmbarcarEmViagemSpecification();
            var dados = (passageiro, viagemId);

            podeEmbarcarSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }
    }
}
