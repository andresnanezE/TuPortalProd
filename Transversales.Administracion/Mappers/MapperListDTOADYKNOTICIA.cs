//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloPortal;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperListDTOADYKNOTICIA
    {
        public static IEnumerable<DYK_NOTICIA_SPDto> AdaptarMenu(IEnumerable<DYK_NOTICIA_SP> origen)
        {
            List<DYK_NOTICIA_SPDto> vRetorno = new List<DYK_NOTICIA_SPDto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadDYKNoticiaADTO(index));
                }
            }
            return vRetorno;
        }

        public static DYK_NOTICIA_SPDto EntidadDYKNoticiaADTO(DYK_NOTICIA_SP item)
        {
            DYK_NOTICIA_SPDto vRetorno = new DYK_NOTICIA_SPDto()
            {
                ACTIVO = item.ACTIVO,
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