using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloUsuariosCentralizado
{
    public class Usuarios
    {
        [Key]
        public int id_usua { get; set; }

        public string log_usua { get; set; }
        public string nom_usua { get; set; }
        public string tip_doc { get; set; }
        public string num_doc { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public DateTime fec_ultima_sesion { get; set; }
        public DateTime fec_expira_clave { get; set; }
        public DateTime fec_regis { get; set; }
        public bool activo { get; set; }
    }
}