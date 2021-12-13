using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Cotizacion_Sedes
    {
                    
        [Key]
        public int Id { get; set; }

        public int Id_Cotizacion { get; set; }
        public int Id_Ciudad { get; set; }
        public int Id_TipoRiesgo { get; set; }
        public string NombreSede { get; set; }
        public int NoPersonalPermanente { get; set; }
        public int NoPersonalVisitantes { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }

        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public Nullable<decimal> ValorReconsideracion { get; set; }

        public string NombreCiudad { get; set; } = null;
        public string NombreTipoRiesgo { get; set; } = null;               

    }
}