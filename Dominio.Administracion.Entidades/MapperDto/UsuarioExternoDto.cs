namespace Dominio.Administracion.Entidades.MapperDto
{
    public class UsuarioExternoDto
    {
        public string Usuario { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Contrasena { get; set; }
        public string NombreUsuario { get; set; }
        public int Rol { get; set; }
        public string Email { get; set; }
    }
}