using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModelKheiron_Logs
{
    public partial class EMH_ENVIO_SMS
    {
        [Key]
        public int ID_MENSAJE { get; set; }

        public string TEXTO { get; set; }
        public Nullable<decimal> DESTINO { get; set; }
        public Nullable<System.DateTime> FECHA_ENVIO { get; set; }
        public string ESTADO { get; set; }
        public Nullable<decimal> ID_ATENCION { get; set; }
    }
}