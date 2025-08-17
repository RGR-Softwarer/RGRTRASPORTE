using Dominio.Entidades.Veiculos;
using Dominio.Enums.Veiculo;
using Dominio.Interfaces;

namespace Dominio.Specifications
{
    public class ModeloVeicularDadosBasicosSpecification : NotificationSpecification<(string descricao, TipoModeloVeiculoEnum tipo, int quantidadeAssento, int quantidadeEixo)>
    {
        public override bool IsSatisfiedBy((string descricao, TipoModeloVeiculoEnum tipo, int quantidadeAssento, int quantidadeEixo) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.descricao) &&
                   dados.descricao.Length <= 100 &&
                   dados.quantidadeAssento > 0 &&
                   dados.quantidadeEixo > 0 &&
                   Enum.IsDefined(typeof(TipoModeloVeiculoEnum), dados.tipo);
        }

        public override string ErrorMessage => "Dados básicos do modelo são inválidos";

        public override void ValidateAndNotify((string descricao, TipoModeloVeiculoEnum tipo, int quantidadeAssento, int quantidadeEixo) dados, IDomainNotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.descricao))
                notificationContext.AddNotification("Descrição do modelo é obrigatória");

            if (dados.descricao?.Length > 100)
                notificationContext.AddNotification("Descrição do modelo deve ter no máximo 100 caracteres");

            if (dados.quantidadeAssento <= 0)
                notificationContext.AddNotification("Quantidade de assentos deve ser maior que zero");

            if (dados.quantidadeEixo <= 0)
                notificationContext.AddNotification("Quantidade de eixos deve ser maior que zero");

            if (!Enum.IsDefined(typeof(TipoModeloVeiculoEnum), dados.tipo))
                notificationContext.AddNotification("Tipo de modelo é inválido");
        }
    }

    public class ModeloVeicularCapacidadeValidaNotificationSpecification : NotificationSpecification<(int quantidadeAssento, int capacidadeMaxima, int passageirosEmPe)>
    {
        public override bool IsSatisfiedBy((int quantidadeAssento, int capacidadeMaxima, int passageirosEmPe) dados)
        {
            return dados.quantidadeAssento > 0 &&
                   dados.passageirosEmPe >= 0 &&
                   dados.capacidadeMaxima >= (dados.quantidadeAssento + dados.passageirosEmPe) &&
                   dados.capacidadeMaxima <= 150;
        }

        public override string ErrorMessage => "Capacidade do modelo é inválida";

        public override void ValidateAndNotify((int quantidadeAssento, int capacidadeMaxima, int passageirosEmPe) dados, IDomainNotificationContext notificationContext)
        {
            if (dados.quantidadeAssento <= 0)
                notificationContext.AddNotification("Quantidade de assentos deve ser maior que zero");

            if (dados.passageirosEmPe < 0)
                notificationContext.AddNotification("Quantidade de passageiros em pé não pode ser negativa");

            if (dados.capacidadeMaxima < (dados.quantidadeAssento + dados.passageirosEmPe))
                notificationContext.AddNotification("Capacidade máxima deve ser maior ou igual à soma de assentos e passageiros em pé");

            if (dados.capacidadeMaxima > 150)
                notificationContext.AddNotification("Capacidade máxima não pode ser maior que 150 passageiros");
        }
    }

    public class ModeloVeicularPodeSerInativadoNotificationSpecification : NotificationSpecification<ModeloVeicular>
    {
        public override bool IsSatisfiedBy(ModeloVeicular modelo)
        {
            return modelo.Situacao && !modelo.Veiculos.Any(v => v.Situacao);
        }

        public override string ErrorMessage => "Modelo não pode ser inativado";

        public override void ValidateAndNotify(ModeloVeicular modelo, IDomainNotificationContext notificationContext)
        {
            if (!modelo.Situacao)
                notificationContext.AddNotification("Modelo já está inativo");

            if (modelo.Veiculos.Any(v => v.Situacao))
                notificationContext.AddNotification("Não é possível inativar um modelo que possui veículos ativos");
        }
    }
}
