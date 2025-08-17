using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;
using Dominio.Events.Base;
using Dominio.Exceptions;
using Dominio.ValueObjects;
using Dominio.Interfaces;
using Dominio.Services;

namespace Dominio.Entidades.Pessoas
{
    public class Motorista : Pessoa
    {
        private Motorista() { } // Para EF Core

        // Construtor privado - usar Factory Methods
        private Motorista(
            string nome,
            CPF cpf,
            string rg,
            string telefone,
            string email,
            SexoEnum sexo,
            string cnh,
            CategoriaCNHEnum categoriaCNH,
            DateTime validadeCNH,
            string observacao) : base(nome, cpf, telefone, email, sexo, observacao)
        {
            ValidarRG(rg);
            ValidarCNH(cnh);
            ValidarValidadeCNH(validadeCNH);

            RG = rg;
            CNH = cnh;
            CategoriaCNH = categoriaCNH;
            ValidadeCNH = validadeCNH;

            AddDomainEvent(new MotoristaCriadoEvent(Id, nome, cpf.Numero));
        }

        // Factory Methods
        public static Motorista CriarMotorista(
            string nome,
            CPF cpf,
            string rg,
            string telefone,
            string email,
            SexoEnum sexo,
            string cnh,
            CategoriaCNHEnum categoriaCNH,
            DateTime validadeCNH,
            string observacao)
        {
            return new Motorista(nome, cpf, rg, telefone, email, sexo, cnh, categoriaCNH, validadeCNH, observacao);
        }

        // Factory Method com validação por NotificationContext
        public static (Motorista? motorista, bool sucesso) CriarMotoristaComValidacao(
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
            INotificationContext notificationContext)
        {
            var validationService = new MotoristaValidationService();
            var valido = validationService.ValidarCriacao(nome, cpf, rg, telefone, email, sexo,
                cnh, categoriaCNH, validadeCNH, observacao, notificationContext);

            if (!valido)
                return (null, false);

            try
            {
                var motorista = CriarMotorista(nome, cpf, rg, telefone, email, sexo, cnh, categoriaCNH, validadeCNH, observacao);
                return (motorista, true);
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return (null, false);
            }
        }

        public string RG { get; private set; } = null!;
        public string CNH { get; private set; } = null!;
        public CategoriaCNHEnum CategoriaCNH { get; private set; }
        public DateTime ValidadeCNH { get; private set; }

        public void AtualizarDocumentos(
            string rg,
            string cnh,
            CategoriaCNHEnum categoriaCNH,
            DateTime validadeCNH)
        {
            ValidarRG(rg);
            ValidarCNH(cnh);
            ValidarValidadeCNH(validadeCNH);

            RG = rg;
            CNH = cnh;
            CategoriaCNH = categoriaCNH;
            ValidadeCNH = validadeCNH;
            UpdateTimestamp();

            AddDomainEvent(new MotoristaDocumentosAtualizadosEvent(Id, Nome, CPF.Numero));
        }

        public void RenovarCNH(DateTime novaValidade)
        {
            ValidarValidadeCNH(novaValidade);
            ValidadeCNH = novaValidade;
            UpdateTimestamp();
            AddDomainEvent(new MotoristaCNHRenovadaEvent(Id, Nome, CPF.Numero, novaValidade));
        }

        public bool CNHExpirada => ValidadeCNH < DateTime.Today;

        public bool PodeDirigirVeiculo(TipoModeloVeiculoEnum tipoVeiculo)
        {
            return tipoVeiculo switch
            {
                TipoModeloVeiculoEnum.Van => CategoriaCNH == CategoriaCNHEnum.B || CategoriaCNH == CategoriaCNHEnum.D,
                TipoModeloVeiculoEnum.Onibus => CategoriaCNH == CategoriaCNHEnum.D,
                _ => false
            };
        }

        protected override string DescricaoFormatada => $"{Nome} ({CPF.NumeroFormatado})";

        private void ValidarRG(string rg)
        {
            if (string.IsNullOrWhiteSpace(rg))
                throw new DomainException("RG � obrigat�rio.");

            if (rg.Length > 20)
                throw new DomainException("RG deve ter no m�ximo 20 caracteres.");
        }

        private void ValidarCNH(string cnh)
        {
            if (string.IsNullOrWhiteSpace(cnh))
                throw new DomainException("CNH � obrigat�ria.");

            if (cnh.Length != 11)
                throw new DomainException("CNH deve ter 11 caracteres.");

            if (!cnh.All(char.IsDigit))
                throw new DomainException("CNH deve conter apenas n�meros.");
        }

        private void ValidarValidadeCNH(DateTime validade)
        {
            if (validade < DateTime.Today)
                throw new DomainException("Data de validade da CNH n�o pode ser anterior � data atual.");

            if (validade > DateTime.Today.AddYears(10))
                throw new DomainException("Data de validade da CNH n�o pode ser superior a 10 anos.");
        }

        #region Propriedades Virtuais

        public string DescricaoAtivo
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }

        #endregion
    }

    // Eventos de dom�nio para Motorista
    public class MotoristaCriadoEvent : DomainEvent
    {
        public long MotoristaId { get; }
        public string Nome { get; } = null!;
        public string CPF { get; } = null!;

        public MotoristaCriadoEvent(long motoristaId, string nome, string cpf)
        {
            MotoristaId = motoristaId;
            Nome = nome;
            CPF = cpf;
        }
    }

    public class MotoristaDocumentosAtualizadosEvent : DomainEvent
    {
        public long MotoristaId { get; }
        public string Nome { get; } = null!;
        public string CPF { get; } = null!;

        public MotoristaDocumentosAtualizadosEvent(long motoristaId, string nome, string cpf)
        {
            MotoristaId = motoristaId;
            Nome = nome;
            CPF = cpf;
        }
    }

    public class MotoristaCNHRenovadaEvent : DomainEvent
    {
        public long MotoristaId { get; }
        public string Nome { get; } = null!;
        public string CPF { get; } = null!;
        public DateTime NovaValidade { get; }

        public MotoristaCNHRenovadaEvent(long motoristaId, string nome, string cpf, DateTime novaValidade)
        {
            MotoristaId = motoristaId;
            Nome = nome;
            CPF = cpf;
            NovaValidade = novaValidade;
        }
    }
}
