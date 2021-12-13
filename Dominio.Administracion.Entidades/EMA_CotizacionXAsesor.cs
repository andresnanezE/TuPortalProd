namespace Dominio.Administracion.Entidades
{
    public class EMA_CotizacionXAsesor
    {
        public int id_asesor { get; set; }
        public string nombreAsesor { get; set; }
        public string numeroDocumento { get; set; }
        public int numeroVisitas { get; set; }
        public string estadoAsesor { get; set; }
    }
}