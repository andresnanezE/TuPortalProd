using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Prima
    {
        [Key]
        public int Id { get; set; }

        public int Id_Factor { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }

        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
    }
}