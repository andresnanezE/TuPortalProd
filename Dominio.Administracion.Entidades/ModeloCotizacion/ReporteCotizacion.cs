namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class ReporteCotizacion
    {
        public string FechaCreacion { get; set; }
        public int AnoActual { get; set; }
        public string NombreEmpresa { get; set; }
        public string Contacto { get; set; }
        public string Cargo { get; set; }
        public decimal Nit { get; set; }
        public string Ciudad { get; set; }
        public string NombreAsesor { get; set; }
        public string CelularAsesor { get; set; }
        public string NombreProducto { get; set; }
        public string NombreDirector { get; set; }
        public string CelularDirector { get; set; }
        public string CorreoDirector { get; set; }
    }
}