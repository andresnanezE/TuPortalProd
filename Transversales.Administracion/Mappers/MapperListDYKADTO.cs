//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloPortal;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperListDYKADTO
    {
        public static IEnumerable<DYK_BANNERDto> AdaptarMenu(IEnumerable<DYK_BANNER> origen)
        {
            List<DYK_BANNERDto> vRetorno = new List<DYK_BANNERDto>();
            if (origen != null)
            {
                foreach (var index in origen)
                {
                    vRetorno.Add(EntidadDYKADTO(index));
                }
            }
            return vRetorno;
        }

        public static DYK_BANNERDto EntidadDYKADTO(DYK_BANNER item)
        {
            DYK_BANNERDto vRetorno = new DYK_BANNERDto()
            {
                BANNERID = item.BANNERID,
                IMAGEN = item.IMAGEN,
                POSICION = item.POSICION,
                URL = item.URL
            };
            return vRetorno;
        }
    }
}