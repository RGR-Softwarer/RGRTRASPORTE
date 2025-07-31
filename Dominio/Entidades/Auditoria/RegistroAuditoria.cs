using Dominio.Enums.Auditoria;
using Dominio.Entidades;

namespace Dominio.Entidades.Auditoria
{
    public class RegistroAuditoria : BaseEntity
    {
        protected RegistroAuditoria() { } // Para EF

        public RegistroAuditoria(
            string nomeEntidade,
            long entidadeId,
            AcaoBancoDadosEnum acao,
            string dados,
            string usuario,
            string ip)
        {
            NomeEntidade = nomeEntidade;
            EntidadeId = entidadeId;
            Acao = acao;
            Dados = dados;
            Usuario = usuario;
            IP = ip;
            DataOcorrencia = DateTime.UtcNow;
        }

        public string NomeEntidade { get; private set; }
        public long EntidadeId { get; private set; }
        public AcaoBancoDadosEnum Acao { get; private set; }
        public string Dados { get; private set; }
        public string Usuario { get; private set; }
        public string IP { get; private set; }
        public DateTime DataOcorrencia { get; private set; }

        protected override string DescricaoFormatada => $"{NomeEntidade}({EntidadeId}) - {Acao}";
    }
} 
