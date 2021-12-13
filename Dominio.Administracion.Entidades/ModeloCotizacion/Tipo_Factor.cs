using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Tipo_Factor
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Boolean Estado { get; set; }
        public int Id_TipoFactor { get; set; }
    }
}