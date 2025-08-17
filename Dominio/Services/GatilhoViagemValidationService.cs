using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Enums.Data;
using Dominio.Interfaces;
using Dominio.Specifications;

namespace Dominio.Services
{
    public class GatilhoViagemValidationService
    {
        public bool ValidarCriacao(
            string descricao,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            decimal valorPassagem,
            int quantidadeVagas,
            decimal distancia,
            string descricaoViagem,
            string polilinhaRota,
            List<DiaSemanaEnum> diasSemana,
            INotificationContext notificationContext)
        {
            var dadosBasicosSpec = new GatilhoViagemDadosBasicosSpecification();
            var horariosValidosSpec = new GatilhoViagemHorariosValidosNotificationSpecification();
            var diasSemanaValidosSpec = new GatilhoViagemDiasSemanaValidosNotificationSpecification();

            var dadosBasicos = (descricao, veiculoId, motoristaId, localidadeOrigemId, localidadeDestinoId, 
                quantidadeVagas, descricaoViagem, polilinhaRota);
            var horarios = (horarioSaida, horarioChegada);
            var financeiro = (valorPassagem, distancia);

            dadosBasicosSpec.ValidateAndNotify(dadosBasicos, notificationContext);
            horariosValidosSpec.ValidateAndNotify(horarios, notificationContext);
            diasSemanaValidosSpec.ValidateAndNotify(diasSemana, notificationContext);

            // Validar dados financeiros
            var dadosFinanceirosSpec = new GatilhoViagemDadosFinanceirosSpecification();
            dadosFinanceirosSpec.ValidateAndNotify(financeiro, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacao(
            GatilhoViagem gatilho,
            string descricao,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            decimal valorPassagem,
            int quantidadeVagas,
            decimal distancia,
            string descricaoViagem,
            string polilinhaRota,
            List<DiaSemanaEnum> diasSemana,
            INotificationContext notificationContext)
        {
            // Verificar se o gatilho pode ser editado
            var podeSerEditadoSpec = new GatilhoViagemPodeSerEditadoSpecification();
            if (!podeSerEditadoSpec.IsSatisfiedBy(gatilho))
            {
                notificationContext.AddNotification(podeSerEditadoSpec.ErrorMessage);
                return false;
            }

            // Validar dados básicos
            return ValidarCriacao(descricao, veiculoId, motoristaId, localidadeOrigemId,
                localidadeDestinoId, horarioSaida, horarioChegada, valorPassagem, quantidadeVagas,
                distancia, descricaoViagem, polilinhaRota, diasSemana, notificationContext);
        }

        public bool ValidarAtualizacaoHorarios(
            GatilhoViagem gatilho,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            INotificationContext notificationContext)
        {
            var podeAtualizarHorariosSpec = new GatilhoViagemPodeAtualizarHorariosSpecification();
            if (!podeAtualizarHorariosSpec.IsSatisfiedBy(gatilho))
            {
                notificationContext.AddNotification(podeAtualizarHorariosSpec.ErrorMessage);
                return false;
            }

            var horariosValidosSpec = new GatilhoViagemHorariosValidosNotificationSpecification();
            var horarios = (horarioSaida, horarioChegada);
            horariosValidosSpec.ValidateAndNotify(horarios, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacaoValorPassagem(
            GatilhoViagem gatilho,
            decimal valorPassagem,
            INotificationContext notificationContext)
        {
            var podeAtualizarValorSpec = new GatilhoViagemPodeAtualizarValorSpecification();
            if (!podeAtualizarValorSpec.IsSatisfiedBy(gatilho))
            {
                notificationContext.AddNotification(podeAtualizarValorSpec.ErrorMessage);
                return false;
            }

            if (valorPassagem <= 0)
            {
                notificationContext.AddNotification("Valor da passagem deve ser maior que zero");
                return false;
            }

            if (valorPassagem > 10000)
            {
                notificationContext.AddNotification("Valor da passagem não pode ser maior que R$ 10.000,00");
                return false;
            }

            return true;
        }

        public bool ValidarGeracaoViagem(
            GatilhoViagem gatilho,
            DateTime dataViagem,
            INotificationContext notificationContext)
        {
            var podeGerarViagemSpec = new GatilhoViagemPodeGerarViagemNotificationSpecification();
            var dados = (gatilho, dataViagem);

            podeGerarViagemSpec.ValidateAndNotify(dados, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtivacao(
            GatilhoViagem gatilho,
            INotificationContext notificationContext)
        {
            var podeSerAtivadoSpec = new GatilhoViagemPodeSerAtivadoSpecification();
            if (!podeSerAtivadoSpec.IsSatisfiedBy(gatilho))
            {
                notificationContext.AddNotification(podeSerAtivadoSpec.ErrorMessage);
                return false;
            }

            return true;
        }

        public bool ValidarDesativacao(
            GatilhoViagem gatilho,
            INotificationContext notificationContext)
        {
            var podeSerDesativadoSpec = new GatilhoViagemPodeSerDesativadoNotificationSpecification();
            podeSerDesativadoSpec.ValidateAndNotify(gatilho, notificationContext);

            return !notificationContext.HasNotifications();
        }
    }
}
