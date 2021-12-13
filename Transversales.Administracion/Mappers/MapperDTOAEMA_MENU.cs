//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloProcesos;

namespace Transversales.Administracion.Mappers
{
    public class MapperDTOAEMA_MENU
    {
        public static EMA_MENU AdaptarMenu(EMA_MENUDto origen)
        {
            EMA_MENU vRetorno = new EMA_MENU();
            if (origen != null)
            {
                vRetorno = EntidadDtoAEMAMENU(origen);
            }
            return vRetorno;
        }

        public static EMA_MENU EntidadDtoAEMAMENU(EMA_MENUDto item)
        {
            EMA_MENU vRetorno = new EMA_MENU()
            {
                ACTION = item.ACTION,
                CONTROLLER = item.CONTROLLER,
                DESCRIPCION = item.DESCRIPCION,
                FECHACREACION = item.FECHACREACION,
                FECHAMODIFICACION = item.FECHAMODIFICACION,
                ICONO = item.ICONO,
                MENUID = item.MENUID,
                NODOPADREID = item.NODOPADREID,
                URL = item.URL,
                USUARIOCREACION = item.USUARIOCREACION,
                USUARIOMODIFICACION = item.USUARIOMODIFICACION
            };
            return vRetorno;
        }
    }
}