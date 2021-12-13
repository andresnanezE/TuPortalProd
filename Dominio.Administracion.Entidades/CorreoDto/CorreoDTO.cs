namespace Dominio.Administracion.Entidades.CorreoDto
{
    public class CorreoDTO
    {
        public string Asunto { get; set; }
        public string Body { get; set; }
        public string Destinos { get; set; }
        public string DestinosCopia { get; set; }
        public string DestinosCopiaOculta { get; set; }
        public string Adjuntos { get; set; }
        public string Tipo { get; set; }
    }
}