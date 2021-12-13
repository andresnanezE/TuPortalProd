//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloProcesos;
using System.Collections.Generic;

namespace Transversales.Administracion.Mappers
{
    public class MapperEMA_MENUADto
    {
        public static IEnumerable<EMA_MENUDto> AdaptarMenu(IEnumerable<EMA_MENU> origen)
        {
            List<EMA_MENUDto> vRetorno = new List<EMA_MENUDto>();
            if (origen != null)
            {
                foreach (var item in origen)
                {
                    vRetorno.Add(EntidadEmaMenuADto(item));
                }
            }
            return vRetorno;
        }

        public static EMA_MENUDto EntidadEmaMenuADto(EMA_MENU item)
        {
            EMA_MENUDto vRetorno = new EMA_MENUDto()
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