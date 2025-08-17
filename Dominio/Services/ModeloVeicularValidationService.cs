using Dominio.Entidades.Veiculos;
using Dominio.Enums.Veiculo;
using Dominio.Interfaces;
using Dominio.Specifications;

namespace Dominio.Services
{
    public class ModeloVeicularValidationService
    {
        public bool ValidarCriacao(
            string descricaoModelo,
            TipoModeloVeiculoEnum tipo,
            int quantidadeAssento,
            int quantidadeEixo,
            int capacidadeMaxima,
            int passageirosEmPe,
            bool possuiBanheiro,
            bool possuiClimatizador,
            IDomainNotificationContext notificationContext)
        {
            var dadosBasicosSpec = new ModeloVeicularDadosBasicosSpecification();
            var capacidadeValidaSpec = new ModeloVeicularCapacidadeValidaNotificationSpecification();

            var dadosBasicos = (descricaoModelo, tipo, quantidadeAssento, quantidadeEixo);
            var dadosCapacidade = (quantidadeAssento, capacidadeMaxima, passageirosEmPe);

            dadosBasicosSpec.ValidateAndNotify(dadosBasicos, notificationContext);
            capacidadeValidaSpec.ValidateAndNotify(dadosCapacidade, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacao(
            ModeloVeicular modelo,
            string descricaoModelo,
            TipoModeloVeiculoEnum tipo,
            int quantidadeAssento,
            int quantidadeEixo,
            int capacidadeMaxima,
            int passageirosEmPe,
            bool possuiBanheiro,
            bool possuiClimatizador,
            IDomainNotificationContext notificationContext)
        {
            // Verificar se o modelo pode ser editado
            var podeSerEditadoSpec = new ModeloVeicularPodeSerEditadoSpecification();
            if (!podeSerEditadoSpec.IsSatisfiedBy(modelo))
            {
                notificationContext.AddNotification(podeSerEditadoSpec.ErrorMessage);
                return false;
            }

            // Validar dados básicos
            return ValidarCriacao(descricaoModelo, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, possuiBanheiro, possuiClimatizador, notificationContext);
        }

        public bool ValidarInativacao(
            ModeloVeicular modelo,
            IDomainNotificationContext notificationContext)
        {
            var podeSerInativadoSpec = new ModeloVeicularPodeSerInativadoNotificationSpecification();
            podeSerInativadoSpec.ValidateAndNotify(modelo, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtivacao(
            ModeloVeicular modelo,
            IDomainNotificationContext notificationContext)
        {
            var podeSerAtivadoSpec = new ModeloVeicularPodeSerAtivadoSpecification();
            if (!podeSerAtivadoSpec.IsSatisfiedBy(modelo))
            {
                notificationContext.AddNotification(podeSerAtivadoSpec.ErrorMessage);
                return false;
            }

            return true;
        }
    }
}
