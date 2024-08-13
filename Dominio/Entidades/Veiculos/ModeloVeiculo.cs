using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Veiculos
{
    internal class ModeloVeiculo
    {

        [NHibernate.Mapping.Attributes.Id(0, Name = "Codigo", Type = "System.Int64", Column = "MOV_CODIGO")]
        [NHibernate.Mapping.Attributes.Generator(1, Class = "native")]
        public virtual long Codigo { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "Descricao", Column = "MOV_DESCRICAO", TypeType = typeof(string), Length = 500, NotNull = true)]
        public virtual string Descricao { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "Situacao", Column = "MOV_SITUACAO", TypeType = typeof(bool), NotNull = true)]
        public virtual bool Situacao { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "Tipo", Column = "MOV_TIPO", TypeType = typeof(Enumeradores.Veiculos.TipoModeloVeiculo), NotNull = true)]
        public virtual Enumeradores.Veiculos.TipoModeloVeiculo Tipo { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "QuantidadeAssento", Column = "MOV_QUANTIDADE_ASSENTO", TypeType = typeof(int), NotNull = false)]
        public virtual int QuantidadeAssento { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "QuantidadeEixo", Column = "MOV_QUANTIDADE_EIXO", TypeType = typeof(int), NotNull = false)]
        public virtual int QuantidadeEixo { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "CapacidadeMaxima", Column = "MOV_CAPACIDADE_MAXIMA", TypeType = typeof(int), NotNull = false)]
        public virtual int CapacidadeMaxima { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "PassageirosEmPe", Column = "MOV_PASSAGEIROS_EM_PE", TypeType = typeof(int), NotNull = false)]
        public virtual int PassageirosEmPe { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "PossuiBanheiro", Column = "MOV_POSSUI_BANHEIRO", TypeType = typeof(bool), NotNull = false)]
        public virtual bool PossuiBanheiro { get; set; }

        [NHibernate.Mapping.Attributes.Property(0, Name = "PossuiClimatizador", Column = "MOV_POSSUI_CLIMATIZADOR", TypeType = typeof(bool), NotNull = false)]
        public virtual bool PossuiClimatizador { get; set; }

        public virtual string DescricaoAtivo
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }
    }
}
