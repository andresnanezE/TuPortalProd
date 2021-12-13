//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloProcesos;
using System.Collections.Generic;
using System.Linq;

namespace Transversales.Administracion.Mappers
{
    public class MapperMenu
    {
        public static IEnumerable<MenuAplicacionDto> AdaptarMenu(IEnumerable<MenuAplicacion> origen)
        {
            List<MenuAplicacionDto> vRetorno = new List<MenuAplicacionDto>();
            if (origen != null)
            {
                foreach (var item in origen)
                {
                    vRetorno.Add(MenuAplicacionDtoEntidad(item));
                }
            }
            return vRetorno;
        }

        public static MenuAplicacionDto MenuAplicacionDtoEntidad(MenuAplicacion origen)
        {
            if (origen.MenuHijos == null || origen.MenuHijos.Count() <= 0)
            {
                origen.MenuHijos = new List<EMA_MENU>();
            }
            MenuAplicacionDto vRetorno = new MenuAplicacionDto()
            {
                Action = origen.Action,
                Controller = origen.Controller,
                Descripcion = origen.Descripcion,
                DescripcionPadre = origen.DescripcionPadre,
                FechaCreacion = origen.FechaCreacion,
                FechaModificacion = origen.FechaModificacion,
                Icono = origen.Icono,
                MenuHijos = convertirMenuHijos(origen.MenuHijos),
                MenuId = origen.MenuId,
                NodoPadreId = origen.NodoPadreId,
                Roles = origen.Roles,
                Url = origen.Url,
                UsuarioCreacion = origen.UsuarioCreacion,
                UsuarioModificacion = origen.UsuarioModificacion
            };
            return vRetorno;
        }

        private static IEnumerable<EMA_MENUDto> convertirMenuHijos(IEnumerable<EMA_MENU> listaOrigen)
        {
            List<EMA_MENUDto> listaRetorno = new List<EMA_MENUDto>();
            if (listaOrigen.Count() > 0)
            {
                foreach (var i in listaOrigen)
                {
                    EMA_MENUDto index = new EMA_MENUDto()
                    {
                        ACTION = i.ACTION,
                        CONTROLLER = i.CONTROLLER,
                        DESCRIPCION = i.DESCRIPCION,
                        FECHACREACION = i.FECHACREACION,
                        FECHAMODIFICACION = i.FECHAMODIFICACION,
                        ICONO = i.ICONO,
                        MENUID = i.MENUID,
                        NODOPADREID = i.NODOPADREID,
                        URL = i.URL,
                        USUARIOCREACION = i.USUARIOCREACION,
                        USUARIOMODIFICACION = i.USUARIOMODIFICACION
                    };
                    listaRetorno.Add(index);
                }
            }
            if (listaRetorno.Count() <= 0 || listaRetorno == null)
                listaRetorno = new List<EMA_MENUDto>();

            return listaRetorno;
        }
    }
}