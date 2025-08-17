using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Enums.Data;
using Dominio.Interfaces;

namespace Dominio.Specifications
{
    public class GatilhoViagemDadosBasicosSpecification : NotificationSpecification<(string descricao, long veiculoId, long motoristaId, long localidadeOrigemId, long localidadeDestinoId, int quantidadeVagas, string descricaoViagem, string polilinhaRota)>
    {
        public override bool IsSatisfiedBy((string descricao, long veiculoId, long motoristaId, long localidadeOrigemId, long localidadeDestinoId, int quantidadeVagas, string descricaoViagem, string polilinhaRota) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.descricao) &&
                   dados.descricao.Length <= 100 &&
                   dados.veiculoId > 0 &&
                   dados.motoristaId > 0 &&
                   dados.localidadeOrigemId > 0 &&
                   dados.localidadeDestinoId > 0 &&
                   dados.localidadeOrigemId != dados.localidadeDestinoId &&
                   dados.quantidadeVagas > 0 &&
                   !string.IsNullOrWhiteSpace(dados.descricaoViagem) &&
                   dados.descricaoViagem.Length <= 500 &&
                   !string.IsNullOrWhiteSpace(dados.polilinhaRota);
        }

        public override string ErrorMessage => "Dados básicos do gatilho são inválidos";

        public override void ValidateAndNotify((string descricao, long veiculoId, long motoristaId, long localidadeOrigemId, long localidadeDestinoId, int quantidadeVagas, string descricaoViagem, string polilinhaRota) dados, IDomainNotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.descricao))
                notificationContext.AddNotification("Descrição do gatilho é obrigatória");

            if (dados.descricao?.Length > 100)
                notificationContext.AddNotification("Descrição do gatilho deve ter no máximo 100 caracteres");

            if (dados.veiculoId <= 0)
                notificationContext.AddNotification("Veículo é obrigatório");

            if (dados.motoristaId <= 0)
                notificationContext.AddNotification("Motorista é obrigatório");

            if (dados.localidadeOrigemId <= 0)
                notificationContext.AddNotification("Localidade de origem é obrigatória");

            if (dados.localidadeDestinoId <= 0)
                notificationContext.AddNotification("Localidade de destino é obrigatória");

            if (dados.localidadeOrigemId == dados.localidadeDestinoId)
                notificationContext.AddNotification("Localidade de destino não pode ser igual à localidade de origem");

            if (dados.quantidadeVagas <= 0)
                notificationContext.AddNotification("Quantidade de vagas deve ser maior que zero");

            if (string.IsNullOrWhiteSpace(dados.descricaoViagem))
                notificationContext.AddNotification("Descrição da viagem é obrigatória");

            if (dados.descricaoViagem?.Length > 500)
                notificationContext.AddNotification("Descrição da viagem não pode ter mais que 500 caracteres");

            if (string.IsNullOrWhiteSpace(dados.polilinhaRota))
                notificationContext.AddNotification("Polilinha da rota é obrigatória");
        }
    }

    public class GatilhoViagemHorariosValidosNotificationSpecification : NotificationSpecification<(TimeSpan horarioSaida, TimeSpan horarioChegada)>
    {
        public override bool IsSatisfiedBy((TimeSpan horarioSaida, TimeSpan horarioChegada) dados)
        {
            return dados.horarioSaida < dados.horarioChegada &&
                   dados.horarioSaida >= TimeSpan.Zero &&
                   dados.horarioChegada <= TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59));
        }

        public override string ErrorMessage => "Horários são inválidos";

        public override void ValidateAndNotify((TimeSpan horarioSaida, TimeSpan horarioChegada) dados, IDomainNotificationContext notificationContext)
        {
            if (dados.horarioSaida >= dados.horarioChegada)
                notificationContext.AddNotification("Horário de chegada deve ser maior que o horário de saída");

            if (dados.horarioSaida < TimeSpan.Zero)
                notificationContext.AddNotification("Horário de saída é inválido");

            if (dados.horarioChegada > TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59)))
                notificationContext.AddNotification("Horário de chegada é inválido");
        }
    }

    public class GatilhoViagemDiasSemanaValidosNotificationSpecification : NotificationSpecification<List<DiaSemanaEnum>>
    {
        public override bool IsSatisfiedBy(List<DiaSemanaEnum> diasSemana)
        {
            return diasSemana != null &&
                   diasSemana.Any() &&
                   diasSemana.All(d => Enum.IsDefined(typeof(DiaSemanaEnum), d)) &&
                   diasSemana.Distinct().Count() == diasSemana.Count;
        }

        public override string ErrorMessage => "Dias da semana são inválidos";

        public override void ValidateAndNotify(List<DiaSemanaEnum> diasSemana, IDomainNotificationContext notificationContext)
        {
            if (diasSemana == null || !diasSemana.Any())
                notificationContext.AddNotification("Pelo menos um dia da semana deve ser selecionado");

            if (diasSemana?.Any(d => !Enum.IsDefined(typeof(DiaSemanaEnum), d)) == true)
                notificationContext.AddNotification("Um ou mais dias da semana são inválidos");

            if (diasSemana?.Distinct().Count() != diasSemana?.Count)
                notificationContext.AddNotification("Dias da semana duplicados não são permitidos");
        }
    }

    public class GatilhoViagemDadosFinanceirosSpecification : NotificationSpecification<(decimal valorPassagem, decimal distancia)>
    {
        public override bool IsSatisfiedBy((decimal valorPassagem, decimal distancia) dados)
        {
            return dados.valorPassagem > 0 &&
                   dados.valorPassagem <= 10000 &&
                   dados.distancia > 0 &&
                   dados.distancia <= 10000;
        }

        public override string ErrorMessage => "Dados financeiros são inválidos";

        public override void ValidateAndNotify((decimal valorPassagem, decimal distancia) dados, IDomainNotificationContext notificationContext)
        {
            if (dados.valorPassagem <= 0)
                notificationContext.AddNotification("Valor da passagem deve ser maior que zero");

            if (dados.valorPassagem > 10000)
                notificationContext.AddNotification("Valor da passagem não pode ser maior que R$ 10.000,00");

            if (dados.distancia <= 0)
                notificationContext.AddNotification("Distância deve ser maior que zero");

            if (dados.distancia > 10000)
                notificationContext.AddNotification("Distância não pode ser maior que 10.000 km");
        }
    }

    public class GatilhoViagemPodeSerDesativadoNotificationSpecification : NotificationSpecification<GatilhoViagem>
    {
        public override bool IsSatisfiedBy(GatilhoViagem gatilho)
        {
            return gatilho.Ativo;
        }

        public override string ErrorMessage => "Gatilho não pode ser desativado";

        public override void ValidateAndNotify(GatilhoViagem gatilho, IDomainNotificationContext notificationContext)
        {
            if (!gatilho.Ativo)
                notificationContext.AddNotification("Gatilho já está inativo");

            // Adicionar validações específicas como:
            // - Não possui viagens futuras geradas
            // - Não há reservas dependentes
            // etc.
        }
    }

    public class GatilhoViagemPodeGerarViagemNotificationSpecification : NotificationSpecification<(GatilhoViagem gatilho, DateTime data)>
    {
        public override bool IsSatisfiedBy((GatilhoViagem gatilho, DateTime data) dados)
        {
            return dados.gatilho.Ativo &&
                   dados.data >= DateTime.Today &&
                   dados.gatilho.DiasSemana.Contains((DiaSemanaEnum)dados.data.DayOfWeek);
        }

        public override string ErrorMessage => "Não é possível gerar viagem para esta data";

        public override void ValidateAndNotify((GatilhoViagem gatilho, DateTime data) dados, IDomainNotificationContext notificationContext)
        {
            if (!dados.gatilho.Ativo)
                notificationContext.AddNotification("Gatilho está inativo");

            if (dados.data < DateTime.Today)
                notificationContext.AddNotification("Data não pode ser anterior à data atual");

            if (!dados.gatilho.DiasSemana.Contains((DiaSemanaEnum)dados.data.DayOfWeek))
                notificationContext.AddNotification("Data não corresponde aos dias da semana configurados no gatilho");
        }
    }
}
