//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloPortal;

namespace Transversales.Administracion.Mappers
{
    public class MapperDTOADYKBanner
    {
        public static DYK_BANNERDto AdaptarMenu(DYK_BANNER origen)
        {
            DYK_BANNERDto vRetorno = new DYK_BANNERDto();
            if (origen != null)
            {
                vRetorno = EntidadDYKBANNERADto(origen);
            }
            return vRetorno;
        }

        public static DYK_BANNERDto EntidadDYKBANNERADto(DYK_BANNER item)
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