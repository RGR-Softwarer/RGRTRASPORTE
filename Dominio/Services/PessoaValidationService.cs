using Dominio.Entidades.Pessoas;
using Dominio.Interfaces;
using Dominio.Specifications;
using Dominio.ValueObjects;
using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;

namespace Dominio.Services
{
    public class PessoaValidationService
    {
        public bool ValidarDadosBasicos(
            string nome,
            CPF cpf,
            string telefone,
            string email,
            IDomainNotificationContext notificationContext)
        {
            var dadosBasicosSpec = new PessoaDadosBasicosSpecification();
            var dados = (nome, cpf, telefone, email);

            dadosBasicosSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacao(
            Pessoa pessoa,
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
            // Validar se a pessoa pode ser editada
            var podeSerEditadaSpec = new PessoaPodeSerEditadaSpecification();
            if (!podeSerEditadaSpec.IsSatisfiedBy(pessoa))
            {
                notificationContext.AddNotification(podeSerEditadaSpec.ErrorMessage);
                return false;
            }

            // Validar dados básicos
            return ValidarDadosBasicos(nome, cpf, telefone, email, notificationContext);
        }

        public bool ValidarAtivacao(
            Pessoa pessoa,
            IDomainNotificationContext notificationContext)
        {
            var podeSerAtivadaSpec = new PessoaPodeSerAtivadaNotificationSpecification();
            podeSerAtivadaSpec.ValidateAndNotify(pessoa, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarInativacao(
            Pessoa pessoa,
            IDomainNotificationContext notificationContext)
        {
            var podeSerInativadaSpec = new PessoaPodeSerInativadaNotificationSpecification();
            podeSerInativadaSpec.ValidateAndNotify(pessoa, notificationContext);

            return !notificationContext.HasNotifications();
        }

        // Validações específicas para Motorista
        public bool ValidarDadosMotorista(
            string rg,
            string cnh,
            DateTime validadeCNH,
            IDomainNotificationContext notificationContext)
        {
            var dadosEspecificosSpec = new MotoristaDadosEspecificosSpecification();
            var dados = (rg, cnh, validadeCNH);

            dadosEspecificosSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarRenovacaoCNH(
            Motorista motorista,
            DateTime novaValidade,
            IDomainNotificationContext notificationContext)
        {
            var podeRenovarCNHSpec = new MotoristaPodeRenovarCNHNotificationSpecification();
            var dados = (motorista, novaValidade);

            podeRenovarCNHSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }
    }
} 
