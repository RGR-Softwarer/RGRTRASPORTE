using Dominio.Entidades.Veiculos;
using Dominio.Interfaces;
using Dominio.ValueObjects;
using Dominio.Enums.Veiculo;

namespace Dominio.Specifications
{
    public class VeiculoDadosBasicosSpecification : NotificationSpecification<(Placa placa, string modelo, string marca, string numeroChassi, int anoModelo, int anoFabricacao, string cor, string renavam)>
    {
        public override bool IsSatisfiedBy((Placa placa, string modelo, string marca, string numeroChassi, int anoModelo, int anoFabricacao, string cor, string renavam) dados)
        {
            return dados.placa != null &&
                   !string.IsNullOrWhiteSpace(dados.modelo) && dados.modelo.Length <= 50 &&
                   !string.IsNullOrWhiteSpace(dados.marca) && dados.marca.Length <= 50 &&
                   !string.IsNullOrWhiteSpace(dados.numeroChassi) && dados.numeroChassi.Length == 17 &&
                   dados.numeroChassi.All(c => char.IsLetterOrDigit(c)) &&
                   !string.IsNullOrWhiteSpace(dados.cor) && dados.cor.Length <= 30 &&
                   !string.IsNullOrWhiteSpace(dados.renavam) && dados.renavam.Length == 11 &&
                   dados.renavam.All(char.IsDigit) &&
                   dados.anoModelo >= dados.anoFabricacao &&
                   dados.anoModelo <= DateTime.Now.Year + 1 &&
                   dados.anoFabricacao >= 1900 &&
                   dados.anoFabricacao <= DateTime.Now.Year;
        }

        public override string ErrorMessage => "Dados básicos do veículo são inválidos.";

        public override void ValidateAndNotify((Placa placa, string modelo, string marca, string numeroChassi, int anoModelo, int anoFabricacao, string cor, string renavam) dados, IDomainNotificationContext notificationContext)
        {
            if (dados.placa == null)
                notificationContext.AddNotification("Placa é obrigatória");

            if (string.IsNullOrWhiteSpace(dados.modelo))
                notificationContext.AddNotification("Modelo é obrigatório");
            else if (dados.modelo.Length > 50)
                notificationContext.AddNotification("Modelo deve ter no máximo 50 caracteres");

            if (string.IsNullOrWhiteSpace(dados.marca))
                notificationContext.AddNotification("Marca é obrigatória");
            else if (dados.marca.Length > 50)
                notificationContext.AddNotification("Marca deve ter no máximo 50 caracteres");

            if (string.IsNullOrWhiteSpace(dados.numeroChassi))
                notificationContext.AddNotification("Número do chassi é obrigatório");
            else if (dados.numeroChassi.Length != 17)
                notificationContext.AddNotification("Número do chassi deve ter 17 caracteres");
            else if (!dados.numeroChassi.All(c => char.IsLetterOrDigit(c)))
                notificationContext.AddNotification("Número do chassi deve conter apenas letras e números");

            if (string.IsNullOrWhiteSpace(dados.cor))
                notificationContext.AddNotification("Cor é obrigatória");
            else if (dados.cor.Length > 30)
                notificationContext.AddNotification("Cor deve ter no máximo 30 caracteres");

            if (string.IsNullOrWhiteSpace(dados.renavam))
                notificationContext.AddNotification("RENAVAM é obrigatório");
            else if (dados.renavam.Length != 11)
                notificationContext.AddNotification("RENAVAM deve ter 11 caracteres");
            else if (!dados.renavam.All(char.IsDigit))
                notificationContext.AddNotification("RENAVAM deve conter apenas números");

            var anoAtual = DateTime.Now.Year;
            if (dados.anoModelo < dados.anoFabricacao)
                notificationContext.AddNotification("Ano do modelo não pode ser anterior ao ano de fabricação");

            if (dados.anoModelo > anoAtual + 1)
                notificationContext.AddNotification("Ano do modelo não pode ser posterior ao ano seguinte");

            if (dados.anoFabricacao < 1900 || dados.anoFabricacao > anoAtual)
                notificationContext.AddNotification("Ano de fabricação inválido");
        }
    }

    public class VeiculoDadosAtualizacaoSpecification : NotificationSpecification<(string modelo, string marca, string cor)>
    {
        public override bool IsSatisfiedBy((string modelo, string marca, string cor) dados)
        {
            return !string.IsNullOrWhiteSpace(dados.modelo) && dados.modelo.Length <= 50 &&
                   !string.IsNullOrWhiteSpace(dados.marca) && dados.marca.Length <= 50 &&
                   !string.IsNullOrWhiteSpace(dados.cor) && dados.cor.Length <= 30;
        }

        public override string ErrorMessage => "Dados de atualização do veículo são inválidos.";

        public override void ValidateAndNotify((string modelo, string marca, string cor) dados, IDomainNotificationContext notificationContext)
        {
            if (string.IsNullOrWhiteSpace(dados.modelo))
                notificationContext.AddNotification("Modelo é obrigatório");
            else if (dados.modelo.Length > 50)
                notificationContext.AddNotification("Modelo deve ter no máximo 50 caracteres");

            if (string.IsNullOrWhiteSpace(dados.marca))
                notificationContext.AddNotification("Marca é obrigatória");
            else if (dados.marca.Length > 50)
                notificationContext.AddNotification("Marca deve ter no máximo 50 caracteres");

            if (string.IsNullOrWhiteSpace(dados.cor))
                notificationContext.AddNotification("Cor é obrigatória");
            else if (dados.cor.Length > 30)
                notificationContext.AddNotification("Cor deve ter no máximo 30 caracteres");
        }
    }

    public class VeiculoPodeSerAtivadoNotificationSpecification : NotificationSpecification<Veiculo>
    {
        public override bool IsSatisfiedBy(Veiculo veiculo)
        {
            return !veiculo.Situacao;
        }

        public override string ErrorMessage => "Veículo já está ativo.";

        public override void ValidateAndNotify(Veiculo veiculo, IDomainNotificationContext notificationContext)
        {
            if (veiculo.Situacao)
                notificationContext.AddNotification("Veículo já está ativo");
        }
    }

    public class VeiculoPodeSerInativadoNotificationSpecification : NotificationSpecification<Veiculo>
    {
        public override bool IsSatisfiedBy(Veiculo veiculo)
        {
            return veiculo.Situacao;
        }

        public override string ErrorMessage => "Veículo já está inativo.";

        public override void ValidateAndNotify(Veiculo veiculo, IDomainNotificationContext notificationContext)
        {
            if (!veiculo.Situacao)
                notificationContext.AddNotification("Veículo já está inativo");
        }
    }

    public class VeiculoPodeAtualizarLicenciamentoNotificationSpecification : NotificationSpecification<(Veiculo veiculo, DateTime vencimento)>
    {
        public override bool IsSatisfiedBy((Veiculo veiculo, DateTime vencimento) dados)
        {
            return dados.veiculo.Situacao && dados.vencimento >= DateTime.Today;
        }

        public override string ErrorMessage => "Não é possível atualizar licenciamento.";

        public override void ValidateAndNotify((Veiculo veiculo, DateTime vencimento) dados, IDomainNotificationContext notificationContext)
        {
            if (!dados.veiculo.Situacao)
                notificationContext.AddNotification("Não é possível atualizar licenciamento de veículo inativo");

            if (dados.vencimento < DateTime.Today)
                notificationContext.AddNotification("Data de vencimento do licenciamento não pode ser anterior à data atual");
        }
    }
} 
