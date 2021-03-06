// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioMenu.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Repositorios;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioMenu : IRepositorioMenu
    {
        #region IRepositorioMenu Members

        public int AgregarMenu(EMA_MENU menu)
        {
            using (var model = new ContextoPortal())
            {
                model.EMA_MENU.Add(menu);
                model.SaveChanges();
                return menu.MENUID;
            }
        }

        public void ModificarMenu(EMA_MENU menu)
        {
            using (var modelo = new ContextoPortal())
            {
                var menuEntidad = modelo.EMA_MENU.FirstOrDefault(m => m.MENUID == menu.MENUID);
                menuEntidad.DESCRIPCION = menu.DESCRIPCION;
                menuEntidad.NODOPADREID = menu.NODOPADREID;
                menuEntidad.FECHAMODIFICACION = menu.FECHAMODIFICACION;
                menuEntidad.USUARIOMODIFICACION = menu.USUARIOMODIFICACION;
                modelo.SaveChanges();
            }
        }

        public IEnumerable<MenuAplicacion> ObtenerMenu()
        {
            using (var modelo = new ContextoPortal())
            {
                var menuPrincipal = modelo.EMA_MENU.ToList();
                return menuPrincipal.Select(m => new MenuAplicacion
                                               {
                                                   MenuId = m.MENUID,
                                                   Descripcion = m.DESCRIPCION,
                                                   NodoPadreId = m.NODOPADREID,
                                                   DescripcionPadre = (modelo.EMA_MENU.Where(menu => menu.MENUID == m.NODOPADREID).Select(menu => menu.DESCRIPCION).FirstOrDefault()),
                                                   Controller = m.CONTROLLER,
                                                   Action = m.ACTION,
                                                   Url = m.URL,
                                                   Roles = String.Join(", ", (from r in modelo.EMA_ROL
                                                                join mr in modelo.EMA_ROLXMENU on r.ROLID equals mr.ROLID
                                                                where mr.MENUID == m.MENUID
                                                            select  r.ROL)),
                                                   FechaCreacion = m.FECHACREACION,
                                                   FechaModificacion = m.FECHAMODIFICACION,
                                                   UsuarioCreacion = m.USUARIOCREACION,
                                                   UsuarioModificacion = m.USUARIOMODIFICACION
                                               }).ToList();
            }
        }

        public IEnumerable<MenuAplicacion> ObtenerMenuFiltros(int menuId, string descripcion, int? nodoPadreId, int rolId)
        {
            using (var modelo = new ContextoPortal())
            {
                var listadoMenu = new List<EMA_MENU>();
                listadoMenu = modelo.EMA_MENU.ToList();
                if (menuId > 0) {
                    listadoMenu = listadoMenu.Where(m => m.MENUID == menuId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(descripcion))
                {
                    listadoMenu = listadoMenu.Any() ? listadoMenu.Where(m => m.DESCRIPCION.Contains(descripcion)).ToList() : modelo.EMA_MENU.Where(m => m.DESCRIPCION.Contains(descripcion)).ToList();
                }

                if (nodoPadreId > 0)
                {
                    listadoMenu = listadoMenu.Any() ? listadoMenu.Where(m => m.NODOPADREID == nodoPadreId).ToList() : modelo.EMA_MENU.Where(m => m.NODOPADREID == nodoPadreId).ToList();
                }
                if (rolId > 0)
                {
                    listadoMenu = listadoMenu.Any() ? (from m in listadoMenu
                        join mr in modelo.EMA_ROLXMENU on m.MENUID equals mr.MENUID
                        where mr.ROLID == rolId
                        select m).ToList(): (from m in modelo.EMA_MENU
                        join mr in modelo.EMA_ROLXMENU on m.MENUID equals mr.MENUID
                        where mr.ROLID == rolId
                        select m
                        ).ToList() ;
                }
                return listadoMenu.Select(m => new MenuAplicacion
                                               {
                                                   MenuId = m.MENUID,
                                                   Descripcion = m.DESCRIPCION,
                                                   NodoPadreId = m.NODOPADREID,
                                                   DescripcionPadre = (modelo.EMA_MENU.Where(menu => menu.MENUID == m.NODOPADREID).Select(menu => menu.DESCRIPCION).FirstOrDefault()),
                                                   Controller = m.CONTROLLER,
                                                   Action = m.ACTION,
                                                   Url = m.URL,
                                                   Roles = String.Join(", ", (from r in modelo.EMA_ROL
                                                                join mr in modelo.EMA_ROLXMENU on r.ROLID equals mr.ROLID
                                                                where mr.MENUID == m.MENUID
                                                            select  r.ROL)),
                                                   FechaCreacion = m.FECHACREACION,
                                                   FechaModificacion = m.FECHAMODIFICACION,
                                                   UsuarioCreacion = m.USUARIOCREACION,
                                                   UsuarioModificacion = m.USUARIOMODIFICACION
                                               }).ToList();
                
            }
        }

        public IEnumerable<EMA_MENU> ObtenerMenuPadres()
        {
            using (var modelo = new ContextoPortal())
            {
                var listaMenu = modelo.EMA_MENU.Where(m => m.NODOPADREID == null).ToList();
                return listaMenu;
            }
        }

        public IEnumerable<MenuAplicacion> ObtenerMenuRoles(List<int> rolesIds)
        {
            using (var modelo = new ContextoPortal())
            {
                var menuPrincipal = (from menu in modelo.EMA_MENU
                    join menuRol in modelo.EMA_ROLXMENU on menu.MENUID equals menuRol.MENUID
                    where menu.NODOPADREID == null && rolesIds.Contains(menuRol.ROLID)
                    select new MenuAplicacion
                           {
                               MenuId = menu.MENUID,
                               Descripcion = menu.DESCRIPCION,
                               NodoPadreId = menu.NODOPADREID,
                               Controller = menu.CONTROLLER,
                               Action = menu.ACTION,
                               Url = menu.URL
                           }).Distinct().ToList();

                foreach (var menuP in menuPrincipal)
                {
                    menuP.MenuHijos = (from menu in modelo.EMA_MENU
                        join menuRol in modelo.EMA_ROLXMENU on menu.MENUID equals menuRol.MENUID
                        where menu.NODOPADREID == menuP.MenuId && rolesIds.Contains(menuRol.ROLID)
                        select menu).Distinct().ToList().OrderBy(menu => menu.DESCRIPCION);
                }
                return menuPrincipal;
            }
        }

        #endregion
    }
}
