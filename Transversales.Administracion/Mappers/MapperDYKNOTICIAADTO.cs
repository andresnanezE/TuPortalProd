//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloPortal;

namespace Transversales.Administracion.Mappers
{
    public class MapperDYKNOTICIAADTO
    {
        public static DYK_NOTICIADto AdaptarMenu(DYK_NOTICIA origen)
        {
            DYK_NOTICIADto vRetorno = new DYK_NOTICIADto();
            if (origen != null)
            {
                vRetorno = EntidadDYKNoticiaADTO(origen);
            }
            return vRetorno;
        }

        public static DYK_NOTICIADto EntidadDYKNoticiaADTO(DYK_NOTICIA item)
        {
            DYK_NOTICIADto vRetorno = new DYK_NOTICIADto()
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