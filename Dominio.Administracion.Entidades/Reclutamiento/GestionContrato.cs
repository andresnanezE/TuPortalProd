using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
	public class GestionContrato
	{
		public int Id { get; set; }
		public string TipoDocumento { get; set; }
		public Int64 NumeroDocumento { get; set; }
		public string NombresApellidos { get; set; }
		public string Estado { get; set; }
		public string Proceso { get; set; }
		public string CorreoElectronico { get; set; }
		public Int64 Telefono { get; set; }
		public bool InformacionPersonal { get; set; }
		public bool Sarlaft { get; set; }
		public bool ExperienciaComercial { get; set; }
		public bool Contrato { get; set; }
		public bool CertificacionTributaria { get; set; }
	}
}
