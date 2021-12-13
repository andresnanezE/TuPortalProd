using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModelKheiron_Logs
{
    public partial class EMH_ENVIO_CORREO
    {
        [Key]
        public int ID { get; set; }

        public string ASUNTO { get; set; }
        public string CUERPO { get; set; }
        public string CORREOS_DES { get; set; }
        public string CORREOS_COP { get; set; }
        public string CORREOS_COO { get; set; }
        public string ADJUNTOS { get; set; }
        public string TIPO { get; set; }
        public int CANTIDAD_INTENTOS_ENVIO { get; set; }
        public bool ENVIADO { get; set; }
    }
}