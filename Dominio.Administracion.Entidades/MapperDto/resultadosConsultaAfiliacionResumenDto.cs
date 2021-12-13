using System;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class resultadosConsultaAfiliacionResumenDto
    {
        // Campos de consulta resumen asesor
        public string CIUDAD { get; set; }

        public string DIRECTOR_COLPATRIA { get; set; }
        public string DIRECTOR_PRODUCTO { get; set; }
        public int? FAMILIAR { get; set; }
        public string NIVEL_DEL_ASESOR { get; set; }
        public string PERIODO_FINAL { get; set; }
        public string PERIODO_INICIO { get; set; }
        public int? PMP { get; set; }
        public int? AREAS_PROTEGIDAS { get; set; }
        public string CC_ASESOR { get; set; }
        public string ASESOR { get; set; }

        // Campos de consula resumen director
        public string NOMB_ASESOR { get; set; }

        public string ESTATUS { get; set; }
        public string CC_DIRECTOR { get; set; }
        public string DIRECTOR { get; set; }

        public DateTime? ACT_HORA_MAENO { get; set; }

        public int PRODUCTIVIDAD { get; set; }
        public decimal CUMPLIMIENTO { get; set; }
        public int METAS { get; set; }

        public DateTime? FECHAINICIO_ASESOR { get; set; }
    }
}