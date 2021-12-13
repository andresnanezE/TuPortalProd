using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Detalle_Factor
    {
        [Key]
        public int Id { get; set; }

        public int Id_Factor { get; set; }        

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorConstante { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorExponente { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal Factor_Ajuste { get; set; }

        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
    }
}