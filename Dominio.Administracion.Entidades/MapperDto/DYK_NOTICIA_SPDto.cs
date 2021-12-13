namespace Dominio.Administracion.Entidades.MapperDto
{
    using System;

    public class DYK_NOTICIA_SPDto
    {
        public Guid NOTICIAID { get; set; }
        public string TITULO { get; set; }
        public string TITULOQS { get; set; }
        public string DESCRIPCION { get; set; }
        public string IMAGEN { get; set; }
        public DateTime FECHA { get; set; }
        public bool ACTIVO { get; set; }
    }
}