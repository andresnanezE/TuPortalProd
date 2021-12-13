namespace Dominio.Administracion.Entidades.MapperDto
{
    public class SesionEnVezDeUsuarioRecuperadoDto
    {
        public int Id { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string Segmento { get; set; }
        public string Perfil { get; set; }
        public string Canal { get; set; }
        public string Estado { get; set; }

        public int registrosMod { get; set; }
    }
}