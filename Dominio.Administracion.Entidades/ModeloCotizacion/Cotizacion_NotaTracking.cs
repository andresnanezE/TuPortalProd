using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Cotizacion_NotaTracking
    {
        [Key]
        public int Id { get; set; }

        public int Id_Cotizacion { get; set; }
        public int Id_Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Nota { get; set; }
        public string DetalleReconsideracion { get; set; }
    }
}
