namespace Dominio.Administracion.Entidades.MapperDto.DtoProcesos
{
    public class EMA_COTIZACIONXASESORDto
    {
        public int CotizacionXAsesorID { get; set; }
        public int CotizacionID { get; set; }
        public int AsesorID { get; set; }
        public string fechaExpiracion { get; set; }
        public string ciudad { get; set; }
        public string nombreAsesor { get; set; }
        public string nombreDirector { get; set; }
        public int PRODUCTOID { get; set; }
        public string NombreProducto { get; set; }
    }
}