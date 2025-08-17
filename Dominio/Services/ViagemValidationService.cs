using Dominio.Entidades.Viagens;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.ValueObjects;
using Dominio.Specifications;
using Dominio.Interfaces;

namespace Dominio.Services
{
    public class ViagemValidationService
    {
        public bool ValidarCriacao(
            DateTime dataViagem,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            int quantidadeVagas,
            decimal distanciaKm,
            string descricaoViagem,
            string polilinhaRota,
            IDomainNotificationContext notificationContext)
        {
            var dadosBasicosSpec = new ViagemDadosBasicosSpecification();
            var regrasNegocioSpec = new ViagemRegrasNegocioSpecification();

            var dadosBasicos = (veiculoId, motoristaId, localidadeOrigemId, localidadeDestinoId, quantidadeVagas, descricaoViagem);
            var regrasNegocio = (localidadeOrigemId, localidadeDestinoId);

            dadosBasicosSpec.ValidateAndNotify(dadosBasicos, notificationContext);
            regrasNegocioSpec.ValidateAndNotify(regrasNegocio, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAtualizacao(
            Viagem viagem,
            DateTime dataViagem,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            int quantidadeVagas,
            IDomainNotificationContext notificationContext)
        {
            // Verificar se a viagem pode ser editada
            var podeSerEditadaSpec = new ViagemPodeSerEditadaSpecification();
            if (!podeSerEditadaSpec.IsSatisfiedBy(viagem))
            {
                notificationContext.AddNotification(podeSerEditadaSpec.ErrorMessage);
                return false;
            }

            // Validar dados básicos da atualização
            var dadosBasicosSpec = new ViagemDadosBasicosSpecification();
            var dadosBasicos = (veiculoId, viagem.MotoristaId, localidadeOrigemId, localidadeDestinoId, quantidadeVagas, viagem.DescricaoViagem);

            dadosBasicosSpec.ValidateAndNotify(dadosBasicos, notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAdicaoPassageiro(
            Viagem viagem,
            Passageiro passageiro,
            IDomainNotificationContext notificationContext)
        {
            var podeReceberPassageiroSpec = new ViagemPodeReceberPassageiroNotificationSpecification();
            var passageiroJaEstaSpec = new PassageiroJaEstaNaviagemNotificationSpecification();

            podeReceberPassageiroSpec.ValidateAndNotify(viagem, notificationContext);
            passageiroJaEstaSpec.ValidateAndNotify((viagem, passageiro.Id), notificationContext);

            return !notificationContext.HasNotifications();
        }

        public bool ValidarAdicaoPosicao(
            Viagem viagem,
            decimal latitude,
            decimal longitude,
            DateTime dataHora,
            IDomainNotificationContext notificationContext)
        {
            var podeReceberPosicaoSpec = new ViagemPodeReceberPosicaoSpecification();
            var coordenadaValidaSpec = new CoordenadaValidaSpecification();

            if (!podeReceberPosicaoSpec.IsSatisfiedBy(viagem))
            {
                notificationContext.AddNotification(podeReceberPosicaoSpec.ErrorMessage);
            }

            var coordenada = (latitude, longitude);
            coordenadaValidaSpec.ValidateAndNotify(coordenada, notificationContext);

            // Validar data/hora
            if (dataHora > DateTime.UtcNow)
            {
                notificationContext.AddNotification("Data/hora não pode ser futura");
            }

            return !notificationContext.HasNotifications();
        }
    }
} 
