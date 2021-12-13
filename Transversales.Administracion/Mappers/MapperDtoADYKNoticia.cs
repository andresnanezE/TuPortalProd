//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloPortal;

namespace Transversales.Administracion.Mappers
{
    public class MapperDtoADYKNoticia
    {
        public static DYK_NOTICIA AdaptarMenu(DYK_NOTICIADto origen)
        {
            DYK_NOTICIA vRetorno = new DYK_NOTICIA();
            if (origen != null)
            {
                vRetorno = EntidadDTOADYKNoticia(origen);
            }
            return vRetorno;
        }

        public static DYK_NOTICIA EntidadDTOADYKNoticia(DYK_NOTICIADto item)
        {
            DYK_NOTICIA vRetorno = new DYK_NOTICIA()
            {
                ACTIVO = item.ACTIVO,
                BANNER = item.BANNER,
                CONTENIDO = item.CONTENIDO,
                DESCRIPCION = item.DESCRIPCION,
                FECHA = item.FECHA,
                IMAGEN = item.IMAGEN,
                NOTICIAID = item.NOTICIAID,
                TITULO = item.TITULO,
                TITULOQS = item.TITULOQS
            };
            return vRetorno;
        }
    }
}