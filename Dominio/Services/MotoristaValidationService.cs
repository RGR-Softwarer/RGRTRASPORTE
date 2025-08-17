using Dominio.Entidades.Pessoas;
using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;
using Dominio.Interfaces;
using Dominio.Specifications;
using Dominio.ValueObjects;

namespace Dominio.Services
{
    public class MotoristaValidationService
    {
        public bool ValidarCriacao(
            string nome,
            CPF cpf,
            string rg,
            string telefone,
            string email,
            SexoEnum sexo,
            string cnh,
            CategoriaCNHEnum categoriaCNH,
            DateTime validadeCNH,
            string observacao,
            IDomainNotificationContext notificationContext)
        {
            var dadosBasicosSpec = new MotoristaDadosBasicosSpecification();
            var cnhValidaSpec = new MotoristaCNHValidaNotificationSpecification();

            var dadosBasicos = (nome, cpf, rg, telefone, email);
            var dadosCNH = (cnh, categoriaCNH, validadeCNH);

            dadosBasicosSpec.ValidateAndNotify(dadosBasicos, notificationContext);
            cnhValidaSpec.ValidateAndNotify(dadosCNH, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacao(
            Motorista motorista,
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
            // Verificar se o motorista pode ser editado
            var podeSerEditadoSpec = new MotoristaPodeSerEditadoSpecification();
            if (!podeSerEditadoSpec.IsSatisfiedBy(motorista))
            {
                notificationContext.AddNotification(podeSerEditadoSpec.ErrorMessage);
                return false;
            }

            // Validar dados básicos usando service da pessoa base
            var pessoaValidationService = new PessoaValidationService();
            return pessoaValidationService.ValidarDadosBasicos(nome, cpf, telefone, email, notificationContext);
        }

        public bool ValidarRenovacaoCNH(
            Motorista motorista,
            DateTime novaValidadeCNH,
            IDomainNotificationContext notificationContext)
        {
            var cnhPodeSerRenovadaSpec = new MotoristaCNHPodeSerRenovadaSpecification();
            var dadosRenovacao = (motorista, novaValidadeCNH);

            cnhPodeSerRenovadaSpec.ValidateAndNotify(dadosRenovacao, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacaoDocumentos(
            Motorista motorista,
            string rg,
            string cnh,
            CategoriaCNHEnum categoriaCNH,
            DateTime validadeCNH,
            IDomainNotificationContext notificationContext)
        {
            var podeAtualizarDocumentosSpec = new MotoristaPodeAtualizarDocumentosSpecification();
            if (!podeAtualizarDocumentosSpec.IsSatisfiedBy(motorista))
            {
                notificationContext.AddNotification(podeAtualizarDocumentosSpec.ErrorMessage);
                return false;
            }

            var cnhValidaSpec = new MotoristaCNHValidaNotificationSpecification();
            var dadosCNH = (cnh, categoriaCNH, validadeCNH);
            cnhValidaSpec.ValidateAndNotify(dadosCNH, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarHabilitacaoParaVeiculo(
            Motorista motorista,
            TipoModeloVeiculoEnum tipoVeiculo,
            IDomainNotificationContext notificationContext)
        {
            var habilitadoParaVeiculoSpec = new MotoristaHabilitadoParaVeiculoNotificationSpecification();
            var dados = (motorista, tipoVeiculo);

            habilitadoParaVeiculoSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }
    }
}
