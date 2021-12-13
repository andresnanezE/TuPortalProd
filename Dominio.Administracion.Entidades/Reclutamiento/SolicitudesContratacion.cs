using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class SolicitudesContratacion
	{
		public int Id { get; set; }
		public string NombresApellidos { get; set; }
		public string TipoIdentificacion { get; set; }
		public Int64 NumeroDocumento { get; set; }
		public string Direccion { get; set; }
		public Int64 Telefono { get; set; }
		public string Proceso { get; set; }
		public string Estado { get; set; }
		public string CorreoElectronico { get; set; }
		public string Ciudad { get; set; }
		public string TipoReclutador { get; set; }
		public string NombresApellidosReclutamiento { get; set; }
		public DateTime FechaRegistro { get; set; }
		public string NombreOriginal { get; set; }
		public string RutaArchivo { get; set; }
		public string EstadoArchivo { get; set; }
		public string NombreOriginalContratacion { get; set; }
		public string NombreArchivoContratacion { get; set; }
		public string RutaArchivoContratacion { get; set; }
		public bool InformacionPersonal { get; set; }
		public bool Sarlaft { get; set; }
		public bool ExperienciaComercial { get; set; }
		public bool Contrato { get; set; }
		public bool CertificacionTributaria { get; set; }
	}

	public class SolicitudContratacion
	{
		public int Id { get; set; }
		public string NombresApellidos { get; set; }
		public string TipoIdentificacion { get; set; }
		public Int64 NumeroDocumento { get; set; }
		public string Direccion { get; set; }
		public Int64 Telefono { get; set; }
		public string Proceso { get; set; }
		public string Estado { get; set; }
		public string CorreoElectronico { get; set; }
		public string Ciudad { get; set; }
		public string TipoReclutador { get; set; }
		public string NombresApellidosReclutamiento { get; set; }
		public DateTime FechaRegistro { get; set; }
		public bool InformacionPersonal { get; set; }
		public bool Sarlaft { get; set; }
		public bool ExperienciaComercial { get; set; }
		public bool Contrato { get; set; }
		public bool CertificacionTributaria { get; set; }
		public List<ArchivosContratacion> Archivos { get; set; }
	}

	public class ArchivosContratacion
	{
		public string NombreOriginal { get; set; }
		public string RutaArchivo { get; set; }
		public string NombreOriginalContratacion { get; set; }
		public string RutaArchivoContratacion { get; set; }
		public string EstadoArchivo { get; set; }
	}
}
