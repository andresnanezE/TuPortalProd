namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class EnvioEmailCotizacion
    {
        public FileData PlantillaCotizacion { get; set; }
        public string Nit { get; set; }
        public string NombreEmpresa { get; set; }
    }
}