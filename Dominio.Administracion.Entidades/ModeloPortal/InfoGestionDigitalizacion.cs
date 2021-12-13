using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class InfoGestionDigitalizacion
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int NumeroFormulario { get; set; }
        public int CedulaAsesor { get; set; }
        public int CedulaContratante { get; set; }
        public int NumeroAfiliados { get; set; }
        public string TipoArchivo { get; set; }
        public List<int> Causales { get; set; }
        public int IdEstado { get; set; }
    }
}
