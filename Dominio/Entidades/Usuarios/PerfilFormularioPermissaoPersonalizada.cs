using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Usuarios
{
    internal class PerfilFormularioPermissaoPersonalizada
    {
        public virtual long Codigo { get; set; }
        public virtual Dominio.Entidades.Usuarios.PerfilFormulario PerfilFormulario { get; set; }
        public virtual int CodigoPermissao { get; set; }

    }
}
