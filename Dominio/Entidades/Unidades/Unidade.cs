using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Unidades
{
    internal class Unidade
    {
        
        public virtual long Codigo { get; set; }

        public virtual string CodigoERP { get; set; }

        public virtual string Descricao { get; set; }

        public virtual bool Tipo { get; set; }

        public virtual bool Situacao { get; set; }

        public virtual string Cep { get; set; }

        public virtual string Endereco { get; set; }

        public virtual string Bairro { get; set; }

        public virtual decimal Latitude { get; set; }

        public virtual decimal Longitude { get; set; }

        public virtual string Numero { get; set; }

        public virtual string Complemento { get; set; }

        //public virtual Dominio.Entidades.Localidades.Localidade Localidade { get; set; }

        //public virtual Dominio.Entidades.Logistica.PontoParada PontoParada { get; set; }

        #region Propriedades Virtuais

        public virtual string SituacaoFormatada
        {
            get
            {
                switch (this.Situacao)
                {
                    case true:
                        return "Ativo";
                    case false:
                        return "Inativo";
                    default:
                        return "";
                }
            }
        }

        public virtual string CEP_Formatado
        {
            get
            {
                long.TryParse(Cep, out long CepFormatado);
                return CepFormatado.ToString(@"00\.000\-000");
            }
        }

        #endregion

    }
}
