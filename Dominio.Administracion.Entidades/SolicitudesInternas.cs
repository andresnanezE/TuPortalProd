using System;

namespace Dominio.Administracion.Entidades
{
    /// <summary>
    /// John Nelson Rodriguez.
    /// </summary>
    public class SolicitudesInternasNotas
    {
        public string IdNota { get; set; }
        public string Nota { get; set; }
        public string url_archivo { get; set; }
        public string IdTicket { get; set; }
        public DateTime Fecha { get; set; }
        public byte[] archivo { get; set; }
        public string nombre_archivo { get; set; }
        public bool EsPrivada { get; set; }
        public string Mensaje { get; set; }
    }

    /// <summary>
    /// John Nelson Rodriguez.
    /// </summary>
    public class SolicitudesInternas
    {
        public string id_solicitud { get; set; }
        public decimal cod_ases { get; set; }
        public string asunto { get; set; }
        public string area { get; set; }
        public string tipo_requerimiento { get; set; }
        public string ciudad { get; set; }
        public DateTime fecha_ini { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public string nombre_archivo { get; set; }
        public string estado { get; set; }
        public byte[] archivo { get; set; }
        public string url_archivo { get; set; }
        public string Descripcion { get; set; }
        public string content_file_name { get; set; }
    }
}