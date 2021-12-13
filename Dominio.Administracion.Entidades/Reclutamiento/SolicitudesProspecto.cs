using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class SolicitudesProspecto
	{
		public int Id { get; set; }
		public string TipoDocumento { get; set; }
		public Int64 NumeroDocumento { get; set; }
		public string NombresApellidos { get; set; }
		public string EstadoArchivo { get; set; }
		public string Estado { get; set; }
		public string Proceso { get; set; }
		public string CorreoElectronico { get; set; }
		public Int64 Telefono { get; set; }
		public string CiudadVinculacion { get; set; }
		public string TipoReclutador { get; set; }
		public string Gestionado { get; set; }
		public int IdUsrDirector { get; set; }
		public string LogUsr { get; set; }
		public string NomUsr { get; set; }
		public DateTime FechaRegistro { get; set; }
		public string NombreOriginal { get; set; }
		public string RutaArchivo { get; set; }
		
	}

	public class SolicitudProspecto
	{
		public int Id { get; set; }
		public string TipoDocumento { get; set; }
		public Int64 NumeroDocumento { get; set; }
		public string NombresApellidos { get; set; }
		public string EstadoArchivo { get; set; }
		public string Estado { get; set; }
		public string Proceso { get; set; }
		public string CorreoElectronico { get; set; }
		public Int64 Telefono { get; set; }
		public string CiudadVinculacion { get; set; }
		public string TipoReclutador { get; set; }
		public string Gestionado { get; set; }
		public int IdUsrDirector { get; set; }
		public string LogUsr { get; set; }
		public string NomUsr { get; set; }
		public DateTime FechaRegistro { get; set; }
		public List<ArchivosProspecto> Archivos { get; set; }
	}

	public class ArchivosProspecto
	{
		public string NombreOriginal { get; set; }
		public string RutaArchivo { get; set; }
		public string EstadoArchivo { get; set; }
	}
}
