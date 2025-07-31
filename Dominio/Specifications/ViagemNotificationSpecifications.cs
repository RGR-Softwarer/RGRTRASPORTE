using Dominio.Entidades.Viagens;
using Dominio.Enums.Viagens;
using Dominio.Interfaces;

namespace Dominio.Specifications
{
    // Specifications que coletam erros no NotificationContext
    public class ViagemPodeReceberPassageiroNotificationSpecification : NotificationSpecification<Viagem>
    {
        public override bool IsSatisfiedBy(Viagem viagem)
        {
            return viagem.Situacao == SituacaoViagemEnum.Agendada && viagem.VagasDisponiveis > 0;
        }

        public override string ErrorMessage => 
            "Viagem deve estar agendada e ter vagas disponíveis para receber passageiros";

        public override void ValidateAndNotify(Viagem viagem, INotificationContext notificationContext)
        {
            if (viagem.Situacao != SituacaoViagemEnum.Agendada)
            {
                notificationContext.AddNotification("Viagem deve estar agendada para receber passageiros");
            }

            if (viagem.VagasDisponiveis <= 0)
            {
                notificationContext.AddNotification("Não há vagas disponíveis na viagem");
            }
        }
    }

    public class ViagemDadosBasicosSpecification : NotificationSpecification<(long veiculoId, long motoristaId, long localidadeOrigemId, long localidadeDestinoId, int quantidadeVagas, string descricaoViagem)>
    {
        public override bool IsSatisfiedBy((long veiculoId, long motoristaId, long localidadeOrigemId, long localidadeDestinoId, int quantidadeVagas, string descricaoViagem) dados)
        {
            return dados.veiculoId > 0 && 
                   dados.motoristaId > 0 && 
                   dados.localidadeOrigemId > 0 && 
                   dados.localidadeDestinoId > 0 && 
                   dados.quantidadeVagas > 0 && 
                   !string.IsNullOrEmpty(dados.descricaoViagem) && 
                   dados.descricaoViagem.Length <= 500;
        }

        public override string ErrorMessage => "Dados básicos da viagem são inválidos";

        public override void ValidateAndNotify((long veiculoId, long motoristaId, long localidadeOrigemId, long localidadeDestinoId, int quantidadeVagas, string descricaoViagem) dados, INotificationContext notificationContext)
        {
            if (dados.veiculoId <= 0)
                notificationContext.AddNotification("O veículo é obrigatório");

            if (dados.motoristaId <= 0)
                notificationContext.AddNotification("O motorista é obrigatório");

            if (dados.localidadeOrigemId <= 0)
                notificationContext.AddNotification("A localidade de origem é obrigatória");

            if (dados.localidadeDestinoId <= 0)
                notificationContext.AddNotification("A localidade de destino é obrigatória");

            if (dados.quantidadeVagas <= 0)
                notificationContext.AddNotification("A quantidade de vagas deve ser maior que zero");

            if (string.IsNullOrEmpty(dados.descricaoViagem))
                notificationContext.AddNotification("A descrição da viagem é obrigatória");
            else if (dados.descricaoViagem.Length > 500)
                notificationContext.AddNotification("A descrição da viagem não pode ter mais que 500 caracteres");
        }
    }

    public class ViagemRegrasNegocioSpecification : NotificationSpecification<(long localidadeOrigemId, long localidadeDestinoId)>
    {
        public override bool IsSatisfiedBy((long localidadeOrigemId, long localidadeDestinoId) dados)
        {
            return dados.localidadeOrigemId != dados.localidadeDestinoId;
        }

        public override string ErrorMessage => "Regras de negócio da viagem são inválidas";

        public override void ValidateAndNotify((long localidadeOrigemId, long localidadeDestinoId) dados, INotificationContext notificationContext)
        {
            if (dados.localidadeOrigemId == dados.localidadeDestinoId)
                notificationContext.AddNotification("A localidade de destino não pode ser igual à localidade de origem");
        }
    }

    public class PassageiroJaEstaNaviagemNotificationSpecification : NotificationSpecification<(Viagem viagem, long passageiroId)>
    {
        public override bool IsSatisfiedBy((Viagem viagem, long passageiroId) dados)
        {
            return !dados.viagem.Passageiros.Any(p => p.PassageiroId == dados.passageiroId);
        }

        public override string ErrorMessage => "Passageiro já está na viagem";
    }

    public class CoordenadaValidaSpecification : NotificationSpecification<(decimal latitude, decimal longitude)>
    {
        public override bool IsSatisfiedBy((decimal latitude, decimal longitude) coordenada)
        {
            return coordenada.latitude >= -90 && coordenada.latitude <= 90 &&
                   coordenada.longitude >= -180 && coordenada.longitude <= 180;
        }

        public override string ErrorMessage => "Coordenadas inválidas";

        public override void ValidateAndNotify((decimal latitude, decimal longitude) coordenada, INotificationContext notificationContext)
        {
            if (coordenada.latitude < -90 || coordenada.latitude > 90)
                notificationContext.AddNotification("Latitude deve estar entre -90 e 90 graus");

            if (coordenada.longitude < -180 || coordenada.longitude > 180)
                notificationContext.AddNotification("Longitude deve estar entre -180 e 180 graus");
        }
    }
} 
