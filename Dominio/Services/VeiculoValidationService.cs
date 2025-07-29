using Dominio.Entidades.Veiculos;
using Dominio.Interfaces;
using Dominio.Specifications;
using Dominio.ValueObjects;
using Dominio.Enums.Veiculo;

namespace Dominio.Services
{
    public class VeiculoValidationService
    {
        public bool ValidarCriacao(
            Placa placa,
            string modelo,
            string marca,
            string numeroChassi,
            int anoModelo,
            int anoFabricacao,
            string cor,
            string renavam,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId,
            INotificationContext notificationContext)
        {
            var dadosBasicosSpec = new VeiculoDadosBasicosSpecification();
            var dados = (placa, modelo, marca, numeroChassi, anoModelo, anoFabricacao, cor, renavam);

            dadosBasicosSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacao(
            Veiculo veiculo,
            string modelo,
            string marca,
            string cor,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId,
            INotificationContext notificationContext)
        {
            // Validar se o veículo pode ser editado
            var podeSerEditadoSpec = new VeiculoPodeSerEditadoSpecification();
            if (!podeSerEditadoSpec.IsSatisfiedBy(veiculo))
            {
                notificationContext.AddNotification(podeSerEditadoSpec.ErrorMessage);
                return false;
            }

            // Validar dados de atualização
            var dadosAtualizacaoSpec = new VeiculoDadosAtualizacaoSpecification();
            var dados = (modelo, marca, cor);

            dadosAtualizacaoSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtivacao(
            Veiculo veiculo,
            INotificationContext notificationContext)
        {
            var podeSerAtivadoSpec = new VeiculoPodeSerAtivadoNotificationSpecification();
            podeSerAtivadoSpec.ValidateAndNotify(veiculo, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarInativacao(
            Veiculo veiculo,
            INotificationContext notificationContext)
        {
            var podeSerInativadoSpec = new VeiculoPodeSerInativadoNotificationSpecification();
            podeSerInativadoSpec.ValidateAndNotify(veiculo, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacaoLicenciamento(
            Veiculo veiculo,
            DateTime vencimento,
            INotificationContext notificationContext)
        {
            var podeAtualizarLicenciamentoSpec = new VeiculoPodeAtualizarLicenciamentoNotificationSpecification();
            var dados = (veiculo, vencimento);

            podeAtualizarLicenciamentoSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }
    }
} 