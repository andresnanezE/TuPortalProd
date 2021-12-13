using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCentralizada
{
    public class usrCentral
    {
        [Key]
        public int id_usr { get; set; }

        public string log_usr { get; set; }
        public string nom_usr { get; set; }
        public string tip_doc { get; set; }
        public string num_doc { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public System.DateTime fec_ultima_sesion { get; set; }
        public System.DateTime fec_expira_clave { get; set; }
        public System.DateTime fec_regis { get; set; }
        public bool activo { get; set; }
        public Nullable<bool> migrar { get; set; }
    }
}