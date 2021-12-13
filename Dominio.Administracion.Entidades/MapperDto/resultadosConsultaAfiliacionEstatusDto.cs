namespace Dominio.Administracion.Entidades.MapperDto
{
    public class resultadosConsultaAfiliacionEstatusDto
    {
        // Campos de consulta resumen estatus
        public string ESTATUS_RESUMEN { get; set; }

        public int CANTIDAD_ASESORES { get; set; }
        public decimal PROMEDIO_ASESORES { get; set; }
        public decimal PRODUCTIVIDAD_ASESORES { get; set; }
    }
}