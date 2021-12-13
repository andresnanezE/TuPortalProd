namespace Dominio.Administracion.Entidades.MapperDto.DtoProcesos
{
    public class EMA_COTIZACIONXPRODUCTODto
    {
        public int CotizacionXProductoID { get; set; }
        public int CotizacionID { get; set; }
        public int ProductoID { get; set; }
        public string nombreProducto { get; set; }
        public string ciudadCotiz { get; set; }
    }
}