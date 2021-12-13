using System;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class ProcesosAsesorDto
    {
        public int ID_PROC { get; set; }
        public int CC_ASESOR { get; set; }
        public int ANIO { get; set; }
        public int MES { get; set; }

        public DateTime FECHA_INI { get; set; }
        public DateTime FECHA_FIN { get; set; }
    }
}