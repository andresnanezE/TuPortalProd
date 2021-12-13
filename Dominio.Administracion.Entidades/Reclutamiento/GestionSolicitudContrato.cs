using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
	public class GestionSolicitudContrato
	{
		public int IdProspecto { get; set; }
		public Int64 NumeroDocumento { get; set; }
		public bool InformacionPersonal { get; set; }
		public bool Sarlaft { get; set; }
		public bool ExperienciaComercial { get; set; }
		public bool Contrato { get; set; }
		public bool CertificacionTributaria { get; set; }
		public string Estado { get; set; }
		public string Observaciones { get; set; }
	}
}
