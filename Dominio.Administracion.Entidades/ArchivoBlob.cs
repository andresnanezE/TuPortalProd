using System.IO;


namespace Dominio.Administracion.Entidades
{
    public class ArchivoBlob
    {

        public Stream File { get; set; }
        public string ContentType { get; set; }
        public long Lengthfile { get; set; }
    }
}