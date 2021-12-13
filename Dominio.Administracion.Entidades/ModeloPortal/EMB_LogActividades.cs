using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class EMB_LogActividades
    {
        [Key]
        public int idLog { get; set; }

        public int? UsuarioId { get; set; }
        public DateTime fecha { get; set; }
        public int idTipoLog { get; set; }
        public string ip { get; set; }
        public int MenuId { get; set; }

        public string DocUsuario1 { get; set; }
        public string NombreUsuario1 { get; set; }

        public string DocUsuario2 { get; set; }
        public string NombreUsuario2 { get; set; }

        public string DatosIngreso { get; set; }
        public string Respuesta { get; set; }
        public string Detalle { get; set; }

        public DateTime? FechaHoraIni { get; set; }
        public DateTime? FechaHoraFin { get; set; }
        public TimeSpan? TiempoSesion { get; set; }

        public bool EsSesionEnVezDe { get; set; }
    }
}