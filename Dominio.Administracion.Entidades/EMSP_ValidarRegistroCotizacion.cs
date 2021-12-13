namespace Dominio.Administracion.Entidades
{
    public class EMSP_ValidarRegistroCotizacion
    {
        public int cotizacionID { set; get; }
        public int asesorID { set; get; }
        public string fechaExpiracion { get; set; }
        public string ciudad { get; set; }
        public string nombreAsesor { get; set; }
        public string nombreDirector { get; set; }
        public int PRODUCTOID { set; get; }
        public string NombreProducto { get; set; }
    }
}