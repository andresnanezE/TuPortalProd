using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class SolicitudesCapacitacion
	{
		public int Id { get; set; }
		public string ApellidosNombres { get; set; }
		public string TipoIdentificacion { get; set; }
		public Int64 NumeroDocumento { get; set; }
		public string Proceso { get; set; }
		public string Estado { get; set; }
		public string CorreoElectronico { get; set; }
		public string Ciudad { get; set; }
		public DateTime FechaRegistro { get; set; }
		public int? IdReclutamientoCapacitador { get; set; }
		public DateTime? FechaCapacitacion { get; set; }
		public DateTime? FechaInicio { get; set; }
		public DateTime? FechaFin { get; set; }
		public string EstadoCapacitacion{ get; set; }
	}
}
