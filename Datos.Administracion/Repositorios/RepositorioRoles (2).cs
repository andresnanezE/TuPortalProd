// ----------------------------------------------------------------------------------------------
// <copyright file="RepositorioRoles.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Repositorios;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioRoles : IRepositorioRoles
    {
        #region IRepositorioRoles Members

        public int AgregarRol(EMA_ROL rol)
        {
            using (var modelo = new ContextoPortal())
            {
                modelo.EMA_ROL.Add(rol);
                modelo.SaveChanges();
                return rol.ROLID;
            }
        }

        public void AgregarRolMenu(IEnumerable<int> rolesIds, int menuId)
        {
            using (var modelo = new ContextoPortal())
            {
                foreach (var rolMenu in rolesIds.Select(rolId => new EMA_ROLXMENU
                                                                 {
                                                                     ROLID = rolId,
                                                                     MENUID = menuId
                                                                 }))
                {
                    modelo.EMA_ROLXMENU.Add(rolMenu);
                }
                modelo.SaveChanges();
            }
        }

        public void AgregarRolUsuario(IEnumerable<int> rolesIds, int usuarioId)
        {
            using (var modelo = new ContextoPortal())
            {
                foreach (var rolUsuario in rolesIds.Select(rolId => new EMA_ROLXUSUARIO
                                                                    {
                                                                        ROLID = rolId, USUARIOID = usuarioId
                                                                    }))
                {
                    modelo.EMA_ROLXUSUARIO.Add(rolUsuario);
                }
                modelo.SaveChanges();
            }
        }

        public void ModificarRol(EMA_ROL rol)
        {
            using (var modelo = new ContextoPortal())
            {
                var rolEntidad = modelo.EMA_ROL.FirstOrDefault(r => r.ROLID == rol.ROLID);
                rolEntidad.ROL = rol.ROL;
                rolEntidad.ACTIVO = rol.ACTIVO;
                rolEntidad.ROL = rol.ROL;
                rolEntidad.USUARIOMODIFICACION = rol.USUARIOMODIFICACION;
                modelo.SaveChanges();
            }
        }

        public void ModificarRolesMenu(IEnumerable<int> rolesIds, int menuId)
        {
            using (var model = new ContextoPortal())
            {
                var rolesMenu = model.EMA_ROLXMENU.Where(ru => ru.MENUID == menuId).ToList();
                var rolesRemove = rolesMenu.Where(ru => !rolesIds.Contains(ru.ROLID));
                model.EMA_ROLXMENU.RemoveRange(rolesRemove);
                foreach (var rolMenuEntidad in from rolId in rolesIds
                    where rolesMenu.All(ru => ru.ROLID != rolId)
                    select new EMA_ROLXMENU()
                           {
                               ROLID = rolId,
                               MENUID = menuId
                           })
                {
                    model.EMA_ROLXMENU.Add(rolMenuEntidad);
                }

                model.SaveChanges();
            }
        }

        public void ModificarRolesUsuario(IEnumerable<int> rolesIds, int usuarioId)
        {
            using (var model = new ContextoPortal())
            {
                var rolesUsuario = model.EMA_ROLXUSUARIO.Where(ru => ru.USUARIOID == usuarioId).ToList();
                var rolesRemove = rolesUsuario.Where(ru => !rolesIds.Contains(ru.ROLID));
                model.EMA_ROLXUSUARIO.RemoveRange(rolesRemove);
                foreach (var rolUsuarioEntidad in from rolId in rolesIds
                    where rolesUsuario.All(ru => ru.ROLID != rolId)
                    select new EMA_ROLXUSUARIO
                           {
                               ROLID = rolId,
                               USUARIOID = usuarioId
                           })
                {
                    model.EMA_ROLXUSUARIO.Add(rolUsuarioEntidad);
                }

                model.SaveChanges();
            }
        }

        public EMA_ROL ObtenerRolId(int rolId)
        {
            using (var modelo = new ContextoPortal())
            {
                var rol = modelo.EMA_ROL.FirstOrDefault(r => r.ROLID == rolId);
                return rol;
            }
        }

        public IEnumerable<EMA_ROLXMENU> ObtenerRolMenu(int menuId)
        {
            using (var modelo = new ContextoPortal())
            {
                var listadoRolesMenu = modelo.EMA_ROLXMENU.Where(ru => ru.MENUID == menuId).ToList();
                return listadoRolesMenu;
            }
        }

        public IEnumerable<EMA_ROLXUSUARIO> ObtenerRolUsuario(int usuarioId)
        {
            using (var modelo = new ContextoPortal())
            {
                var listadoRolesUsuario = modelo.EMA_ROLXUSUARIO.Where(ru => ru.USUARIOID == usuarioId).ToList();
                return listadoRolesUsuario;
            }
        }

        public IEnumerable<EMA_ROL> ObtenerRoles()
        {
            using (var modelo = new ContextoPortal())
            {
                var listadoRoles = modelo.EMA_ROL.ToList();
                return listadoRoles;
            }
        }

        public IEnumerable<EMA_ROL> ObtenerRolesActivos()
        {
            using (var modelo = new ContextoPortal())
            {
                var listadoRoles = modelo.EMA_ROL.Where(r => r.ACTIVO).ToList();
                return listadoRoles;
            }
        }

        public IEnumerable<EMA_ROL> ObtenerRolesFiltros(int rolId, string rol)
        {
            using (var modelo = new ContextoPortal())
            {
                var listadoRoles = new List<EMA_ROL>();
                if (rolId > 0)
                {
                    listadoRoles = modelo.EMA_ROL.Where(r => r.ROLID == rolId).ToList();
                }

                if (!string.IsNullOrWhiteSpace(rol))
                {
                    listadoRoles = listadoRoles.Any() ? listadoRoles.Where(r => r.ROL.Contains(rol)).ToList() : modelo.EMA_ROL.Where(r => r.ROL.Contains(rol)).ToList();
                }

                if (rolId == 0 && string.IsNullOrWhiteSpace(rol))
                {
                    listadoRoles = modelo.EMA_ROL.ToList();
                }

                return listadoRoles;

                return listadoRoles;
            }
        }

        #endregion
    }
}
