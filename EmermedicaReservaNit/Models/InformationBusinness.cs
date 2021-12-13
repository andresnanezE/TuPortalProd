using Dominio.Administracion.Entidades.ModeloCotizacion;
using System.Collections.Generic;

namespace EmermedicaReservaNit.Models
{
    public class InformationBusinness
    {
        public string DigitoVerificacion { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreCanal { get; set; }
        public string NombreCiudad { get; set; }
        public List<Productos> ProductosDisponibles { get; set; }
    }
}