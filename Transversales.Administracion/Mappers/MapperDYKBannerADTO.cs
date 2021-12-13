//using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModeloPortal;

namespace Transversales.Administracion.Mappers
{
    public class MapperDYKBannerADTO
    {
        public static DYK_BANNER AdaptarMenu(DYK_BANNERDto origen)
        {
            DYK_BANNER vRetorno = new DYK_BANNER();
            if (origen != null)
            {
                vRetorno = EntidadDtoADYKBANNER(origen);
            }
            return vRetorno;
        }

        public static DYK_BANNER EntidadDtoADYKBANNER(DYK_BANNERDto item)
        {
            DYK_BANNER vRetorno = new DYK_BANNER()
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