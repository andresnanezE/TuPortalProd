using System;

namespace Dominio.Administracion.Entidades
{
    public class ResultadosConsultaAfiliacionResumenTabla
    {
        // Campos de consulta resumen asesor

        public string CANAL { get; set; }
        public string SEGMENTO { get; set; }
        public string CIUDAD { get; set; }
        public int? AREAS_PROTEGIDAS { get; set; }
        public int? PMP { get; set; }
        public int? FAMILIAR { get; set; }
        public string PERIODO_INICIO { get; set; }
        public string PERIODO_FINAL { get; set; }

        // Campos de consula resumen director
        public string NOMB_ASESOR { get; set; }

        public string ESTATUS { get; set; }
        public string CC_DIRECTOR { get; set; }
        public string DIRECTOR { get; set; }
        public DateTime? ACT_HORA_MAENO { get; set; }
    }
}